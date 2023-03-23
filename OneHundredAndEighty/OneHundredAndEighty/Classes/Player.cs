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
        public bool IsTurnFault { get; set; } //  Был ли бросок штрафным
        //public Throw throw1; //  Первый бросок
        //public Throw throw2; //  Второй бросок
        //public Throw throw3; //  Третий бросок

        public Throw[] throws;



        public int ThrowCount
        {
            get
            {
                if (throws[0] == null) return 0;
                else if (throws[1] == null && throws[0] != null) return 1;
                else if (throws[2] == null && throws[1] != null) return 2;
                else return 3;
            }
        }
        public int ThrowsLeft { get => 3 - ThrowCount; }

        public void ClearHand() //  Обнуление очередного подхода
        {
            handPoints = 0;
            throws = new Throw[3];
        }

        public Player(string tag, int id, string name, int pointsToOut) //  Конструктор нового игрока
        {
            Tag = tag;
            DbId = id;
            Name = name;
            this.pointsToOut = pointsToOut;

            ClearHand();
        }
    }
}