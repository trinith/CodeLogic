using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ISequenceSubmitButton : IButton
    {
        event EventHandler<EventArgs> SubmitSequence;
    }

    public class SequenceSubmitButton : ButtonBase, ISequenceSubmitButton
    {
        public const double SUBMIT_HOLD_TIME = 0.5;

        private ISpriteBatch _spriteBatch;
        private double? _submitTimeRemaining = null;
        private IDeviceTheme _theme;

        public event EventHandler<EventArgs> SubmitSequence;

        public SequenceSubmitButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();

            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        protected virtual void OnSubmitSequence(EventArgs e)
        {
            if (this.SubmitSequence != null)
                this.SubmitSequence(this, e);
        }

        protected override void OnTouched(ButtonEventArgs e)
        {
            base.OnTouched(e);

            _submitTimeRemaining = SUBMIT_HOLD_TIME;
        }

        protected override void OnReleased(ButtonEventArgs e)
        {
            base.OnReleased(e);

            _submitTimeRemaining = null;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_submitTimeRemaining != null)
            {
                _submitTimeRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_submitTimeRemaining.Value <= 0)
                {
                    OnSubmitSequence(new EventArgs());
                    _submitTimeRemaining = null;
                }
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IAssetBank bank = this.Host.AssetBank;

            _spriteBatch.Draw(bank.Get<ITexture2D>("SequenceSubmitButtonBackground"), this.Bounds, _theme.SubmitButtonBackgroundMask);

            if (_submitTimeRemaining != null)
            {
                float progress = 1f - (float)(_submitTimeRemaining.Value / SUBMIT_HOLD_TIME);
                _spriteBatch.Draw(
                    bank.Get<ITexture2D>("SequenceSubmitButtonHoldOverlay"),
                    new RectangleF(
                        this.Bounds.Left,
                        this.Bounds.Top,
                        this.Bounds.Width * progress,
                        this.Bounds.Height
                    ),
                    _theme.SubmitButtonHoldOverlayMask
                );
            }

            _spriteBatch.Draw(bank.Get<ITexture2D>("SequenceSubmitButtonForeground"), this.Bounds, _theme.SubmitButtonForegroundMask);
        }
    }
}
