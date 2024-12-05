using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class PlanePositionController_Tests
    {
        private PlanePositionController _sut;
        private IGameEntity _mockEntity;

        private Vector2 _pos = new Vector2(200, 100);
        private SizeF _size = new SizeF(400, 300);
        private RectangleF _bounds;

        [TestInitialize]
        public void Initialize()
        {
            _bounds = new RectangleF(_pos, _size);

            _mockEntity = Substitute.For<IGameEntity>();
            _mockEntity.Bounds.Returns(_bounds);
        }

        private void Construct()
        {
            _sut = new PlanePositionController(_mockEntity);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_PlaneEntity()
        {
            _sut = new PlanePositionController(null);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetEntityPositionToOriginalPosition()
        {
            Construct();
            _mockEntity.Bounds = new RectangleF(100, 100, 300, 300);

            _sut.Reset();

            _mockEntity.Received(1).Bounds = _bounds;
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldSetPositionToExpectedValue()
        {
            Construct();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2.5)));

            Vector2 offset = new Vector2(0, (float)Math.Sin((2.5f * (MathHelper.TwoPi / 7f))) * 10f);
            _mockEntity.Received(1).Bounds = new RectangleF(_pos + offset, _size);
        }

        [TestMethod]
        public void UpdateAfterFullCycleShouldSetPositionToExpectedValue()
        {
            Construct();

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(8.5)));

            // Use 1.5 seconds because 8.5 - 7 = 1.5 seconds. 7 seconds is the cycle time :)
            Vector2 offset = new Vector2(0, (float)Math.Sin((1.5f * (MathHelper.TwoPi / 7f))) * 10f);
            _mockEntity.Received(1).Bounds = new RectangleF(_pos + offset, _size);
        }
        #endregion
    }
}
