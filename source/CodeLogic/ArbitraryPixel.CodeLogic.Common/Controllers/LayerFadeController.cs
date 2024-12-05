using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Animation;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface ILayerFadeController : IController
    {
        bool IsAnimating { get; }
        bool IsAnimationComplete(FadeMode mode);
        void AddLayer(ILayer layer);
        void StartAnimation(FadeMode mode);
    }

    public class LayerFadeController : ILayerFadeController
    {
        private List<ILayer> _layers = new List<ILayer>();
        private IAnimationCollection<float> _animations;
        private IValueAnimation<float> _currentAnimation;

        public LayerFadeController(IAnimationFactory<float> animationFactory)
            : this(animationFactory, CodeLogicEngine.Constants.FadeSceneTransitionTime)
        {
        }

        public LayerFadeController(IAnimationFactory<float> animationFactory, float fadeTransitionTime)
        {
            var tAnimationFactory = animationFactory ?? throw new ArgumentNullException();

            _animations = animationFactory.CreateAnimationCollection();
            _animations.Add(
                FadeMode.FadeOut.ToString(),
                animationFactory.CreateValueAnimation(1f, new float[] { 0f, fadeTransitionTime })
            );
            _animations.Add(
                FadeMode.FadeIn.ToString(),
                animationFactory.CreateValueAnimation(0f, new float[] { 1f, fadeTransitionTime })
            );
        }

        #region ILayerFadeController Implementation
        public bool IsAnimating => _currentAnimation != null;

        public bool IsAnimationComplete(FadeMode mode)
        {
            return _animations[mode.ToString()].IsComplete;
        }

        public void AddLayer(ILayer layer)
        {
            _layers.Add(layer ?? throw new ArgumentNullException());
        }

        public void Reset()
        {
            _currentAnimation = null;

            foreach (IValueAnimation<float> animation in _animations)
                animation.Reset();

            SetLayerOpacityForAnimation(_animations[FadeMode.FadeIn.ToString()]);
        }

        public void Update(GameTime gameTime)
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update(gameTime);

                SetLayerOpacityForAnimation(_currentAnimation);

                if (_currentAnimation.IsComplete)
                    _currentAnimation = null;
            }
        }

        public void StartAnimation(FadeMode mode)
        {
            _currentAnimation = _animations[mode.ToString()];
            _currentAnimation.Reset();
            SetLayerOpacityForAnimation(_currentAnimation);
        }
        #endregion

        private void SetLayerOpacityForAnimation(IValueAnimation<float> animation)
        {
            foreach (ILayer layer in _layers)
            {
                layer.MainSpriteBatch.Opacity = animation.Value;
                layer.Enabled = animation.IsComplete;
            }
        }
    }
}
