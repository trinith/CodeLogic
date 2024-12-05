using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface ICloudIntensityController : IController
    {
        bool IsComplete { get; }
    }

    public class CloudIntensityController : ICloudIntensityController
    {
        private ICloud _cloud;
        private IValueAnimation<float> _animator;

        public bool IsComplete => _animator.IsComplete;

        public CloudIntensityController(ICloud cloud, IValueAnimation<float> intensityAnimator)
        {
            _cloud = cloud ?? throw new ArgumentNullException();
            _animator = intensityAnimator ?? throw new ArgumentNullException();

            _cloud.Intensity = _animator.Value;
        }

        #region IController Implementation
        public void Reset()
        {
            _animator.Reset();
            _cloud.Intensity = _animator.Value;
        }

        public void Update(GameTime gameTime)
        {
            if (!_animator.IsComplete)
                _animator.Update(gameTime);

            _cloud.Intensity = _animator.Value;
        }
        #endregion
    }
}
