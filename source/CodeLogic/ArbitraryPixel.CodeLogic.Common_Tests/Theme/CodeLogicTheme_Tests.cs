using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.Theme;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Theme
{
    [TestClass]
    public class CodeLogicTheme_Tests
    {
        private class CodeLogicTheme_Testable : CodeLogicTheme
        {
            public string ExposedAssetPathPrefix => this.AssetPathPrefix;

            public override string ThemeID => throw new NotImplementedException();
            public override string ObjectID => throw new NotImplementedException();
        }

        private CodeLogicTheme_Testable _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new CodeLogicTheme_Testable();
        }

        [TestMethod]
        public void AssetPathPrefixShouldBeExpectedValue()
        {
            Assert.AreEqual<string>("Themes", _sut.ExposedAssetPathPrefix);
        }
    }
}
