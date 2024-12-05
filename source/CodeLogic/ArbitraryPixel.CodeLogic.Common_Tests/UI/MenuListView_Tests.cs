using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests.UI
{
    [TestClass]
    public class MenuListView_Tests
    {
        private MenuListView _sut;
        private IMenuFactory _mockFactory;
        private ISpriteFont _mockFont;
        private ISpriteBatch _mockSpriteBatch;
        private IMenuItem _mockItem;
        private RectangleF _bounds;
        private IEngine _mockEngine;

        private IMenuItem _mockParent;
        private IMenuItem _mockBackItem;
        private ITexture2D _mockPixel;

        private IMenuItemView[] _mockSubViews;
        private IMenuItem[] _mockSubItems;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _bounds = new RectangleF(200, 100, 400, 300);
            _mockItem = Substitute.For<IMenuItem>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockFont = Substitute.For<ISpriteFont>();
            _mockFactory = Substitute.For<IMenuFactory>();

            _mockSubItems = new IMenuItem[]
                {
                    Substitute.For<IMenuItem>(),
                    Substitute.For<IMenuItem>(),
                    Substitute.For<IMenuItem>(),
                };
            _mockItem.Items.Returns(_mockSubItems);

            _mockSubViews = new IMenuItemView[]
                {
                    Substitute.For<IMenuItemView>(),
                    Substitute.For<IMenuItemView>(),
                    Substitute.For<IMenuItemView>(),
                    Substitute.For<IMenuItemView>(),
                };
            for (int i = 0; i < 3; i++)
                _mockSubViews[i].ViewOf.Returns(_mockSubItems[i]);

            _mockEngine.AssetBank.Get<ITexture2D>("Pixel").Returns(_mockPixel = Substitute.For<ITexture2D>());
            _mockFont.LineSpacing.Returns(10);

            _mockItem.Parent.Returns(_mockParent = Substitute.For<IMenuItem>());

            _mockFactory.CreateMenuItemView(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<IMenuItem>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<TextLineAlignment>()).Returns(
                _mockSubViews[0],
                _mockSubViews[1],
                _mockSubViews[2],
                _mockSubViews[3]
            );

            _mockFactory.CreateMenuItem("Back", CodeLogicEngine.Constants.MenuItemHeight).Returns(_mockBackItem = Substitute.For<IMenuItem>());

            for (int i = 0; i < _mockItem.Items.Length; i++)
            {
                _mockItem.Items[i].Text.Returns("SubItem" + i.ToString());
                _mockItem.Items[i].Items.Returns(new IMenuItem[] { });
                _mockItem.Items[i].Parent.Returns((IMenuItem)null);
            }
        }

        private void Construct()
        {
            _sut = new MenuListView(_mockEngine, _bounds, _mockItem, _mockSpriteBatch, _mockFont, _mockFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_SpriteBatch()
        {
            _sut = new MenuListView(_mockEngine, _bounds, _mockItem, null, _mockFont, _mockFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MenuFont()
        {
            _sut = new MenuListView(_mockEngine, _bounds, _mockItem, _mockSpriteBatch, null, _mockFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullParameterShouldThrowException_MenuFactory()
        {
            _sut = new MenuListView(_mockEngine, _bounds, _mockItem, _mockSpriteBatch, _mockFont, null);
        }

        [TestMethod]
        public void ConstructShouldRequestPixelTextureFromAssetBank()
        {
            Construct();
            _mockEngine.AssetBank.Received(1).Get<ITexture2D>("Pixel");
        }

        [TestMethod]
        public void ConstructShouldCreateExpectedMenuItemViewsForView()
        {
            Construct();

            Received.InOrder(
                () =>
                {
                    Vector2 pos = _bounds.Location;

                    // Create views for each menu item
                    for (int i = 0; i < 3; i++)
                    {
                        _mockFactory.CreateMenuItemView(_mockEngine, new RectangleF(pos + new Vector2(0, i * 10), new SizeF(400, 10)), _mockItem.Items[i], _mockSpriteBatch, _mockFont, TextLineAlignment.Right);
                        _mockSubViews[i].Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                    }

                    // Create view for back button
                    _mockFactory.CreateMenuItem("Back", CodeLogicEngine.Constants.MenuItemHeight);
                    _mockFactory.CreateMenuItemView(_mockEngine, new RectangleF(_bounds.Left, _bounds.Top + 40, 400, 10), _mockBackItem, _mockSpriteBatch, _mockFont, TextLineAlignment.Right);
                    _mockSubViews[3].Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldCreateExpectedMenuItemViewsForViewsWithHeight()
        {
            _mockItem.Items[1].Height.Returns(25);

            Construct();

            Received.InOrder(
                () =>
                {
                    Vector2 pos = _bounds.Location;
                    float yOffset = 0;

                    // Create views for each menu item
                    for (int i = 0; i < 3; i++)
                    {
                        float height = Math.Max(_mockFont.LineSpacing, _mockItem.Items[i].Height);
                        _mockFactory.CreateMenuItemView(_mockEngine, new RectangleF(pos + new Vector2(0, yOffset), new SizeF(400, height)), _mockItem.Items[i], _mockSpriteBatch, _mockFont, TextLineAlignment.Right);
                        _mockSubViews[i].Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();

                        yOffset += height;
                    }
                }
            );
        }

        [TestMethod]
        public void ConstructShouldNotSetSelectedItem()
        {
            Construct();

            Assert.IsNull(_sut.SelectedItem);
        }
        #endregion

        #region Property Tests
        [TestMethod]
        public void PropertyShouldHaveExpectedDefaultValue_ViewHasChanged()
        {
            Construct();

            Assert.IsFalse(_sut.ViewHasChanged);
        }
        #endregion

        #region ViewOfSet Tests
        [TestMethod]
        public void ViewOfSetShouldInciateAViewChange()
        {
            Construct();

            IMenuItem mockItem = Substitute.For<IMenuItem>();

            _sut.ViewOf = mockItem;

            Assert.IsTrue(_sut.ViewHasChanged);
        }

        [TestMethod]
        public void ViewOfWithNoParentShouldCreateExpectedMenuItemViews()
        {
            Construct();

            _mockFactory.ClearReceivedCalls();
            foreach (var subView in _mockSubViews)
                subView.ClearReceivedCalls();

            _mockFactory.CreateMenuItemView(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<IMenuItem>(), Arg.Any<ISpriteBatch>(), Arg.Any<ISpriteFont>(), Arg.Any<TextLineAlignment>()).Returns(
                _mockSubViews[0],
                _mockSubViews[1],
                _mockSubViews[2],
                _mockSubViews[3]
            );

            IMenuItem mockItem = Substitute.For<IMenuItem>();
            mockItem.Parent.Returns((IMenuItem)null);
            mockItem.Items.Returns(_mockSubItems);
            _sut.ViewOf = mockItem;
            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    Vector2 pos = _bounds.Location;

                    // Create views for each menu item
                    for (int i = 0; i < 3; i++)
                    {
                        _mockFactory.CreateMenuItemView(_mockEngine, new RectangleF(pos + new Vector2(0, i * 10), new SizeF(400, 10)), _mockItem.Items[i], _mockSpriteBatch, _mockFont, TextLineAlignment.Right);
                        _mockSubViews[i].Tapped += Arg.Any<EventHandler<ButtonEventArgs>>();
                    }
                }
            );

            // No back menu created.
            _mockFactory.Received(0).CreateMenuItem("Back", CodeLogicEngine.Constants.MenuItemHeight);

            // First item selected.
            Assert.AreEqual<IMenuItem>(_mockSubItems[0], _sut.SelectedItem);
        }
        #endregion

        #region Event Tests
        [TestMethod]
        public void SubViewTappedShouldFireMenuItemTappedEvent()
        {
            Construct();

            EventHandler<MenuItemEventArgs> mockSubscriber = Substitute.For<EventHandler<MenuItemEventArgs>>();
            _sut.MenuItemTapped += mockSubscriber;

            for (int i = 0; i < 3; i++)
            {
                _mockSubViews[i].Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockSubViews[i], new ButtonEventArgs(Vector2.Zero));
                mockSubscriber.Received(1)(_sut, Arg.Is<MenuItemEventArgs>(x => x.Item == _mockSubItems[i]));
            }
        }

        [TestMethod]
        public void BackViewTappedShouldFireBackTappedEvent()
        {
            Construct();

            EventHandler<EventArgs> mockSubscriber = Substitute.For<EventHandler<EventArgs>>();
            _sut.MenuBackTapped += mockSubscriber;

            _mockSubViews[3].Tapped += Raise.Event<EventHandler<ButtonEventArgs>>(_mockSubViews[3], new ButtonEventArgs(Vector2.Zero));

            mockSubscriber.Received(1)(_sut, Arg.Any<EventArgs>());
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateShouldUpdateAllSubViews()
        {
            Construct();

            GameTime expectedGT = new GameTime();
            _sut.Update(expectedGT);

            Received.InOrder(
                () =>
                {
                    _mockSubViews[2].Update(expectedGT);
                    _mockSubViews[1].Update(expectedGT);
                    _mockSubViews[0].Update(expectedGT);

                    _mockSubViews[3].Update(expectedGT);
                }
            );
        }

        [TestMethod]
        public void UpdateWhenSelectedItemMatchesViewViewOfShouldSetIsSelectedOnItem()
        {
            Construct();

            _sut.SelectedItem = _mockSubItems[1];

            _sut.Update(new GameTime());

            Received.InOrder(
                () =>
                {
                    _mockSubViews[2].IsSelected = false;
                    _mockSubViews[1].IsSelected = true;
                    _mockSubViews[0].IsSelected = false;
                }
            );
        }

        [TestMethod]
        public void UpdateWhenViewHasChangedTrueShouldClearViewHasChanged()
        {
            Construct();

            IMenuItem mockItem = Substitute.For<IMenuItem>();
            _sut.ViewOf = mockItem;

            _sut.Update(new GameTime());

            Assert.IsFalse(_sut.ViewHasChanged);
        }
        #endregion

        #region Draw Tests
        [TestMethod]
        public void DrawShouldDrawBackground()
        {
            Construct();

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, _bounds, CodeLogicEngine.Constants.ClrMenuBGMid);
        }

        [TestMethod]
        public void DrawShouldCallDrawOnSubViews()
        {
            Construct();

            GameTime expectedGT = new GameTime();

            _sut.Draw(expectedGT);

            Received.InOrder(
                () =>
                {
                    _mockSubViews[0].Draw(expectedGT);
                    _mockSubViews[1].Draw(expectedGT);
                    _mockSubViews[2].Draw(expectedGT);
                    _mockSubViews[3].Draw(expectedGT);
                }
            );
        }

        [TestMethod]
        public void DrawWithSelectedSubViewShouldDrawBackground()
        {
            Construct();

            _mockSubViews[0].IsSelected.Returns(true);
            _mockSubViews[0].Bounds.Returns(new RectangleF(20, 10, 40, 30));

            _sut.Draw(new GameTime());

            _mockSpriteBatch.Received(1).Draw(_mockPixel, new RectangleF(20, 10, 40, 30), CodeLogicEngine.Constants.ClrMenuBGMid);
        }
        #endregion
    }
}
