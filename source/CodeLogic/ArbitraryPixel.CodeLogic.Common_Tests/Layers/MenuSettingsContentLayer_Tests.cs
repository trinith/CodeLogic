using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;
using System;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Layers
{
    [TestClass]
    public class MenuSettingsContentLayer_Tests
    {
        private sealed class MockControl
        {
            private enum MockControlEnum : int
            {
                SoundEnabled = 0,
                SoundVolume,
                MusicEnabled,
                MusicVolume
            }

            public static int SoundEnabled => (int)MockControlEnum.SoundEnabled;
            public static int SoundVolume => (int)MockControlEnum.SoundVolume;
            public static int MusicEnabled => (int)MockControlEnum.MusicEnabled;
            public static int MusicVolume => (int)MockControlEnum.MusicVolume;
        }

        private MenuSettingsContentLayer _sut;
        private IEngine _mockEngine;
        private ISpriteBatch _mockSpriteBatch;
        private RectangleF _bounds = new RectangleF(0, 0, 1000, 750);
        private IAudioControlsFactory _mockAudioControlsFactory;

        private IEntity[] _mockControls;

        [TestInitialize]
        public void Initialize()
        {
            _mockEngine = Substitute.For<IEngine>();
            _mockSpriteBatch = Substitute.For<ISpriteBatch>();
            _mockAudioControlsFactory = Substitute.For<IAudioControlsFactory>();

            _mockControls = new IEntity[]
                {
                    Substitute.For<ICheckButton>(), // Sound Enabled
                    Substitute.For<ISlider>(),      // Sound Volume
                    Substitute.For<ICheckButton>(), // Music Enabled
                    Substitute.For<ISlider>(),      // Music Volume
                };

            _mockAudioControlsFactory.CreateControls(Arg.Any<Vector2>(), Arg.Any<SizeF>(), Arg.Any<ISpriteBatch>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
                // Mock Sound Controls
                new AudioControlsFactoryResult(
                    new IEntity[] { _mockControls[MockControl.SoundEnabled], _mockControls[MockControl.SoundVolume] },
                    (ICheckButton)_mockControls[MockControl.SoundEnabled],
                    (ISlider)_mockControls[MockControl.SoundVolume],
                    new Vector2(11, 22)
                ),

                // Mock Music Controls
                new AudioControlsFactoryResult(
                    new IEntity[] { _mockControls[2], _mockControls[3] },
                    (ICheckButton)_mockControls[2],
                    (ISlider)_mockControls[3],
                    new Vector2(33, 44)
                )
            );
        }

        private void Construct()
        {
            _sut = new MenuSettingsContentLayer(_mockEngine, _mockSpriteBatch, _bounds, _mockAudioControlsFactory);
        }

        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructWithNullAudioControlsFactoryShouldThrowException()
        {
            _sut = new MenuSettingsContentLayer(_mockEngine, _mockSpriteBatch, _bounds, null);
        }

        [TestMethod]
        public void ConstructShouldCreateSoundControls()
        {
            Construct();

            _mockAudioControlsFactory.Received(1).CreateControls(new Vector2(20, 20), new SizeF(20, 20), _mockSpriteBatch, "MainMenuContentFont", "Enable Sound", "Sound Volume");
        }

        [TestMethod]
        public void ConstructShouldCreateMusicControls()
        {
            Construct();

            _mockAudioControlsFactory.Received(1).CreateControls(new Vector2(11, 22), new SizeF(20, 20), _mockSpriteBatch, "MainMenuContentFont", "Enable Music", "Music Volume");
        }

        [TestMethod]
        public void ConstructShouldAddCreatedAudioControlsToEntities()
        {
            Construct();

            foreach (IEntity entity in _mockControls)
                Assert.IsTrue(_sut.Entities.ToList().Contains(entity));
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_SoundEnabledChecked_TestA()
        {
            ICheckButton mockSoundEnabled = (ICheckButton)_mockControls[MockControl.SoundEnabled];
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.SoundEnabled.Returns(true);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockSoundEnabled.CheckStateChanged -= Arg.Any<EventHandler>();
                    mockSoundEnabled.Checked = true;
                    mockSoundEnabled.CheckStateChanged += Arg.Any<EventHandler>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_SoundEnabledChecked_TestB()
        {
            ICheckButton mockSoundEnabled = (ICheckButton)_mockControls[MockControl.SoundEnabled];
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.SoundEnabled.Returns(false);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockSoundEnabled.CheckStateChanged -= Arg.Any<EventHandler>();
                    mockSoundEnabled.Checked = false;
                    mockSoundEnabled.CheckStateChanged += Arg.Any<EventHandler>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_SoundVolumeValue_TestA()
        {
            ISlider mockSoundVolume = (ISlider)_mockControls[MockControl.SoundVolume];

            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.SoundVolume.Returns(0.123f);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockSoundVolume.ValueChanged -= Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                    mockSoundVolume.Value = 0.123f;
                    mockSoundVolume.ValueChanged += Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_SoundVolumeValue_TestB()
        {
            ISlider mockSoundVolume = (ISlider)_mockControls[MockControl.SoundVolume];

            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.SoundVolume.Returns(0.75f);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockSoundVolume.ValueChanged -= Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                    mockSoundVolume.Value = 0.75f;
                    mockSoundVolume.ValueChanged += Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_MusicEnabledChecked_TestA()
        {
            ICheckButton mockMusicEnabled = (ICheckButton)_mockControls[MockControl.MusicEnabled];
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.MusicEnabled.Returns(true);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockMusicEnabled.CheckStateChanged -= Arg.Any<EventHandler>();
                    mockMusicEnabled.Checked = true;
                    mockMusicEnabled.CheckStateChanged += Arg.Any<EventHandler>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_MusicEnabledChecked_TestB()
        {
            ICheckButton mockMusicEnabled = (ICheckButton)_mockControls[MockControl.MusicEnabled];
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.MusicEnabled.Returns(false);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockMusicEnabled.CheckStateChanged -= Arg.Any<EventHandler>();
                    mockMusicEnabled.Checked = false;
                    mockMusicEnabled.CheckStateChanged += Arg.Any<EventHandler>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_MusicVolumeValue_TestA()
        {
            ISlider mockMusicVolume = (ISlider)_mockControls[MockControl.MusicVolume];

            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.MusicVolume.Returns(0.123f);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockMusicVolume.ValueChanged -= Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                    mockMusicVolume.Value = 0.123f;
                    mockMusicVolume.ValueChanged += Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                }
            );
        }

        [TestMethod]
        public void ConstructShouldSetControlDefaultValueFromSettings_MusicVolumeValue_TestB()
        {
            ISlider mockMusicVolume = (ISlider)_mockControls[MockControl.MusicVolume];

            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            mockSettings.MusicVolume.Returns(0.75f);

            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            Construct();

            Received.InOrder(
                () =>
                {
                    mockMusicVolume.ValueChanged -= Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                    mockMusicVolume.Value = 0.75f;
                    mockMusicVolume.ValueChanged += Arg.Any<EventHandler<StateChangedEventArgs<float>>>();
                }
            );
        }
        #endregion

        #region SoundEnable Button Tapped Tests
        [TestMethod]
        public void SoundEnableTappedShouldSetSongManagerMutedState_True()
        {
            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.SoundEnabled];

            Construct();
            mockButton.Checked.Returns(true);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            _mockEngine.AudioManager.SoundController.Received(1).Enabled = true;
        }

        [TestMethod]
        public void SoundEnableTappedShouldSetSongManagerMutedState_False()
        {
            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.SoundEnabled];

            Construct();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            _mockEngine.AudioManager.SoundController.Received(1).Enabled = false;
        }

        [TestMethod]
        public void SoundEnableTappedShouldUpdateConfigStoreMusicEnabled()
        {
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.SoundEnabled];

            Construct();
            _mockEngine.ClearReceivedCalls();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            Received.InOrder(
                () =>
                {
                    _mockEngine.GetComponent<ICodeLogicSettings>();
                    mockSettings.SoundEnabled = false;
                }
            );
        }

        [TestMethod]
        public void SoundEnableTappedShouldPlayExpectedSound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.SoundEnabled];

            Construct();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            // Expect the sound to play after we change the state of the sound so that the sound is only heard when it is supposed to be.
            Received.InOrder(
                () =>
                {
                    _mockEngine.AudioManager.SoundController.Enabled = Arg.Any<bool>();
                    mockSound.Play();
                }
            );
        }
        #endregion

        #region SoundVolume Slider Value Changed Tests
        [TestMethod]
        public void SoundVolumeValueChangedShouldSetSoundControllerVolumeToExpectedValue()
        {
            ISlider mockSlider = (ISlider)_mockControls[MockControl.SoundVolume];

            Construct();

            mockSlider.ValueChanged += Raise.Event<EventHandler<StateChangedEventArgs<float>>>(mockSlider, new StateChangedEventArgs<float>(0f, 0.123f));

            _mockEngine.AudioManager.SoundController.Received(1).Volume = 0.123f;
        }

        [TestMethod]
        public void SoundVolumeValueChangedShouldUpdateConfigStoreSoundVolume()
        {
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            ISlider mockSlider = (ISlider)_mockControls[MockControl.SoundVolume];

            Construct();
            _mockEngine.ClearReceivedCalls();

            mockSlider.ValueChanged += Raise.Event<EventHandler<StateChangedEventArgs<float>>>(mockSlider, new StateChangedEventArgs<float>(0f, 0.123f));

            Received.InOrder(
                () =>
                {
                    _mockEngine.GetComponent<ICodeLogicSettings>();
                    mockSettings.SoundVolume = 0.123f;
                }
            );
        }
        #endregion

        #region MusicEnable Button Tapped Tests
        [TestMethod]
        public void MusicEnableTappedShouldSetMusicControllerEnabledToExpectedValue_True()
        {
            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.MusicEnabled];

            Construct();
            mockButton.Checked.Returns(true);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            _mockEngine.AudioManager.MusicController.Received(1).Enabled = true;
        }

        [TestMethod]
        public void MusicEnableTappedShouldSetMusicControllerEnabledToExpectedValue_False()
        {
            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.MusicEnabled];

            Construct();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            _mockEngine.AudioManager.MusicController.Received(1).Enabled = false;
        }

        [TestMethod]
        public void MusicEnableTappedShouldUpdateConfigStoreMusicEnabled()
        {
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.MusicEnabled];

            Construct();
            _mockEngine.ClearReceivedCalls();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            Received.InOrder(
                () =>
                {
                    _mockEngine.GetComponent<ICodeLogicSettings>();
                    mockSettings.MusicEnabled = false;
                }
            );
        }

        [TestMethod]
        public void MusicEnableTappedShouldPlayExpectedSound()
        {
            ISoundResource mockSound = Substitute.For<ISoundResource>();
            _mockEngine.AssetBank.Get<ISoundResource>("ButtonPress").Returns(mockSound);

            ICheckButton mockButton = (ICheckButton)_mockControls[MockControl.MusicEnabled];

            Construct();
            mockButton.Checked.Returns(false);

            mockButton.CheckStateChanged += Raise.Event<EventHandler>(mockButton, new EventArgs());

            // No order requirements for this one, just make sure it played.
            mockSound.Received(1).Play();
        }
        #endregion

        #region MusicVolume Slider Value Changed Tests
        [TestMethod]
        public void MusicVolumeValueChangedShouldSetMusicControllerVolumeToExpectedValue()
        {
            ISlider mockSlider = (ISlider)_mockControls[MockControl.MusicVolume];

            Construct();

            mockSlider.ValueChanged += Raise.Event<EventHandler<StateChangedEventArgs<float>>>(mockSlider, new StateChangedEventArgs<float>(0f, 0.123f));

            _mockEngine.AudioManager.MusicController.Received(1).Volume = 0.123f;
        }

        [TestMethod]
        public void MusicVolumeValueChangedShouldUpdateConfigStoreMusicVolume()
        {
            ICodeLogicSettings mockSettings = Substitute.For<ICodeLogicSettings>();
            _mockEngine.GetComponent<ICodeLogicSettings>().Returns(mockSettings);

            ISlider mockSlider = (ISlider)_mockControls[MockControl.MusicVolume];

            Construct();
            _mockEngine.ClearReceivedCalls();

            mockSlider.ValueChanged += Raise.Event<EventHandler<StateChangedEventArgs<float>>>(mockSlider, new StateChangedEventArgs<float>(0f, 0.123f));

            Received.InOrder(
                () =>
                {
                    _mockEngine.GetComponent<ICodeLogicSettings>();
                    mockSettings.MusicVolume = 0.123f;
                }
            );
        }
        #endregion
    }
}
