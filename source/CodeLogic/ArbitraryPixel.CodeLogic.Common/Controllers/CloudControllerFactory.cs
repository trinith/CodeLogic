using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.Common;
using ArbitraryPixel.Platform2D.Animation;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface ICloudControllerFactory
    {
        ICloudMovementController CreateCloudMovementController(ICloud cloud, IEntityGenerator<ICloud> cloudGenerator, Vector2 movementDirection);
        ICloudFlashController CreateCloudFlashController(ICloud[] clouds, IRandom random, ICloudControllerFactory cloudControllerFactory, IObjectSearcher objectSearcher);
        ICloudIntensityController CreateCloudIntensityController(ICloud cloud, IValueAnimation<float> animator);
        IValueAnimation<float> CreateCloudIntensityAnimation(IRandom random);
        IValueAnimation<float> CreateCloudIntensityAnimation(int outcome);
    }

    public class CloudControllerFactory : ICloudControllerFactory
    {
        private IAnimationSetPoint<float>[][] _availableSetPoints = null;
        private IAnimationSetPoint<float>[][] AvailableSetPoints
        {
            get
            {
                if (_availableSetPoints == null)
                {
                    _availableSetPoints = new IAnimationSetPoint<float>[][]
                    {
                        new IAnimationSetPoint<float>[]
                        {
                            new AnimationSetPoint<float>(2.2f, 0.10f),
                            new AnimationSetPoint<float>(0.5f, 0.10f),
                            new AnimationSetPoint<float>(0.75f, 0.05f),
                            new AnimationSetPoint<float>(0.75f, 2.00f),
                        },

                        new IAnimationSetPoint<float>[]
                        {
                            new AnimationSetPoint<float>(1.50f, 0.10f),
                            new AnimationSetPoint<float>(0.75f, 0.10f),
                            new AnimationSetPoint<float>(0.75f, 1.00f),
                            new AnimationSetPoint<float>(2.20f, 0.10f),
                            new AnimationSetPoint<float>(1.50f, 0.10f),
                            new AnimationSetPoint<float>(2.20f, 0.10f),
                            new AnimationSetPoint<float>(0.75f, 0.10f),
                            new AnimationSetPoint<float>(0.75f, 2.00f),
                        },

                        new IAnimationSetPoint<float>[]
                        {
                            new AnimationSetPoint<float>(2.20f, 0.10f),
                            new AnimationSetPoint<float>(0.50f, 0.10f),
                            new AnimationSetPoint<float>(2.20f, 0.10f),
                            new AnimationSetPoint<float>(0.90f, 0.25f),
                            new AnimationSetPoint<float>(1.15f, 0.25f),
                            new AnimationSetPoint<float>(0.90f, 0.25f),
                            new AnimationSetPoint<float>(1.15f, 0.25f),
                            new AnimationSetPoint<float>(0.90f, 0.25f),
                            new AnimationSetPoint<float>(1.15f, 0.25f),
                            new AnimationSetPoint<float>(0.75f, 1.00f),
                            new AnimationSetPoint<float>(0.75f, 2.00f),
                        },
                    };
                }

                return _availableSetPoints;
            }
        }
        private IAnimationSetPoint<float>[] GetAnimationSetPoints(IRandom random)
        {
            return GetAnimationSetPoints(random.Next(0, 10));
        }
        private IAnimationSetPoint<float>[] GetAnimationSetPoints(int outcome)
        {
            int test = MathHelper.Clamp(outcome, 0, 9);
            IAnimationSetPoint<float>[] points = null;
            if (test >= 9)
                points = this.AvailableSetPoints[2];
            else if (test >= 6 && test < 9)
                points = this.AvailableSetPoints[1];
            else
                points = this.AvailableSetPoints[0];

            return points;
        }

        public ICloudIntensityController CreateCloudIntensityController(ICloud cloud, IValueAnimation<float> animator)
        {
            return new CloudIntensityController(cloud, animator);
        }

        public ICloudFlashController CreateCloudFlashController(ICloud[] clouds, IRandom random, ICloudControllerFactory cloudControllerFactory, IObjectSearcher objectSearcher)
        {
            return new CloudFlashController(clouds, random, cloudControllerFactory, objectSearcher);
        }

        public ICloudMovementController CreateCloudMovementController(ICloud cloud, IEntityGenerator<ICloud> cloudGenerator, Vector2 movementDirection)
        {
            return new CloudMovementController(cloud, cloudGenerator, movementDirection);
        }

        public IValueAnimation<float> CreateCloudIntensityAnimation(IRandom random)
        {
            IAnimationSetPoint<float>[] animationSetPoints = GetAnimationSetPoints(random);

            IValueAnimation<float> animation = new FloatValueAnimation(0.75f, animationSetPoints);
            animation.IsLooping = false;

            return animation;
        }

        public IValueAnimation<float> CreateCloudIntensityAnimation(int outcome)
        {
            IAnimationSetPoint<float>[] animationSetPoints = GetAnimationSetPoints(outcome);

            IValueAnimation<float> animation = new FloatValueAnimation(0.75f, animationSetPoints);
            animation.IsLooping = false;

            return animation;
        }
    }
}
