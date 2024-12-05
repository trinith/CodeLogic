using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public class GameStatsModel : IGameStatsModel
    {
        #region IGameStatsModel Implementation
        public ulong GamesPlayed { get; set; }
        public ulong GamesWon { get; set; }

        public ulong TotalGuesses { get; set; }
        public ulong LeastGuessesToWin { get; set; }

        public ulong CurrentWinStreak { get; set; }
        public ulong CurrentLossStreak { get; set; }

        public TimeSpan TotalGameTime { get; set; }
        public TimeSpan FastestWin { get; set; }
        public TimeSpan SlowestWin { get; set; }

        public void Reset()
        {
            this.GamesPlayed = 0;
            this.GamesWon = 0;

            this.TotalGuesses = 0;
            this.LeastGuessesToWin = 0;

            this.CurrentWinStreak = 0;
            this.CurrentLossStreak = 0;

            this.TotalGameTime = TimeSpan.Zero;
            this.FastestWin = TimeSpan.Zero;
            this.SlowestWin = TimeSpan.Zero;
        }
        #endregion
    }
}
