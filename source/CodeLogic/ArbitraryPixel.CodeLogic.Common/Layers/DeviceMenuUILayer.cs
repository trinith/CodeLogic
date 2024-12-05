using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IDeviceMenuUILayer : ILayer
    {
        event EventHandler<EventArgs> ReturnButtonTapped;
        event EventHandler<EventArgs> AbortButtonTapped;
        event EventHandler<EventArgs> SettingsButtonTapped;
    }

    public class DeviceMenuUILayer : LayerBase, IDeviceMenuUILayer
    {
        public event EventHandler<EventArgs> ReturnButtonTapped;
        public event EventHandler<EventArgs> AbortButtonTapped;
        public event EventHandler<EventArgs> SettingsButtonTapped;

        public DeviceMenuUILayer(IEngine host, ISpriteBatch mainSpriteBatch)
            : base(host, mainSpriteBatch)
        {
            // Create UI elements for the device.
            IDeviceTheme currentTheme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            SizeF buttonPadding = new SizeF(25, 0);
            RectangleF worldBounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);

            SizeF returnButtonSize = new SizeF(100, 200);
            RectangleF returnButtonBounds = new RectangleF(
                new Vector2(this.Host.ScreenManager.World.X - returnButtonSize.Width, this.Host.ScreenManager.World.Y /2f - returnButtonSize.Height / 2f),
                returnButtonSize
            );

            SizeF menuButtonSize = new SizeF(300, 300);
            RectangleF screenBounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);
            RectangleF buttonBounds = new RectangleF(
                new Vector2(
                    screenBounds.Width / 2f,
                    screenBounds.Height / 2f - menuButtonSize.Height / 2f
                ),
                menuButtonSize
            );

            int numButtons = 2;
            buttonBounds.Location = new Vector2(buttonBounds.Location.X - (numButtons * menuButtonSize.Width / 2f) - ((numButtons - 1) * buttonPadding.Width), buttonBounds.Location.Y);

            IButtonObjectDefinitionFactory factory = GameObjectFactory.Instance.CreateButtonObjectDefinitionFactory();
            IGenericButton returnButton = GameObjectFactory.Instance.CreateGenericButton(this.Host, returnButtonBounds, mainSpriteBatch);

            returnButton.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("SceneChangeButtonFill"), currentTheme.SceneChangeBackgroundMask, SpriteEffects.FlipHorizontally));
            returnButton.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("SceneChangeButtonBorder"), currentTheme.SceneChangeBorderNormalMask, currentTheme.SceneChangeBorderHighlightMask, SpriteEffects.FlipHorizontally));

            IButtonObjectDefinition buttonIcon;
            returnButton.AddButtonObject(buttonIcon = factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("IconBack"), currentTheme.SceneChangeBorderNormalMask, currentTheme.SceneChangeBorderHighlightMask, SpriteEffects.FlipHorizontally));
            buttonIcon.GlobalOffset = currentTheme.SceneChangeIconOffset * new Vector2(-1, 1);

            returnButton.Tapped += (sender, e) => OnReturnButtonTapped(new EventArgs());
            this.AddEntity(returnButton);

            IButton abortButton = GameObjectFactory.Instance.CreateDeviceMenuButton(this.Host, buttonBounds, mainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("DeviceMenuButtonAbort"));
            abortButton.Tapped += (sender, e) => OnAbortButtonTapped(new EventArgs());
            this.AddEntity(abortButton);

            buttonBounds.Location = new Vector2(buttonBounds.Location.X + menuButtonSize.Width + buttonPadding.Width, buttonBounds.Location.Y);
            IButton settingsButton = GameObjectFactory.Instance.CreateDeviceMenuButton(this.Host, buttonBounds, mainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("DeviceMenuButtonSettings"));
            settingsButton.Tapped += (sender, e) => OnSettingsButtonTapped(new EventArgs());
            this.AddEntity(settingsButton);
        }

        protected virtual void OnReturnButtonTapped(EventArgs e)
        {
            if (this.ReturnButtonTapped != null)
                this.ReturnButtonTapped(this, e);
        }

        protected virtual void OnAbortButtonTapped(EventArgs e)
        {
            if (this.AbortButtonTapped != null)
                this.AbortButtonTapped(this, e);
        }

        protected virtual void OnSettingsButtonTapped(EventArgs e)
        {
            if (this.SettingsButtonTapped != null)
                this.SettingsButtonTapped(this, e);
        }
    }
}
