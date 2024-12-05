using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.UI;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuItemEventArgs_Tests
    {
        private MenuItemEventArgs _sut;

        [TestMethod]
        public void ItemReturnsConstructorParameter_Null()
        {
            _sut = new MenuItemEventArgs(null);

            Assert.IsNull(_sut.Item);
        }

        [TestMethod]
        public void ItemReturnsConstructorParameter_Item()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut = new MenuItemEventArgs(mockItem);

            Assert.AreSame(mockItem, _sut.Item);
        }
    }
}
