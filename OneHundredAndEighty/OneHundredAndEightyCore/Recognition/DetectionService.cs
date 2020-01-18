﻿#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;

#endregion

namespace OneHundredAndEightyCore.Recognition
{
    public class DetectionService
    {
        private readonly MainWindow mainWindow;
        private readonly DrawService drawService;
        private readonly ConfigService configService;
        private readonly ThrowService throwService;
        private readonly Logger logger;
        private List<CamService> cams;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;
        private double moveDetectedSleepTime;
        private double extractionSleepTime;
        private double thresholdSleepTime;
        private bool withDetection;
        private Stack<DetectedThrow> anotherThrows;
        private readonly object locker = new object();

        public DetectionService(MainWindow mainWindow,
                                DrawService drawService,
                                ConfigService configService,
                                ThrowService throwService,
                                Logger logger)
        {
            this.mainWindow = mainWindow;
            this.drawService = drawService;
            this.configService = configService;
            this.throwService = throwService;
            this.logger = logger;
        }

        private void Prepare()
        {
            anotherThrows = new Stack<DetectedThrow>();
            cams = new List<CamService>();
            var cam1Active = configService.Read<bool>(SettingsType.Cam1CheckBox);
            var cam2Active = configService.Read<bool>(SettingsType.Cam2CheckBox);
            var cam3Active = configService.Read<bool>(SettingsType.Cam3CheckBox);
            var cam4Active = configService.Read<bool>(SettingsType.Cam4CheckBox);
            if (cam1Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam1Grid.Name));
            }

            if (cam2Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam2Grid.Name));
            }

            if (cam3Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam3Grid.Name));
            }

            if (cam4Active)
            {
                cams.Add(new CamService(mainWindow, mainWindow.Cam4Grid.Name));
            }

            cts = new CancellationTokenSource();
            cancelToken = cts.Token;

            extractionSleepTime = configService.Read<double>(SettingsType.ExtractionSleepTime);
            thresholdSleepTime = configService.Read<double>(SettingsType.ThresholdSleepTime);
            moveDetectedSleepTime = configService.Read<double>(SettingsType.MoveDetectedSleepTime);
            withDetection = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
        }

        private void FindThrowOnRemainingCams(CamService succeededCam)
        {
            logger.Info($"Finding throws from remaining cams start. Succeeded cam: {succeededCam.camNumber}");

            foreach (var cam in cams.Where(cam => cam != succeededCam))
            {
                cam.FindThrow();
                cam.FindAndProcessDartContour();
            }

            var thrw = throwService.GetThrow();
            if (thrw != null)
            {
                PushThrow(thrw);
            }

            logger.Info($"Finding throws from remaining cams end");
        }

        public void RunDetection()
        {
            Prepare();

            Task.Run(() =>
                     {
                         Thread.CurrentThread.Name = $"Recognition_workerThread";

                         cams.ForEach(c => c.DoCapture(true));

                         while (!cancelToken.IsCancellationRequested)
                         {
                             foreach (var cam in cams)
                             {
                                 logger.Debug($"Cam_{cam.camNumber} detection start");

                                 var response = withDetection
                                                    ? cam.DetectMove()
                                                    : ResponseType.Nothing;

                                 if (response == ResponseType.Move)
                                 {
                                     Thread.Sleep(TimeSpan.FromSeconds(moveDetectedSleepTime));
                                     response = cam.DetectThrow();

                                     if (response == ResponseType.Trow)
                                     {
                                         cam.FindAndProcessDartContour();

                                         FindThrowOnRemainingCams(cam);

                                         logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Trow}'. Cycle break");
                                         break;
                                     }

                                     if (response == ResponseType.Extraction)
                                     {
                                         Thread.Sleep(TimeSpan.FromSeconds(extractionSleepTime));

                                         drawService.ProjectionClear();
                                         cams.ForEach(c => c.DoCapture(true));

                                         logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Extraction}'. Cycle break");
                                         break;
                                     }
                                 }

                                 Thread.Sleep(TimeSpan.FromSeconds(thresholdSleepTime));

                                 logger.Debug($"Cam_{cam.camNumber} detection end with response type '{ResponseType.Nothing}'");
                             }
                         }

                         cams.ForEach(c =>
                                      {
                                          c.Dispose();
                                          c.ClearImageBoxes();
                                      });

                         logger.Info($"Detection for {cams.Count} cams end. Cancellation requested");
                     });
        }

        public void StopDetection()
        {
            cts?.Cancel();
        }

        private void PushThrow(DetectedThrow thrw)
        {
            lock (locker)
            {
                anotherThrows.Push(thrw);
            }
        }

        public DetectedThrow TryPopThrow()
        {
            lock (locker)
            {
                if (anotherThrows.Count > 0)
                {
                    return anotherThrows.Pop();
                }

                return null;
            }
        }
    }
}