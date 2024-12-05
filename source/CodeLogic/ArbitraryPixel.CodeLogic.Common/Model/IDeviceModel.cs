using ArbitraryPixel.Platform2D.Time;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IDeviceModel
    {
        bool GameWon { get; set; }

        int CurrentTrial { get; set; }
        AlarmLevel AlarmLevel { get; }

        ICodeValueColourMap CodeColourMap { get; }

        ICodeSequence InputSequence { get; }
        ICodeSequence TargetSequence { get; }
        ISequenceAttemptCollection Attempts { get; }

        IStopwatch Stopwatch { get; }

        void Reset();
    }
}