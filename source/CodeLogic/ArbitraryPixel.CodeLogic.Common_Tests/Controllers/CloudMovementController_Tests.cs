using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.EntityGenerators;
using ArbitraryPixel.Common.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class CloudMovementController_Tests
    {
        private CloudMovementController _sut;

        private ICloud _mockCloud;
        private IEntityGenerator<ICloud> _mockCloudGenerator;

        private Vector2 _dir;

        [TestInitialize]
        public void Initialize()
        {
            _mockCloud = Substitute.For<ICloud>();
            _mockCloudGenerator = Substitute.For<IEntityGenerator<ICloud>>();

            _dir = new Vector2(1, 1);
            _dir.Normalize();
        }

        private void Construct()
        {
            _sut = new CloudMovementController(_mockCloud, _mockCloudGenerator, _dir);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_Cloud()
        {
            _sut = new CloudMovementController(null, _mockCloudGenerator, _dir);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowExpectedException_CloudGenerator()
        {
            _sut = new CloudMovementController(_mockCloud, null, _dir);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetShouldSetCloudBoundsToOriginalPosition()
        {
            _mockCloud.Bounds.Returns(new RectangleF(123, 456, 321, 654));
            Construct();

            _sut.Reset();

            _mockCloud.Received(1).Bounds = new RectangleF(123, 456, 321, 654);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldSetCloudBoundsToExpectedValue_TestA()
        {
            _mockCloud.Bounds.Returns(new RectangleF(200, 100, 400, 300));
            _mockCloud.Depth.Returns(3);

            Construct();

            GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            _sut.Update(gameTime);

            _mockCloud.Received(1).Bounds = new RectangleF(
                new Vector2(200, 100) + (2f * 10f * 3f * _dir) * 1f,
                new SizeF(400, 300)
            );
        }

        [TestMethod]
        public void UpdateShouldSetCloudBoundsToExpectedValue_TestB()
        {
            _mockCloud.Bounds.Returns(new RectangleF(500, 400, 400, 300));
            _mockCloud.Depth.Returns(6);

            Construct();

            GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2));
            _sut.Update(gameTime);

            _mockCloud.Received(1).Bounds = new RectangleF(
                new Vector2(500, 400) + (2f * 10f * 6f * _dir) * 2f,
                new SizeF(400, 300)
            );
        }

        [TestMethod]
        public void UpdateWhenCloudOffScreenShouldRepositionCloud()
        {
            _mockCloud.Bounds.Returns(new RectangleF(-100, 400, 400, 300));
            _mockCloud.Texture.Width.Returns(50);

            Construct();

            _sut.Update(new GameTime());

            _mockCloudGenerator.Received(1).RepositionEntity(_mockCloud);
        }

        [TestMethod]
        public void UpdateWhenCloudNotOffScreenShouldNotRepositionCloud()
        {
            _mockCloud.Bounds.Returns(new RectangleF(-100, 400, 400, 300));
            _mockCloud.Texture.Width.Returns(150);

            Construct();

            _sut.Update(new GameTime());

            _mockCloudGenerator.Received(0).RepositionEntity(_mockCloud);
        }
        #endregion
    }
}
