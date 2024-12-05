using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Entities
{
    [TestClass]
    public class StatusIndicator_Tests
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private IDeviceModel _mockDeviceModel;
        private IDeviceTheme _mockDeviceTheme;

        private IThemeManagerCollection _mockThemeCollection;

        private ITexture2D _mockTextureTextBG;
        private ITexture2D _mockTextureAlarmBG;
        private ITexture2D _mockTextureBorder;
        private ISpriteFont _mockFont;

        private RectangleF _bounds = new RectangleF(100, 200, 300, 400);

        private StatusIndicator _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();

            _mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(_mockThemeCollection);

            _mockThemeCollection[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(_mockDeviceTheme = Substitute.For<IDeviceTheme>());

            _mockEngine.AssetBank.Get<ISpriteFont>("StatusIndicatorFont").Returns(_mockFont = Substitute.For<ISpriteFont>());
            _mockEngine.AssetBank.Get<ITexture2D>("StatusIndicatorTextArea").Returns(_mockTextureTextBG = Substitute.For<ITexture2D>());
            _mockEngine.AssetBank.Get<ITexture2D>("StatusIndicatorAlarmArea").Returns(_mockTextureAlarmBG = Substitute.For<ITexture2D>());
            _mockEngine.AssetBank.Get<ITexture2D>("StatusIndicatorBorder").Returns(_mockTextureBorder = Substitute.For<ITexture2D>());

            _mockDeviceTheme.GetFullAssetName(Arg.Any<string>()).Returns(x => x[0].ToString());

            _sut = new StatusIndicator(_mockEngine, _bounds, _mockSpriteBatch, _mockDeviceModel);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNullSpriteBatchShouldThrowException()
        {
            _sut = new StatusIndicator(_mockEngine, _bounds, null, _mockDeviceModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorwithNullDeviceModelShouldThrowException()
        {
            _sut = new StatusIndicator(_mockEngine, _bounds, _mockSpriteBatch, null);
        }

        [TestMethod]
        public void ConstructorShouldRetrieveTheme()
        {
            _mockThemeCollection[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyGetShouldReturnExpectedValue_SpriteBatch()
        {
            Assert.AreSame(_mockSpriteBatch, _sut.SpriteBatch);
        }

        [TestMethod]
        public void PropertyGetShouldReturnExpectedValue_Model()
        {
            Assert.AreSame(_mockDeviceModel, _sut.Model);
        }

        [TestMethod]
        public void PropertyGetShouldReturnExpectedValue_Theme()
        {
            Assert.AreSame(_mockDeviceTheme, _sut.Theme);
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateMovingFromNonCriticalToCriticalShouldUseAlarmCriticalColour()
        {
            _mockDeviceTheme.AlarmCriticalBlinkFrequency.Returns(0.5);
            _mockDeviceTheme.StatusIndicatorBackgroundMask.Returns(Color.Purple);
            _mockDeviceTheme.AlarmHighMask.Returns(Color.Pink);

            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);
            _sut.Update(new GameTime());
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.Critical);

            _sut.Update(new GameTime());

            Assert.AreEqual<Color>(Color.Pink, _sut.CurrentAlarmMask);
        }

        [TestMethod]
        public void UpdateInCriticalBeforeIntervalShouldUseAlarmCriticalColour()
        {
            _mockDeviceTheme.AlarmCriticalBlinkFrequency.Returns(0.5);
            _mockDeviceTheme.StatusIndicatorBackgroundMask.Returns(Color.Purple);
            _mockDeviceTheme.AlarmHighMask.Returns(Color.Pink);

            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);
            _sut.Update(new GameTime());
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.Critical);
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(new TimeSpan(), TimeSpan.FromSeconds(0.49)));

            Assert.AreEqual<Color>(Color.Pink, _sut.CurrentAlarmMask);
        }

        [TestMethod]
        public void UpdateInCriticalAfterIntervalShouldUseBackgroundColour()
        {
            _mockDeviceTheme.AlarmCriticalBlinkFrequency.Returns(0.5);
            _mockDeviceTheme.StatusIndicatorProgressBackgroundMask.Returns(Color.Purple);
            _mockDeviceTheme.AlarmHighMask.Returns(Color.Pink);

            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);
            _sut.Update(new GameTime());
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.Critical);
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(new TimeSpan(), TimeSpan.FromSeconds(0.50)));

            Assert.AreEqual<Color>(Color.Purple, _sut.CurrentAlarmMask);
        }

        [TestMethod]
        public void UpdateInCriticalAfterIntervalAgainShouldUseAlarmCriticalColour()
        {
            _mockDeviceTheme.AlarmCriticalBlinkFrequency.Returns(0.5);
            _mockDeviceTheme.StatusIndicatorBackgroundMask.Returns(Color.Purple);
            _mockDeviceTheme.AlarmHighMask.Returns(Color.Pink);

            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.High);
            _sut.Update(new GameTime());
            _mockDeviceModel.AlarmLevel.Returns(AlarmLevel.Critical);
            _sut.Update(new GameTime());

            _sut.Update(new GameTime(new TimeSpan(), TimeSpan.FromSeconds(0.53)));
            _sut.Update(new GameTime(new TimeSpan(), TimeSpan.FromSeconds(0.53)));

            Assert.AreEqual<Color>(Color.Pink, _sut.CurrentAlarmMask);
        }
        #endregion
    }
}
