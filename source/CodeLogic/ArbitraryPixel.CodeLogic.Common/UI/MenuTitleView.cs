using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuTitleView : MenuViewBase
    {
        private ITexture2D _pixel;
        private ISpriteBatch _spriteBatch;
        private ISpriteFont _menuFont;

        public string TitleText { get; private set; }

        public MenuTitleView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont)
            : base(host, bounds, viewOf)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _menuFont = menuFont ?? throw new ArgumentNullException();

            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");

            RefreshText();
        }

        private void RefreshText()
        {
            this.TitleText = "";

            if (this.ViewOf == null)
                return;

            StringBuilder menuText = new StringBuilder();

            IMenuItem item = this.ViewOf;
            menuText.Append(this.ViewOf.Text);

            while (item.Parent != null)
            {
                item = item.Parent;
                menuText.Insert(0, " > ");
                menuText.Insert(0, item.Text);
            }

            this.TitleText = menuText.ToString();
        }

        protected override void OnViewOfSet()
        {
            base.OnViewOfSet();
            RefreshText();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Draw(_pixel, this.Bounds, CodeLogicEngine.Constants.ClrMenuBGHigh);

            if (this.TitleText != "")
            {
                _spriteBatch.DrawString(_menuFont, this.TitleText, this.Bounds.Location + new Vector2(5, 5), CodeLogicEngine.Constants.ClrMenuTextSelected);
            }
        }
    }
}
