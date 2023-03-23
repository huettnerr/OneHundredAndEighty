#region Usings

using System.Windows.Controls;

#endregion

namespace OneHundredAndEighty
{
    public class Player //  Класс игрока
    {
        public string Name { get; } //  Имя игрока
        public string Tag { get; } //  Тэг

        public int DbId { get; } //  Id игрока в БД

        //Матч
        public int setsWon; //  Количество выигранных сетов
        public int legsWon; //  Количество выигранных легов в сете
        public int pointsToOut; //  Количество очков на завершение лега
        public int handPoints; //  Набранное количестов очков в подходе
        public int _180; //  Количество 180 в матче
        public bool ismrZ; //  Ачивка 
        public bool is3Bull; //  Ачивка 
        public Throw throw1; //  Первый бросок
        public Throw throw2; //  Второй бросок
        public Throw throw3; //  Третий бросок

        public int ThrowsLeft { 
            get
            {
                if (throw1 == null) return 3;
                else if (throw2 == null && throw1 != null) return 2;
                else if (throw3 == null && throw2 != null) return 1;
                else return 0;
            } 
        }

        public void ClearHand() //  Обнуление очередного подхода
        {
            handPoints = 0;
            throw1 = null;
            throw2 = null;
            throw3 = null;
        }

        //  Инфо-панель
        //public Grid helpPanel { get; private set; } //  Панель помощи
        //public Label helpLabel { get; private set; } //  Лейбл помощи
        //public Label setsWonLabel { get; private set; } //  Лейбл выиграных сетов
        //public Label legsWonLabel { get; private set; } //  Лейбл выиграных легов
        //public Label pointsLabel { get; private set; } //  Лейбл набраных очнов

        public Player(string tag, int id, string name, 
            //Grid helpPanel, Label helpLabel, Label setsWonLabel, Label legsWonLabel, Label pointsLabel, 
            int pointsToOut) //  Конструктор нового игрока
        {
            Tag = tag;
            DbId = id;
            Name = name;
            //this.helpPanel = helpPanel;
            //this.helpLabel = helpLabel;
            //this.setsWonLabel = setsWonLabel;
            //this.legsWonLabel = legsWonLabel;
            //this.pointsLabel = pointsLabel;
            this.pointsToOut = pointsToOut;
        }
    }
}