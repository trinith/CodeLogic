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
    public class DeviceMenuButton : ButtonBase
    {
        private ISpriteBatch _spriteBatch;
        private ITexture2D _foregroundTexture;

        private IDeviceTheme _theme;
        private bool _isPressed = false;

        public DeviceMenuButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITexture2D foregroundTexture)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _foregroundTexture = foregroundTexture ?? throw new ArgumentNullException();

            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _isPressed = this.State == ButtonState.Pressed;
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IAssetBank bank = this.Host.AssetBank;
            Color highlightColour = (_isPressed) ? _theme.DeviceMenuButtonForegroundHighlightMask : _theme.DeviceMenuButtonForegroundMask;

            _spriteBatch.Draw(bank.Get<ITexture2D>("DeviceMenuButtonBackground"), this.Bounds, _theme.DeviceMenuButtonBackgroundMask);
            _spriteBatch.Draw(bank.Get<ITexture2D>("DeviceMenuButtonBorder"), this.Bounds, highlightColour);
            _spriteBatch.Draw(_foregroundTexture, this.Bounds, highlightColour);
        }
    }
}
