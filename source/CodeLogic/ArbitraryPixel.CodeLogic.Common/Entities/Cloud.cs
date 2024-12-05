using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ICloud : ITextureEntity
    {
        int Depth { get; set; }
        float Intensity { get; set; }
    }

    public class Cloud : TextureEntity, ICloud
    {
        public static class Constants
        {
            public const float MaxIntensity = 2.2f;
            public const float MinIntensity = 0.5f;
        }

        private float _intensity = 0.75f;

        public Cloud(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null)
            : base(host, bounds, spriteBatch, texture, mask, sourceRectangle)
        {
        }

        #region ICloud Implementation
        public int Depth { get; set; } = 0;

        public float Intensity
        {
            get { return _intensity; }
            set { _intensity = MathHelper.Clamp(value, Constants.MinIntensity, Constants.MaxIntensity); }
        }
        #endregion
    }
}
