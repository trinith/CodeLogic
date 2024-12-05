using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.UI;
using NSubstitute;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuItem_Tests
    {
        private MenuItem _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new MenuItem("Root", 23);
        }

        [TestMethod]
        public void TextShouldReturnConstructorParameter()
        {
            Assert.AreEqual<string>("Root", _sut.Text);
        }

        [TestMethod]
        public void HeightShouldReturnConstructorParameter()
        {
            Assert.AreEqual<float>(23, _sut.Height);
        }

        [TestMethod]
        public void ParentShouldDefaultNull()
        {
            Assert.IsNull(_sut.Parent);
        }

        [TestMethod]
        public void ParentShouldReturnSetValue()
        {
            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut.Parent = mockItem;

            Assert.AreSame(mockItem, _sut.Parent);
        }

        [TestMethod]
        public void ItemsShouldDefaultEmpty()
        {
            Assert.AreEqual<int>(0, _sut.Items.Length);
        }

        [TestMethod]
        public void CreateChildShouldReturnItemWithExpectedText()
        {
            IMenuItem newItem = _sut.CreateChild("Child", 23);

            Assert.AreEqual<string>("Child", newItem.Text);
        }

        [TestMethod]
        public void CreateChildShouldReturnItemWithExpectedHeight()
        {
            IMenuItem newItem = _sut.CreateChild("Child", 23);

            Assert.AreEqual<float>(23, newItem.Height);
        }

        [TestMethod]
        public void CreateChildShouldParentToOwner()
        {
            IMenuItem newItem = _sut.CreateChild("Child", 23);

            Assert.AreSame(_sut, newItem.Parent);
        }

        [TestMethod]
        public void CreateChildShouldAddNewlyCreatedItemToItems()
        {
            IMenuItem newItem = _sut.CreateChild("Child", 23);

            Assert.IsTrue(_sut.Items.ToList().Contains(newItem));
        }
    }
}
