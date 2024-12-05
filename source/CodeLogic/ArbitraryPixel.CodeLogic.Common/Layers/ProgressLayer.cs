using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IProgressLayer : ILayer
    {
        RectangleF ProgressBarBounds { get; }
        float Minimum { get; set; }
        float Maximum { get; set; }
        float Value { get; set; }
    }

    public class ProgressLayer : LayerBase, IProgressLayer
    {
        private IProgressBar _progressBar;

        public ProgressLayer(IEngine host, ISpriteBatch mainSpriteBatch, ISpriteFont titleFont, string titleText)
            : base(host, mainSpriteBatch)
        {
            SizeF progressSize = new SizeF(CodeLogicEngine.Constants.AdProgressBarSize.Width, CodeLogicEngine.Constants.AdProgressBarSize.Height);
            RectangleF progressBounds = new RectangleF(((SizeF)this.Host.ScreenManager.World).Centre - progressSize.Centre, progressSize);

            _progressBar = GameObjectFactory.Instance.CreateProgressBar(this.Host, progressBounds, mainSpriteBatch);
            this.AddEntity(_progressBar);

            if (titleFont != null && !string.IsNullOrEmpty(titleText))
            {
                SizeF textSize = titleFont.MeasureString(titleText);
                this.AddEntity(
                    GameObjectFactory.Instance.CreateGenericTextLabel(
                        this.Host,
                        new Vector2(progressBounds.Left + progressBounds.Width / 2f - textSize.Width / 2f, progressBounds.Top - 10f - textSize.Height),
                        mainSpriteBatch,
                        titleFont,
                        titleText,
                        Color.White
                    )
                );
            }
        }

        #region IProgressLayer Implementation
        public RectangleF ProgressBarBounds => _progressBar.Bounds;

        public float Minimum
        {
            get { return _progressBar.Minimum; }
            set { _progressBar.Minimum = value; }
        }

        public float Maximum
        {
            get { return _progressBar.Maximum; }
            set { _progressBar.Maximum = value; }
        }

        public float Value
        {
            get { return _progressBar.Value; }
            set { _progressBar.Value = value; }
        }
        #endregion
    }
}
