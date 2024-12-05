using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public class StatusBar : StatusIndicator
    {
        public StatusBar(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, IDeviceModel model)
            : base(host, bounds, spriteBatch, model)
        {
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IAssetBank bank = this.Host.AssetBank;
            this.SpriteBatch.Draw(bank.Get<ITexture2D>("StatusBarBackgroundFill"), this.Bounds, this.Theme.StatusIndicatorBackgroundMask);
            this.SpriteBatch.Draw(bank.Get<ITexture2D>("StatusBarBackgroundBorder"), this.Bounds, this.Theme.StatusIndicatorBorderMask);

            ITexture2D progressFrameTexture = bank.Get<ITexture2D>("StatusBarProgressFrame");
            SizeF progressFrameSize = new SizeF(progressFrameTexture.Width, progressFrameTexture.Height);
            Rectangle progressFrameBounds = (Rectangle)(new RectangleF(
                this.Bounds.Centre - progressFrameSize.Centre,
                progressFrameSize
            ));
            this.SpriteBatch.Draw(bank.Get<ITexture2D>("StatusBarProgressFillBackground"), progressFrameBounds, this.Theme.StatusIndicatorProgressBackgroundMask);

            int trial = this.Model.CurrentTrial - 1;
            Rectangle progressFillRect = new Rectangle(
                Point.Zero,
                new Point(
                    (int)(this.Theme.StatusIndicatorProgressFrameBorderSize.Width + (trial * (this.Theme.StatusIndicatorProgressFrameCellSize.Width + this.Theme.StatusIndicatorProgressFrameCellBorderSize.Width))),
                    (int)progressFrameBounds.Height
                )
            );

            if (this.Model.CurrentTrial > 1)
            {
                this.SpriteBatch.Draw(bank.Get<ITexture2D>("StatusBarProgressFill"), progressFrameBounds.Location.ToVector2(), progressFillRect, this.CurrentAlarmMask);
            }

            if (this.Model.CurrentTrial <= CodeLogicEngine.Constants.MaximumTrials)
            {
                Vector2 indicatorLocation = progressFrameBounds.Location.ToVector2() + new Vector2(progressFillRect.Width, this.Theme.StatusIndicatorProgressFrameBorderSize.Height) + this.Theme.StatusIndicatorProgressFrameCellBorderSize;
                this.SpriteBatch.Draw(bank.Get<ITexture2D>("StatusBarCurrentTrialIndicator"), indicatorLocation, this.Theme.StatusIndicatorProgressBorderMask);
            }

            this.SpriteBatch.Draw(progressFrameTexture, progressFrameBounds, this.Theme.StatusIndicatorProgressBorderMask);
        }
    }
}
