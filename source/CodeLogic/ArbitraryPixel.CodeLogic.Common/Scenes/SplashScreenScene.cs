using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Logging;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.UI;
using ArbitraryPixel.Platform2D.UI.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class SplashScreenScene : SceneBase
    {
        private IUIObjectFactory _uiObjectFactory;
        private ILayerFadeController _layerFadeController;
        private ILayer _logoBGLayer;
        private bool _wasTapped = false;
        private bool _fadeOutStarted = false;
        private double _viewDelay = CodeLogicEngine.Constants.SplashScreenViewTime;
        private double _startDelay = CodeLogicEngine.Constants.SplashScreenFadeInDelay;

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            ITexture2DFactory textureFactory = this.Host.GrfxFactory.Texture2DFactory;
            bank.Put<ITexture2D>("APLogo", textureFactory.Create(content, @"Textures\APLogo"));
        }

        public SplashScreenScene(IEngine host, IUIObjectFactory uiObjectFactory)
            : base(host)
        {
            _uiObjectFactory = uiObjectFactory ?? throw new ArgumentNullException();

            _layerFadeController = GameObjectFactory.Instance.CreateLayerFadeController(GameObjectFactory.Instance.CreateFloatAnimationFactory(), CodeLogicEngine.Constants.SplashScreenFadeTime);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Want the screen to start white, logo fade in, then both logo and white fade to black. Gonna need a bunch of layers... and it'll be dumb, I promise!

            ITexture2D pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");

            ITexture2D logoTexture = this.Host.AssetBank.Get<ITexture2D>("APLogo");
            SizeF screenSize = (SizeF)this.Host.ScreenManager.World;
            SizeF textureSize = new SizeF(logoTexture.Width * 2, logoTexture.Height * 2);

            // Background Layer
            ILayer bgLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));

            IGenericButton button = _uiObjectFactory.CreateGenericButton(this.Host, new RectangleF(Vector2.Zero, screenSize), bgLayer.MainSpriteBatch);
            button.Tapped += Handle_ButtonTapped;
            bgLayer.AddEntity(button);

            bgLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, new RectangleF(Vector2.Zero, screenSize), bgLayer.MainSpriteBatch, pixel, Color.Black));
            this.AddEntity(bgLayer);

            // White Layer
            _logoBGLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _logoBGLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, new RectangleF(Vector2.Zero, screenSize), _logoBGLayer.MainSpriteBatch, pixel, Color.White));
            this.AddEntity(_logoBGLayer);

            // Logo Layer
            ILayer logoLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            ITextureEntity logo = GameObjectFactory.Instance.CreateTextureEntity(
                this.Host,
                new RectangleF(screenSize.Centre - textureSize.Centre, textureSize),
                logoLayer.MainSpriteBatch,
                logoTexture,
                Color.White
            );

            logoLayer.AddEntity(logo);
            this.AddEntity(logoLayer);

            _layerFadeController.AddLayer(logoLayer);
            _layerFadeController.Reset();
        }

        private void Handle_ButtonTapped(object sender, ButtonEventArgs e)
        {
            _wasTapped = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_startDelay > 0)
            {
                _startDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_startDelay <= 0 || _wasTapped)
                {
                    _startDelay = 0;
                    _layerFadeController.StartAnimation(FadeMode.FadeIn);
                }
            }

            if (_layerFadeController.IsAnimationComplete(FadeMode.FadeIn) && _viewDelay > 0)
            {
                _viewDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_viewDelay <= 0)
                    _viewDelay = 0;
            }

            if ((_wasTapped == true || _viewDelay == 0) && _fadeOutStarted == false && _layerFadeController.IsAnimationComplete(FadeMode.FadeIn))
            {
                _layerFadeController.AddLayer(_logoBGLayer);
                _layerFadeController.StartAnimation(FadeMode.FadeOut);
                _fadeOutStarted = true;
            }

            _layerFadeController.Update(gameTime);

            if (_layerFadeController.IsAnimationComplete(FadeMode.FadeOut))
                this.ChangeScene(this.Host.Scenes["MainMenu"]);
        }
    }
}
