using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface ILogPanelLayer : ILayer
    {
        event EventHandler<EventArgs> ModeChangeStarted;
    }

    public class LogPanelLayer : LayerBase, ILogPanelLayer
    {
        private const float ANIMATE_TIME = 0.5f; // seconds

        private IDeviceModel _deviceModel;
        private ILogPanelModel _panelModel;

        private IButton _button;
        private IThemeManagerCollection _themeCollection;

        private Dictionary<IEntity, RectangleF> _initialBounds = new Dictionary<IEntity, RectangleF>();
        private List<ITexture2D> _recordTextures = new List<ITexture2D>();

        public event EventHandler<EventArgs> ModeChangeStarted;

        public LogPanelLayer(IEngine host, ISpriteBatch mainSpriteBatch, IDeviceModel deviceModel, ILogPanelModel panelModel)
            : base(host, mainSpriteBatch)
        {
            _deviceModel = deviceModel ?? throw new ArgumentNullException();
            _panelModel = panelModel ?? throw new ArgumentNullException();

            _themeCollection = this.Host.GetComponent<IThemeManagerCollection>();
            IDeviceTheme theme = _themeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            _panelModel.ModelReset += (sender, e) => UpdateEntityPosition(_button, _panelModel.CurrentOffset);

            SizeF logButtonSize = new SizeF(200f, 100f);
            RectangleF logButtonBounds = new RectangleF(_panelModel.WorldBounds.Width / 2f - logButtonSize.Width / 2f, _panelModel.WorldBounds.Height - logButtonSize.Height, logButtonSize.Width, logButtonSize.Height);

            IButtonObjectDefinitionFactory factory = GameObjectFactory.Instance.CreateButtonObjectDefinitionFactory();
            IGenericButton button = GameObjectFactory.Instance.CreateGenericButton(this.Host, logButtonBounds, mainSpriteBatch);
            button.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("LogButtonFill"), theme.SceneChangeBackgroundMask, SpriteEffects.None));
            button.AddButtonObject(factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("LogButtonBorder"), theme.SceneChangeBorderNormalMask, theme.SceneChangeBorderHighlightMask, SpriteEffects.None));

            IButtonObjectDefinition iconObject = factory.CreateButtonTextureDefinition(this.Host.AssetBank.Get<ITexture2D>("IconLog"), theme.SceneChangeBorderNormalMask, theme.SceneChangeBorderHighlightMask, SpriteEffects.None);
            iconObject.GlobalOffset = new Vector2(theme.SceneChangeIconOffset.Y, -1 * theme.SceneChangeIconOffset.X);
            button.AddButtonObject(iconObject);

            _button = button;
            _button.Tapped += LogButton_Tapped;
            this.AddEntity(_button);
            UpdateEntityPosition(_button, _panelModel.CurrentOffset);

            this.AddEntity(GameObjectFactory.Instance.CreateLogPanelContentLayer(this.Host, this.MainSpriteBatch, _deviceModel, _panelModel));
        }

        private void LogButton_Tapped(object sender, ButtonEventArgs e)
        {
            SetupNextMode();
        }

        private void SetupNextMode()
        {
            if (_panelModel.NextMode == null)
            {
                if (this.ModeChangeStarted != null)
                    this.ModeChangeStarted(this, new EventArgs());

                _panelModel.PreviousOffset = _panelModel.CurrentOffset;
                _panelModel.ProgressValue = Vector2.Zero;

                switch (_panelModel.CurrentMode)
                {
                    case LogPanelMode.Closed:
                        _panelModel.NextMode = LogPanelMode.PartialView;
                        _panelModel.TargetOffset = _panelModel.PartialSize;
                        break;
                    case LogPanelMode.PartialView:
                        _panelModel.NextMode = LogPanelMode.FullView;
                        _panelModel.TargetOffset = _panelModel.FullSize;
                        break;
                    case LogPanelMode.FullView:
                        _panelModel.NextMode = LogPanelMode.Closed;
                        _panelModel.TargetOffset = _panelModel.ClosedSize;
                        break;
                }

                // Animate from 0 to target over ANIMATE_TIME seconds.
                _panelModel.ProgressSpeed = _panelModel.ProgressTarget / new Vector2(ANIMATE_TIME);
            }
        }

        private void UpdateEntityPosition(IGameEntity entity, Vector2 offset)
        {
            if (!_initialBounds.ContainsKey(entity))
                _initialBounds.Add(entity, entity.Bounds);

            RectangleF currentBounds = entity.Bounds;
            entity.Bounds = new RectangleF(
                _initialBounds[entity].X - offset.X,
                _initialBounds[entity].Y - offset.Y,
                currentBounds.Width,
                currentBounds.Height
            );
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            bool updatePositions = (_panelModel.NextMode != null);
            _panelModel.Update(gameTime);

            if (updatePositions)
            {
                UpdateEntityPosition(_button, _panelModel.CurrentOffset);
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            ISpriteBatch sb = this.MainSpriteBatch;

            if (_panelModel.CurrentMode != LogPanelMode.Closed || _panelModel.NextMode != null)
            {
                IDeviceTheme theme = _themeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
                ITexture2D pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
                RectangleF bgBounds = new RectangleF(0, _button.Bounds.Bottom, _panelModel.WorldBounds.Width, _panelModel.WorldBounds.Height - _button.Bounds.Bottom);

                sb.Draw(pixel, bgBounds, theme.SceneChangeBorderNormalMask);
                bgBounds.Location += new Vector2(0, 2);
                sb.Draw(pixel, bgBounds, theme.SceneChangeBackgroundMask);
            }

            base.OnDraw(gameTime);
        }
    }
}
