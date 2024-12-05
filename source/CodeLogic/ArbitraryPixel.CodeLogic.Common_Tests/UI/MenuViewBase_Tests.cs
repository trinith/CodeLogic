using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Platform2D.Engine;
using NSubstitute;
using ArbitraryPixel.Common.Drawing;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuViewBase_Tests
    {
        #region Test Class
        private class MenuViewBase_Testable : MenuViewBase
        {
            public event EventHandler<EventArgs> ViewOfSet;

            public MenuViewBase_Testable(IEngine host, RectangleF bounds, IMenuItem viewOf) : base(host, bounds, viewOf)
            {
            }

            protected override void OnViewOfSet()
            {
                base.OnViewOfSet();

                if (this.ViewOfSet != null)
                    this.ViewOfSet(this, new EventArgs());
            }
        }
        #endregion

        private MenuViewBase_Testable _sut;
        private IEngine _mockEngine;
        private IMenuItem _mockMenuItem;
        private RectangleF _bounds = new RectangleF(200, 100, 400, 300);

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockMenuItem = Substitute.For<IMenuItem>();

            _sut = new MenuViewBase_Testable(_mockEngine, _bounds, _mockMenuItem);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullViewOfShouldThrowException()
        {
            _sut = new MenuViewBase_Testable(_mockEngine, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldSetViewOfToParameter()
        {
            Assert.AreSame(_mockMenuItem, _sut.ViewOf);
        }
        #endregion

        #region ViewOf Tests
        [TestMethod]
        public void ViewOfShouldReturnSetValue()
        {
            IMenuItem mockOther = Substitute.For<IMenuItem>();

            _sut.ViewOf = mockOther;

            Assert.AreSame(mockOther, _sut.ViewOf);
        }

        [TestMethod]
        public void ViewOfSetShouldCallOnViewOfSet()
        {
            // Exposed via event on test class.
            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.ViewOfSet += mockSubscriber;

            _sut.ViewOf = Substitute.For<IMenuItem>();

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion
    }
}
