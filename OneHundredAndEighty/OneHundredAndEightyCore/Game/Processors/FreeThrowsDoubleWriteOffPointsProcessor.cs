﻿#region Usings

using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Domain;
using OneHundredAndEightyCore.Windows.Score;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleWriteOffPointsProcessor : ProcessorBase
    {
        public FreeThrowsDoubleWriteOffPointsProcessor(Domain.Game game,
                                                       DBService dbService,
                                                       ScoreBoardService scoreBoard)
            : base(game, dbService, scoreBoard)
        {
            Game.Players.ForEach(p => p.LegPoints = Game.legPoints);
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            if (IsLegOver(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.LegWon);

                dbService.StatisticUpdateAddLegsPlayedForPlayers(Game.Id);
                dbService.StatisticUpdateAddLegsWonForPlayer(Game.PlayerOnThrow, Game.Id);

                foreach (var player in Game.Players)
                {
                    player.LegPoints = Game.legPoints;
                    scoreBoard.SetPointsTo(player, Game.legPoints);
                    scoreBoard.CheckPointsHintFor(player);
                }

                ClearPlayerOnThrowHand();

                TogglePlayerOnThrow();
                return;
            }

            if (IsFault(thrw))
            {
                ConvertAndSaveThrow(thrw, ThrowResult.Fault);

                OnFault();
                return;
            }

            Game.PlayerOnThrow.HandPoints += thrw.TotalPoints;
            Game.PlayerOnThrow.LegPoints -= thrw.TotalPoints;
            scoreBoard.AddPointsTo(Game.PlayerOnThrow, thrw.TotalPoints * -1);

            var dbThrow = ConvertAndSaveThrow(thrw, ThrowResult.Ordinary);

            Game.PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180();
                ClearPlayerOnThrowHand();
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
                TogglePlayerOnThrow();
            }
            else
            {
                Game.PlayerOnThrow.ThrowNumber += 1;
                scoreBoard.SetThrowNumber(Game.PlayerOnThrow.ThrowNumber);
                scoreBoard.CheckPointsHintFor(Game.PlayerOnThrow);
            }
        }
    }
}