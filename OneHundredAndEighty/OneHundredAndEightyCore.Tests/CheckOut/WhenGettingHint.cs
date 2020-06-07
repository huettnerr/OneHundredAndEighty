﻿#region Usings

using NUnit.Framework;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Tests.CheckOut
{
    public class WhenGettingHint
    {
        [TestCase(2, ThrowNumber.FirstThrow, ExpectedResult = "D1")]
        [TestCase(2, ThrowNumber.SecondThrow, ExpectedResult = "D1")]
        [TestCase(2, ThrowNumber.ThirdThrow, ExpectedResult = "D1")]
        [TestCase(3, ThrowNumber.FirstThrow, ExpectedResult = "1 D1")]
        [TestCase(3, ThrowNumber.SecondThrow, ExpectedResult = "1 D1")]
        [TestCase(3, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(4, ThrowNumber.FirstThrow, ExpectedResult = "D2")]
        [TestCase(4, ThrowNumber.SecondThrow, ExpectedResult = "D2")]
        [TestCase(4, ThrowNumber.ThirdThrow, ExpectedResult = "D2")]
        [TestCase(5, ThrowNumber.FirstThrow, ExpectedResult = "1 D2")]
        [TestCase(5, ThrowNumber.SecondThrow, ExpectedResult = "1 D2")]
        [TestCase(5, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(6, ThrowNumber.FirstThrow, ExpectedResult = "D3")]
        [TestCase(6, ThrowNumber.SecondThrow, ExpectedResult = "D3")]
        [TestCase(6, ThrowNumber.ThirdThrow, ExpectedResult = "D3")]
        [TestCase(7, ThrowNumber.FirstThrow, ExpectedResult = "3 D2")]
        [TestCase(7, ThrowNumber.SecondThrow, ExpectedResult = "3 D2")]
        [TestCase(7, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(8, ThrowNumber.FirstThrow, ExpectedResult = "D4")]
        [TestCase(8, ThrowNumber.SecondThrow, ExpectedResult = "D4")]
        [TestCase(8, ThrowNumber.ThirdThrow, ExpectedResult = "D4")]
        [TestCase(9, ThrowNumber.FirstThrow, ExpectedResult = "1 D4")]
        [TestCase(9, ThrowNumber.SecondThrow, ExpectedResult = "1 D4")]
        [TestCase(9, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(10, ThrowNumber.FirstThrow, ExpectedResult = "D5")]
        [TestCase(10, ThrowNumber.SecondThrow, ExpectedResult = "D5")]
        [TestCase(10, ThrowNumber.ThirdThrow, ExpectedResult = "D5")]
        [TestCase(11, ThrowNumber.FirstThrow, ExpectedResult = "3 D4")]
        [TestCase(11, ThrowNumber.SecondThrow, ExpectedResult = "3 D4")]
        [TestCase(11, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(12, ThrowNumber.FirstThrow, ExpectedResult = "D6")]
        [TestCase(12, ThrowNumber.SecondThrow, ExpectedResult = "D6")]
        [TestCase(12, ThrowNumber.ThirdThrow, ExpectedResult = "D6")]
        [TestCase(13, ThrowNumber.FirstThrow, ExpectedResult = "5 D4")]
        [TestCase(13, ThrowNumber.SecondThrow, ExpectedResult = "5 D4")]
        [TestCase(13, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(14, ThrowNumber.FirstThrow, ExpectedResult = "D7")]
        [TestCase(14, ThrowNumber.SecondThrow, ExpectedResult = "D7")]
        [TestCase(14, ThrowNumber.ThirdThrow, ExpectedResult = "D7")]
        [TestCase(15, ThrowNumber.FirstThrow, ExpectedResult = "7 D4")]
        [TestCase(15, ThrowNumber.SecondThrow, ExpectedResult = "7 D4")]
        [TestCase(15, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(16, ThrowNumber.FirstThrow, ExpectedResult = "D8")]
        [TestCase(16, ThrowNumber.SecondThrow, ExpectedResult = "D8")]
        [TestCase(16, ThrowNumber.ThirdThrow, ExpectedResult = "D8")]
        [TestCase(17, ThrowNumber.FirstThrow, ExpectedResult = "9 D4")]
        [TestCase(17, ThrowNumber.SecondThrow, ExpectedResult = "9 D4")]
        [TestCase(17, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(18, ThrowNumber.FirstThrow, ExpectedResult = "D9")]
        [TestCase(18, ThrowNumber.SecondThrow, ExpectedResult = "D9")]
        [TestCase(18, ThrowNumber.ThirdThrow, ExpectedResult = "D9")]
        [TestCase(19, ThrowNumber.FirstThrow, ExpectedResult = "3 D8")]
        [TestCase(19, ThrowNumber.SecondThrow, ExpectedResult = "3 D8")]
        [TestCase(19, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(20, ThrowNumber.FirstThrow, ExpectedResult = "D10")]
        [TestCase(20, ThrowNumber.SecondThrow, ExpectedResult = "D10")]
        [TestCase(20, ThrowNumber.ThirdThrow, ExpectedResult = "D10")]
        [TestCase(21, ThrowNumber.FirstThrow, ExpectedResult = "5 D8")]
        [TestCase(21, ThrowNumber.SecondThrow, ExpectedResult = "5 D8")]
        [TestCase(21, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(22, ThrowNumber.FirstThrow, ExpectedResult = "D11")]
        [TestCase(22, ThrowNumber.SecondThrow, ExpectedResult = "D11")]
        [TestCase(22, ThrowNumber.ThirdThrow, ExpectedResult = "D11")]
        [TestCase(23, ThrowNumber.FirstThrow, ExpectedResult = "7 D8")]
        [TestCase(23, ThrowNumber.SecondThrow, ExpectedResult = "7 D8")]
        [TestCase(23, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(24, ThrowNumber.FirstThrow, ExpectedResult = "D12")]
        [TestCase(24, ThrowNumber.SecondThrow, ExpectedResult = "D12")]
        [TestCase(24, ThrowNumber.ThirdThrow, ExpectedResult = "D12")]
        [TestCase(25, ThrowNumber.FirstThrow, ExpectedResult = "1 D12")]
        [TestCase(25, ThrowNumber.SecondThrow, ExpectedResult = "1 D12")]
        [TestCase(25, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(26, ThrowNumber.FirstThrow, ExpectedResult = "D13")]
        [TestCase(26, ThrowNumber.SecondThrow, ExpectedResult = "D13")]
        [TestCase(26, ThrowNumber.ThirdThrow, ExpectedResult = "D13")]
        [TestCase(27, ThrowNumber.FirstThrow, ExpectedResult = "3 D12")]
        [TestCase(27, ThrowNumber.SecondThrow, ExpectedResult = "3 D12")]
        [TestCase(27, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(28, ThrowNumber.FirstThrow, ExpectedResult = "D14")]
        [TestCase(28, ThrowNumber.SecondThrow, ExpectedResult = "D14")]
        [TestCase(28, ThrowNumber.ThirdThrow, ExpectedResult = "D14")]
        [TestCase(29, ThrowNumber.FirstThrow, ExpectedResult = "5 D12")]
        [TestCase(29, ThrowNumber.SecondThrow, ExpectedResult = "5 D12")]
        [TestCase(29, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(30, ThrowNumber.FirstThrow, ExpectedResult = "D15")]
        [TestCase(30, ThrowNumber.SecondThrow, ExpectedResult = "D15")]
        [TestCase(30, ThrowNumber.ThirdThrow, ExpectedResult = "D15")]
        [TestCase(31, ThrowNumber.FirstThrow, ExpectedResult = "7 D12")]
        [TestCase(31, ThrowNumber.SecondThrow, ExpectedResult = "7 D12")]
        [TestCase(31, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(32, ThrowNumber.FirstThrow, ExpectedResult = "D16")]
        [TestCase(32, ThrowNumber.SecondThrow, ExpectedResult = "D16")]
        [TestCase(32, ThrowNumber.ThirdThrow, ExpectedResult = "D16")]
        [TestCase(33, ThrowNumber.FirstThrow, ExpectedResult = "1 D16")]
        [TestCase(33, ThrowNumber.SecondThrow, ExpectedResult = "1 D16")]
        [TestCase(33, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(34, ThrowNumber.FirstThrow, ExpectedResult = "D17")]
        [TestCase(34, ThrowNumber.SecondThrow, ExpectedResult = "D17")]
        [TestCase(34, ThrowNumber.ThirdThrow, ExpectedResult = "D17")]
        [TestCase(35, ThrowNumber.FirstThrow, ExpectedResult = "3 D16")]
        [TestCase(35, ThrowNumber.SecondThrow, ExpectedResult = "3 D16")]
        [TestCase(35, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(36, ThrowNumber.FirstThrow, ExpectedResult = "D18")]
        [TestCase(36, ThrowNumber.SecondThrow, ExpectedResult = "D18")]
        [TestCase(36, ThrowNumber.ThirdThrow, ExpectedResult = "D18")]
        [TestCase(37, ThrowNumber.FirstThrow, ExpectedResult = "5 D16")]
        [TestCase(37, ThrowNumber.SecondThrow, ExpectedResult = "5 D16")]
        [TestCase(37, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(38, ThrowNumber.FirstThrow, ExpectedResult = "D19")]
        [TestCase(38, ThrowNumber.SecondThrow, ExpectedResult = "D19")]
        [TestCase(38, ThrowNumber.ThirdThrow, ExpectedResult = "D19")]
        [TestCase(39, ThrowNumber.FirstThrow, ExpectedResult = "7 D6")]
        [TestCase(39, ThrowNumber.SecondThrow, ExpectedResult = "7 D6")]
        [TestCase(39, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(40, ThrowNumber.FirstThrow, ExpectedResult = "D20")]
        [TestCase(40, ThrowNumber.SecondThrow, ExpectedResult = "D20")]
        [TestCase(40, ThrowNumber.ThirdThrow, ExpectedResult = "D20")]
        [TestCase(41, ThrowNumber.FirstThrow, ExpectedResult = "9 D16")]
        [TestCase(41, ThrowNumber.SecondThrow, ExpectedResult = "9 D16")]
        [TestCase(41, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(42, ThrowNumber.FirstThrow, ExpectedResult = "10 D16")]
        [TestCase(42, ThrowNumber.SecondThrow, ExpectedResult = "10 D16")]
        [TestCase(42, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(43, ThrowNumber.FirstThrow, ExpectedResult = "3 D20")]
        [TestCase(43, ThrowNumber.SecondThrow, ExpectedResult = "3 D20")]
        [TestCase(43, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(44, ThrowNumber.FirstThrow, ExpectedResult = "4 D20")]
        [TestCase(44, ThrowNumber.SecondThrow, ExpectedResult = "4 D20")]
        [TestCase(44, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(45, ThrowNumber.FirstThrow, ExpectedResult = "13 D16")]
        [TestCase(45, ThrowNumber.SecondThrow, ExpectedResult = "13 D16")]
        [TestCase(45, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(46, ThrowNumber.FirstThrow, ExpectedResult = "6 D20")]
        [TestCase(46, ThrowNumber.SecondThrow, ExpectedResult = "6 D20")]
        [TestCase(46, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(47, ThrowNumber.FirstThrow, ExpectedResult = "7 D20")]
        [TestCase(47, ThrowNumber.SecondThrow, ExpectedResult = "7 D20")]
        [TestCase(47, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(48, ThrowNumber.FirstThrow, ExpectedResult = "16 D16")]
        [TestCase(48, ThrowNumber.SecondThrow, ExpectedResult = "16 D16")]
        [TestCase(48, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(49, ThrowNumber.FirstThrow, ExpectedResult = "17 D16")]
        [TestCase(49, ThrowNumber.SecondThrow, ExpectedResult = "17 D16")]
        [TestCase(49, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(50, ThrowNumber.FirstThrow, ExpectedResult = "Bull")]
        [TestCase(50, ThrowNumber.SecondThrow, ExpectedResult = "Bull")]
        [TestCase(50, ThrowNumber.ThirdThrow, ExpectedResult = "Bull")]
        [TestCase(51, ThrowNumber.FirstThrow, ExpectedResult = "19 D16")]
        [TestCase(51, ThrowNumber.SecondThrow, ExpectedResult = "19 D16")]
        [TestCase(51, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(52, ThrowNumber.FirstThrow, ExpectedResult = "20 D16")]
        [TestCase(52, ThrowNumber.SecondThrow, ExpectedResult = "20 D16")]
        [TestCase(52, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(53, ThrowNumber.FirstThrow, ExpectedResult = "13 D20")]
        [TestCase(53, ThrowNumber.SecondThrow, ExpectedResult = "13 D20")]
        [TestCase(53, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(54, ThrowNumber.FirstThrow, ExpectedResult = "14 D20")]
        [TestCase(54, ThrowNumber.SecondThrow, ExpectedResult = "14 D20")]
        [TestCase(54, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(55, ThrowNumber.FirstThrow, ExpectedResult = "15 D20")]
        [TestCase(55, ThrowNumber.SecondThrow, ExpectedResult = "15 D20")]
        [TestCase(55, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(56, ThrowNumber.FirstThrow, ExpectedResult = "16 D20")]
        [TestCase(56, ThrowNumber.SecondThrow, ExpectedResult = "16 D20")]
        [TestCase(56, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(57, ThrowNumber.FirstThrow, ExpectedResult = "17 D20")]
        [TestCase(57, ThrowNumber.SecondThrow, ExpectedResult = "17 D20")]
        [TestCase(57, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(58, ThrowNumber.FirstThrow, ExpectedResult = "18 D20")]
        [TestCase(58, ThrowNumber.SecondThrow, ExpectedResult = "18 D20")]
        [TestCase(58, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(59, ThrowNumber.FirstThrow, ExpectedResult = "19 D20")]
        [TestCase(59, ThrowNumber.SecondThrow, ExpectedResult = "19 D20")]
        [TestCase(59, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(60, ThrowNumber.FirstThrow, ExpectedResult = "20 D20")]
        [TestCase(60, ThrowNumber.SecondThrow, ExpectedResult = "20 D20")]
        [TestCase(60, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(61, ThrowNumber.FirstThrow, ExpectedResult = "T15 D8")]
        [TestCase(61, ThrowNumber.SecondThrow, ExpectedResult = "T15 D8")]
        [TestCase(61, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(62, ThrowNumber.FirstThrow, ExpectedResult = "T10 D16")]
        [TestCase(62, ThrowNumber.SecondThrow, ExpectedResult = "T10 D16")]
        [TestCase(62, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(63, ThrowNumber.FirstThrow, ExpectedResult = "T13 D12")]
        [TestCase(63, ThrowNumber.SecondThrow, ExpectedResult = "T13 D12")]
        [TestCase(63, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(64, ThrowNumber.FirstThrow, ExpectedResult = "T16 D8")]
        [TestCase(64, ThrowNumber.SecondThrow, ExpectedResult = "T16 D8")]
        [TestCase(64, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(65, ThrowNumber.FirstThrow, ExpectedResult = "T19 D4")]
        [TestCase(65, ThrowNumber.SecondThrow, ExpectedResult = "T19 D4")]
        [TestCase(65, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(66, ThrowNumber.FirstThrow, ExpectedResult = "T14 D12")]
        [TestCase(66, ThrowNumber.SecondThrow, ExpectedResult = "T14 D12")]
        [TestCase(66, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(67, ThrowNumber.FirstThrow, ExpectedResult = "T17 D8")]
        [TestCase(67, ThrowNumber.SecondThrow, ExpectedResult = "T17 D8")]
        [TestCase(67, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(68, ThrowNumber.FirstThrow, ExpectedResult = "T20 D4")]
        [TestCase(68, ThrowNumber.SecondThrow, ExpectedResult = "T20 D4")]
        [TestCase(68, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(69, ThrowNumber.FirstThrow, ExpectedResult = "T19 D6")]
        [TestCase(69, ThrowNumber.SecondThrow, ExpectedResult = "T19 D6")]
        [TestCase(69, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(70, ThrowNumber.FirstThrow, ExpectedResult = "T18 D8")]
        [TestCase(70, ThrowNumber.SecondThrow, ExpectedResult = "T18 D8")]
        [TestCase(70, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(71, ThrowNumber.FirstThrow, ExpectedResult = "T13 16")]
        [TestCase(71, ThrowNumber.SecondThrow, ExpectedResult = "T13 16")]
        [TestCase(71, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(72, ThrowNumber.FirstThrow, ExpectedResult = "T16 D12")]
        [TestCase(72, ThrowNumber.SecondThrow, ExpectedResult = "T16 D12")]
        [TestCase(72, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(73, ThrowNumber.FirstThrow, ExpectedResult = "T19 D8")]
        [TestCase(73, ThrowNumber.SecondThrow, ExpectedResult = "T19 D8")]
        [TestCase(73, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(74, ThrowNumber.FirstThrow, ExpectedResult = "T14 D16")]
        [TestCase(74, ThrowNumber.SecondThrow, ExpectedResult = "T14 D16")]
        [TestCase(74, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(75, ThrowNumber.FirstThrow, ExpectedResult = "T17 D12")]
        [TestCase(75, ThrowNumber.SecondThrow, ExpectedResult = "T17 D12")]
        [TestCase(75, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(76, ThrowNumber.FirstThrow, ExpectedResult = "T20 D8")]
        [TestCase(76, ThrowNumber.SecondThrow, ExpectedResult = "T20 D8")]
        [TestCase(76, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(77, ThrowNumber.FirstThrow, ExpectedResult = "T19 D10")]
        [TestCase(77, ThrowNumber.SecondThrow, ExpectedResult = "T19 D10")]
        [TestCase(77, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(78, ThrowNumber.FirstThrow, ExpectedResult = "T18 D12")]
        [TestCase(78, ThrowNumber.SecondThrow, ExpectedResult = "T18 D12")]
        [TestCase(78, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(79, ThrowNumber.FirstThrow, ExpectedResult = "T19 D11")]
        [TestCase(79, ThrowNumber.SecondThrow, ExpectedResult = "T19 D11")]
        [TestCase(79, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(80, ThrowNumber.FirstThrow, ExpectedResult = "T20 D10")]
        [TestCase(80, ThrowNumber.SecondThrow, ExpectedResult = "T20 D10")]
        [TestCase(80, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(81, ThrowNumber.FirstThrow, ExpectedResult = "T19 D12")]
        [TestCase(81, ThrowNumber.SecondThrow, ExpectedResult = "T19 D12")]
        [TestCase(81, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(82, ThrowNumber.FirstThrow, ExpectedResult = "Bull D16")]
        [TestCase(82, ThrowNumber.SecondThrow, ExpectedResult = "Bull D16")]
        [TestCase(82, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(83, ThrowNumber.FirstThrow, ExpectedResult = "T17 D16")]
        [TestCase(83, ThrowNumber.SecondThrow, ExpectedResult = "T17 D16")]
        [TestCase(83, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(84, ThrowNumber.FirstThrow, ExpectedResult = "T20 D12")]
        [TestCase(84, ThrowNumber.SecondThrow, ExpectedResult = "T20 D12")]
        [TestCase(84, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(85, ThrowNumber.FirstThrow, ExpectedResult = "T15 D20")]
        [TestCase(85, ThrowNumber.SecondThrow, ExpectedResult = "T15 D20")]
        [TestCase(85, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(86, ThrowNumber.FirstThrow, ExpectedResult = "T18 D18")]
        [TestCase(86, ThrowNumber.SecondThrow, ExpectedResult = "T18 D18")]
        [TestCase(86, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(87, ThrowNumber.FirstThrow, ExpectedResult = "T17 D18")]
        [TestCase(87, ThrowNumber.SecondThrow, ExpectedResult = "T17 D18")]
        [TestCase(87, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(88, ThrowNumber.FirstThrow, ExpectedResult = "T20 D14")]
        [TestCase(88, ThrowNumber.SecondThrow, ExpectedResult = "T20 D14")]
        [TestCase(88, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(89, ThrowNumber.FirstThrow, ExpectedResult = "T19 D16")]
        [TestCase(89, ThrowNumber.SecondThrow, ExpectedResult = "T19 D16")]
        [TestCase(89, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(90, ThrowNumber.FirstThrow, ExpectedResult = "T20 D15")]
        [TestCase(90, ThrowNumber.SecondThrow, ExpectedResult = "T20 D15")]
        [TestCase(90, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(91, ThrowNumber.FirstThrow, ExpectedResult = "T17 D20")]
        [TestCase(91, ThrowNumber.SecondThrow, ExpectedResult = "T17 D20")]
        [TestCase(91, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(92, ThrowNumber.FirstThrow, ExpectedResult = "T20 D16")]
        [TestCase(92, ThrowNumber.SecondThrow, ExpectedResult = "T20 D16")]
        [TestCase(92, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(93, ThrowNumber.FirstThrow, ExpectedResult = "T19 D18")]
        [TestCase(93, ThrowNumber.SecondThrow, ExpectedResult = "T19 D18")]
        [TestCase(93, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(94, ThrowNumber.FirstThrow, ExpectedResult = "T18 D20")]
        [TestCase(94, ThrowNumber.SecondThrow, ExpectedResult = "T18 D20")]
        [TestCase(94, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(95, ThrowNumber.FirstThrow, ExpectedResult = "T19 D19")]
        [TestCase(95, ThrowNumber.SecondThrow, ExpectedResult = "T19 D19")]
        [TestCase(95, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(96, ThrowNumber.FirstThrow, ExpectedResult = "T20 D18")]
        [TestCase(96, ThrowNumber.SecondThrow, ExpectedResult = "T20 D18")]
        [TestCase(96, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(97, ThrowNumber.FirstThrow, ExpectedResult = "T19 D20")]
        [TestCase(97, ThrowNumber.SecondThrow, ExpectedResult = "T19 D20")]
        [TestCase(97, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(98, ThrowNumber.FirstThrow, ExpectedResult = "T20 D19")]
        [TestCase(98, ThrowNumber.SecondThrow, ExpectedResult = "T20 D19")]
        [TestCase(98, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(99, ThrowNumber.FirstThrow, ExpectedResult = "T19 10 D16")]
        [TestCase(99, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(99, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(100, ThrowNumber.FirstThrow, ExpectedResult = "T20 D20")]
        [TestCase(100, ThrowNumber.SecondThrow, ExpectedResult = "T20 D20")]
        [TestCase(100, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(101, ThrowNumber.FirstThrow, ExpectedResult = "T17 Bull")]
        [TestCase(101, ThrowNumber.SecondThrow, ExpectedResult = "T17 Bull")]
        [TestCase(101, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(102, ThrowNumber.FirstThrow, ExpectedResult = "T16 14 D20")]
        [TestCase(102, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(102, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(103, ThrowNumber.FirstThrow, ExpectedResult = "T19 6 D20")]
        [TestCase(103, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(103, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(104, ThrowNumber.FirstThrow, ExpectedResult = "T18 Bull")]
        [TestCase(104, ThrowNumber.SecondThrow, ExpectedResult = "T18 Bull")]
        [TestCase(104, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(105, ThrowNumber.FirstThrow, ExpectedResult = "T20 13 D16")]
        [TestCase(105, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(105, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(106, ThrowNumber.FirstThrow, ExpectedResult = "T20 6 D20")]
        [TestCase(106, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(106, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(107, ThrowNumber.FirstThrow, ExpectedResult = "T19 Bull")]
        [TestCase(107, ThrowNumber.SecondThrow, ExpectedResult = "T19 Bull")]
        [TestCase(107, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(108, ThrowNumber.FirstThrow, ExpectedResult = "T20 16 D16")]
        [TestCase(108, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(108, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(109, ThrowNumber.FirstThrow, ExpectedResult = "T20 17 D16")]
        [TestCase(109, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(109, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(110, ThrowNumber.FirstThrow, ExpectedResult = "T20 Bull")]
        [TestCase(110, ThrowNumber.SecondThrow, ExpectedResult = "T20 Bull")]
        [TestCase(110, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(111, ThrowNumber.FirstThrow, ExpectedResult = "T19 14 D20")]
        [TestCase(111, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(111, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(112, ThrowNumber.FirstThrow, ExpectedResult = "T20 20 D16")]
        [TestCase(112, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(112, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(113, ThrowNumber.FirstThrow, ExpectedResult = "T19 16 D20")]
        [TestCase(113, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(113, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(114, ThrowNumber.FirstThrow, ExpectedResult = "T20 14 D20")]
        [TestCase(114, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(114, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(115, ThrowNumber.FirstThrow, ExpectedResult = "T20 15 D20")]
        [TestCase(115, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(115, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(116, ThrowNumber.FirstThrow, ExpectedResult = "T20 16 D20")]
        [TestCase(116, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(116, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(117, ThrowNumber.FirstThrow, ExpectedResult = "T20 17 D20")]
        [TestCase(117, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(117, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(118, ThrowNumber.FirstThrow, ExpectedResult = "T20 18 D20")]
        [TestCase(118, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(118, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(119, ThrowNumber.FirstThrow, ExpectedResult = "T19 12 Bull")]
        [TestCase(119, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(119, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(120, ThrowNumber.FirstThrow, ExpectedResult = "T20 20 D20")]
        [TestCase(120, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(120, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(121, ThrowNumber.FirstThrow, ExpectedResult = "T20 11 Bull")]
        [TestCase(121, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(121, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(122, ThrowNumber.FirstThrow, ExpectedResult = "T18 18 Bull")]
        [TestCase(122, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(122, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(123, ThrowNumber.FirstThrow, ExpectedResult = "T19 16 Bull")]
        [TestCase(123, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(123, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(124, ThrowNumber.FirstThrow, ExpectedResult = "T20 14 Bull")]
        [TestCase(124, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(124, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(125, ThrowNumber.FirstThrow, ExpectedResult = "25 T20 D20")]
        [TestCase(125, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(125, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(126, ThrowNumber.FirstThrow, ExpectedResult = "T19 19 Bull")]
        [TestCase(126, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(126, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(127, ThrowNumber.FirstThrow, ExpectedResult = "T20 17 Bull")]
        [TestCase(127, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(127, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(128, ThrowNumber.FirstThrow, ExpectedResult = "18 T20 Bull")]
        [TestCase(128, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(128, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(129, ThrowNumber.FirstThrow, ExpectedResult = "19 T20 Bull")]
        [TestCase(129, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(129, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(130, ThrowNumber.FirstThrow, ExpectedResult = "T20 20 Bull")]
        [TestCase(130, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(130, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(131, ThrowNumber.FirstThrow, ExpectedResult = "T20 T13 D16")]
        [TestCase(131, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(131, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(132, ThrowNumber.FirstThrow, ExpectedResult = "25 T19 Bull")]
        [TestCase(132, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(132, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(133, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D8")]
        [TestCase(133, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(133, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(134, ThrowNumber.FirstThrow, ExpectedResult = "T20 T14 D16")]
        [TestCase(134, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(134, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(135, ThrowNumber.FirstThrow, ExpectedResult = "25 T20 Bull")]
        [TestCase(135, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(135, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(136, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D8")]
        [TestCase(136, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(136, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(137, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D10")]
        [TestCase(137, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(137, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(138, ThrowNumber.FirstThrow, ExpectedResult = "T20 T18 D12")]
        [TestCase(138, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(138, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(139, ThrowNumber.FirstThrow, ExpectedResult = "T19 T14 D20")]
        [TestCase(139, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(139, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(140, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D10")]
        [TestCase(140, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(140, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(141, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D12")]
        [TestCase(141, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(141, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(142, ThrowNumber.FirstThrow, ExpectedResult = "T20 T14 D20")]
        [TestCase(142, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(142, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(143, ThrowNumber.FirstThrow, ExpectedResult = "T20 T17 D16")]
        [TestCase(143, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(143, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(144, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D12")]
        [TestCase(144, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(144, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(145, ThrowNumber.FirstThrow, ExpectedResult = "T20 T15 D20")]
        [TestCase(145, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(145, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(146, ThrowNumber.FirstThrow, ExpectedResult = "T20 T18 D16")]
        [TestCase(146, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(146, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(147, ThrowNumber.FirstThrow, ExpectedResult = "T20 T17 D18")]
        [TestCase(147, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(147, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(148, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D14")]
        [TestCase(148, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(148, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(149, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D16")]
        [TestCase(149, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(149, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(150, ThrowNumber.FirstThrow, ExpectedResult = "T20 T18 D18")]
        [TestCase(150, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(150, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(151, ThrowNumber.FirstThrow, ExpectedResult = "T20 T17 D20")]
        [TestCase(151, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(151, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(152, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D16")]
        [TestCase(152, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(152, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(153, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D18")]
        [TestCase(153, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(153, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(154, ThrowNumber.FirstThrow, ExpectedResult = "T20 T18 D20")]
        [TestCase(154, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(154, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(155, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D19")]
        [TestCase(155, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(155, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(156, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D18")]
        [TestCase(156, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(156, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(157, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 D20")]
        [TestCase(157, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(157, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(158, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D19")]
        [TestCase(158, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(158, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(160, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 D20")]
        [TestCase(160, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(160, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(161, ThrowNumber.FirstThrow, ExpectedResult = "T20 T17 Bull")]
        [TestCase(161, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(161, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(164, ThrowNumber.FirstThrow, ExpectedResult = "T20 T18 Bull")]
        [TestCase(164, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(164, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(167, ThrowNumber.FirstThrow, ExpectedResult = "T20 T19 Bull")]
        [TestCase(167, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(167, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        [TestCase(170, ThrowNumber.FirstThrow, ExpectedResult = "T20 T20 Bull")]
        [TestCase(170, ThrowNumber.SecondThrow, ExpectedResult = null)]
        [TestCase(170, ThrowNumber.ThirdThrow, ExpectedResult = null)]
        public string HintGetsCorrectly(int points, ThrowNumber number)

        {
            var hint = Windows.Score.CheckOut.Get(points, number);
            return hint;
        }
    }
}