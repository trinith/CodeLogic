using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class CodeInputLayer_Tests
    {
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private List<ICodeInputButton> _mockButtons;
        private IDeviceModel _mockDeviceModel;

        private GameObjectFactory _mockGameObjectFactory;
        private SizeF _expectedButtonSize = new SizeF(200, 200);
        private int _expectedPadding = 25;
        private int _entityID = 1;

        private CodeInputLayer _sut;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockButtons = new List<ICodeInputButton>();
            _mockDeviceModel = Substitute.For<IDeviceModel>();
            _entityID = 1;

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockEngine.ScreenManager.World.Returns(new Point(1000, 500));

            _mockGameObjectFactory.CreateCodeInputButtonModel(Arg.Any<IDeviceModel>()).Returns(Substitute.For<ICodeInputButtonModel>());

            _mockGameObjectFactory.CreateTextureEntity(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ITexture2D>(), Arg.Any<Color>()).Returns(
                x =>
                {
                    ITextureEntity mockEntity = Substitute.For<ITextureEntity>();
                    mockEntity.Alive.Returns(true);
                    string id = "Texture_" + (_entityID++).ToString();
                    mockEntity.UniqueId.Returns(id);

                    return mockEntity;
                }
            );

            _mockGameObjectFactory.CreateCodeInputButton(Arg.Any<IEngine>(), Arg.Any<RectangleF>(), Arg.Any<ISpriteBatch>(), Arg.Any<ICodeInputButtonModel>(), Arg.Any<int>()).Returns(
                x =>
                {
                    ICodeInputButton mockButton = Substitute.For<ICodeInputButton>();
                    mockButton.Enabled.Returns(true);
                    mockButton.Visible.Returns(true);
                    mockButton.Alive.Returns(true);
                    mockButton.Bounds.Returns((RectangleF)x[1]);
                    mockButton.Model.Returns(x[3] as ICodeInputButtonModel);

                    string id = "Button_" + (_mockButtons.Count + 1).ToString();
                    mockButton.UniqueId.Returns(id);

                    _mockButtons.Add(mockButton);

                    return mockButton;
                }
            );
        }

        private void Construct()
        {
            _sut = new CodeInputLayer(_mockEngine, _mockSpriteBatch, _mockDeviceModel);
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructorShouldSetSamplerStateToExpectedValue()
        {
            Construct();
            Assert.AreSame(SamplerState.PointClamp, _sut.SamplerState);
        }

        [TestMethod]
        public void ConstructShouldRequestThemeFromThemeManager()
        {
            IThemeManagerCollection mockThemes = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemes);

            Construct();
            mockThemes[ThemeObjectType.Device].Received(1).GetCurrentTheme<IDeviceTheme>();
        }

        [TestMethod]
        public void ConstructorShouldCallGameObjectFactoryCreateCodeInputButton_Button1()
        {
            Construct();

            int index = 0;
            Vector2 expectedButtonLocation = new Vector2(62.5f + index * (_expectedButtonSize.Width + _expectedPadding), 150);
            _mockGameObjectFactory.Received(1).CreateCodeInputButton(_mockEngine, new RectangleF(expectedButtonLocation, _expectedButtonSize), _mockSpriteBatch, _mockButtons[index].Model, index);
        }

        [TestMethod]
        public void ConstructorShouldCallGameObjectFactoryCreateCodeInputButton_Button2()
        {
            Construct();

            int index = 1;
            Vector2 expectedButtonLocation = new Vector2(62.5f + index * (_expectedButtonSize.Width + _expectedPadding), 150);
            _mockGameObjectFactory.Received(1).CreateCodeInputButton(_mockEngine, new RectangleF(expectedButtonLocation, _expectedButtonSize), _mockSpriteBatch, _mockButtons[index].Model, index);
        }

        [TestMethod]
        public void ConstructorShouldCallGameObjectFactoryCreateCodeInputButton_Button3()
        {
            Construct();

            int index = 2;
            Vector2 expectedButtonLocation = new Vector2(62.5f + index * (_expectedButtonSize.Width + _expectedPadding), 150);
            _mockGameObjectFactory.Received(1).CreateCodeInputButton(_mockEngine, new RectangleF(expectedButtonLocation, _expectedButtonSize), _mockSpriteBatch, _mockButtons[index].Model, index);
        }

        [TestMethod]
        public void ConstructorShouldCallGameObjectFactoryCreateCodeInputButton_Button4()
        {
            Construct();

            int index = 3;
            Vector2 expectedButtonLocation = new Vector2(62.5f + index * (_expectedButtonSize.Width + _expectedPadding), 150);
            _mockGameObjectFactory.Received(1).CreateCodeInputButton(_mockEngine, new RectangleF(expectedButtonLocation, _expectedButtonSize), _mockSpriteBatch, _mockButtons[index].Model, index);
        }

        [TestMethod]
        public void ConstructorShoudlCallGameObjectFactoryCreateCodeInputButtonExpectedNumberOfTimes()
        {
            Construct();

            _mockGameObjectFactory.Received(4).CreateCodeInputButton(_mockEngine, Arg.Any<RectangleF>(), _mockSpriteBatch, Arg.Any<ICodeInputButtonModel>(), Arg.Any<int>());
        }

        [TestMethod]
        public void ConstructorShouldCreateExpectedEntities()
        {
            Construct();

            Assert.AreEqual<int>(6, _sut.Entities.Count);
        }

        [TestMethod]
        public void ConstructorShouldAddButtonToEntities_Button1()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockButtons[0]));
        }

        [TestMethod]
        public void ConstructorShouldAddButtonToEntities_Button2()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockButtons[1]));
        }

        [TestMethod]
        public void ConstructorShouldAddButtonToEntities_Button3()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockButtons[2]));
        }

        [TestMethod]
        public void ConstructorShouldAddButtonToEntities_Button4()
        {
            Construct();

            Assert.IsTrue(_sut.Entities.Contains(_mockButtons[3]));
        }

        [TestMethod]
        public void ConstructorShouldSubscribeToTouchedEvent()
        {
            Construct();

            foreach (ICodeInputButton button in _mockButtons)
                button.Received(1).Touched += Arg.Any<EventHandler<ButtonEventArgs>>();
        }

        [TestMethod]
        public void ConstructorShouldSubscribeToSelectorOpenedEvent()
        {
            Construct();

            foreach (ICodeInputButton button in _mockButtons)
                button.Received(1).SelectorOpened += Arg.Any<EventHandler<EventArgs>>();
        }

        [TestMethod]
        public void ConstructorShouldSubscribeToSelectorClosedEvent()
        {
            Construct();

            foreach (ICodeInputButton button in _mockButtons)
                button.Received(1).SelectorClosed += Arg.Any<EventHandler<SelectorClosedEventArgs>>();
        }

        [TestMethod]
        public void ConstructorShouldCreateBackgroundFillEntity()
        {
            IThemeManagerCollection mockThemes = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemes);

            ITexture2D mockBorder = Substitute.For<ITexture2D>();
            mockBorder.Width.Returns(500);
            mockBorder.Height.Returns(200);
            _mockEngine.AssetBank.Get<ITexture2D>("InputBackgroundBorder").Returns(mockBorder);

            ITexture2D mockFill = Substitute.For<ITexture2D>();
            _mockEngine.AssetBank.Get<ITexture2D>("InputBackgroundFill").Returns(mockFill);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockThemes[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.BackgroundImageMask.Returns(Color.Pink);

            RectangleF expectedBounds = new RectangleF(
                (1000f / 2f) - (500f / 2f),
                (500f / 2f) - (200f / 2f),
                500f,
                200f
            );

            Construct();

            _mockGameObjectFactory.Received(1).CreateTextureEntity(
                _mockEngine,
                expectedBounds,
                _mockSpriteBatch,
                mockFill,
                Color.Pink
            );
        }

        [TestMethod]
        public void ConstructorShouldCreateBackgroundBorderEntity()
        {
            IThemeManagerCollection mockThemes = Substitute.For<IThemeManagerCollection>();
            _mockEngine.GetComponent<IThemeManagerCollection>().Returns(mockThemes);

            ITexture2D mockBorder = Substitute.For<ITexture2D>();
            mockBorder.Width.Returns(500);
            mockBorder.Height.Returns(200);
            _mockEngine.AssetBank.Get<ITexture2D>("InputBackgroundBorder").Returns(mockBorder);

            IDeviceTheme mockTheme = Substitute.For<IDeviceTheme>();
            mockThemes[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().Returns(mockTheme);
            mockTheme.NormalColourMask.Returns(Color.Pink);

            RectangleF expectedBounds = new RectangleF(
                (1000f / 2f) - (500f / 2f),
                (500f / 2f) - (200f / 2f),
                500f,
                200f
            );

            Construct();

            _mockGameObjectFactory.Received(1).CreateTextureEntity(
                _mockEngine,
                expectedBounds,
                _mockSpriteBatch,
                mockBorder,
                Color.Pink
            );
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateAfterTouchShouldMoveEntityToEndOfList()
        {
            Construct();

            _mockButtons[0].Touched += Raise.Event<EventHandler<ButtonEventArgs>>(_mockButtons[0], new ButtonEventArgs(Vector2.Zero));

            _sut.Update(new GameTime());

            Assert.AreSame(_mockButtons[0], _sut.Entities[_sut.Entities.Count - 1]);
        }

        [TestMethod]
        public void UpdateWithoutTouchShouldKeepSameEntityOrder()
        {
            Construct();

            _sut.Update(new GameTime());

            for (int i = 0; i < _mockButtons.Count; i++)
            {
                Assert.AreSame(_mockButtons[i], _sut.Entities[2 + i]);
            }
        }
        #endregion

        #region CodeInputSelector Open/Close Event Tests
        [TestMethod]
        public void CodeInputButtonSelectorOpenedShouldPlayExpectedSound()
        {
            Construct();
            ISoundResource mockSoundResource = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSoundResource);

            _mockButtons[0].SelectorOpened += Raise.Event<EventHandler<EventArgs>>(_mockButtons[0], new EventArgs());

            mockSoundResource.Received(1).Play();            
        }

        [TestMethod]
        public void CodeInputButtonSelectorClosedWithValueChangeShouldPlayExpectedSound()
        {
            Construct();
            ISoundResource mockSoundResource = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("IndexValueChanged").Returns(mockSoundResource);

            _mockButtons[0].SelectorClosed += Raise.Event<EventHandler<SelectorClosedEventArgs>>(_mockButtons[0], new SelectorClosedEventArgs(true));

            mockSoundResource.Received(1).Play();
        }

        [TestMethod]
        public void CodeInputButtonSelectorClosedWithoutValueChangedShouldPlayExpectedSound()
        {
            Construct();
            ISoundResource mockSoundResource = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSoundResource);

            _mockButtons[0].SelectorClosed += Raise.Event<EventHandler<SelectorClosedEventArgs>>(_mockButtons[0], new SelectorClosedEventArgs(false));

            mockSoundResource.Received(1).Play();
        }
        #endregion
    }
}
