using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class FadeSceneTransition : SceneTransition
    {
        #region Static Methods
        public static FadeSceneTransition Create(IEngine host, IScene startScene, IScene endScene, FadeSceneTransitionMode mode, float transitionTime)
        {
            IRenderTarget2D startTarget = host.GrfxFactory.RenderTargetFactory.Create(host.Graphics.GraphicsDevice, host.ScreenManager.Screen.X, host.ScreenManager.Screen.Y, RenderTargetUsage.DiscardContents);
            IRenderTarget2D endTarget = host.GrfxFactory.RenderTargetFactory.Create(host.Graphics.GraphicsDevice, host.ScreenManager.Screen.X, host.ScreenManager.Screen.Y, RenderTargetUsage.DiscardContents);

            ISpriteBatch spriteBatch = host.GrfxFactory.SpriteBatchFactory.Create(host.Graphics.GraphicsDevice);
            IFadeSceneTransitionModel model = GameObjectFactory.Instance.CreateFadeSceneTransitionModel(startScene, endScene, startTarget, endTarget, mode, transitionTime);

            return new FadeSceneTransition(host, model, spriteBatch);
        }
        #endregion

        private ISpriteBatch _spriteBatch;

        public FadeSceneTransition(IEngine host, ISceneTransitionModel model, ISpriteBatch spriteBatch)
            : base(host, model)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IFadeSceneTransitionModel model = this.GetModel<IFadeSceneTransitionModel>();
            Rectangle screenRect = new Rectangle(Point.Zero, this.Host.ScreenManager.Screen);

            int startA = (int)MathHelper.Clamp(model.StartSceneOpacity * 256, 0, 255);
            int endA = (int)MathHelper.Clamp(model.EndSceneOpacity * 256, 0, 255);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            _spriteBatch.Draw(this.Host.AssetBank.Get<ITexture2D>("Pixel"), screenRect, model.Background);
            _spriteBatch.Draw(model.StartTarget, Vector2.Zero, new Color(Color.White, startA));
            _spriteBatch.Draw(model.EndTarget, Vector2.Zero, new Color(Color.White, endA));
            _spriteBatch.End();
        }
    }
}
