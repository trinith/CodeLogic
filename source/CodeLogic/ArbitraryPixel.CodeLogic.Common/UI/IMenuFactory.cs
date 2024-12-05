using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IMenuFactory
    {
        IMenuItem CreateMenuItem(string text, float height);
        IMenuView CreateMenuTitleView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont);
        IMenuListView CreateMenuListView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont, IMenuFactory menuFactory);
        IMenuItemView CreateMenuItemView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont font, TextLineAlignment lineAlignment);
    }
}
