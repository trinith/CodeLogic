using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuFactory : IMenuFactory
    {
        public IMenuItem CreateMenuItem(string text, float height)
        {
            return new MenuItem(text, height);
        }

        public IMenuItemView CreateMenuItemView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont font, TextLineAlignment lineAlignment)
        {
            return new MenuItemView(host, bounds, viewOf, spriteBatch, font, lineAlignment);
        }

        public IMenuListView CreateMenuListView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont, IMenuFactory menuFactory)
        {
            return new MenuListView(host, bounds, viewOf, spriteBatch, menuFont, menuFactory);
        }

        public IMenuView CreateMenuTitleView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont)
        {
            return new MenuTitleView(host, bounds, viewOf, spriteBatch, menuFont);
        }
    }
}
