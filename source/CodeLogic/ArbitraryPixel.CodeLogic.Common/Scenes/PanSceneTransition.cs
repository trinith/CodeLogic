using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class PanSceneTransition : SceneTransition
    {
        #region Static Methods
        public static PanSceneTransition Create(IEngine host, IScene startScene, IScene endScene, PanSceneTransitionMode transitionMode, double transitionTime)
        {
            IRenderTarget2D startTarget = host.GrfxFactory.RenderTargetFactory.Create(host.Graphics.GraphicsDevice, host.ScreenManager.Screen.X, host.ScreenManager.Screen.Y, RenderTargetUsage.DiscardContents);
            IRenderTarget2D endTarget = host.GrfxFactory.RenderTargetFactory.Create(host.Graphics.GraphicsDevice, host.ScreenManager.Screen.X, host.ScreenManager.Screen.Y, RenderTargetUsage.DiscardContents);

            IPanSceneTransitionModel model = GameObjectFactory.Instance.CreatePanSceneTransitionModel(startScene, endScene, startTarget, endTarget, transitionMode, transitionTime, (SizeF)host.ScreenManager.Screen);
            ISpriteBatch spriteBatch = host.GrfxFactory.SpriteBatchFactory.Create(host.Graphics.GraphicsDevice);

            return new PanSceneTransition(host, model, spriteBatch);
        }
        #endregion

        private ISpriteBatch _spriteBatch;

        public PanSceneTransition(IEngine host, IPanSceneTransitionModel model, ISpriteBatch spriteBatch)
            : base(host, model)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IPanSceneTransitionModel model = this.GetModel<IPanSceneTransitionModel>();

            _spriteBatch.Begin();

            _spriteBatch.Draw(model.StartTarget, model.StartAnchor, Color.White);
            _spriteBatch.Draw(model.EndTarget, model.EndAnchor, Color.White);

            _spriteBatch.End();
        }
    }
}
