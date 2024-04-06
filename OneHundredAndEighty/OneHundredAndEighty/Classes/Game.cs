#region Usings

using MyToolkit.Model;
using OneHundredAndEighty.Classes;
using OneHundredAndEighty.Controls;
using OneHundredAndEighty.OBS;
using OneHundredAndEighty.Score;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

#endregion

namespace OneHundredAndEighty
{
    public class Game : ObservableObject
    {
        private MainWindow mainWindow { get => (MainWindow)System.Windows.Application.Current.MainWindow; }  //  Ссылка на главное окно для доступа к элементам
        private readonly InfoPanelLogic infoPanelLogic = new InfoPanelLogic(); //  Инфо-панель
        private readonly SettingsPanelLogic settingsPanelLogic = new SettingsPanelLogic(); //  Панель настроек матча
        public readonly StatisticsWindowLogic statisticsWindowLogic = new StatisticsWindowLogic(); //  Окно статистики
        private ScoreViewModel scoreVM { get => ((App)Application.Current).ScoreVM; }

        public bool IsOn { get; private set; } //  Флаг работы матча
        private Player player1; //  Игрок 1
        private Player player2; //  Игрок 2
        private Player playerOnThrow; //  Чей подход
        private Player playerOnLeg; //  Кто начинает лег
        public ObservableStack<Throw> AllMatchThrows { get; } = new ObservableStack<Throw>(); //  Коллекция бросков матча
        private readonly Stack<SavePoint> savePoints = new Stack<SavePoint>(); //  Коллекция точек сохранения игры
        private readonly string backupFilePath = @"log\backup.txt";
        private int pointsToGo; //  Очков в леге
        private int legsToGo; //  Легов в сете
        private int setsToGo; //  Сетов в матче

        public Game()
        {
            settingsPanelLogic.PanelShow(); //  Прячем панель настроек
            infoPanelLogic.PanelHide(); //  Показываем инфо-панель
        }

        public void StartGame(int p1Id, int p2Id) //  Начало нового матча
        {
            // Global settings
            IsOn = true; //  Флаг матча
            mainWindow.PlayerTab.IsEnabled = false;
            settingsPanelLogic.PanelHide(); //  Прячем панель настроек
            infoPanelLogic.PanelShow(); //  Показываем инфо-панель
            BoardPanelLogic.PanelShow(); //  Показываем панель секторов
            PlayerOverview.ClearPanel(); //  Очищаем панель данных игроков

            //Set OBS 
            ObsManager.ChangeScene(ObsManager.SCENENAME_SPLITSCREEN);

            // initiate game mode
            pointsToGo = settingsPanelLogic.PointsToGo(); //  Получаем количество очков лега
            setsToGo = settingsPanelLogic.SetsToGo(); //  Получаем количество легов сета
            legsToGo = settingsPanelLogic.LegsToGo(); //  Получаем количество сетов матча

            // initiate players
            player1 = new Player("Player1", p1Id, settingsPanelLogic.Player1Name(), pointsToGo); //  Игрок 1
            player2 = new Player("Player2", p2Id, settingsPanelLogic.Player2Name(), pointsToGo); //  Игрок 2
            playerOnThrow = settingsPanelLogic.WhoThrowFirst(player1, player2); //  Кто первый бросает
            playerOnLeg = playerOnThrow; //  Чей первый лег

            // initiate scoring module
            scoreVM.NewGame(pointsToGo, legsToGo, setsToGo, player1, player2, playerOnThrow); //  Новая инфопанель
            scoreVM.HelpCheck(player1); //  Проверка помощи
            scoreVM.HelpCheck(player2); //  Проверка помощи
        }

        private void EndGame() //  Конец матча
        {
            //  Сообщение
            WinnerWindowLogic.ShowWinner(playerOnThrow, player1, player2, AllMatchThrows); //  Показываем окно победителя и статистику
            //  Панели
            mainWindow.PlayerTab.IsEnabled = true;
            infoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide(); //  Прячем панель секторов
            settingsPanelLogic.PanelShow(); //  Показываем панель настроек
            //  Сохранение в БД
            //DBwork.AfterMatchSave(statisticsWindowLogic);
            //DBwork.UpdateAchieves(statisticsWindowLogic);
            //  Обнуление коллекций
            ClearCollections(); //  Зануляем коллекции бросков
        }

        public void AbortGame() //  Отмена текущего матча
        {
            IsOn = false; //  Флаг матча
            ClearCollections(); //  Зануляем коллекции бросков
            //  Панели
            mainWindow.PlayerTab.IsEnabled = true;
            infoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide(); //  Прячем панель секторов
            settingsPanelLogic.PanelShow(); //  Показываем панель настроек
            //  Окно
            var window = new Windows.AbortWindow {Owner = mainWindow};
            window.ShowDialog(); //  Показываем окно отмены матча
        }

        private void SetPlayerOnThrow(Player player) //  Установка игрока на подходе
        {
            scoreVM.HelpCheck(playerOnThrow); //  Проверка помощи
            playerOnThrow = player; //  Устанавливаем игрока на броске
            scoreVM.WhoThrowSliderSet(playerOnThrow); //  Устанавливаем стайдер инфо-панели
            scoreVM.HelpCheck(playerOnThrow); //  Проверка помощи
        }

        private void TogglePlayerOnThrow() //  Смена игрока на подходе
        {
            switch (playerOnThrow.Tag)
            {
                case "Player1":
                    SetPlayerOnThrow(player2);
                    break;
                case "Player2":
                    SetPlayerOnThrow(player1);
                    break;
            }
        }

        private void TogglePlayerOnLeg() //  Смена игрока на начало лега
        {
            switch (playerOnLeg.Tag)
            {
                case "Player1":
                    playerOnLeg = player2;
                    break;
                case "Player2":
                    playerOnLeg = player1;
                    break;
            }

            SetPlayerOnThrow(playerOnLeg); //  Меняем игрока на подходе

            scoreVM.DotSet(playerOnThrow); //  Перемещаем точку
        }

        private void ClearHands() //  Очистка бросков игроков
        {
            player1.ClearHand();
            player2.ClearHand();
        }

        private void ClearCollections() //  Зануляем коллекции бросков
        {
            statisticsWindowLogic.ClearCollection(); //  Зануляем коллекцию статистики
            infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
            AllMatchThrows.Clear(); //  Зануляем коллекцию бросков матча
            savePoints.Clear(); //  Зануляем точки сохнанения
        }

        public void NextThrow(Throw thrw) //  Очередной бросок
        {
            SavePoint(); //  Сохраняем точку игры перед броском
            infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

            thrw.WhoThrowTag = playerOnThrow.Tag; //  Записываем в бросок кто его бросил
            thrw.WhoThrowName = playerOnThrow.Name; //  Записываем в бросок кто его бросил
            playerOnThrow.throws[playerOnThrow.ThrowCount] = thrw;
            thrw.HandNumber = playerOnThrow.ThrowCount;
            thrw.PointsToGo = playerOnThrow.pointsToOut;

            if (IsLegIsOver(ref thrw) )
            {
                calculatePoints(ref playerOnThrow, ref thrw);
                scoreVM.CountThrow(ref playerOnThrow, ref thrw);
                AllMatchThrows.PushObservable(thrw); //  Записываем в последный бросок в коллекцию матча

                if (IsOn) //  Если игра продолжается
                {
                    ObsManager.GameShot(ObsReplaySource.Player, false);
                    ClearHands(); //  Очищаем броски
                    TogglePlayerOnLeg(); //  Смена игрока на начало лега 
                    player1.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 1
                    player2.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 2
                    scoreVM.ClearScores(player1, pointsToGo); //  Обновляем инфопанель
                    scoreVM.ClearScores(player2, pointsToGo); //  Обновляем инфопанель
                    scoreVM.HelpCheck(player1); //  Проверка помощи
                    scoreVM.HelpCheck(player2); //  Проверка помощи
                }
                else
                {
                    ObsManager.GameShot(ObsReplaySource.Player, true);
                    EndGame(); //  Матч окончен
                }

                WriteBackup();
                return;
            }

            calculatePoints(ref playerOnThrow, ref thrw);
            scoreVM.CountThrow(ref playerOnThrow, ref thrw);
            AllMatchThrows.PushObservable(thrw); //  Записываем в последный бросок в коллекцию матча

            //If leg not over check if turn is over
            if (IsTurnThrow(thrw))
            {
                //Turn is over
                //infoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(playerOnThrow.Name).Append(" FAULT").ToString()); //  Пишем в текстовую панель

                ClearHands(); //  Очищаем броски
                TogglePlayerOnThrow(); //  Меняем игрока на броске
            }
            else
            {
                scoreVM.HelpCheck(playerOnThrow); //  Проверяем помощь
            }

            WriteBackup();
        }

        private void SavePoint()
        {
            savePoints.Push(new SavePoint(player1, player2, playerOnThrow, playerOnLeg));
        }

        private void WriteBackup()
        {
            if(!Directory.Exists(Path.GetDirectoryName(backupFilePath))) Directory.CreateDirectory(Path.GetDirectoryName(backupFilePath));
            //if(!File.Exists(backupFilePath)) File.Create(backupFilePath, );

            using(StreamWriter sw = new StreamWriter(backupFilePath, false))
            {
                sw.WriteLine($"{player1.Name} S{player1.setsWon} L{player1.legsWon} {player1.pointsToOut} - {player2.pointsToOut} L{player2.legsWon} S{player2.setsWon} {player2.Name} ({playerOnThrow.Name} on throw)");
            }
        }

        private void calculatePoints(ref Player p, ref Throw t)
        {
            //Calculate new Points
            p.pointsToOut -= (int)t.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            p.handPoints += (int)t.Points; //  Плюсуем набраные за подход очки игрока

            if (!t.IsLegWon && p.pointsToOut <= 1) //If check is not possible
            {
                t.IsFault = true; //  Помечаем бросок как штрафной 
                p.pointsToOut += p.handPoints; //  Отменяем подход игрока
            }
        }

        private bool IsLegIsOver(ref Throw t) //  Или штраф, или правильное окончание лега, или это был последний бросок в подходе 
        {
            if ((playerOnThrow.pointsToOut - t.Points) == 0 && (t.Multiplier.Equals("Double") || t.Multiplier.Equals("Bull_Eye"))) //  Игрок правильно закрыл лег
            {
                playerOnThrow.legsWon += 1; //  Плюсуем выиграный лег
                scoreVM.LegsSet(playerOnThrow); //  Обновляем инфопанель
                t.IsLegWon = true; //  Помечаем бросок как выигравший лег

                IsSetIsOver(ref t); //  Проверяем не закончен ли сет
                return true;
            }

            return false;
        }

        private void IsSetIsOver(ref Throw t) //  Проверка закончен ли сет
        {
            if (playerOnThrow.legsWon == legsToGo) //  Если игрок выиграл требуемое количество легов для окончания сета
            {
                playerOnThrow.setsWon += 1; //  Добавляем игроку выигранный сет
                scoreVM.SetsSet(playerOnThrow); //  Обновляем инфопанель
                player1.legsWon = 0; //  Обнуляем леги игроков
                player2.legsWon = 0; //  Обнуляем леги игроков
                scoreVM.LegsClear(); //  Обновляем инфопанель
                t.IsSetWon = true; //  Помечаем бросок как выигравший сет
                IsGameIsOver(ref t); //  Проверяем не закончен ли матч
            }
        }

        private void IsGameIsOver(ref Throw t) //  Проверка закончен ли матч
        {
            if (playerOnThrow.setsWon == setsToGo) //  Если игрок выиграл требуемое количество сетов для завершения матча
            {
                t.IsMatchWon = true; //  Помечаем бросок как выигравший матч
                IsOn = false; //  Флаг матча
            }
        }

        public static bool IsTurnThrow(Throw t)
        {
            return (t.HandNumber == 3 || t.IsFault || t.IsLegWon);
        }

        public void UndoThrow() //  Отмена последнего броска
        {
            if (AllMatchThrows.Count != 0) //  Проверяем возможность отмены броска, если коллекция бросков матча не пуста
            {
                infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска

                SavePoint sp = savePoints.Pop();
                player1.setsWon = sp.Player1SetsWon; //  Восстанавливаем Игроку 1 выигранные сеты
                player1.legsWon = sp.Player1LegsWon; //  Восстанавливаем Игроку 1 выигранные леги
                player1.pointsToOut = sp.Player1PointsToOut; //  Восстанавливаем Игроку 1 очки на завершение лега
                player2.setsWon = sp.Player2SetsWon; //  Восстанавливаем Игроку 2 выигранные сеты
                player2.legsWon = sp.Player2LegsWon; //  Восстанавливаем Игроку 2 выигранные леги
                player2.pointsToOut = sp.Player2PointsToOut; //  Восстанавливаем Игроку 2 очки на завершение лега
                scoreVM.SetsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 1
                scoreVM.LegsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 1
                //scoreVM.PointsSet(player1); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 1
                scoreVM.SetsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 2
                scoreVM.LegsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 2
                //scoreVM.PointsSet(player2); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 2
                playerOnThrow = sp.PlayerOnThrow; //  Восстанавливаем игрока на броске
                playerOnLeg = sp.PlayerOnLeg; //  Восстанавливаем игрока на начало лега
                scoreVM.UndoThrow(playerOnThrow);

                Throw t = AllMatchThrows.PopObservable(); //  Удалаяем последний бросок из коллекции матча
                if (t.IsLegWon || t.IsFault || t.HandNumber == 3) //  Если последний бросок был переходным
                {
                    if (t.IsLegWon) // Если отменяемым броском выигран лег
                    {
                        scoreVM.DotSet(playerOnThrow); //  Перемещаем точку начала лега
                    }
                }

                SetPlayerOnThrow(playerOnThrow); //  Восстанавливаем игрока на подходе
                playerOnThrow.throws[0] = sp.FirstThrow; //  Восстанавливаем игроку на броске первый бросок
                playerOnThrow.throws[1] = sp.SecondThrow; //  Восстанавливаем игроку на броске второй бросок
                playerOnThrow.throws[2] = sp.ThirdThrow; //  Восстанавливаем игроку на броске третий бросок
                playerOnThrow.handPoints = sp.PlayerOnThrowHand; //  Восстанавливаем игроку на броске очки подхода
                scoreVM.HelpCheck(playerOnThrow); //  Проверяем помощь

                infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

                if (AllMatchThrows.Count == 0) //  Если бросков для отмены больше нет
                {
                    infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
                }

                WriteBackup();
            }
        }

        public void AbortGameRequest()
        {
            ((MainWindow)Application.Current.MainWindow).FadeIn();
            var window = new Windows.AbortWindowConfirm { Owner = (MainWindow)Application.Current.MainWindow };
            window.ShowDialog();

            if (window.Result)
            {
                AbortGame();
            }

            ((MainWindow)Application.Current.MainWindow).FadeOut();
        }
    }
}