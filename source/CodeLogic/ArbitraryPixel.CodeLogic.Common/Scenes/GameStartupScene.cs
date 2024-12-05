using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    /// <summary>
    /// A scene to act as a starting point for the entire game.
    /// </summary>
    public class GameStartupScene : SceneBase
    {
        private IScene _startScene;

        /// <summary>
        /// Create a new object.
        /// </summary>
        /// <param name="host">The host for this scene.</param>
        /// <param name="startScene">The scene the game should start at.</param>
        public GameStartupScene(IEngine host, IScene startScene) : base(host)
        {
            _startScene = startScene ?? throw new ArgumentNullException();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.NextScene = _startScene;
            this.SceneComplete = true;
        }
    }
}
