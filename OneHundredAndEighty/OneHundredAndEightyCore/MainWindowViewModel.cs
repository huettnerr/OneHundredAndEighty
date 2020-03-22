﻿#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autofac;
using Microsoft.Win32;
using NLog;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Game;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow mainWindow;
        private readonly Logger logger;
        private readonly DBService dbService;
        private readonly ConfigService configService;
        private readonly DrawService drawService;
        private readonly ThrowService throwService;
        private readonly DetectionService detectionService;
        private ScoreBoardService scoreBoardService;
        private readonly GameService gameService;
        private CancellationTokenSource cts;
        private CancellationToken cancelToken;

        private const double AppVersion = 2.0;
        public bool IsSettingsDirty { get; set; }


        #region Bindable props

        private List<Player> players;

        #endregion

        public List<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            logger = MainWindow.ServiceContainer.Resolve<Logger>();
            dbService = MainWindow.ServiceContainer.Resolve<DBService>();
            configService = MainWindow.ServiceContainer.Resolve<ConfigService>();
            drawService = MainWindow.ServiceContainer.Resolve<DrawService>();
            throwService = MainWindow.ServiceContainer.Resolve<ThrowService>();
            gameService = MainWindow.ServiceContainer.Resolve<GameService>();
            scoreBoardService = MainWindow.ServiceContainer.Resolve<ScoreBoardService>();
            detectionService = MainWindow.ServiceContainer.Resolve<DetectionService>();
            detectionService.OnErrorOccurred += OnDetectionServiceErrorOccurred;

            CheckVersion(AppVersion);
            LoadSettings();
            drawService.ProjectionPrepare();
            mainWindow.NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(Resources.EmptyUserIcon);
            LoadPlayers();
        }

        private void CheckVersion(double appVersion)
        {
            var dbVersion = configService.Read<double>(SettingsType.DBVersion);
            if (appVersion != dbVersion)
            {
                var errorText = Resources.ResourceManager.GetString("VersionsMismatchErrorText");
                MessageBox.Show(errorText, "Error", MessageBoxButton.OK);
                throw new Exception("DB version and App version is different");
            }
        }

        private void LoadPlayers()
        {
            var playersTable = dbService.PlayersLoadAll();
            Players = Converter.PlayersFromTable(playersTable);
        }

        public void StartGame()
        {
            if (!Validator.ValidateImplementedGameTypes(mainWindow.NewGameControls))
            {
                MessageBox.Show(Resources.NotImplementedYetErrorText, "Error", MessageBoxButton.OK);
                return;
            }

            if (!Validator.ValidateStartNewGamePlayersSelected(mainWindow.NewGameControls))
            {
                MessageBox.Show(Resources.NewGamePlayersNotSelectedErrorText, "Error", MessageBoxButton.OK);
                return;
            }

            if (!Validator.ValidateStartNewClassicGamePoints(mainWindow.NewGameControls))
            {
                MessageBox.Show(Resources.NewClassicGamePointsNotSelectedErrorText, "Error", MessageBoxButton.OK);
                return;
            }

            ToggleMainTabItemsEnabled();
            ToggleMatchControlsEnabled();

            try
            {
                gameService.StartGame();
            }
            catch (Exception e)
            {
                CloseScoreBoard();
                StopGame(GameResultType.Error);
                MessageBox.Show($"{e.Message} \n {e.StackTrace}", "Error", MessageBoxButton.OK);
            }
        }

        public void StopGame(GameResultType type = GameResultType.NotDefined)
        {
            ToggleMainTabItemsEnabled();
            ToggleMatchControlsEnabled();

            gameService.StopGame(type);
        }

        public void SaveNewPlayer()
        {
            var newPlayerName = mainWindow.NewPlayerNameTextBox.Text;
            var newPlayerNickName = mainWindow.NewPlayerNickNameTextBox.Text;
            if (!Validator.ValidateNewPlayerNameAndNickName(newPlayerName, newPlayerNickName))
            {
                MessageBox.Show(Resources.NewPlayerEmptyDataErrorText, "Error", MessageBoxButton.OK);
                return;
            }

            var newPlayer = new Player(newPlayerName,
                                       newPlayerNickName,
                                       -1,
                                       mainWindow.NewPlayerAvatar.Source as BitmapImage);
            try
            {
                dbService.PlayerSaveNew(newPlayer);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK); // todo need to explain error
                return;
            }

            MessageBox.Show(string.Format(Resources.NewPlayerSuccessfullySavedText, newPlayer), "New player saved", MessageBoxButton.OK);
            mainWindow.NewPlayerNameTextBox.Text = string.Empty;
            mainWindow.NewPlayerNickNameTextBox.Text = string.Empty;
            mainWindow.NewPlayerAvatar.Source = Converter.BitmapToBitmapImage(Resources.EmptyUserIcon);

            LoadPlayers();
        }

        public void CalibrateCamsSetupPoint()
        {
            const double startRadSector = -3.14159;
            const double radSectorStep = 0.314159;
            const int dartboardDiameterInPixels = 1020;
            const int dartboardDiameterInCm = 34;

            var toCam1CmDistance = Convert.ToDouble(mainWindow.ToCam1Distance.Text, CultureInfo.InvariantCulture);
            var toCam2CmDistance = Convert.ToDouble(mainWindow.ToCam2Distance.Text, CultureInfo.InvariantCulture);
            var toCam3CmDistance = Convert.ToDouble(mainWindow.ToCam3Distance.Text, CultureInfo.InvariantCulture);
            var toCam4CmDistance = Convert.ToDouble(mainWindow.ToCam4Distance.Text, CultureInfo.InvariantCulture);

            var toCam1Pixels = dartboardDiameterInPixels * toCam1CmDistance / dartboardDiameterInCm;
            var toCam2Pixels = dartboardDiameterInPixels * toCam2CmDistance / dartboardDiameterInCm;
            var toCam3Pixels = dartboardDiameterInPixels * toCam3CmDistance / dartboardDiameterInCm;
            var toCam4Pixels = dartboardDiameterInPixels * toCam4CmDistance / dartboardDiameterInCm;

            var calibratedCam1SetupPoint = new PointF
                                           {
                                               X = (int) (drawService.projectionCenterPoint.X + Math.Cos(startRadSector + 2 * radSectorStep) * toCam1Pixels),
                                               Y = (int) (drawService.projectionCenterPoint.Y + Math.Sin(startRadSector + 2 * radSectorStep) * toCam1Pixels)
                                           };
            var calibratedCam2SetupPoint = new PointF
                                           {
                                               X = (int) (drawService.projectionCenterPoint.X + Math.Cos(startRadSector + 4 * radSectorStep) * toCam2Pixels),
                                               Y = (int) (drawService.projectionCenterPoint.Y + Math.Sin(startRadSector + 4 * radSectorStep) * toCam2Pixels)
                                           };
            var calibratedCam3SetupPoint = new PointF
                                           {
                                               X = (int) (drawService.projectionCenterPoint.X + Math.Cos(startRadSector + 6 * radSectorStep) * toCam3Pixels),
                                               Y = (int) (drawService.projectionCenterPoint.Y + Math.Sin(startRadSector + 6 * radSectorStep) * toCam3Pixels)
                                           };
            var calibratedCam4SetupPoint = new PointF
                                           {
                                               X = (int) (drawService.projectionCenterPoint.X + Math.Cos(startRadSector + 8 * radSectorStep) * toCam4Pixels),
                                               Y = (int) (drawService.projectionCenterPoint.Y + Math.Sin(startRadSector + 8 * radSectorStep) * toCam4Pixels)
                                           };

            mainWindow.Cam1XTextBox.Text = calibratedCam1SetupPoint.X.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam1YTextBox.Text = calibratedCam1SetupPoint.Y.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam2XTextBox.Text = calibratedCam2SetupPoint.X.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam2YTextBox.Text = calibratedCam2SetupPoint.Y.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam3XTextBox.Text = calibratedCam3SetupPoint.X.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam3YTextBox.Text = calibratedCam3SetupPoint.Y.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam4XTextBox.Text = calibratedCam4SetupPoint.X.ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam4YTextBox.Text = calibratedCam4SetupPoint.Y.ToString(CultureInfo.InvariantCulture);

            configService.Write(SettingsType.Cam1X, calibratedCam1SetupPoint.X);
            configService.Write(SettingsType.Cam1Y, calibratedCam1SetupPoint.Y);
            configService.Write(SettingsType.Cam2X, calibratedCam2SetupPoint.X);
            configService.Write(SettingsType.Cam2Y, calibratedCam2SetupPoint.Y);
            configService.Write(SettingsType.Cam3X, calibratedCam3SetupPoint.X);
            configService.Write(SettingsType.Cam3Y, calibratedCam3SetupPoint.Y);
            configService.Write(SettingsType.Cam4X, calibratedCam4SetupPoint.X);
            configService.Write(SettingsType.Cam4Y, calibratedCam4SetupPoint.Y);
            configService.Write(SettingsType.ToCam1Distance, mainWindow.ToCam1Distance.Text);
            configService.Write(SettingsType.ToCam2Distance, mainWindow.ToCam2Distance.Text);
            configService.Write(SettingsType.ToCam3Distance, mainWindow.ToCam3Distance.Text);
            configService.Write(SettingsType.ToCam4Distance, mainWindow.ToCam4Distance.Text);
        }

        public void SelectAvatarImage()
        {
            var ofd = new OpenFileDialog
                      {
                          Title = $"{Resources.ChoosePlayerAvatarText}",
                          Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif"
                      };
            if (ofd.ShowDialog() == true)
            {
                var image = new BitmapImage(new Uri(ofd.FileName));
                if (Validator.ValidateNewPlayerAvatar(image))
                {
                    mainWindow.NewPlayerAvatar.Source = image;
                }
                else
                {
                    MessageBox.Show(Resources.PlayerAvatarTooBigErrorText, "Error", MessageBoxButton.OK);
                }
            }
        }

        #region Error

        private void OnDetectionServiceErrorOccurred(Exception e)
        {
            MessageBox.Show($"{e.Message} \n {e.StackTrace}", "Error", MessageBoxButton.OK);
            StopGame(GameResultType.Error);
        }

        #endregion

        #region CamSetupCapturing

        public async void StartCamSetupCapturing(string gridName)
        {
            ToggleCamSetupGridControlsEnabled(gridName);
            cts = new CancellationTokenSource();
            var cancelToken = cts.Token;

            try
            {
                await Task.Run(() =>
                               {
                                   var cam = new CamService(mainWindow, gridName, CamServiceWorkingMode.Setup);
                                   while (!cancelToken.IsCancellationRequested)
                                   {
                                       cam.DoCapture(true);
                                       cam.RefreshImageBoxes();
                                   }

                                   cam.ClearImageBoxes();
                                   cam.Dispose();
                               });
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} \n {e.StackTrace}", "Error", MessageBoxButton.OK);
                StopCamSetupCapturing(gridName);
            }
        }

        public void StopCamSetupCapturing(string gridName)
        {
            cts?.Cancel();
            ToggleCamSetupGridControlsEnabled(gridName);
        }

        #endregion

        #region Controls toggles

        private void ToggleCamSetupGridControlsEnabled(string gridName)
        {
            ToggleMainTabItemsEnabled();
            ToggleSetupTabItemsEnabled();

            switch (gridName)
            {
                case "Cam1Grid":
                    mainWindow.Cam1StartButton.IsEnabled = !mainWindow.Cam1StartButton.IsEnabled;
                    mainWindow.Cam1StopButton.IsEnabled = !mainWindow.Cam1StopButton.IsEnabled;
                    mainWindow.Cam1TresholdSlider.IsEnabled = !mainWindow.Cam1TresholdSlider.IsEnabled;
                    mainWindow.Cam1SurfaceSlider.IsEnabled = !mainWindow.Cam1SurfaceSlider.IsEnabled;
                    mainWindow.Cam1SurfaceCenterSlider.IsEnabled = !mainWindow.Cam1SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam1RoiPosYSlider.IsEnabled = !mainWindow.Cam1RoiPosYSlider.IsEnabled;
                    mainWindow.Cam1RoiHeightSlider.IsEnabled = !mainWindow.Cam1RoiHeightSlider.IsEnabled;
                    break;
                case "Cam2Grid":
                    mainWindow.Cam2StartButton.IsEnabled = !mainWindow.Cam2StartButton.IsEnabled;
                    mainWindow.Cam2StopButton.IsEnabled = !mainWindow.Cam2StopButton.IsEnabled;
                    mainWindow.Cam2TresholdSlider.IsEnabled = !mainWindow.Cam2TresholdSlider.IsEnabled;
                    mainWindow.Cam2SurfaceSlider.IsEnabled = !mainWindow.Cam2SurfaceSlider.IsEnabled;
                    mainWindow.Cam2SurfaceCenterSlider.IsEnabled = !mainWindow.Cam2SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam2RoiPosYSlider.IsEnabled = !mainWindow.Cam2RoiPosYSlider.IsEnabled;
                    mainWindow.Cam2RoiHeightSlider.IsEnabled = !mainWindow.Cam2RoiHeightSlider.IsEnabled;
                    break;
                case "Cam3Grid":
                    mainWindow.Cam3StartButton.IsEnabled = !mainWindow.Cam3StartButton.IsEnabled;
                    mainWindow.Cam3StopButton.IsEnabled = !mainWindow.Cam3StopButton.IsEnabled;
                    mainWindow.Cam3TresholdSlider.IsEnabled = !mainWindow.Cam3TresholdSlider.IsEnabled;
                    mainWindow.Cam3SurfaceSlider.IsEnabled = !mainWindow.Cam3SurfaceSlider.IsEnabled;
                    mainWindow.Cam3SurfaceCenterSlider.IsEnabled = !mainWindow.Cam3SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam3RoiPosYSlider.IsEnabled = !mainWindow.Cam3RoiPosYSlider.IsEnabled;
                    mainWindow.Cam3RoiHeightSlider.IsEnabled = !mainWindow.Cam3RoiHeightSlider.IsEnabled;
                    break;
                case "Cam4Grid":
                    mainWindow.Cam4StartButton.IsEnabled = !mainWindow.Cam4StartButton.IsEnabled;
                    mainWindow.Cam4StopButton.IsEnabled = !mainWindow.Cam4StopButton.IsEnabled;
                    mainWindow.Cam4TresholdSlider.IsEnabled = !mainWindow.Cam4TresholdSlider.IsEnabled;
                    mainWindow.Cam4SurfaceSlider.IsEnabled = !mainWindow.Cam4SurfaceSlider.IsEnabled;
                    mainWindow.Cam4SurfaceCenterSlider.IsEnabled = !mainWindow.Cam4SurfaceCenterSlider.IsEnabled;
                    mainWindow.Cam4RoiPosYSlider.IsEnabled = !mainWindow.Cam4RoiPosYSlider.IsEnabled;
                    mainWindow.Cam4RoiHeightSlider.IsEnabled = !mainWindow.Cam4RoiHeightSlider.IsEnabled;
                    break;
            }
        }

        private void ToggleSetupTabItemsEnabled()
        {
            foreach (TabItem tabItem in mainWindow.SetupTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }
        }

        private void ToggleMainTabItemsEnabled()
        {
            foreach (TabItem tabItem in mainWindow.MainTabControl.Items)
            {
                tabItem.IsEnabled = !tabItem.IsEnabled;
            }
        }

        private void ToggleMatchControlsEnabled()
        {
            mainWindow.StartGameButton.IsEnabled = !mainWindow.StartGameButton.IsEnabled;
            mainWindow.StopGameButton.IsEnabled = !mainWindow.StopGameButton.IsEnabled;
            mainWindow.NewGameTypeComboBox.IsEnabled = !mainWindow.NewGameTypeComboBox.IsEnabled;
            mainWindow.NewGamePlayer1ComboBox.IsEnabled = !mainWindow.NewGamePlayer1ComboBox.IsEnabled;
            mainWindow.NewGamePlayer2ComboBox.IsEnabled = !mainWindow.NewGamePlayer2ComboBox.IsEnabled;
            mainWindow.NewGameSetsComboBox.IsEnabled = !mainWindow.NewGameSetsComboBox.IsEnabled;
            mainWindow.NewGameLegsComboBox.IsEnabled = !mainWindow.NewGameLegsComboBox.IsEnabled;
        }

        public void ToggleNewGameControlsVisibility()
        {
            var selectedGameType = Converter.NewGameControlsToGameTypeUi(mainWindow.NewGameControls);

            switch (selectedGameType)
            {
                case GameTypeUi.FreeThrowsSingle:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameTypeUi.FreeThrowsDouble:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Hidden;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Hidden;
                    break;
                case GameTypeUi.Classic:
                    mainWindow.NewGamePlayer2ComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGamePlayer2Label.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGameSetsLabel.Visibility = Visibility.Visible;
                    mainWindow.NewGameLegsComboBox.Visibility = Visibility.Visible;
                    mainWindow.NewGameLegsLabel.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CloseScoreBoard()
        {
            scoreBoardService.CloseScoreBoard();
        }

        #endregion

        #region Settings

        private void LoadSettings()
        {
            logger.Debug("Load settings start");

            mainWindow.Left = configService.Read<double>(SettingsType.MainWindowPositionLeft);
            mainWindow.Top = configService.Read<double>(SettingsType.MainWindowPositionTop);
            mainWindow.CamFovTextBox.Text = configService.Read<int>(SettingsType.CamFovAngle).ToString();
            mainWindow.CamResolutionHeightTextBox.Text = configService.Read<int>(SettingsType.ResolutionHeight).ToString();
            mainWindow.CamResolutionWidthTextBox.Text = configService.Read<int>(SettingsType.ResolutionWidth).ToString();
            mainWindow.MovesExtractionTextBox.Text = configService.Read<int>(SettingsType.MovesExtraction).ToString();
            mainWindow.MoveDetectedSleepTimeTextBox.Text = configService.Read<double>(SettingsType.MoveDetectedSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.MovesNoiseTextBox.Text = configService.Read<int>(SettingsType.MovesNoise).ToString();
            mainWindow.SmoothGaussTextBox.Text = configService.Read<int>(SettingsType.SmoothGauss).ToString();
            mainWindow.ThresholdSleepTimeTimeTextBox.Text = configService.Read<double>(SettingsType.ThresholdSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.ExtractionSleepTimeTimeTextBox.Text = configService.Read<double>(SettingsType.ExtractionSleepTime).ToString(CultureInfo.InvariantCulture);
            mainWindow.MinContourArcTextBox.Text = configService.Read<int>(SettingsType.MinContourArc).ToString();
            mainWindow.MovesDartTextBox.Text = configService.Read<int>(SettingsType.MovesDart).ToString();
            mainWindow.Cam1IdTextBox.Text = configService.Read<string>(SettingsType.Cam1Id);
            mainWindow.Cam2IdTextBox.Text = configService.Read<string>(SettingsType.Cam2Id);
            mainWindow.Cam3IdTextBox.Text = configService.Read<string>(SettingsType.Cam3Id);
            mainWindow.Cam4IdTextBox.Text = configService.Read<string>(SettingsType.Cam4Id);
            mainWindow.ToCam1Distance.Text = configService.Read<double>(SettingsType.ToCam1Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam2Distance.Text = configService.Read<double>(SettingsType.ToCam2Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam3Distance.Text = configService.Read<double>(SettingsType.ToCam3Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.ToCam4Distance.Text = configService.Read<double>(SettingsType.ToCam4Distance).ToString(CultureInfo.InvariantCulture);
            mainWindow.Cam1XTextBox.Text = configService.Read<int>(SettingsType.Cam1X).ToString();
            mainWindow.Cam2XTextBox.Text = configService.Read<int>(SettingsType.Cam2X).ToString();
            mainWindow.Cam3XTextBox.Text = configService.Read<int>(SettingsType.Cam3X).ToString();
            mainWindow.Cam4XTextBox.Text = configService.Read<int>(SettingsType.Cam4X).ToString();
            mainWindow.Cam1YTextBox.Text = configService.Read<int>(SettingsType.Cam1Y).ToString();
            mainWindow.Cam2YTextBox.Text = configService.Read<int>(SettingsType.Cam2Y).ToString();
            mainWindow.Cam3YTextBox.Text = configService.Read<int>(SettingsType.Cam3Y).ToString();
            mainWindow.Cam4YTextBox.Text = configService.Read<int>(SettingsType.Cam4Y).ToString();
            mainWindow.Cam1CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam1CheckBox);
            mainWindow.Cam2CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam2CheckBox);
            mainWindow.Cam3CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam3CheckBox);
            mainWindow.Cam4CheckBox.IsChecked = configService.Read<bool>(SettingsType.Cam4CheckBox);
            mainWindow.WithDetectionCheckBox.IsChecked = configService.Read<bool>(SettingsType.WithDetectionCheckBox);
            mainWindow.Cam1TresholdSlider.Value = configService.Read<double>(SettingsType.Cam1TresholdSlider);
            mainWindow.Cam1SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceSlider);
            mainWindow.Cam1SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam1SurfaceCenterSlider);
            mainWindow.Cam1RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam1RoiPosYSlider);
            mainWindow.Cam1RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam1RoiHeightSlider);
            mainWindow.Cam2TresholdSlider.Value = configService.Read<double>(SettingsType.Cam2TresholdSlider);
            mainWindow.Cam2SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceSlider);
            mainWindow.Cam2SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam2SurfaceCenterSlider);
            mainWindow.Cam2RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam2RoiPosYSlider);
            mainWindow.Cam2RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam2RoiHeightSlider);
            mainWindow.Cam3TresholdSlider.Value = configService.Read<double>(SettingsType.Cam3TresholdSlider);
            mainWindow.Cam3SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceSlider);
            mainWindow.Cam3SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam3SurfaceCenterSlider);
            mainWindow.Cam3RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam3RoiPosYSlider);
            mainWindow.Cam3RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam3RoiHeightSlider);
            mainWindow.Cam4TresholdSlider.Value = configService.Read<double>(SettingsType.Cam4TresholdSlider);
            mainWindow.Cam4SurfaceSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceSlider);
            mainWindow.Cam4SurfaceCenterSlider.Value = configService.Read<double>(SettingsType.Cam4SurfaceCenterSlider);
            mainWindow.Cam4RoiPosYSlider.Value = configService.Read<double>(SettingsType.Cam4RoiPosYSlider);
            mainWindow.Cam4RoiHeightSlider.Value = configService.Read<double>(SettingsType.Cam4RoiHeightSlider);

            IsSettingsDirty = false;

            logger.Debug("Load settings end");
        }

        public void SaveSettingsIfDirty()
        {
            logger.Debug("Save settings start");
            logger.Debug($"Is settings dirty: {IsSettingsDirty}");

            if (IsSettingsDirty)
            {
                configService.Write(SettingsType.MainWindowPositionLeft, mainWindow.Left);
                configService.Write(SettingsType.MainWindowPositionTop, mainWindow.Top);
                configService.Write(SettingsType.CamFovAngle, mainWindow.CamFovTextBox.Text);
                configService.Write(SettingsType.ResolutionHeight, mainWindow.CamResolutionHeightTextBox.Text);
                configService.Write(SettingsType.ResolutionWidth, mainWindow.CamResolutionWidthTextBox.Text);
                configService.Write(SettingsType.MovesExtraction, mainWindow.MovesExtractionTextBox.Text);
                configService.Write(SettingsType.MoveDetectedSleepTime, mainWindow.MoveDetectedSleepTimeTextBox.Text);
                configService.Write(SettingsType.MovesNoise, mainWindow.MovesNoiseTextBox.Text);
                configService.Write(SettingsType.SmoothGauss, mainWindow.SmoothGaussTextBox.Text);
                configService.Write(SettingsType.ThresholdSleepTime, mainWindow.ThresholdSleepTimeTimeTextBox.Text);
                configService.Write(SettingsType.ExtractionSleepTime, mainWindow.ExtractionSleepTimeTimeTextBox.Text);
                configService.Write(SettingsType.MinContourArc, mainWindow.MinContourArcTextBox.Text);
                configService.Write(SettingsType.MovesDart, mainWindow.MovesDartTextBox.Text);
                configService.Write(SettingsType.Cam1Id, mainWindow.Cam1IdTextBox.Text);
                configService.Write(SettingsType.Cam2Id, mainWindow.Cam2IdTextBox.Text);
                configService.Write(SettingsType.Cam3Id, mainWindow.Cam3IdTextBox.Text);
                configService.Write(SettingsType.Cam4Id, mainWindow.Cam4IdTextBox.Text);
                configService.Write(SettingsType.ToCam1Distance, mainWindow.ToCam1Distance.Text);
                configService.Write(SettingsType.ToCam2Distance, mainWindow.ToCam2Distance.Text);
                configService.Write(SettingsType.ToCam3Distance, mainWindow.ToCam3Distance.Text);
                configService.Write(SettingsType.ToCam4Distance, mainWindow.ToCam4Distance.Text);
                configService.Write(SettingsType.Cam1X, mainWindow.Cam1XTextBox.Text);
                configService.Write(SettingsType.Cam2X, mainWindow.Cam2XTextBox.Text);
                configService.Write(SettingsType.Cam3X, mainWindow.Cam3XTextBox.Text);
                configService.Write(SettingsType.Cam4X, mainWindow.Cam4XTextBox.Text);
                configService.Write(SettingsType.Cam1Y, mainWindow.Cam1YTextBox.Text);
                configService.Write(SettingsType.Cam2Y, mainWindow.Cam2YTextBox.Text);
                configService.Write(SettingsType.Cam3Y, mainWindow.Cam3YTextBox.Text);
                configService.Write(SettingsType.Cam4Y, mainWindow.Cam4YTextBox.Text);
                configService.Write(SettingsType.Cam1CheckBox, mainWindow.Cam1CheckBox.IsChecked);
                configService.Write(SettingsType.Cam2CheckBox, mainWindow.Cam2CheckBox.IsChecked);
                configService.Write(SettingsType.Cam3CheckBox, mainWindow.Cam3CheckBox.IsChecked);
                configService.Write(SettingsType.Cam4CheckBox, mainWindow.Cam4CheckBox.IsChecked);
                configService.Write(SettingsType.WithDetectionCheckBox, mainWindow.WithDetectionCheckBox.IsChecked);
                configService.Write(SettingsType.Cam1TresholdSlider, mainWindow.Cam1TresholdSlider.Value);
                configService.Write(SettingsType.Cam1SurfaceSlider, mainWindow.Cam1SurfaceSlider.Value);
                configService.Write(SettingsType.Cam1SurfaceCenterSlider, mainWindow.Cam1SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam1RoiPosYSlider, mainWindow.Cam1RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam1RoiHeightSlider, mainWindow.Cam1RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam2TresholdSlider, mainWindow.Cam2TresholdSlider.Value);
                configService.Write(SettingsType.Cam2SurfaceSlider, mainWindow.Cam2SurfaceSlider.Value);
                configService.Write(SettingsType.Cam2SurfaceCenterSlider, mainWindow.Cam2SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam2RoiPosYSlider, mainWindow.Cam2RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam2RoiHeightSlider, mainWindow.Cam2RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam3TresholdSlider, mainWindow.Cam3TresholdSlider.Value);
                configService.Write(SettingsType.Cam3SurfaceSlider, mainWindow.Cam3SurfaceSlider.Value);
                configService.Write(SettingsType.Cam3SurfaceCenterSlider, mainWindow.Cam3SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam3RoiPosYSlider, mainWindow.Cam3RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam3RoiHeightSlider, mainWindow.Cam3RoiHeightSlider.Value);
                configService.Write(SettingsType.Cam4TresholdSlider, mainWindow.Cam4TresholdSlider.Value);
                configService.Write(SettingsType.Cam4SurfaceSlider, mainWindow.Cam4SurfaceSlider.Value);
                configService.Write(SettingsType.Cam4SurfaceCenterSlider, mainWindow.Cam4SurfaceCenterSlider.Value);
                configService.Write(SettingsType.Cam4RoiPosYSlider, mainWindow.Cam4RoiPosYSlider.Value);
                configService.Write(SettingsType.Cam4RoiHeightSlider, mainWindow.Cam4RoiHeightSlider.Value);
                IsSettingsDirty = false;
            }

            logger.Debug("Save settings end");
        }

        #endregion

        #region PropertyChangingFire

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}