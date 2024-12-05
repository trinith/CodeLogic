using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class SequenceSubmitButton_Tests
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceTheme _mockDeviceTheme;

        private IThemeManagerCollection _mockThemeCollection;

        private ITexture2D _mockTextureForeground;
        private ITexture2D _mockTextureBackground;
        private ITexture2D _mockTextureHoldOverlay;

        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);

        private SequenceSubmitButton _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(x => (Vector2)x[0]);

            _mockEngine.AssetBank.Get<ITexture2D>("SequenceSubmitButtonForeground").Returns(_mockTextureForeground = Substitute.For<ITexture2D>());
            _mockEngine.AssetBank.Get<ITexture2D>("SequenceSubmitButtonBackground").Returns(_mockTextureBackground = Substitute.For<ITexture2D>());
            _mockEngine.AssetBank.Get<ITexture2D>("SequenceSubmitButtonHoldOverlay").Returns(_mockTextureHoldOverlay = Substitute.For<ITexture2D>());

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());
            _mockDeviceTheme.GetFullAssetName(Arg.Any<string>()).Returns(x => x[0].ToString());

            _sut = new SequenceSubmitButton(_mockEngine, _bounds, _mockSpriteBatch);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullSpriteBatchShouldThrowException()
        {
            _sut = new SequenceSubmitButton(_mockEngine, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldRetrieveCurrentTheme()
        {
            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBackgroundTexture()
        {
            _mockDeviceTheme.SubmitButtonBackgroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTextureBackground, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawForegroundTexture()
        {
            _mockDeviceTheme.SubmitButtonForegroundMask.Returns(Color.Pink);

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTextureForeground, _bounds, Color.Pink);
        }

        [TestMethod]
        public void DrawShouldDrawOverlayTextureWhenHeld()
        {
            _mockDeviceTheme.SubmitButtonHoldOverlayMask.Returns(Color.Pink);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, false));
            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, true));
            _sut.Update(new GameTime());
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTextureHoldOverlay, Arg.Any<RectangleF>(), Color.Pink);
        }

        [TestMethod]
        public void DrawShouldNotDrawOverlayWhenNotHeld()
        {
            _mockDeviceTheme.SubmitButtonHoldOverlayMask.Returns(Color.Pink);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, false));
            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, false));
            _sut.Update(new GameTime());
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(0).Draw(_mockTextureHoldOverlay, Arg.Any<Rectangle>(), Color.Pink);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateWhenHeldShouldCauseDrawOfExpectedPortionOfOverlayTexture_Quarter()
        {
            _mockDeviceTheme.SubmitButtonHoldOverlayMask.Returns(Color.Pink);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, false));
            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, true));
            _sut.Update(new GameTime());
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.125)));

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTextureHoldOverlay, new RectangleF(_bounds.Left, _bounds.Top, _bounds.Width * 0.25f, _bounds.Height), Color.Pink);
        }

        [TestMethod]
        public void UpdateWhenHeldShouldCauseDrawOfExpectedPortionOfOverlayTexture_Half()
        {
            _mockDeviceTheme.SubmitButtonHoldOverlayMask.Returns(Color.Pink);
            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, false));
            _sut.Update(new GameTime());
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_sut.Bounds.Centre, true));
            _sut.Update(new GameTime());
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.25)));

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockTextureHoldOverlay, new RectangleF(_bounds.Left, _bounds.Top, _bounds.Width * 0.5f, _bounds.Height), Color.Pink);
        }
        #endregion
    }
}
