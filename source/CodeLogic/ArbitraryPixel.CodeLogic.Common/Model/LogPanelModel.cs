using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.Common.Drawing;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public enum LogPanelMode
    {
        Closed,
        PartialView,
        FullView
    }

    public interface ILogPanelModel
    {
        event EventHandler<EventArgs> ModelReset;

        SizeF WorldBounds { get; }
        Vector2 CurrentOffset { get; set; }
        Vector2 PreviousOffset { get; set; }
        Vector2 TargetOffset { get; set; }

        Vector2 ProgressTarget { get; set; }
        Vector2 ProgressValue { get; set; }
        Vector2 ProgressSpeed { get; set; }

        LogPanelMode CurrentMode { get; set; }
        LogPanelMode? NextMode { get; set; }

        SizeF ClosedSize { get; set; }
        SizeF PartialSize { get; set; }
        SizeF FullSize { get; set; }

        void Reset();
        void Update(GameTime gameTime);
        void SetOffsetForMode();
    }

    public class LogPanelModel : ILogPanelModel
    {
        private ICodeLogicSettings _settings;
        private LogPanelMode _currentMode = LogPanelMode.Closed;

        public event EventHandler<EventArgs> ModelReset;

        public SizeF WorldBounds { get; private set; }
        public Vector2 CurrentOffset { get; set; }
        public Vector2 PreviousOffset { get; set; }
        public Vector2 TargetOffset { get; set; }

        public Vector2 ProgressTarget { get; set; }
        public Vector2 ProgressValue { get; set; }
        public Vector2 ProgressSpeed { get; set; }

        public LogPanelMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                _settings.LogPanelMode = value;
            }
        }
        public LogPanelMode? NextMode { get; set; } = null;

        public SizeF ClosedSize { get; set; } = SizeF.Empty;
        public SizeF PartialSize { get; set; } = SizeF.Empty;
        public SizeF FullSize { get; set; } = SizeF.Empty;

        public LogPanelModel(ICodeLogicSettings settings, SizeF worldBounds)
        {
            _settings = settings ?? throw new ArgumentNullException();
            this.WorldBounds = worldBounds;

            Reset();
        }

        public void SetOffsetForMode()
        {
            Vector2 offset = this.ClosedSize;

            switch (this.CurrentMode)
            {
                case LogPanelMode.FullView:
                    offset = this.FullSize;
                    break;
                case LogPanelMode.PartialView:
                    offset = this.PartialSize;
                    break;
                case LogPanelMode.Closed:
                default:
                    break;
            }

            this.CurrentOffset = this.ProgressTarget * offset;
        }

        protected void OnReset(EventArgs e)
        {
            if (this.ModelReset != null)
                this.ModelReset(this, e);
        }

        public void Reset()
        {
            this.PreviousOffset = Vector2.Zero;
            this.TargetOffset = Vector2.Zero;

            this.ProgressTarget = new Vector2(0, 1);
            this.ProgressValue = Vector2.Zero;
            this.ProgressSpeed = Vector2.Zero;

            this.CurrentMode = _settings.LogPanelMode;
            this.NextMode = null;

            SetOffsetForMode();

            OnReset(new EventArgs());
        }

        public void Update(GameTime gameTime)
        {
            if (this.NextMode != null)
            {
                this.ProgressValue += this.ProgressSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.ProgressValue.X >= 1)
                    this.ProgressValue = new Vector2(1, this.ProgressValue.Y);

                if (this.ProgressValue.Y >= 1)
                    this.ProgressValue = new Vector2(this.ProgressValue.X, 1);

                this.CurrentOffset = new Vector2(
                    this.PreviousOffset.X + MathHelper.SmoothStep(0, 1, this.ProgressValue.X) * (this.TargetOffset.X - this.PreviousOffset.X),
                    this.PreviousOffset.Y + MathHelper.SmoothStep(0, 1, this.ProgressValue.Y) * (this.TargetOffset.Y - this.PreviousOffset.Y)
                );

                if (this.ProgressValue == this.ProgressTarget)
                {
                    this.CurrentMode = this.NextMode.Value;
                    this.NextMode = null;
                }
            }
        }
    }
}
