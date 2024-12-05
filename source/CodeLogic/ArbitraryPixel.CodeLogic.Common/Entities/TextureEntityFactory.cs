using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ITextureEntityFactory
    {
        ITextureEntity Create(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null);
    }

    public class TextureEntityFactory : ITextureEntityFactory
    {
        public ITextureEntity Create(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D texture, Color mask, Rectangle? sourceRectangle = null)
        {
            return new TextureEntity(host, bounds, spriteBatch, texture, mask, sourceRectangle);
        }
    }
}
