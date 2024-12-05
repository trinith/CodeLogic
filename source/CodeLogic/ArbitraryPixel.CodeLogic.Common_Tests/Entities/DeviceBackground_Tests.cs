using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class DeviceBackground_Tests : UnitTestBase<DeviceBackground>
    {
        private IEngine _mockHost;
        private IGrfxDevice _mockGrfxDevice;
        private ISpriteBatch _mockSpriteBatch;
        private IContentManager _mockContentManager;
        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockHost = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockHost.Graphics.GraphicsDevice.Returns(_mockGrfxDevice = Substitute.For<IGrfxDevice>());
            _mockHost.Content.Returns(_mockContentManager = Substitute.For<IContentManager>());
            _mockHost.ScreenManager.ScaleMatrix.Returns(Matrix.CreateScale(1, 2, 3));
            _mockHost.ScreenManager.World.Returns(new Point(33, 44));
        }

        protected override DeviceBackground OnCreateSUT()
        {
            return new DeviceBackground(_mockHost, _bounds, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowException()
        {
            _sut = new DeviceBackground(_mockHost, _bounds, null);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldCallSpriteBatchDraw()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockHost.AssetBank.Get<ITexture2D>("DeviceBackground").Returns(mockTexture);
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds, Color.White);
        }

        [TestMethod]
        public void DrawShouldCallSpriteBatchDrawWithExpectedColour()
        {
            _sut.Colour = Color.Pink;
            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(Arg.Any<ITexture2D>(), _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldRequestDeviceBackgroundTextureFromAssetBank()
        {
            _sut.Draw(new GameTime());

            _mockHost.AssetBank.Received(1).Get<ITexture2D>("DeviceBackground");
        }
        #endregion
    }
}
