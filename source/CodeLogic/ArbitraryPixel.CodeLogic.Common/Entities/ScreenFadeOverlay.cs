using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface IScreenFadeOverlay : IGameEntity
    {
        Color OpaqueColour { get; set; }
        FadeMode CurrentMode { get; set; }
        float CurrentValue { get; }
        event EventHandler<EventArgs> AnimationFinished;
    }

    public class ScreenFadeOverlay : GameEntityBase, IScreenFadeOverlay
    {
        private IAnimationCollection<float> _fadeAnimations;
        private ISpriteBatch _spriteBatch;
        private ITexture2D _overlayTexture;

        private FadeMode _currentMode;
        private IValueAnimation<float> _currentAnimation;

        public ScreenFadeOverlay(IEngine host, ISpriteBatch spriteBatch, ITexture2D overlayTexture, IAnimationFactory<float> animationFactory)
            : base(host, new RectangleF(0, 0, host.ScreenManager.World.X, host.ScreenManager.World.Y))
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _overlayTexture = overlayTexture ?? throw new ArgumentNullException();

            var animationFactoryCheck = animationFactory ?? throw new ArgumentNullException();

            _fadeAnimations = animationFactory.CreateAnimationCollection();
            _fadeAnimations.Add(
                FadeMode.FadeOut.ToString(),
                animationFactory.CreateValueAnimation(0f, new float[] { 1f, CodeLogicEngine.Constants.FadeSceneTransitionTime })
            );
            _fadeAnimations.Add(
                FadeMode.FadeIn.ToString(),
                animationFactory.CreateValueAnimation(1f, new float[] { 1f, 1f, 0f, CodeLogicEngine.Constants.FadeSceneTransitionTime })
            );

            this.CurrentMode = FadeMode.FadeIn;
        }

        private void SetupForMode(FadeMode mode)
        {
            _currentAnimation = _fadeAnimations[mode.ToString()];
            _currentAnimation.Reset();
        }

        #region IScreenFadeOverlay Implementation
        public Color OpaqueColour { get; set; } = Color.Black;

        public FadeMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                SetupForMode(_currentMode);
            }
        }

        public float CurrentValue
        {
            get { return _currentAnimation.Value; }
        }

        public event EventHandler<EventArgs> AnimationFinished;
        #endregion

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (!_currentAnimation.IsComplete)
            {
                _currentAnimation.Update(gameTime);

                if (_currentAnimation.IsComplete && this.AnimationFinished != null)
                    this.AnimationFinished(this, new EventArgs());
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Draw(_overlayTexture, this.Bounds, this.OpaqueColour * _currentAnimation.Value);
        }
    }
}
