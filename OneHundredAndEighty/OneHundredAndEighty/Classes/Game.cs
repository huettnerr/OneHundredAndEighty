#region Usings

using OneHundredAndEighty.Controls;
using OneHundredAndEighty.Score;
using System.Collections.Generic;
using System.Text;

#endregion

namespace OneHundredAndEighty
{
    public class Game
    {
        private readonly MainWindow mainWindow = (MainWindow) System.Windows.Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам
        private readonly InfoPanelLogic infoPanelLogic = new InfoPanelLogic(); //  Инфо-панель
        private readonly SettingsPanelLogic settingsPanelLogic = new SettingsPanelLogic(); //  Панель настроек матча
        public readonly StatisticsWindowLogic statisticsWindowLogic = new StatisticsWindowLogic(); //  Окно статистики
        public readonly ScoreViewModel scoreVM = new ScoreViewModel(); //  Окно статистики
        public bool IsOn { get; private set; } //  Флаг работы матча
        private Player player1; //  Игрок 1
        private Player player2; //  Игрок 2
        private Player playerOnThrow; //  Чей подход
        private Player playerOnLeg; //  Кто начинает лег
        private readonly Stack<Throw> allMatchThrows = new Stack<Throw>(); //  Коллекция бросков матча
        private readonly Stack<SavePoint> savePoints = new Stack<SavePoint>(); //  Коллекция точек сохранения игры
        private int pointsToGo; //  Очков в леге
        private int legsToGo; //  Легов в сете
        private int setsToGo; //  Сетов в матче

        public void StartGame() //  Начало нового матча
        {
            IsOn = true; //  Флаг матча
            //  Панели
            mainWindow.PlayerTab.IsEnabled = false;
            settingsPanelLogic.PanelHide(); //  Прячем панель настроек
            infoPanelLogic.PanelShow(); //  Показываем инфо-панель
            BoardPanelLogic.PanelShow(); //  Показываем панель секторов
            PlayerOverview.ClearPanel(); //  Очищаем панель данных игроков
            //  Настройка матча
            pointsToGo = settingsPanelLogic.PointsToGo(); //  Получаем количество очков лега
            setsToGo = settingsPanelLogic.SetsToGo(); //  Получаем количество легов сета
            legsToGo = settingsPanelLogic.LegsToGo(); //  Получаем количество сетов матча
            //  Игроки
            player1 = new Player("Player1", (int) mainWindow.SettingsControl.Player1NameCombobox.SelectedValue, settingsPanelLogic.Player1Name(), pointsToGo); //  Игрок 1
            player2 = new Player("Player2", (int) mainWindow.SettingsControl.Player2NameCombobox.SelectedValue, settingsPanelLogic.Player2Name(), pointsToGo); //  Игрок 2
            playerOnThrow = settingsPanelLogic.WhoThrowFirst(player1, player2); //  Кто первый бросает
            playerOnLeg = playerOnThrow; //  Чей первый лег
            //  Инфо-панель
            scoreVM.NewGame(pointsToGo, legsToGo, setsToGo, player1, player2, playerOnThrow); //  Новая инфопанель
            scoreVM.HelpCheck(player1); //  Проверка помощи
            scoreVM.HelpCheck(player2); //  Проверка помощи
            //  Текстовая панель
            infoPanelLogic.TextLogAdd(new StringBuilder()
                                      .Append("First to ")
                                      .Append(setsToGo)
                                      .Append(" sets in ")
                                      .Append(legsToGo)
                                      .Append(" legs with ")
                                      .Append(pointsToGo)
                                      .Append(" points").ToString());
            infoPanelLogic.TextLogAdd("Game on");
            infoPanelLogic.TextLogAdd(new StringBuilder().Append(playerOnThrow.Name).Append(" on throw:").ToString());
        }

        private void EndGame() //  Конец матча
        {
            IsOn = false; //  Флаг матча
            //  Сообщение
            WinnerWindowLogic.ShowWinner(playerOnThrow, player1, player2, allMatchThrows); //  Показываем окно победителя и статистику
            //  Панели
            mainWindow.PlayerTab.IsEnabled = true;
            infoPanelLogic.PanelHide(); //  Прячем инфопанель
            BoardPanelLogic.PanelHide(); //  Прячем панель секторов
            settingsPanelLogic.PanelShow(); //  Показываем панель настроек
            //  Сохранение в БД
            DBwork.AfterMatchSave(statisticsWindowLogic);
            DBwork.UpdateAchieves(statisticsWindowLogic);
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
            infoPanelLogic.TextLogClear(); //  Очищаем текстовую панель
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
            infoPanelLogic.TextLogAdd(new StringBuilder().Append(playerOnThrow.Name).Append(" on throw:").ToString()); //  Пишем в текстовую панель
        }

        private void TogglePlayerOnThrow() //  Смена игрока на подходе
        {
            switch (playerOnThrow.Tag)
            {
                //  Меняем игрока
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
                //  Меняем игрока
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
            infoPanelLogic.TextLogClear(); //  Очищаем текстовую панель
            infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
            allMatchThrows.Clear(); //  Зануляем коллекцию бросков матча
            savePoints.Clear(); //  Зануляем точки сохнанения
        }

        public void NextThrow(Throw thrw) //  Очередной бросок
        {
            SavePoint(); //  Сохраняем точку игры перед броском
            infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

            infoPanelLogic.TextLogAdd(new StringBuilder().Append("    > ").Append(playerOnThrow.Name).Append(" throws ").Append(thrw.Points).ToString()); //  Пишем в текстовую панель

            thrw.WhoThrow = playerOnThrow.Tag; //  Записываем в бросок кто его бросил
            playerOnThrow.throws[playerOnThrow.ThrowCount] = thrw;
            thrw.HandNumber = playerOnThrow.ThrowCount;

            if(IsLegIsOver(ref thrw) )
            {
                calculatePoints(ref playerOnThrow, ref thrw);
                scoreVM.CountThrow(ref playerOnThrow, ref thrw);
                allMatchThrows.Push(thrw); //  Записываем в последный бросок в коллекцию матча

                if (IsOn) //  Если игра продолжается
                {
                    ClearHands(); //  Очищаем броски
                    TogglePlayerOnLeg(); //  Смена игрока на начало лега 
                    player1.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 1
                    player2.pointsToOut = pointsToGo; //  Обновляем очки нового лега игрока 2
                    scoreVM.ClearScores(pointsToGo); //  Обновляем инфопанель
                    scoreVM.HelpCheck(player1); //  Проверка помощи
                    scoreVM.HelpCheck(player2); //  Проверка помощи
                }
                return;
            }

            calculatePoints(ref playerOnThrow, ref thrw);
            scoreVM.CountThrow(ref playerOnThrow, ref thrw);
            allMatchThrows.Push(thrw); //  Записываем в последный бросок в коллекцию матча

            //If leg not over check if turn is over
            if (playerOnThrow.ThrowsLeft == 0 || thrw.IsFault)
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
        }

        public void UndoThrow() //  Отмена последнего броска
        {
            if (allMatchThrows.Count != 0) //  Проверяем возможность отмены броска, если коллекция бросков матча не пуста
            {
                infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска

                player1.setsWon = savePoints.Peek().Player1SetsWon; //  Восстанавливаем Игроку 1 выигранные сеты
                player1.legsWon = savePoints.Peek().Player1LegsWon; //  Восстанавливаем Игроку 1 выигранные леги
                player1.pointsToOut = savePoints.Peek().Player1PointsToOut; //  Восстанавливаем Игроку 1 очки на завершение лега
                player2.setsWon = savePoints.Peek().Player2SetsWon; //  Восстанавливаем Игроку 2 выигранные сеты
                player2.legsWon = savePoints.Peek().Player2LegsWon; //  Восстанавливаем Игроку 2 выигранные леги
                player2.pointsToOut = savePoints.Peek().Player2PointsToOut; //  Восстанавливаем Игроку 2 очки на завершение лега
                scoreVM.SetsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 1
                scoreVM.LegsSet(player1); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 1
                scoreVM.PointsSet(player1); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 1
                scoreVM.SetsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных сетов Игрока 2
                scoreVM.LegsSet(player2); //  Восстанавливаем в инфо-панели очки выигранных легов Игрока 2
                scoreVM.PointsSet(player2); //  Восстанавливаем в инфо-панели очки на завершение лега Игрока 2
                playerOnThrow = savePoints.Peek().PlayerOnThrow; //  Восстанавливаем игрока на броске
                playerOnLeg = savePoints.Peek().PlayerOnLeg; //  Восстанавливаем игрока на начало лега
                infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели

                if (allMatchThrows.Peek().IsLegWon || allMatchThrows.Peek().IsFault || allMatchThrows.Peek().HandNumber == 3) //  Если последний бросок был переходным
                {
                    infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    if (allMatchThrows.Peek().IsLegWon) // Если отменяемым броском выигран лег
                    {
                        scoreVM.DotSet(playerOnThrow); //  Перемещаем точку начала лега
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }

                    if (allMatchThrows.Peek().IsSetWon) // Если отменяемым броском выигран и сет
                    {
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }

                    if (allMatchThrows.Peek().IsFault) //  Если отменяемый бросок был штрафным
                    {
                        infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                    }
                }

                SetPlayerOnThrow(playerOnThrow); //  Восстанавливаем игрока на подходе
                infoPanelLogic.TextLogUndo(); //  Удаяем строку текстовой панели
                playerOnThrow.throws[0] = savePoints.Peek().FirstThrow; //  Восстанавливаем игроку на броске первый бросок
                playerOnThrow.throws[1] = savePoints.Peek().SecondThrow; //  Восстанавливаем игроку на броске второй бросок
                playerOnThrow.throws[2] = savePoints.Peek().ThirdThrow; //  Восстанавливаем игроку на броске третий бросок
                playerOnThrow.handPoints = savePoints.Peek().PlayerOnThrowHand; //  Восстанавливаем игроку на броске очки подхода
                scoreVM.HelpCheck(playerOnThrow); //  Проверяем помощь

                allMatchThrows.Pop(); //  Удалаяем последний бросок из коллекции матча
                savePoints.Pop(); //  Удаляем последнюю точку сохранения

                infoPanelLogic.UndoThrowButtonOn(); //  Включаем кнопку отмены броска

                if (allMatchThrows.Count == 0) //  Если бросков для отмены больше нет
                {
                    infoPanelLogic.UndoThrowButtonOff(); //  Выключаем кнопку отмены броска
                }
            }
        }

        private void SavePoint()
        {
            savePoints.Push(new SavePoint(player1, player2, playerOnThrow, playerOnLeg));
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
                infoPanelLogic.TextLogAdd(new StringBuilder().Append("Leg goes to ").Append(playerOnThrow.Name).ToString()); //  Пишем в текстовую панель
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
                infoPanelLogic.TextLogAdd(new StringBuilder().Append("Set goes to ").Append(playerOnThrow.Name).ToString()); //  Пишем в текстовую панель
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
                EndGame(); //  Матч окончен
            }
        }
    }
}