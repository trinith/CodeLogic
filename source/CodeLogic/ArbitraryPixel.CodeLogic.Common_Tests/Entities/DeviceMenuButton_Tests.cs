using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using Microsoft.Xna.Framework;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class DeviceMenuButton_Tests
    {
        private DeviceMenuButton _sut;
        private ITexture2D _mockTexture;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(200, 100, 400, 300);
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockTexture = Substitute.For<ITexture2D>();
        }

        private void Construct()
        {
            _sut = new DeviceMenuButton(_mockEngine, _bounds, _mockSpriteBatch, _mockTexture);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new DeviceMenuButton(_mockEngine, _bounds, null, _mockTexture);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_ForegroundTexture()
        {
            _sut = new DeviceMenuButton(_mockEngine, _bounds, _mockSpriteBatch, null);
        }

        [TestMethod]
        public void ConstructShouldRequestCurrentDeviceTheme()
        {
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            Construct();

            mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawWhenNotPressedShouldDrawWithExpectedColours()
        {
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            ITexture2D mockBackground = Substitute.For<ITexture2D>();
            ITexture2D mockBorder = Substitute.For<ITexture2D>();

            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBackground").Returns(mockBackground);
            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBorder").Returns(mockBorder);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.DeviceMenuButtonForegroundMask.Returns(Color.Pink);
            mockTheme.DeviceMenuButtonBackgroundMask.Returns(Color.Purple);
            Construct();

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBackground");
                    _mockSpriteBatch.Draw(mockBackground, _bounds, Color.Purple);
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBorder");
                    _mockSpriteBatch.Draw(mockBorder, _bounds, Color.Pink);
                    _mockSpriteBatch.Draw(_mockTexture, _bounds, Color.Pink);
                }
            );
        }

        [TestMethod]
        public void DrawWhenPressedShouldDrawWithExpectedColours()
        {
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemeCollection);

            _mockEngine.InputManager.IsActive.Returns(true);
            _mockEngine.InputManager.ShouldConsumeInput(Arg.Any<object>()).Returns(true);
            _mockEngine.InputManager.GetSurfaceState().Returns(new SurfaceState(_bounds.Centre, true));
            _mockEngine.ScreenManager.PointToWorld(Arg.Any<Vector2>()).Returns(_bounds.Centre);

            ITexture2D mockBackground = Substitute.For<ITexture2D>();
            ITexture2D mockBorder = Substitute.For<ITexture2D>();

            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBackground").Returns(mockBackground);
            _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBorder").Returns(mockBorder);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.DeviceMenuButtonForegroundHighlightMask.Returns(Color.Pink);
            mockTheme.DeviceMenuButtonBackgroundMask.Returns(Color.Purple);
            Construct();
            _sut.Update(new GameTime());

            _sut.Draw(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBackground");
                    _mockSpriteBatch.Draw(mockBackground, _bounds, Color.Purple);
                    _mockEngine.AssetBank.Get<ITexture2D>("DeviceMenuButtonBorder");
                    _mockSpriteBatch.Draw(mockBorder, _bounds, Color.Pink);
                    _mockSpriteBatch.Draw(_mockTexture, _bounds, Color.Pink);
                }
            );
        }
        #endregion
    }
}
