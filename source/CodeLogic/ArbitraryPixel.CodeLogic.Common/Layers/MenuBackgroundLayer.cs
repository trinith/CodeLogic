using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuBackgroundLayer : ILayer
    {
        void Reset();
        void StopSounds();
        void StartEndSequence();
        void HideText();

        event EventHandler<EventArgs> SequenceEnded;
    }

    public class MenuBackgroundLayer : LayerBase, IMenuBackgroundLayer
    {
        public const float MaxPlaneAmbientVolume = 0.1f;

        private IController _planeController;
        private IScreenFadeOverlay _fadeOverlay;
        private ISoundPlaybackController _soundController;
        private IGameEntity _planeEntity;
        private IGameEntity _tapScreenText;
        private ISound _airplaneAmbientSound;

        public MenuBackgroundLayer(IEngine host, ISpriteBatch mainSpriteBatch, ISoundPlaybackController soundController)
            : base(host, mainSpriteBatch)
        {
            _soundController = soundController ?? throw new ArgumentNullException();

            PopulateEntities();
        }

        #region Private Methods
        private void PopulateEntities()
        {
            this.ClearEntities();

            var screenRect = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);

            IAnimationFactory<float> animationFactory = GameObjectFactory.Instance.CreateFloatAnimationFactory();

            // Need a separate layer for the backdrop so it doesn't render over the cloud layer.
            ILayer backDropLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            backDropLayer.AddEntity(GameObjectFactory.Instance.CreateTextureEntity(this.Host, screenRect, backDropLayer.MainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("MainMenuBackground"), Color.White));
            this.AddEntity(backDropLayer);

            IRandom random = GameObjectFactory.Instance.CreateRandom(1337);
            ISpriteBatch cloudBatch = this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice);
            ITexture2D[] cloudTextures =
                new ITexture2D[]
                {
                    this.Host.AssetBank.Get<ITexture2D>("cloud1"),
                    this.Host.AssetBank.Get<ITexture2D>("cloud1"),
                    this.Host.AssetBank.Get<ITexture2D>("cloud0"),
                    this.Host.AssetBank.Get<ITexture2D>("cloud1"),
                    this.Host.AssetBank.Get<ITexture2D>("cloud1"),
                };
            ICloudCoverLayer cloudCoverLayer = GameObjectFactory.Instance.CreateCloudCoverLayer(
                this.Host,
                cloudBatch,
                GameObjectFactory.Instance.CreateCloudGenerator((SizeF)this.Host.ScreenManager.World, cloudTextures, cloudBatch, random),
                GameObjectFactory.Instance.CreateCloudControllerFactory(),
                random,
                GameObjectFactory.Instance.CreateObjectSearcher()
            );
            cloudCoverLayer.LightningFlashed += Handle_CloudCoverLayerLightningFlashed;
            this.AddEntity(cloudCoverLayer);

            ITexture2D planeTexture = this.Host.AssetBank.Get<ITexture2D>("Plane");
            RectangleF planeRect = new RectangleF(Vector2.Zero, new SizeF(planeTexture.Width, planeTexture.Height));
            planeRect.Location = screenRect.Centre - planeRect.Size.Centre;
            _planeEntity = GameObjectFactory.Instance.CreateTextureEntity(this.Host, planeRect, this.MainSpriteBatch, planeTexture, Color.White);
            this.AddEntity(_planeEntity);

            _planeController = GameObjectFactory.Instance.CreatePlanePositionController(_planeEntity);

            this.AddEntity(
                _tapScreenText = GameObjectFactory.Instance.CreateTapScreenText(
                    this.Host,
                    this.MainSpriteBatch,
                    this.Host.AssetBank.Get<ISpriteFont>("CreditsTitleFont"),
                    animationFactory
                )
            );

            IBuildInfoStore buildInfoStore = this.Host.GetComponent<IBuildInfoStore>();
            string versionText = $"{buildInfoStore.BuildName} - {buildInfoStore.Version.ToString()} ({buildInfoStore.Platform})";
            ISpriteFont versionFont = this.Host.AssetBank.Get<ISpriteFont>("VersionFont");
            this.AddEntity(
                GameObjectFactory.Instance.CreateGenericTextLabel(
                    this.Host,
                    new Vector2(CodeLogicEngine.Constants.TextWindowPadding.Width, this.Host.ScreenManager.World.Y - CodeLogicEngine.Constants.TextWindowPadding.Height - versionFont.MeasureString(versionText).Height),
                    this.MainSpriteBatch,
                    versionFont,
                    versionText,
                    Color.White
                )
            );

            _fadeOverlay = GameObjectFactory.Instance.CreateScreenFadeOverlay(this.Host, this.MainSpriteBatch, this.Host.AssetBank.Get<ITexture2D>("Pixel"), animationFactory);
            _fadeOverlay.OpaqueColour = Color.Black;
            _fadeOverlay.AnimationFinished += Handle_FadeOverlayAnimationFinished;
            this.AddEntity(_fadeOverlay);

            _airplaneAmbientSound = this.Host.AssetBank.Get<ISoundResource>("AirplaneNormal").CreateInstance();
            _airplaneAmbientSound.Volume = 0.0f;
            _airplaneAmbientSound.IsLooped = true;
            _airplaneAmbientSound.Play();
        }

        private void Handle_FadeOverlayAnimationFinished(object sender, EventArgs e)
        {
            if (_fadeOverlay.CurrentMode == FadeMode.FadeOut && this.SequenceEnded != null)
            {
                this.SequenceEnded(this, new EventArgs());
            }
        }

        private void Handle_CloudCoverLayerLightningFlashed(object sender, LightningFlashEventArgs e)
        {
            var sound = this.Host.AssetBank.Get<ISoundResource>("Thunder1").CreateInstance();
            float delay = 0f;
            switch (e.Cloud.Depth)
            {
                case 1:
                    sound.Volume = 0.1f;
                    delay = 3f;
                    break;
                case 2:
                    sound.Volume = 0.5f;
                    delay = 2f;
                    break;
                case 3:
                default:
                    sound.Volume = 0.8f;
                    delay = 1f;
                    break;
            }

            _soundController.AddSound(sound, delay);
        }

        private void Handle_PlaneLandingControllerLandingFinished(object sender, EventArgs e)
        {
            _fadeOverlay.CurrentMode = FadeMode.FadeOut;
            _planeController = null;
        }

        private void Handle_SoundControllerEnabledChanged(object sender, StateChangedEventArgs<bool> e)
        {
            if (e.PreviousState == false && e.CurrentState == true)
                _airplaneAmbientSound.Play();
        }
        #endregion

        #region IMenuBackgroundLayer Implementation
        public void Reset()
        {
            _soundController.Reset();
            PopulateEntities();

            this.Host.AudioManager.SoundController.EnabledChanged += Handle_SoundControllerEnabledChanged;
        }

        public void StopSounds()
        {
            _soundController.Reset();

            this.Host.AudioManager.SoundController.EnabledChanged -= Handle_SoundControllerEnabledChanged;
        }

        public void StartEndSequence()
        {
            // Replace the existing plane controller with a landing controller.
            var landingController = GameObjectFactory.Instance.CreatePlaneLandingController(_planeEntity, (SizeF)this.Host.ScreenManager.World);
            landingController.LandingFinished += Handle_PlaneLandingControllerLandingFinished;
            _planeController = landingController;

            // Stop the current ambient sound and play the reduce power one.
            float currentVolume = _airplaneAmbientSound.Volume;
            _airplaneAmbientSound.Stop();
            _airplaneAmbientSound = this.Host.AssetBank.Get<ISoundResource>("AirplaneReducePowerFade").CreateInstance();
            _airplaneAmbientSound.Volume = currentVolume;
            _airplaneAmbientSound.Play();
        }

        public void HideText()
        {
            _tapScreenText.Visible = false;
            _tapScreenText.Alive = false;
        }

        public event EventHandler<EventArgs> SequenceEnded;
        #endregion

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _planeController?.Update(gameTime);
            _soundController.Update(gameTime);

            _airplaneAmbientSound.Volume = MathHelper.Lerp(0f, MaxPlaneAmbientVolume, 1f - _fadeOverlay.CurrentValue);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);
        }
    }
}
