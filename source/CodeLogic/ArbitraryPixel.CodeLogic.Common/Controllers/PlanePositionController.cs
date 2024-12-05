using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public class PlanePositionController : IController
    {
        private IGameEntity _planeEntity;
        private Vector2 _originalPosition;
        private SizeF _originalSize;
        private float _cycleTime = 7f;
        private float _input = 0f;
        private float _inputVelocity = 0f;
        private float _offsetScale = 10f;

        public PlanePositionController(IGameEntity planeEntity)
        {
            _planeEntity = planeEntity ?? throw new ArgumentNullException();

            _originalPosition = _planeEntity.Bounds.Location;
            _originalSize = _planeEntity.Bounds.Size;
            _inputVelocity = (MathHelper.TwoPi / _cycleTime);
        }

        public void Reset()
        {
            _input = 0f;
            UpdateEntityPositionForInput();
        }

        public void Update(GameTime gameTime)
        {
            _input += _inputVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_input > MathHelper.TwoPi)
                _input -= MathHelper.TwoPi;

            UpdateEntityPositionForInput();
        }

        private void UpdateEntityPositionForInput()
        {
            Vector2 offset = new Vector2(0, (float)Math.Sin(_input) * _offsetScale);

            _planeEntity.Bounds = new RectangleF(
                _originalPosition + offset,
                _originalSize
            );
        }
    }
}
