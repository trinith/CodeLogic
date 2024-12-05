using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public interface IPlaneLandingController : IController
    {
        event EventHandler<EventArgs> LandingFinished;
    }

    public class PlaneLandingController : IPlaneLandingController
    {
        public static class Constants
        {
            public const float InitialDescentVelocity = 25f;
            public const float DecentAcceleration = 9.81f * 50;
        }

        private IGameEntity _planeEntity;
        private Vector2 _originalPosition;
        private Vector2 _dir;
        private float _descentVelocity;
        private SizeF _screenSize;

        public event EventHandler<EventArgs> LandingFinished;

        public PlaneLandingController(IGameEntity planeEntity, SizeF screenSize)
        {
            _screenSize = screenSize;
            _planeEntity = planeEntity ?? throw new ArgumentNullException();

            _originalPosition = _planeEntity.Bounds.Location;

            float angle = 75;
            float angleRadians = MathHelper.ToRadians(angle);

            _dir = new Vector2(
                (float)Math.Cos(angleRadians),
                (float)Math.Sin(angleRadians)
            );
            _dir.Normalize();

            _descentVelocity = Constants.InitialDescentVelocity;
        }

        public void Reset()
        {
            _planeEntity.Bounds = new RectangleF(_originalPosition, _planeEntity.Bounds.Size);
            _descentVelocity = Constants.InitialDescentVelocity;
        }

        public void Update(GameTime gameTime)
        {
            if (_planeEntity.Bounds.Top < _screenSize.Height)
            {
                _descentVelocity += Constants.DecentAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 newPos = _planeEntity.Bounds.Location + _dir * _descentVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _planeEntity.Bounds = new RectangleF(newPos, _planeEntity.Bounds.Size);

                if (_planeEntity.Bounds.Top >= _screenSize.Height && this.LandingFinished != null)
                    this.LandingFinished(this, new EventArgs());
            }
        }
    }
}
