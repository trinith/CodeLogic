using ArbitraryPixel.CodeLogic.Common.Model;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface IGameStatsController
    {
        event EventHandler<EventArgs> Updated;

        IGameStatsModel GameStatsModel { get; }
        void Update(IDeviceModel deviceModel);
    }

    public class GameStatsController : IGameStatsController
    {
        public GameStatsController(IGameStatsModel gameStatsModel)
        {
            this.GameStatsModel = gameStatsModel ?? throw new ArgumentNullException();
        }

        #region IGameStatsController Implementation
        public event EventHandler<EventArgs> Updated;

        public IGameStatsModel GameStatsModel { get; private set; }

        public void Update(IDeviceModel deviceModel)
        {
            if (deviceModel == null)
                throw new ArgumentNullException();

            this.GameStatsModel.GamesPlayed++;
            this.GameStatsModel.TotalGuesses += (ulong)deviceModel.Attempts.Count;
            this.GameStatsModel.TotalGameTime = this.GameStatsModel.TotalGameTime.Add(deviceModel.Stopwatch.ElapsedTime);

            if (deviceModel.GameWon)
            {
                if (this.GameStatsModel.LeastGuessesToWin == 0 || (ulong)deviceModel.Attempts.Count < this.GameStatsModel.LeastGuessesToWin)
                    this.GameStatsModel.LeastGuessesToWin = (ulong)deviceModel.Attempts.Count;

                this.GameStatsModel.GamesWon++;
                this.GameStatsModel.CurrentWinStreak++;
                this.GameStatsModel.CurrentLossStreak = 0;

                if (this.GameStatsModel.FastestWin == TimeSpan.Zero || deviceModel.Stopwatch.ElapsedTime < this.GameStatsModel.FastestWin)
                    this.GameStatsModel.FastestWin = deviceModel.Stopwatch.ElapsedTime;

                if (this.GameStatsModel.SlowestWin == TimeSpan.Zero || deviceModel.Stopwatch.ElapsedTime > this.GameStatsModel.SlowestWin)
                    this.GameStatsModel.SlowestWin = deviceModel.Stopwatch.ElapsedTime;
            }
            else
            {
                this.GameStatsModel.CurrentWinStreak = 0;
                this.GameStatsModel.CurrentLossStreak++;
            }

            if (this.Updated != null)
                this.Updated(this, new EventArgs());
        }
        #endregion
    }
}
