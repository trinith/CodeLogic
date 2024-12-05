using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.Common.Drawing;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface ICloudMovementController : IController
    {
        ICloud Cloud { get; }
    }

    public class CloudMovementController : ICloudMovementController
    {
        private ICloud _cloud;
        private IEntityGenerator<ICloud> _cloudGenerator;
        private Vector2 _direction;

        private Vector2 _startPosition;

        public ICloud Cloud => _cloud;

        public CloudMovementController(ICloud cloud, IEntityGenerator<ICloud> cloudGenerator, Vector2 movementDirection)
        {
            _cloud = cloud ?? throw new ArgumentNullException();
            _cloudGenerator = cloudGenerator ?? throw new ArgumentNullException();
            _direction = movementDirection;

            _startPosition = cloud.Bounds.Location;
        }

        public void Reset()
        {
            _cloud.Bounds = new RectangleF(
                _startPosition,
                _cloud.Bounds.Size
            );
        }

        public void Update(GameTime gameTime)
        {
            float speed = 2;
            float factor = 10f * _cloud.Depth;
            _cloud.Bounds = new RectangleF(
                _cloud.Bounds.Location + (speed * factor * _direction) * (float)gameTime.ElapsedGameTime.TotalSeconds,
                _cloud.Bounds.Size
            );

            if (_cloud.Bounds.Location.X + _cloud.Texture.Width < 0)
                _cloudGenerator.RepositionEntity(_cloud);
        }
    }
}
