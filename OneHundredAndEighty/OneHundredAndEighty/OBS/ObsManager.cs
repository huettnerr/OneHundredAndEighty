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
    public enum ObsReplaySource { Player, Board, None }

    public static class ObsManager
    {
        private readonly static string server = "ws://192.168.0.23:4455";
        private readonly static string password = "GVq4uqNEHnH1V6y6";

        private readonly static string SCENENAME_SPLITSCREEN = "Live - Splitscreen";
        private readonly static string SCENENAME_BOARD = "Live Board Only";
        private readonly static string SCENENAME_PLAYER = "Live - PlayerCam";
        private readonly static string SCENENAME_REPLAY = "Live - Instant Replay";

        private readonly static string BOARDVIEW_SELECTOR_SCENE = "COMBINE_BOARD";
        private readonly static string REPLAY_SELECTOR_SCENE = "REPLAYS";

        private readonly static Tuple<KeyModifier, OBSHotkey> PlayerReplay_Save = new Tuple<KeyModifier, OBSHotkey>(KeyModifier.Control, OBSHotkey.OBS_KEY_0x30);
        private readonly static Tuple<KeyModifier, OBSHotkey> PlayerReplay_Replay = new Tuple<KeyModifier, OBSHotkey>(KeyModifier.Control, OBSHotkey.OBS_KEY_0x31);
        private readonly static Tuple<KeyModifier, OBSHotkey> BoardReplay_Save = new Tuple<KeyModifier, OBSHotkey>(KeyModifier.Control, OBSHotkey.OBS_KEY_0x32);
        private readonly static Tuple<KeyModifier, OBSHotkey> BoardReplay_Replay = new Tuple<KeyModifier, OBSHotkey>(KeyModifier.Control, OBSHotkey.OBS_KEY_0x33);

        private static bool isConnected = false;
        private static OBSWebsocket obs;
        private static Timer reconnectTimer;

        private static string currentView = "";
        private static bool viewChangeBlocked = false;

        #region Connectivity stuff

        public static void Init()
        {
            obs = new OBSWebsocket();
            obs.Connected += onConnect;
            obs.Disconnected += onDisconnect;

            reconnectTimer = new Timer(5000);
            reconnectTimer.Elapsed += reconnectTimer_Tick;
            reconnectTimer.Enabled = true;
            reconnectTimer.Stop();

            connect();
        }

        public static void Close()
        {
            obs.Disconnect();
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
                        reconnectTimer?.Start();
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
                reconnectTimer?.Stop();
            }
        }

        private static void onConnect(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to OBS");
            isConnected = true;
            reconnectTimer?.Stop();

            NormalBoardView();
            ChangeScene(SCENENAME_SPLITSCREEN);
        }

        private static void onDisconnect(object sender, OBSWebsocketDotNet.Communication.ObsDisconnectionInfo e)
        {
            Console.WriteLine("Disconnected from OBS");

            isConnected = false;
            reconnectTimer?.Start();
        }

        #endregion

        #region Boardview

        public static void NormalBoardView()
        {
            if(!currentView.Equals("Normal"))
            {
                ChangeBoardView("Normal");
            }
        }

        public static void ChangeBoardView(string boardViewCode)
        {
            if (!isConnected || viewChangeBlocked) return;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            //deactivate all current actives
            disableAllSceneItems(BOARDVIEW_SELECTOR_SCENE);

            //activate the requested view
            enableSceneItem(BOARDVIEW_SELECTOR_SCENE, $"S_Board{boardViewCode}");
            currentView = boardViewCode;

            sw.Stop();
            Console.WriteLine($"View changed to {boardViewCode} in {sw.ElapsedMilliseconds} ms");
        }

        #endregion

        public static void ChangeScene(string scene)
        {
            if (!isConnected || viewChangeBlocked) return;

            obs.SetCurrentProgramScene(scene);
        }

        public static void GameShot(ObsReplaySource replaySource)
        {
            blockViewChange(3000, "Normal");

            disableAllSceneItems(REPLAY_SELECTOR_SCENE);
            switch(replaySource)
            {
                case ObsReplaySource.Player:
                    enableSceneItem(REPLAY_SELECTOR_SCENE, "PlayerReplaySource");
                    triggerHotkey(PlayerReplay_Save);
                    Task.Delay(100).Wait();
                    triggerHotkey(PlayerReplay_Replay);
                    break;
                case ObsReplaySource.Board:
                    enableSceneItem(REPLAY_SELECTOR_SCENE, "BoardReplaySource");
                    triggerHotkey(BoardReplay_Save);
                    Task.Delay(100).Wait();
                    triggerHotkey(BoardReplay_Replay);
                    break;
                case ObsReplaySource.None:
                default:
                    return;
            }

            Task.Delay(100).Wait();
            obs.SetCurrentProgramScene(SCENENAME_REPLAY);
        }

        private static void triggerHotkey(Tuple<KeyModifier, OBSHotkey> Combination)
        {
            obs.TriggerHotkeyByKeySequence(Combination.Item2, Combination.Item1);
        }

        private static void disableAllSceneItems(string scene)
        {
            List<int> activeViews = getActiveSceneViewItemIds(scene);
            foreach (int viewId in activeViews)
            {
                obs.SetSceneItemEnabled(scene, viewId, false);
            }
        }

        private static void enableSceneItem(string scene, string itemName)
        {
            int itemId = obs.GetSceneItemId(scene, itemName, 0);
            obs.SetSceneItemEnabled(scene, itemId, true);
        }

        private static List<int> getActiveSceneViewItemIds(string scene)
        {
            List<int> activeIds = new List<int>();
            var sources = new List<SceneItemDetails>();
            sources.AddRange(obs.GetSceneItemList(scene));

            foreach (var source in sources)
            {
                if (obs.GetSceneItemEnabled(scene, source.ItemId))
                {
                    activeIds.Add(source.ItemId);
                }
            }

            return activeIds;
        }

        private static void blockViewChange(int msDelay, string boardViewCodeAfterBlock = "", string sceneAfterBlock = "")
        {
            viewChangeBlocked = true;
            Task.Run(async () =>
            {
                await Task.Delay(msDelay);
                viewChangeBlocked = false;

                if (!boardViewCodeAfterBlock.Equals(String.Empty))
                {
                    ChangeBoardView(boardViewCodeAfterBlock);
                }

                if (!sceneAfterBlock.Equals(String.Empty))
                {
                    ChangeScene(sceneAfterBlock);
                }
            });
        }
    }
}
