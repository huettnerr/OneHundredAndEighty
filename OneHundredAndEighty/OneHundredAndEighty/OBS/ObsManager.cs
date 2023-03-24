using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace OneHundredAndEighty.OBS
{
    public static class ObsManager
    {
        private readonly static string server = "ws://192.168.0.23:4455";
        private readonly static string password = "GVq4uqNEHnH1V6y6";
        private readonly static string BOARDVIEW_NAME = "COMBINE_BOARD";

        private static bool _isConnected = false;
        private static OBSWebsocket obs;
        private static Timer _reconnectTimer;

        private static string currentView = "";

        #region Connectivity stuff

        public static void Init()
        {
            obs = new OBSWebsocket();
            obs.Connected += onConnect;
            obs.Disconnected += onDisconnect;

            _reconnectTimer = new Timer(5000);
            _reconnectTimer.Elapsed += reconnectTimer_Tick;
            _reconnectTimer.Enabled = true;
            _reconnectTimer.Stop();

            connect();
        }

        private static void connect()
        {
            if (!obs.IsConnected)
            {
                Console.WriteLine("Try establish connection to OBS");
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        obs.ConnectAsync(server, password);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Connect failed : " + ex.Message, "Error");
                        _reconnectTimer?.Start();
                        return;
                    }
                });
            }
        }

        private static void reconnectTimer_Tick(object sender, EventArgs e)
        {
            if(!obs.IsConnected)
            {
                connect();
            }
            else
            {
                _reconnectTimer?.Stop();
            }
        }

        private static void onConnect(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to OBS");
            _isConnected = true;
            _reconnectTimer?.Stop();

            NormalView();
        }

        private static void onDisconnect(object sender, OBSWebsocketDotNet.Communication.ObsDisconnectionInfo e)
        {
            Console.WriteLine("Disconnected from OBS");

            _isConnected = false;
            _reconnectTimer?.Start();
        }

        #endregion

        #region Boardview

        public static void NormalView()
        {
            if(!currentView.Equals("Normal"))
            {
                ChangeBoardView("Normal");
            }
        }

        public static void ChangeBoardView(string boardViewCode)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //deactivate all current actives
            List<int> activeViews = getActiveBoardViewItemIds();
            foreach (int viewId in activeViews)
            {
                obs.SetSceneItemEnabled(BOARDVIEW_NAME, viewId, false);
            }

            //activate the requested view
            int itemId = obs.GetSceneItemId(BOARDVIEW_NAME, $"S_Board{boardViewCode}", 0);
            obs.SetSceneItemEnabled(BOARDVIEW_NAME, itemId, true);
            currentView = boardViewCode;

            sw.Stop();
            Console.WriteLine($"View changed in {sw.ElapsedMilliseconds} ms");
        }

        private static List<int> getActiveBoardViewItemIds()
        {
            List<int> activeIds = new List<int>();
            var sources = new List<SceneItemDetails>();
            sources.AddRange(obs.GetSceneItemList(BOARDVIEW_NAME));

            foreach (var source in sources) 
            {
                if(obs.GetSceneItemEnabled(BOARDVIEW_NAME, source.ItemId))
                {
                    activeIds.Add(source.ItemId);
                }
            }

            return activeIds;
        }

        #endregion

        public static void GameShot()
        {
            ChangeBoardView("Normal");
        }
    }
}
