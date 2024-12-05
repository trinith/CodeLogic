using System;
using ArbitraryPixel.CodeLogic.Common.Controllers;
using ArbitraryPixel.Common.Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using NSubstitute;

namespace ArbitraryPixel.CodeLogic.Common_Tests.Controllers
{
    [TestClass]
    public class SoundPlaybackController_Tests
    {
        private SoundPlaybackController _sut;

        [TestInitialize]
        public void Initialize()
        {
        }

        private void Construct()
        {
            _sut = new SoundPlaybackController();
        }

        #region AddSound Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSoundWithNullSoundShouldThrowException()
        {
            Construct();

            _sut.AddSound(null);
        }
        #endregion

        #region Reset Tests
        [TestMethod]
        public void ResetBeforeDelayExpiresShouldNotCallPlayOnSoundAfterDelayExpires()
        {
            ISound mockSound = Substitute.For<ISound>();
            Construct();
            _sut.AddSound(mockSound);

            _sut.Reset();
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            mockSound.Received(0).Play();
        }

        [TestMethod]
        public void ResetShouldCallStopOnPlayingSounds()
        {
            Construct();

            ISound mockSound = Substitute.For<ISound>();
            mockSound.State.Returns(SoundState.Playing);
            _sut.AddSound(mockSound);

            _sut.Update(new GameTime());

            _sut.Reset();

            mockSound.Received(1).Stop();
        }
        #endregion

        #region Update Tests
        [TestMethod]
        public void UpdateBeforeDelayExpiresShouldNotCallPlayOnSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            Construct();
            _sut.AddSound(mockSound, 3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(2)));

            mockSound.Received(0).Play();
        }

        [TestMethod]
        public void UpdateAfterDelayExpiresShouldCallPlayOnSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            Construct();
            _sut.AddSound(mockSound, 3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(4)));

            mockSound.Received(1).Play();
        }

        [TestMethod]
        public void UpdateAfterSoundPlayedShouldNotCallPlayOnSound()
        {
            ISound mockSound = Substitute.For<ISound>();
            Construct();
            _sut.AddSound(mockSound, 3);

            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(4)));
            mockSound.ClearReceivedCalls();
            _sut.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(999)));

            mockSound.Received(0).Play();
        }

        [TestMethod]
        public void UpdateWithPlayingSoundThatStopsShouldRemoveSoundFromTracking()
        {
            Construct();

            ISound mockSound = Substitute.For<ISound>();
            mockSound.State.Returns(SoundState.Stopped);
            _sut.AddSound(mockSound);

            _sut.Reset(); // Use reset to ensure we don't get a stop... because it's not tracked anymore.

            mockSound.Received(0).Stop();
        }
        #endregion
    }
}
