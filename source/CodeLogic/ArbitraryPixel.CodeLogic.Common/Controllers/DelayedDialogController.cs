using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Controllers
{
    public class DelayedDialogController : GameEntityBase, IController
    {
        private IDialog _dialog;
        private double _delayTime = 0;

        public DelayedDialogController(IEngine host, IDialog dialog, double delayTime)
            : base(host, (dialog == null) ? throw new ArgumentNullException() : dialog.Bounds)
        {
            _dialog = dialog;
            _delayTime = delayTime;

            _dialog.DialogClosed += Handle_DialogClosed;
        }

        private void Handle_DialogClosed(object sender, DialogClosedEventArgs e)
        {
            _dialog.DialogClosed -= Handle_DialogClosed;
            _dialog.Dispose();
            this.Dispose();
        }

        public void Reset()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_delayTime > 0)
            {
                _delayTime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (_delayTime <= 0)
                {
                    _delayTime = 0;
                    _dialog.Show();
                }
            }

            _dialog.Update(gameTime);
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);
            _dialog.PreDraw(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);
            _dialog.Draw(gameTime);
        }
    }
}
