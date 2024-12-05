using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuBriefingContentLayer_Tests
    {
        private MenuBriefingContentLayer _sut;

        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private List<ILayer> _mockContentPages;

        private GameObjectFactory _mockGOF;

        private RectangleF _contentBounds;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockGOF = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGOF);
             
            _mockContentPages = new List<ILayer>();
            _mockContentPages.Add(Substitute.For<ILayer>());
            _mockContentPages.Add(Substitute.For<ILayer>());

            _contentBounds = new RectangleF(200, 100, 400, 300);
        }

        private void Construct()
        {
            _sut = new MenuBriefingContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, _mockContentPages.ToArray());
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullContentPagesShouldThrowException()
        {
            _sut = new MenuBriefingContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithEmptyContentPagesShouldThrowException()
        {
            _sut = new MenuBriefingContentLayer(_mockEngine, _mockSpriteBatch, _contentBounds, new ILayer[] { });
        }

        [TestMethod]
        public void ConstructShouldCreateGenericLayerForContentHost()
        {
            Construct();

            _mockGOF.Received(1).CreateGenericLayer(_mockEngine, _mockSpriteBatch);
        }

        [TestMethod]
        public void ConstructShouldAddFirstContentPageToHostLayerEntities()
        {
            ILayer mockContentLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentLayer);

            Construct();

            mockContentLayer.Received(1).AddEntity(_mockContentPages[0]);
        }

        [TestMethod]
        public void ConstructShouldCreateMenuBriefingUILayer()
        {
            RectangleF expectedBounds = _contentBounds;
            expectedBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            Construct();

            _mockGOF.Received(1).CreateMenuBriefingUILayer(_mockEngine, _mockSpriteBatch, expectedBounds, _sut);
        }

        [TestMethod]
        public void ConstructShouldAddEntitiesInExpectedOrder()
        {
            ILayer mockContentLayer = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentLayer);
            mockContentLayer.UpdateOrder.Returns(111);

            ILayer mockUILayer = Substitute.For<ILayer>();
            _mockGOF.CreateMenuBriefingUILayer(_mockEngine, _mockSpriteBatch, Arg.Any<RectangleF>(), Arg.Any<IMenuBriefingContentLayer>()).Returns(mockUILayer);
            mockUILayer.UpdateOrder.Returns(222);

            Construct();

            Assert.IsTrue(_sut.Entities.SequenceEqual(new IEntity[] { mockContentLayer, mockUILayer }));
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void CurrentPageShouldDefaultToZero()
        {
            Construct();

            Assert.AreEqual<int>(1, _sut.CurrentPage);
        }

        [TestMethod]
        public void TotalPagesShouldReturnContentPageCount()
        {
            Construct();

            Assert.AreEqual<int>(2, _sut.TotalPages);
        }
        #endregion

        #region NextPage Tests
        [TestMethod]
        public void NextPageWhenAvailableShouldIncrementCurrentPage()
        {
            Construct();

            _sut.NextPage();

            Assert.AreEqual<int>(2, _sut.CurrentPage);
        }

        [TestMethod]
        public void NextPageWhenAvailableShouldSwapCurrentPageInHostLayer()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();

            _sut.NextPage();

            Received.InOrder(
                () =>
                {
                    mockContentHost.RemoveEntity(_mockContentPages[0]);
                    mockContentHost.AddEntity(_mockContentPages[1]);
                }
            );
        }

        [TestMethod]
        public void NextPageWhenNoneAvailableShouldNotModifyHostLayerEntities()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();
            _sut.NextPage();
            mockContentHost.ClearReceivedCalls();

            _sut.NextPage();

            mockContentHost.Received(0).RemoveEntity(Arg.Any<IEntity>());
            mockContentHost.Received(0).AddEntity(Arg.Any<IEntity>());
        }
        #endregion

        #region PreviousPage Tests
        [TestMethod]
        public void PreviousPageWhenAvailableShouldDecrementCurrentPage()
        {
            Construct();
            _sut.NextPage();

            _sut.PreviousPage();

            Assert.AreEqual<int>(1, _sut.CurrentPage);
        }

        [TestMethod]
        public void PreviousPageWhenAvailableShouldSwapCurrentPageInHostLayer()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();
            _sut.NextPage();
            mockContentHost.ClearReceivedCalls();

            _sut.PreviousPage();

            Received.InOrder(
                () =>
                {
                    mockContentHost.RemoveEntity(_mockContentPages[1]);
                    mockContentHost.AddEntity(_mockContentPages[0]);

                }
            );
        }

        [TestMethod]
        public void PreviousPageWhenNoneAvailableShouldNotModifyHostLayerEntities()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();
            mockContentHost.ClearReceivedCalls();

            _sut.PreviousPage();

            mockContentHost.Received(0).RemoveEntity(Arg.Any<IEntity>());
            mockContentHost.Received(0).AddEntity(Arg.Any<IEntity>());
        }
        #endregion

        #region SetPage Tests
        [TestMethod]
        public void SetPageWithNewPageShouldRemoveCurrentPageFromContentHost()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();

            _sut.SetPage(GameObjectFactory.BriefingPage.Contents);

            mockContentHost.Received(1).RemoveEntity(_mockContentPages[0]);
        }

        [TestMethod]
        public void SetPageWithNewPageShouldAddNewPageToContentHost()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();

            _sut.SetPage(GameObjectFactory.BriefingPage.Contents);

            mockContentHost.Received(1).AddEntity(_mockContentPages[1]);
        }

        [TestMethod]
        public void SetPageWithNewPageShouldUpdateCurrentPage()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();

            _sut.SetPage(GameObjectFactory.BriefingPage.Contents);

            Assert.AreEqual<int>(2, _sut.CurrentPage);
        }

        [TestMethod]
        public void SetPageWithSamePageShouldNotRemoveCurrentPage()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();

            _sut.SetPage(GameObjectFactory.BriefingPage.Cover);

            mockContentHost.Received(0).RemoveEntity(Arg.Any<IEntity>());
        }

        [TestMethod]
        public void SetPageWithSamePageShouldNotAddNewPage()
        {
            ILayer mockContentHost = Substitute.For<ILayer>();
            _mockGOF.CreateGenericLayer(_mockEngine, _mockSpriteBatch).Returns(mockContentHost);

            Construct();
            mockContentHost.ClearReceivedCalls();

            _sut.SetPage(GameObjectFactory.BriefingPage.Cover);

            mockContentHost.Received(0).AddEntity(Arg.Any<IEntity>());
        }
        #endregion
    }
}
