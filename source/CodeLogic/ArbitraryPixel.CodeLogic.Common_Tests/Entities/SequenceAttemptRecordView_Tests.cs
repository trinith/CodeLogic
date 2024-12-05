using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Entities;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class SequenceAttemptRecordView_Tests
    {
        private SequenceAttemptRecordView _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
        }

        private void Construct()
        {
            // Put this in a separate place so we can stage stuff :)
            _sut = new SequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, new Vector2(100, 100), _mockDeviceModel, 1);
        }

        #region Construct Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullSpriteBatchShouldThrowException()
        {
            _sut = new SequenceAttemptRecordView(_mockEngine, null, Vector2.Zero, _mockDeviceModel, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullDeviceModelShouldThrowException()
        {
            _sut = new SequenceAttemptRecordView(_mockEngine, _mockSpriteBatch, Vector2.Zero, null, 1);
        }

        [TestMethod]
        public void ConstructShouldGetFrameTextureFromAssetBank()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("HistoryAttemptFrame");
        }

        [TestMethod]
        public void ConstructShouldSetBoundsSizeToFrameTextureSize()
        {
            ITexture2D mockFrameTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrame").Returns(mockFrameTexture);
            mockFrameTexture.Width.Returns(200);
            mockFrameTexture.Height.Returns(100);

            Construct();

            Assert.AreEqual<RectangleF>(new RectangleF(100, 100, 200, 100), _sut.Bounds);
        }

        [TestMethod]
        public void ConstructShouldGetCurrentDeviceTheme()
        {
            IThemeManagerCollection mockThemes = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemes);

            Construct();

            mockThemes[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }

        [TestMethod]
        public void ConstructShouldGetFontFromAssetBank()
        {
            Construct();

            _mockEngine.AssetBank.Received(1).Get<ISpriteFont>("HistoryAttemptIndexFont");
        }

        [TestMethod]
        public void ConstructShouldMeasureTargetIndexString()
        {
            ISpriteFont mockFont = Substitute.For<ISpriteFont>();
            _mockEngine.AssetBank.Get<ISpriteFont>("HistoryAttemptIndexFont").Returns(mockFont);

            Construct();

            mockFont.Received(1).MeasureString("1");
        }
        #endregion

        #region Bounds Tests
        [TestMethod]
        public void BoundsSetShouldOnlyUpdateLocation()
        {
            ITexture2D mockFrameTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("HistoryAttemptFrame").Returns(mockFrameTexture);
            mockFrameTexture.Width.Returns(200);
            mockFrameTexture.Height.Returns(100);

            Construct();

            _sut.Bounds = new RectangleF(400, 500, 1, 1);

            Assert.AreEqual<RectangleF>(new RectangleF(400, 500, 200, 100), _sut.Bounds);
        }
        #endregion
    }
}
