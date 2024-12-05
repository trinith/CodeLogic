using ArbitraryPixel.CodeLogic.Common.Theme;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Theme
{
    [TestClass]
    public class CodeLogicThemeCollection_Tests : UnitTestBase<CodeLogicThemeCollection>
    {
        protected override CodeLogicThemeCollection OnCreateSUT()
        {
            return new CodeLogicThemeCollection();
        }

        [TestMethod]
        public void ConstructShouldRegisterDeviceThemeManager()
        {
            Assert.IsTrue(_sut.ManagerExists(ThemeObjectType.Device));
        }
    }
}
