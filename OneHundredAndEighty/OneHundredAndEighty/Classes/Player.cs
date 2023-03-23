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

        public Player(string tag, int id, string name, int pointsToOut) //  Конструктор нового игрока
        {
            Tag = tag;
            DbId = id;
            Name = name;
            this.pointsToOut = pointsToOut;
        }
    }
}