using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ITextureEntity : IStaticTexture
    {
    }

    public class TextureEntity : StaticTexture, ITextureEntity
    {
        public TextureEntity(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null)
            : base(host, bounds, spriteBatch, texture)
        {
            this.Mask = mask;
            this.SourceRectangle = sourceRectangle;
        }
    }
}
