using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Score
{
    public class Finish
    {
        public int ScoreLeft;
        public string HelpText;
        public int MinThrows;
        public string ObsScene;

        public Finish(int scoreLeft, string helpText, int minThrows, string obsScene = "")
        {
            this.ScoreLeft = scoreLeft;
            this.HelpText = helpText;
            this.MinThrows = minThrows;
            this.ObsScene = obsScene;
        }

        public Finish(string helpText, int minThrows, string obsScene = "")
        {
            this.ScoreLeft = 0;
            this.HelpText = helpText;
            this.MinThrows = minThrows;
            this.ObsScene = obsScene;
        }
    }

    public static class FinishHelper
    {
        #region List

        private static readonly List<Finish> checkoutTable = new List<Finish>()
        {
            new Finish(2, "D1", 1, "D1/5/20"),
            new Finish(4, "D2", 1, "D2/15/17"),
            new Finish(6, "D3", 1, "D3/7/19"),
            new Finish(8, "D4", 1, "D4/18"),
            new Finish(10, "D5", 1, "D1/5/20"),
            new Finish(12, "D6", 1, "D6/10/13"),
            new Finish(14, "D7", 1, "D3/7/19"),
            new Finish(16, "D8", 1, "D8/16"),
            new Finish(18, "D9", 1, "D9/12"),
            new Finish(20, "D10", 1, "D6/10/13"),
            new Finish(22, "D11", 1, "D11/14"),
            new Finish(24, "D12", 1, "D9/12"),
            new Finish(26, "D13", 1, "D6/10/13"),
            new Finish(28, "D14", 1, "D11/14"),
            new Finish(30, "D15", 1, "D2/15/17"),
            new Finish(32, "D16", 1, "D8/16"),
            new Finish(34, "D17", 1, "D2/15/17"),
            new Finish(36, "D18", 1, "D4/18"),
            new Finish(38, "D19", 1, "D3/7/19"),
            new Finish(40, "D20", 1, "D1/5/20"),
            new Finish(3, "1 D1", 2),
            new Finish(5, "1 D2", 2),
            new Finish(7, "3 D2", 2),
            new Finish(9, "1 D4", 2),
            new Finish(11, "3 D4", 2),
            new Finish(13, "5 D4", 2),
            new Finish(15, "7 D4", 2),
            new Finish(17, "9 D4", 2),
            new Finish(19, "3 D8", 2),
            new Finish(21, "5 D8", 2),
            new Finish(23, "7 D8", 2),
            new Finish(25, "1 D12", 2),
            new Finish(27, "3 D12", 2),
            new Finish(29, "5 D12", 2),
            new Finish(31, "7 D12", 2),
            new Finish(33, "1 D16", 2),
            new Finish(35, "3 D16", 2),
            new Finish(37, "5 D16", 2),
            new Finish(39, "7 D6", 2),
            new Finish(41, "9 D16", 2),
            new Finish(42, "10 D16", 2),
            new Finish(43, "3 D20", 2),
            new Finish(44, "4 D20", 2),
            new Finish(45, "13 D16", 2),
            new Finish(46, "6 D20", 2),
            new Finish(47, "7 D20", 2),
            new Finish(48, "16 D16", 2),
            new Finish(49, "17 D16", 2),
            new Finish(50, "Bull", 1, "Bull"),
            new Finish(50, "18 D16", 2),
            new Finish(51, "19 D16", 2),
            new Finish(52, "20 D16", 2),
            new Finish(53, "13 D20", 2),
            new Finish(54, "14 D20", 2),
            new Finish(55, "15 D20", 2),
            new Finish(56, "16 D20", 2),
            new Finish(57, "17 D20", 2),
            new Finish(58, "18 D20", 2),
            new Finish(59, "19 D20", 2),
            new Finish(60, "20 D20", 2),
            new Finish(61, "T15 D8", 2),
            new Finish(62, "T10 D16", 2),
            new Finish(63, "T13 D12", 2),
            new Finish(64, "T16 D8", 2),
            new Finish(65, "T19 D4", 2),
            new Finish(66, "T14 D12", 2),
            new Finish(67, "T17 D8", 2),
            new Finish(68, "T20 D4", 2),
            new Finish(69, "T19 D6", 2),
            new Finish(70, "T18 D8", 2),
            new Finish(71, "T13 16", 2),
            new Finish(72, "T16 D12", 2),
            new Finish(73, "T19 D8", 2),
            new Finish(74, "T14 D16", 2),
            new Finish(75, "T17 D12", 2),
            new Finish(76, "T20 D8", 2),
            new Finish(77, "T19 D10", 2),
            new Finish(78, "T18 D12", 2),
            new Finish(79, "T19 D11", 2),
            new Finish(80, "T20 D10", 2),
            new Finish(81, "T19 D12", 2),
            new Finish(82, "Bull D16", 2),
            new Finish(82, "T14 D20", 3),
            new Finish(83, "T17 D16", 2),
            new Finish(84, "T20 D12", 2),
            new Finish(85, "T15 D20", 2),
            new Finish(86, "T18 D18", 2),
            new Finish(87, "T17 D18", 2),
            new Finish(88, "T20 D14", 2),
            new Finish(89, "T19 D16", 2),
            new Finish(90, "T20 D15", 2),
            new Finish(91, "T17 D20", 2),
            new Finish(92, "T20 D16", 2),
            new Finish(93, "T19 D18", 2),
            new Finish(94, "T18 D20", 2),
            new Finish(95, "T19 D19", 2),
            new Finish(96, "T20 D18", 2),
            new Finish(97, "T19 D20", 2),
            new Finish(98, "T20 D19", 2),
            new Finish(100, "T20 D20", 2),
            new Finish(99, "T19 10 D16", 3),
            new Finish(101, "T17 Bull", 2),
            new Finish(101, "T20 9 D16", 3),
            new Finish(102, "T16 14 D20", 3),
            new Finish(103, "T19 6 D20", 3),
            new Finish(104, "T18 Bull", 2),
            new Finish(104, "T16 16 D20", 3),
            new Finish(105, "T20 13 D16", 3),
            new Finish(106, "T20 6 D20", 3),
            new Finish(107, "T19 Bull", 2),
            new Finish(107, "T19 10 D20", 3),
            new Finish(108, "T20 16 D16", 3),
            new Finish(109, "T20 17 D16", 3),
            new Finish(110, "T20 Bull", 2),
            new Finish(110, "T20 10 D20", 3),
            new Finish(111, "T19 14 D20", 3),
            new Finish(112, "T20 20 D16", 3),
            new Finish(113, "T19 16 D20", 3),
            new Finish(114, "T20 14 D20", 3),
            new Finish(115, "T20 15 D20", 3),
            new Finish(116, "T20 16 D20", 3),
            new Finish(117, "T20 17 D20", 3),
            new Finish(118, "T20 18 D20", 3),
            new Finish(119, "T19 12 Bull", 3),
            new Finish(120, "T20 20 D20", 3),
            new Finish(121, "T20 11 Bull", 3),
            new Finish(122, "T18 18 Bull", 3),
            new Finish(123, "T19 16 Bull", 3),
            new Finish(124, "T20 14 Bull", 3),
            new Finish(125, "25 T20 D20", 3),
            new Finish(126, "T19 19 Bull", 3),
            new Finish(127, "T20 17 Bull", 3),
            new Finish(128, "18 T20 Bull", 3),
            new Finish(129, "19 T20 Bull", 3),
            new Finish(130, "T20 20 Bull", 3),
            new Finish(131, "T20 T13 D16", 3),
            new Finish(132, "25 T19 Bull", 3),
            new Finish(133, "T20 T19 D8", 3),
            new Finish(134, "T20 T14 D16", 3),
            new Finish(135, "25 T20 Bull", 3),
            new Finish(136, "T20 T20 D8", 3),
            new Finish(137, "T20 T19 D10", 3),
            new Finish(138, "T20 T18 D12", 3),
            new Finish(139, "T19 T14 D20", 3),
            new Finish(140, "T20 T20 D10", 3),
            new Finish(141, "T20 T19 D12", 3),
            new Finish(142, "T20 T14 D20", 3),
            new Finish(143, "T20 T17 D16", 3),
            new Finish(144, "T20 T20 D12", 3),
            new Finish(145, "T20 T15 D20", 3),
            new Finish(146, "T20 T18 D16", 3),
            new Finish(147, "T20 T17 D18", 3),
            new Finish(148, "T20 T20 D14", 3),
            new Finish(149, "T20 T19 D16", 3),
            new Finish(150, "T20 T18 D18", 3),
            new Finish(151, "T20 T17 D20", 3),
            new Finish(152, "T20 T20 D16", 3),
            new Finish(153, "T20 T19 D18", 3),
            new Finish(154, "T20 T18 D20", 3),
            new Finish(155, "T20 T19 D19", 3),
            new Finish(156, "T20 T20 D18", 3),
            new Finish(157, "T20 T19 D20", 3),
            new Finish(158, "T20 T20 D19", 3),
            new Finish(160, "T20 T20 D20", 3),
            new Finish(161, "T20 T17 Bull", 3),
            new Finish(164, "T20 T18 Bull", 3),
            new Finish(167, "T20 T19 Bull", 3),
            new Finish(170, "T20 T20 Bull", 3),
        };

        #endregion

        private static readonly ILookup<int, Finish> checkouts = checkoutTable.ToLookup(f => f.ScoreLeft);

        public static IEnumerable<Finish> GetFinishes(int points, int throwsLeft)
        {
            return checkouts[points].Where(co => co.MinThrows <= throwsLeft);
        }

        public static Finish GetBestFinish(int points, int throwsLeft)
        {
            IEnumerable<Finish> finishes = GetFinishes(points, throwsLeft).OrderByDescending(f => f.MinThrows);
            return finishes.FirstOrDefault();
        }
    }
}
