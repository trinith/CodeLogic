using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Time;
using System;
using System.Diagnostics;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public class DeviceModel : IDeviceModel
    {
        private const int CODE_LENGTH = 4;
        private IRandom _random;

        private IStopwatchManager _stopwatchManager;

        public bool GameWon { get; set; } = false;

        public ICodeValueColourMap CodeColourMap { get; private set; }
        public ISequenceAttemptCollection Attempts { get; private set; }
        public ICodeSequence InputSequence { get; private set; }
        public ICodeSequence TargetSequence { get; private set; }
        public int CurrentTrial { get; set; } = 1;
        public IStopwatch Stopwatch { get; private set; }

        public AlarmLevel AlarmLevel
        {
            get
            {
                AlarmLevel level = AlarmLevel.Critical;

                switch (this.CurrentTrial)
                {
                    case 1:
                    case 2:
                    case 3:
                        level = AlarmLevel.Low;
                        break;
                    case 4:
                    case 5:
                    case 6:
                        level = AlarmLevel.Medium;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        level = AlarmLevel.High;
                        break;
                    case 10:
                    default:
                        level = AlarmLevel.Critical;
                        break;
                }

                return level;
            }
        }

        public DeviceModel(IRandom random, ICodeValueColourMap colourMap, IStopwatchManager stopwatchManager)
        {
            _random = random ?? throw new ArgumentNullException();
            this.CodeColourMap = colourMap ?? throw new ArgumentNullException();
            _stopwatchManager = stopwatchManager ?? throw new ArgumentNullException();

            Reset();
        }

        public void Reset()
        {
            this.GameWon = false;
            this.CurrentTrial = 1;
            this.InputSequence = GameObjectFactory.Instance.CreateCodeSequence(CODE_LENGTH);
            this.TargetSequence = GameObjectFactory.Instance.CreateCodeSequence(CODE_LENGTH);
            this.Attempts = GameObjectFactory.Instance.CreateSequenceAttemptCollection();

            this.TargetSequence.GenerateRandomCode(_random);

            if (this.Stopwatch != null)
                this.Stopwatch.Dispose();

            this.Stopwatch = _stopwatchManager.Create();

#if DEBUG
            // TODO: For debugging... remove this later ;)
            Debug.WriteLine(this.TargetSequence.ToString());
#endif
        }
    }
}
