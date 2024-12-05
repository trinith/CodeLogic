using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Model
{
    [TestClass]
    public class MenuContentMap_Tests
    {
        private MenuContentMap _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new MenuContentMap();
        }

        [TestMethod]
        public void HasMappedLayerWithRegisteredItemShouldReturnTrue()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            IMenuContentLayer mockLayer = Substitute.For<IMenuContentLayer>();

            _sut.RegisterContentLayer(mockItem, mockLayer);

            Assert.IsTrue(_sut.HasMappedLayer(mockItem));
        }

        [TestMethod]
        public void HasMappedLayerWithUnregisteredItemShouldReturnFalse()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();

            Assert.IsFalse(_sut.HasMappedLayer(mockItem));
        }

        [TestMethod]
        public void GetLayerWithRegisteredItemShouldReturnMappedLayer()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            IMenuContentLayer mockLayer = Substitute.For<IMenuContentLayer>();
            _sut.RegisterContentLayer(mockItem, mockLayer);

            Assert.AreSame(mockLayer, _sut.GetLayer(mockItem));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetLayerWithUnregisteredItemShouldThrowException()
        {
            _sut.GetLayer(Substitute.For<IMenuItem>());
        }

        [TestMethod]
        public void ClearShouldRemoveMappings()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut.RegisterContentLayer(mockItem, Substitute.For<IMenuContentLayer>());

            _sut.Clear();

            Assert.IsFalse(_sut.HasMappedLayer(mockItem));
        }
    }
}
