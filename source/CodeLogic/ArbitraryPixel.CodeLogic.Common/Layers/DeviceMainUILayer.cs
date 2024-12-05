using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
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
    public interface IDeviceMainUILayer : ILayer
    {
        event EventHandler<EventArgs> SubmitSequence;
        event EventHandler<EventArgs> MenuButtonTapped;
    }

    public class DeviceMainUILayer : LayerBase, IDeviceMainUILayer
    {
        public event EventHandler<EventArgs> SubmitSequence;
        public event EventHandler<EventArgs> MenuButtonTapped;

        private ISound _submitSound;

        public DeviceMainUILayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel deviceModel, ILogPanelModel panelModel)
            : base(host, mainSpriteBatch)
        {
            SizeF edgePadding = CodeLogicEngine.Constants.TextWindowPadding;
            RectangleF worldBounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);

            _submitSound = this.Host.AssetBank.Get<ISoundResource>("SubmitSequence").CreateInstance();

            // Create UI elements for the device.
            IDeviceTheme currentTheme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            // Element for status indicator
            IStatusIndicator statusIndicator = GameObjectFactory.Instance.CreateStatusBar(this.Host, new RectangleF(0, 0, 600, 100), mainSpriteBatch, deviceModel);
            statusIndicator.Bounds = new RectangleF(new Vector2(worldBounds.Centre.X - statusIndicator.Bounds.Centre.X, 0), statusIndicator.Bounds.Size);
            this.AddEntity(statusIndicator);

            // Element for submit button
            ISequenceSubmitButton submitButton = GameObjectFactory.Instance.CreateSequenceSubmitButton(
                this.Host,
                new RectangleF(
                    this.Host.ScreenManager.World.X - 100,
                    this.Host.ScreenManager.World.Y / 2f - 100,
                    100,
                    200),
                mainSpriteBatch
            );
            submitButton.SubmitSequence += Handle_SubmitButtonSubmitSequence;
            submitButton.Touched += Handle_SubmitButtonTouched;
            submitButton.Released += Handle_SubmitButtonReleased;
            this.AddEntity(submitButton);

            // Element for scene change buttons (left and right)
            SizeF sceneButtonSize = new SizeF(100, 200);
            RectangleF menuSceneButtonBounds = new RectangleF(
                new Vector2(0, this.Host.ScreenManager.World.Y / 2f - sceneButtonSize.Height / 2f),
                sceneButtonSize
            );

            IButtonObjectDefinitionFactory factory = GameObjectFactory.Instance.CreateButtonObjectDefinitionFactory();
            IGenericButton menuSceneButton = GameObjectFactory.Instance.CreateGenericButton(this.Host, menuSceneButtonBounds, mainSpriteBatch);
            menuSceneButton.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("SceneChangeButtonFill"), currentTheme.SceneChangeBackgroundMask, SpriteEffects.None));
            menuSceneButton.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("SceneChangeButtonBorder"), currentTheme.SceneChangeBorderNormalMask, currentTheme.SceneChangeBorderHighlightMask, SpriteEffects.None));

            IButtonObjectDefinition buttonIcon;
            menuSceneButton.AddButtonObject(buttonIcon = factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("IconMenu"), currentTheme.SceneChangeBorderNormalMask, currentTheme.SceneChangeBorderHighlightMask, SpriteEffects.None));
            buttonIcon.GlobalOffset = currentTheme.SceneChangeIconOffset;

            menuSceneButton.Tapped += (sender, e) => OnMenuButtonTapped(new EventArgs());
            this.AddEntity(menuSceneButton);

            ILogPanelLayer logPanelLayer = GameObjectFactory.Instance.CreateLogPanelLayer(this.Host, this.MainSpriteBatch, deviceModel, panelModel);
            logPanelLayer.ModeChangeStarted += Handle_LogPanelLayerModeChangeStarted;
            this.AddEntity(logPanelLayer);
        }

        private void Handle_SubmitButtonSubmitSequence(object sender, EventArgs e)
        {
            _submitSound.Stop();
            OnSubmitSequence(new EventArgs());
        }

        private void Handle_SubmitButtonTouched(object sender, ButtonEventArgs e)
        {
            _submitSound.Play();
        }

        private void Handle_SubmitButtonReleased(object sender, ButtonEventArgs e)
        {
            _submitSound.Stop();
        }

        private void Handle_LogPanelLayerModeChangeStarted(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
        }

        protected virtual void OnSubmitSequence(EventArgs e)
        {
            if (this.SubmitSequence != null)
                this.SubmitSequence(this, e);
        }

        protected virtual void OnMenuButtonTapped(EventArgs e)
        {
            if (this.MenuButtonTapped != null)
                this.MenuButtonTapped(this, e);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);
        }
    }
}
