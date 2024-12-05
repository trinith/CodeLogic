using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.CodeLogic.Common.Model;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Theme;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.CodeLogic.Common;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class StatusBar_Tests
    {
        private StatusBar _sut;
        private IEngine _mockEngine;
        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;

        private IDeviceTheme _mockDeviceTheme;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _mockDeviceTheme = Substitute.For<IDeviceTheme>();

            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme);

            _sut = new StatusBar(_mockEngine, _bounds, _mockSpriteBatch, _mockDeviceModel);
        }

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBackgroundFill()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarBackgroundFill").Returns(mockTexture);
            _mockDeviceTheme.StatusIndicatorBackgroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawBackgroundBorder()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarBackgroundBorder").Returns(mockTexture);
            _mockDeviceTheme.StatusIndicatorBorderMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawProgressBackground()
        {
            ITexture2D mockFrameTexture = Substitute.For<ITexture2D>();
            mockFrameTexture.Width.Returns(100);
            mockFrameTexture.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockFrameTexture);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFillBackground").Returns(mockTexture);
            _mockDeviceTheme.StatusIndicatorProgressBackgroundMask.Returns(Color.Pink);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, (Rectangle)expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawProgressFrame()
        {
            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            mockTexture.Width.Returns(100);
            mockTexture.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockTexture);
            _mockDeviceTheme.StatusIndicatorProgressBorderMask.Returns(Color.Pink);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, (Rectangle)expectedBounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawProgressFillWithRespectToCurrentTrial_Test1()
        {
            _mockDeviceModel.CurrentTrial.Returns(2);

            ITexture2D mockFrame = Substitute.For<ITexture2D>();
            mockFrame.Width.Returns(100);
            mockFrame.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockFrame);

            ITexture2D mockFill = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFill").Returns(mockFill);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _mockDeviceTheme.StatusIndicatorProgressFrameCellBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressFrameCellSize.Returns(new SizeF(5));
            _mockDeviceTheme.StatusIndicatorProgressFrameBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.AlarmLowMask.Returns(Color.Pink);

            Rectangle expectedClip = new Rectangle(0, 0, 1 + (1 * (5 + 1)), 50);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockFill, expectedBounds.Location, expectedClip, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawProgressFillWithRespectToCurrentTrial_Test2()
        {
            _mockDeviceModel.CurrentTrial.Returns(8);
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);

            ITexture2D mockFrame = Substitute.For<ITexture2D>();
            mockFrame.Width.Returns(100);
            mockFrame.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockFrame);

            ITexture2D mockFill = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFill").Returns(mockFill);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _mockDeviceTheme.StatusIndicatorProgressFrameCellBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressFrameCellSize.Returns(new SizeF(5));
            _mockDeviceTheme.StatusIndicatorProgressFrameBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.AlarmHighMask.Returns(Color.Pink);

            Rectangle expectedClip = new Rectangle(0, 0, 1 + (7 * (5 + 1)), 50);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockFill, expectedBounds.Location, expectedClip, Color.Pink);
        }

        [TestMethod]
        public void DrawWithCurrentTrialOneShouldNotDrawProgressFill()
        {
            _mockDeviceModel.CurrentTrial.Returns(1);
            ITexture2D mockFill = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFill").Returns(mockFill);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockFill, Arg.Any<Vector2>(), Arg.Any<Rectangle>(), Arg.Any<Color>());
        }

        [TestMethod]
        public void DrawShouldDrawCurrentTrialIndicator()
        {
            _mockDeviceModel.CurrentTrial.Returns(6);
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);

            ITexture2D mockFrame = Substitute.For<ITexture2D>();
            mockFrame.Width.Returns(100);
            mockFrame.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockFrame);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarCurrentTrialIndicator").Returns(mockTexture);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _mockDeviceTheme.StatusIndicatorProgressFrameCellBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressFrameCellSize.Returns(new SizeF(5));
            _mockDeviceTheme.StatusIndicatorProgressFrameBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressBorderMask.Returns(Color.Pink);

            Rectangle expectedClip = new Rectangle(0, 0, 1 + (5 * (5 + 1)), 50);

            Vector2 expectedLocation = expectedBounds.Location + new Vector2(expectedClip.Width, 1) + new Vector2(1);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(mockTexture, expectedLocation, Color.Pink);
        }

        [TestMethod]
        public void DrawWhenCurrentTrialsGreaterThanMaxTrialsShouldNotDrawCurrentTrialIndicator()
        {
            _mockDeviceModel.CurrentTrial.Returns(CodeLogicEngine.Constants.MaximumTrials + 1);
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);

            ITexture2D mockFrame = Substitute.For<ITexture2D>();
            mockFrame.Width.Returns(100);
            mockFrame.Height.Returns(50);
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarProgressFrame").Returns(mockFrame);

            ITexture2D mockTexture = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("StatusBarCurrentTrialIndicator").Returns(mockTexture);

            SizeF textureSize = new SizeF(100, 50);
            RectangleF expectedBounds = new RectangleF(
                _bounds.Centre - textureSize.Centre,
                textureSize
            );

            _mockDeviceTheme.StatusIndicatorProgressFrameCellBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressFrameCellSize.Returns(new SizeF(5));
            _mockDeviceTheme.StatusIndicatorProgressFrameBorderSize.Returns(new SizeF(1));
            _mockDeviceTheme.StatusIndicatorProgressBorderMask.Returns(Color.Pink);

            Rectangle expectedClip = new Rectangle(0, 0, 1 + (5 * (5 + 1)), 50);

            Vector2 expectedLocation = expectedBounds.Location + new Vector2(expectedClip.Width, 1) + new Vector2(1);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(mockTexture, Arg.Any<Vector2>(), Arg.Any<Color>());
        }
        #endregion
    }
}
