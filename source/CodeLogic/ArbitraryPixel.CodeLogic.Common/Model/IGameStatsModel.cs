using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IGameStatsModel
    {
        ulong GamesPlayed { get; set; }
        ulong GamesWon { get; set; }

        ulong TotalGuesses { get; set; }
        ulong LeastGuessesToWin { get; set; }

        ulong CurrentWinStreak { get; set; }
        ulong CurrentLossStreak { get; set; }

        TimeSpan TotalGameTime { get; set; }
        TimeSpan FastestWin { get; set; }
        TimeSpan SlowestWin { get; set; }

        void Reset();
    }
}
