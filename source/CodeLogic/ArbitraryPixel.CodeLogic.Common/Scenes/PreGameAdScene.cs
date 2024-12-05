using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Advertising;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class PreGameAdScene : SceneBase
    {
        private const float MAX_AD_LOAD_TIME = 5f;
        private const string NEXTSCENE_NORMAL = "DeviceBoot";
        private const string NEXTSCENE_ADFAIL = "NoAdMessage";

        private ITexture2D _pixel;
        private IProgressLayer _progressLayer;
        private IAdProvider _adProvider;

        private bool _skipAd = false;
        private bool _adLaunched = false;

        private string _nextScene = NEXTSCENE_NORMAL;

        public PreGameAdScene(IEngine host)
            : base(host)
        {
            _adProvider = this.Host.GetComponent<IAdProvider>();

            if (_adProvider != null)
            {
                _adProvider.AdClosed += Handle_AdClosed;
                _adProvider.AdLoaded += Handle_AdLoaded;
            }
        }

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);

            bank.Put<ISpriteFont>("AdLoadTitleFont", this.Host.GrfxFactory.SpriteFontFactory.Create(content, @"Fonts\AdLoadTitleFont"));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");
            ILayer mainLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            mainLayer.AddEntity(
                GameObjectFactory.Instance.CreateTextureEntity(
                    this.Host,
                    new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World),
                    mainLayer.MainSpriteBatch,
                    _pixel,
                    Color.Black
                )
            );
            this.AddEntity(mainLayer);

            _progressLayer = GameObjectFactory.Instance.CreateProgressLayer(
                this.Host,
                mainLayer.MainSpriteBatch,
                this.Host.AssetBank.Get<ISpriteFont>("AdLoadTitleFont"),
                "Loading ad..."
            );
            _progressLayer.Minimum = 0;
            _progressLayer.Maximum = MAX_AD_LOAD_TIME;
            this.AddEntity(_progressLayer);
        }

        protected override void OnReset()
        {
            base.OnReset();
            _skipAd = false;
            _adLaunched = false;
            _progressLayer.Value = _progressLayer.Maximum;
            _progressLayer.Visible = false;
            _nextScene = NEXTSCENE_NORMAL;
            this.SceneComplete = false;

            if (_adProvider == null)
            {
                _skipAd = true;
            }
            else
            {
                if (_adProvider != null && _adProvider.AdReady == false)
                {
                    // If we don't have an ad yet, request one and wait a bit to load.
                    _adProvider.RequestAd();
                    _progressLayer.Visible = true;
                }
                else
                {
                    // We have an ad, we can move on.
                    _progressLayer.Value = 0;
                }
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (this._skipAd == true)
            {
                this.FinishScene();
            }
            else if (_progressLayer.Visible == true && _progressLayer.Value > 0)
            {
                _progressLayer.Value -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_progressLayer.Value <= 0)
                    _progressLayer.Value = 0;
            }
            else if (_progressLayer.Value <= 0 && _adLaunched == false)
            {
                _adLaunched = true;

                try
                {
                    if (_adProvider == null || _adProvider.AdReady == false)
                        throw new Exception(); // Just some exception to get to the catch.

                    _adProvider.ShowAd();
                }
                catch
                {
                    _skipAd = true;
                    _nextScene = NEXTSCENE_ADFAIL;
                }
            }
        }

        private void Handle_AdLoaded(object sender, EventArgs e)
        {
            _progressLayer.Visible = false;
            _progressLayer.Value = 0;
        }

        private void Handle_AdClosed(object sender, EventArgs e)
        {
            this.FinishScene();
        }

        private void FinishScene()
        {
            if (_nextScene == NEXTSCENE_ADFAIL)
            {
                // Failed to get an ad, hard cut to next scene.
                this.ChangeScene(this.Host.Scenes[_nextScene]);
            }
            else
            {
                // Going to a real scene to fade it in.
                this.ChangeScene(GameObjectFactory.Instance.CreateFadeSceneTransition(this.Host, this, this.Host.Scenes[_nextScene], FadeSceneTransitionMode.In, CodeLogicEngine.Constants.FadeSceneTransitionTime));
            }
        }
    }
}
