using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Graphics.Factory;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Common.Screen;
using ArbitraryPixel.Common.ValueConversion;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Audio;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests
{
    [TestClass]
    public class CodeLogicEngineTestBase : UnitTestBase<CodeLogicEngine>
    {
        protected IComponentContainer _mockContainer;

        protected IGrfxDeviceManager _mockGraphics;
        protected IContentManager _mockContentManager;
        protected ISurfaceInputManager _mockInputManager;
        protected IScreenManager _mockScreenManager;
        protected IGrfxFactory _mockGrfxFactory;
        protected IAssetBank _mockAssetBank;
        protected IAudioManager _mockAudioManager;
        protected IBuildInfoStore _mockBuildInfoStore;
        protected ICodeLogicSettings _mockSettings;
        protected IStopwatchManager _mockStopwatchManager;
        protected IGameStatsData _mockGameStatsData;

        protected IMenuFactory _mockMenuFactory;

        protected IGrfxDevice _mockGrfxDevice;
        protected ISpriteBatchFactory _mockSpriteBatchFactory;
        protected ISpriteBatch _mockSpriteBatch;

        protected IDeviceModel _mockDeviceModel;
        protected ILogPanelModel _mockLogPanelModel;

        protected GameObjectFactory _mockGameObjectFactory;

        protected override void OnInitializing()
        {
            base.OnInitializing();

            _mockContainer = Substitute.For<IComponentContainer>();

            _mockGameObjectFactory = Substitute.For<GameObjectFactory>();
            GameObjectFactory.SetInstance(_mockGameObjectFactory);

            _mockGrfxDevice = Substitute.For<IGrfxDevice>();
            _mockSpriteBatchFactory = Substitute.For<ISpriteBatchFactory>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();

            _mockGraphics = Substitute.For<IGrfxDeviceManager>();
            _mockContainer.GetComponent<IGrfxDeviceManager>().Returns(_mockGraphics);

            _mockContentManager = Substitute.For<IContentManager>();
            _mockContainer.GetComponent<IContentManager>().Returns(_mockContentManager);

            _mockInputManager = Substitute.For<ISurfaceInputManager>();
            _mockContainer.GetComponent<ISurfaceInputManager>().Returns(_mockInputManager);

            _mockScreenManager = Substitute.For<IScreenManager>();
            _mockContainer.GetComponent<IScreenManager>().Returns(_mockScreenManager);

            _mockGrfxFactory = Substitute.For<IGrfxFactory>();
            _mockContainer.GetComponent<IGrfxFactory>().Returns(_mockGrfxFactory);

            _mockAssetBank = Substitute.For<IAssetBank>();
            _mockContainer.GetComponent<IAssetBank>().Returns(_mockAssetBank);

            _mockAudioManager = Substitute.For<IAudioManager>();
            _mockContainer.GetComponent<IAudioManager>().Returns(_mockAudioManager);

            _mockBuildInfoStore = Substitute.For<IBuildInfoStore>();
            _mockContainer.GetComponent<IBuildInfoStore>().Returns(_mockBuildInfoStore);

            _mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockContainer.GetComponent<ICodeLogicSettings>().Returns(_mockSettings);

            _mockStopwatchManager = Substitute.For<IStopwatchManager>();
            _mockContainer.GetComponent<IStopwatchManager>().Returns(_mockStopwatchManager);

            _mockGameStatsData = Substitute.For<IGameStatsData>();
            _mockContainer.GetComponent<IGameStatsData>().Returns(_mockGameStatsData);

            _mockGraphics.GraphicsDevice.Returns(_mockGrfxDevice);
            _mockGrfxFactory.SpriteBatchFactory.Returns(_mockSpriteBatchFactory);
            _mockSpriteBatchFactory.Create(Arg.Any<IGrfxDevice>()).Returns(_mockSpriteBatch);

            _mockGameObjectFactory.CreateDeviceModel(Arg.Any<IRandom>(), Arg.Any<IStopwatchManager>()).Returns(_mockDeviceModel = Substitute.For<IDeviceModel>());
            _mockGameObjectFactory.CreateLogPanelModel(Arg.Any<ICodeLogicSettings>(), Arg.Any<SizeF>()).Returns(_mockLogPanelModel = Substitute.For<ILogPanelModel>());

            _mockGameObjectFactory.CreateMenuFactory().Returns(_mockMenuFactory = Substitute.For<IMenuFactory>());
        }

        protected override CodeLogicEngine OnCreateSUT()
        {
            return new CodeLogicEngine(_mockContainer);
        }
    }
}
