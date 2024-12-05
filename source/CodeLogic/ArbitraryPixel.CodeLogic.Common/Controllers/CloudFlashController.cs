using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public class LightningFlashEventArgs : EventArgs
    {
        public ICloud Cloud { get; }

        public LightningFlashEventArgs(ICloud cloud)
        {
            this.Cloud = cloud ?? throw new ArgumentNullException();
        }
    }

    public interface ICloudFlashController : IController
    {
        bool IsFlashing(ICloud cloud);
        event EventHandler<LightningFlashEventArgs> LightningFlashed;
    }

    public class CloudFlashController : ICloudFlashController
    {
        private ICloud[] _clouds;
        private IRandom _random;
        private ICloudControllerFactory _cloudControllerFactory;
        private IObjectSearcher _objectSearcher;

        private double? _nextFlash = null;
        private Dictionary<ICloud, ICloudIntensityController> _intensityControllers = new Dictionary<ICloud, ICloudIntensityController>();
        private bool _hadFlash = false;

        public CloudFlashController(ICloud[] clouds, IRandom random, ICloudControllerFactory cloudControllerFactory, IObjectSearcher objectSearcher)
        {
            _clouds = clouds ?? throw new ArgumentNullException();
            _random = random ?? throw new ArgumentNullException();
            _cloudControllerFactory = cloudControllerFactory ?? throw new ArgumentNullException();
            _objectSearcher = objectSearcher ?? throw new ArgumentNullException();

            if (_clouds.Length == 0)
                throw new ArgumentException("Must have at least one ICloud object present.", "clouds");

            _nextFlash = null;
        }

        #region ICloudFlashController Implementation
        public event EventHandler<LightningFlashEventArgs> LightningFlashed;

        public bool IsFlashing(ICloud cloud)
        {
            return _intensityControllers.ContainsKey(cloud);
        }
        #endregion

        private bool IsCloudWithinRange(ICloud targetObject, ICloud compareObject)
        {
            bool result = false;

            float dist = (targetObject.Bounds.Centre - compareObject.Bounds.Centre).Length();

            if (targetObject == compareObject || (dist < 150 && targetObject.Depth == compareObject.Depth))
                result = true;

            return result;
        }

        public void Reset()
        {
            _intensityControllers.Clear();
            _nextFlash = null;
            _hadFlash = false;
        }

        public void Update(GameTime gameTime)
        {
            // Update any existing flashes. Do this before creating any new ones.
            ICloud[] activeClouds = _intensityControllers.Keys.ToArray();
            foreach (ICloud cloud in activeClouds)
            {
                _intensityControllers[cloud].Update(gameTime);
                if (_intensityControllers[cloud].IsComplete)
                    _intensityControllers.Remove(cloud);
            }

            // Check if we should create a new flash
            if (_nextFlash != null)
            {
                _nextFlash -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_nextFlash <= 0)
                {
                    ICloud targetCloud = _clouds[_random.Next(0, _clouds.Length)];
                    ICloud[] cloudsToFlash = _objectSearcher.GetMatchingObjects<ICloud>(_clouds, targetCloud, IsCloudWithinRange);

                    if (cloudsToFlash != null && cloudsToFlash.Length > 0)
                    {
                        int outcome = _random.Next(0, 10);

                        foreach (ICloud cloud in cloudsToFlash)
                        {
                            if (_intensityControllers.ContainsKey(cloud) == false)
                            {
                                IValueAnimation<float> animation = _cloudControllerFactory.CreateCloudIntensityAnimation(outcome);
                                _intensityControllers.Add(
                                    cloud,
                                    _cloudControllerFactory.CreateCloudIntensityController(cloud, animation)
                                );
                            }
                        }

                        if (this.LightningFlashed != null)
                            this.LightningFlashed(this, new LightningFlashEventArgs(targetCloud));
                    }

                    _nextFlash = null;
                }
            }

            // Create a new time for a flash.
            if (_nextFlash == null)
            {
                _nextFlash = (!_hadFlash) ? _random.Next(2, 3) : _random.Next(10, 15);
                _hadFlash = true;
            }
        }
    }
}
