using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.Platform2D.Theme;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Theme
{
    [TestClass]
    public class DeviceThemeManager_Tests : UnitTestBase<DeviceThemeManager>
    {
        protected override DeviceThemeManager OnCreateSUT()
        {
            return new DeviceThemeManager();
        }

        [TestMethod]
        public void ConstructShouldSetCurrentThemeIDToAgent()
        {
            Assert.AreEqual<string>(ThemeType.Agent, _sut.CurrentThemeID);
        }

        [TestMethod]
        public void ConstructShouldSetDefaultThemeToAgent()
        {
            Assert.AreEqual<ITheme>(_sut[ThemeType.Agent], _sut.DefaultTheme);
        }

        [TestMethod]
        public void ConstructShouldRegisterDeviceAgentTheme()
        {
            _sut.CurrentThemeID = ThemeType.Agent;
            Assert.AreEqual<ITheme>(_sut[ThemeType.Agent], _sut.GetCurrentTheme());
        }
    }
}
