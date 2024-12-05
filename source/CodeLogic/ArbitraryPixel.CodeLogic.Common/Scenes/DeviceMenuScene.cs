using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class DeviceMenuScene : SceneBase
    {
        private IDeviceModel _model;
        private ITextObjectBuilder _textBuilder;
        private IDialog _abortDialog = null;
        private IDialog _settingsDialog = null;
        private IMenuContentLayer _settingsContent = null;
        private bool _abort = false;

        public DeviceMenuScene(IEngine host, IDeviceModel model)
            : base(host)
        {
            _model = model ?? throw new ArgumentNullException();

            this.Host.ExternalActionOccurred += Handle_ExternalActionOccurred;
        }

        protected override void OnReset()
        {
            base.OnReset();

            _abort = false;
            this.SceneComplete = false;
        }

        protected override void OnEnding()
        {
            base.OnEnding();

            if (_abort)
            {
                this.Host.AudioManager.MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _textBuilder = GameObjectFactory.Instance.CreateTextObjectBuilderWithConsoleFonts(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory(),
                this.Host.AssetBank
            );

            this.AddEntity(CreateBackgroundLayer());
            this.AddEntity(CreateUILayer());

            this.AddEntity(_abortDialog = CreateAbortDialog());
            this.AddEntity(_settingsDialog = CreateSettingsDialog());
        }

        private ILayer CreateBackgroundLayer()
        {
            IDeviceBackground background;
            ILayer bgLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            bgLayer.AddEntity(background = GameObjectFactory.Instance.CreateDeviceBackground(this.Host, new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World), bgLayer.MainSpriteBatch));
            background.Colour = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().BackgroundImageMask;

            return bgLayer;
        }

        private ILayer CreateUILayer()
        {
            IDeviceMenuUILayer uiLayer = GameObjectFactory.Instance.CreateDeviceMenuUILayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            uiLayer.ReturnButtonTapped += Handle_ReturnButtonTapped;
            uiLayer.AbortButtonTapped += Handle_AbortButtonTapped;
            uiLayer.SettingsButtonTapped += Handle_SettingsButtonTapped;

            return uiLayer;
        }

        private IDialog CreateAbortDialog()
        {
            IDeviceTheme currentTheme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            RectangleF windowBounds = new RectangleF(0, 0, 750, 200);
            windowBounds.Location = ((SizeF)this.Host.ScreenManager.World).Centre - windowBounds.Size.Centre;
            IDialog dialog = GameObjectFactory.Instance.CreateOkCancelDialog(
                this.Host,
                windowBounds,
                _textBuilder,
                "{TPC:0}{C:White}Agent, by leaving now all mission progress will be lost!\n\n{C:Red}Abort mission{C:White} and request immediate evac?"
            );
            dialog.DialogClosed += Handle_AbortDialogClosed;

            dialog.BackgroundColour = currentTheme.BackgroundColourMask;
            dialog.BorderColour = currentTheme.NormalColourMask;

            return dialog;
        }

        private IDialog CreateSettingsDialog()
        {
            IDeviceTheme currentTheme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            RectangleF windowBounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World * 0.75f);
            windowBounds.Location = ((SizeF)this.Host.ScreenManager.World).Centre - windowBounds.Size.Centre;

            IDialog dialog = GameObjectFactory.Instance.CreateDialog(this.Host, windowBounds, _textBuilder, "");

            var factory = GameObjectFactory.Instance.CreateMainMenuContentLayerFactory();
            _settingsContent = factory.CreateMenuSettingsContentLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), dialog.ClientRectangle);
            _settingsContent.BlendState = BlendState.NonPremultiplied;
            dialog.AddContentLayer(_settingsContent);

            dialog.BackgroundColour = currentTheme.BackgroundColourMask;
            dialog.BorderColour = currentTheme.NormalColourMask;

            return dialog;
        }

        #region Event Handlers
        private void Handle_AbortDialogClosed(object sender, DialogClosedEventArgs e)
        {
            if (e.Result == DialogResult.Ok)
            {
                _abort = true;
                this.ChangeScene(GameObjectFactory.Instance.CreateFadeSceneTransition(this.Host, this, this.Host.Scenes["MissionDebriefing"], FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime));
            }
        }

        private void Handle_ReturnButtonTapped(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            this.ChangeScene(GameObjectFactory.Instance.CreatePanSceneTransition(this.Host, this, this.Host.Scenes["DeviceMain"], PanSceneTransitionMode.PanLeft, CodeLogicEngine.Constants.DeviceSceneTransitionTime));
        }

        private void Handle_AbortButtonTapped(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            _abortDialog.Show();
        }

        private void Handle_SettingsButtonTapped(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            
            // Tell our content layer it should update its controls.
            _settingsContent.Show();

            // Tell the dialog it can show itself.
            _settingsDialog.Show();
        }

        private void Handle_ExternalActionOccurred(object sender, ExternalActionEventArgs e)
        {
            if (e.Data != null && e.Data.Equals(CodeLogicEngine.Constants.ExternalActions.BackPressed) && this.Host.CurrentScene == this)
            {
                if (_abortDialog.IsOpen)
                    _abortDialog.Close();
                else
                    _abortDialog.Show();
            }
        }
        #endregion
    }
}
