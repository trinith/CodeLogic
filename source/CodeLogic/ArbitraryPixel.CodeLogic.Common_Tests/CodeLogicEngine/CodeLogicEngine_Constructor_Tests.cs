using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class CodeLogicEngine_Constructor_Tests
    {
        private IComponentContainer _mockContainer;
        private GameObjectFactory _mockGameObjectFactory;

        [TestInitialize]
        public void Initialize()
        {
            _mockContainer = Substitute.For<IComponentContainer>();

            _mockContainer.GetComponent<IGrfxDeviceManager>().Returns(Substitute.For<IGrfxDeviceManager>());
            _mockContainer.GetComponent<IContentManager>().Returns(Substitute.For<IContentManager>());
            _mockContainer.GetComponent<IScreenManager>().Returns(Substitute.For<IScreenManager>());
            _mockContainer.GetComponent<ISurfaceInputManager>().Returns(Substitute.For<ISurfaceInputManager>());
            _mockContainer.GetComponent<IGrfxFactory>().Returns(Substitute.For<IGrfxFactory>());
            _mockContainer.GetComponent<IAssetBank>().Returns(Substitute.For<IAssetBank>());
            _mockContainer.GetComponent<IAudioManager>().Returns(Substitute.For<IAudioManager>());
            _mockContainer.GetComponent<IBuildInfoStore>().Returns(Substitute.For<IBuildInfoStore>());
            _mockContainer.GetComponent<ICodeLogicSettings>().Returns(Substitute.For<ICodeLogicSettings>());
            _mockContainer.GetComponent<IStopwatchManager>().Returns(Substitute.For<IStopwatchManager>());
            _mockContainer.GetComponent<IValueConverterManager>().Returns(Substitute.For<IValueConverterManager>());

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);
        }

        [TestMethod]
        public void ConstructorShouldRegisterThemeManagerCollection()
        {
            IThemeManagerCollection mockThemeCollection = Substitute.For<IThemeManagerCollection>();
            _mockGameObjectFactory.CreateThemeManagerCollection().Returns(mockThemeCollection);

            var sut = new CodeLogicEngine(_mockContainer);

            _mockContainer.Received(1).RegisterComponent<IThemeManagerCollection>(mockThemeCollection);
        }

        [TestMethod]
        [ExpectedException(typeof(GameObjectFactoryInstanceNullException))]
        public void ConstructWithNullGameObjectFactoryInstanceShouldThrowException()
        {
            GameObjectFactory.SetInstance(null);
            var sut = new CodeLogicEngine(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void ConstructWithoutComponentShouldThrowException_IBuildInfoStore()
        {
            _mockContainer.GetComponent<IBuildInfoStore>().Returns((IBuildInfoStore)null);

            var sut = new CodeLogicEngine(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void ConstructWithoutComponentShouldThrowException_ICodeLogicSettings()
        {
            _mockContainer.GetComponent<ICodeLogicSettings>().Returns((ICodeLogicSettings)null);

            var sut = new CodeLogicEngine(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void ConstructWithoutComponentShouldThrowException_IStopwatchManager()
        {
            _mockContainer.GetComponent<IStopwatchManager>().Returns((IStopwatchManager)null);

            var sut = new CodeLogicEngine(_mockContainer);
        }

        [TestMethod]
        [ExpectedException(typeof(RequiredComponentMissingException))]
        public void ConstructWithoutComponentShouldThrowException_IValueConverterManager()
        {
            _mockContainer.GetComponent<IValueConverterManager>().Returns((IValueConverterManager)null);

            var sut = new CodeLogicEngine(_mockContainer);
        }
    }
}
