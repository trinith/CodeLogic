using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class OkCancelDialog : Dialog
    {
        private ISimpleButton _okButton;
        private ISimpleButton _cancelButton;

        public OkCancelDialog(IEngine host, RectangleF windowBounds, ITextObjectBuilder textBuilder, string textFormat)
            : base(host, windowBounds, textBuilder, textFormat)
        {
        }

        protected override void OnCreateAdditionalEntities(ILayer layer, RectangleF clientRectangle)
        {
            base.OnCreateAdditionalEntities(layer, clientRectangle);

            SizeF buttonSize = CodeLogicEngine.Constants.MenuButtonSize;
            ISpriteFont buttonFont = this.Host.AssetBank.Get<ISpriteFont>("MainButtonFont");

            RectangleF buttonRect = new RectangleF(new Vector2(clientRectangle.Right - buttonSize.Width, clientRectangle.Bottom - buttonSize.Height), buttonSize);
            _cancelButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonRect, layer.MainSpriteBatch, buttonFont);
            _cancelButton.Text = "Cancel";
            _cancelButton.Visible = false;
            _cancelButton.Tapped += Handle_CancelButtonTapped;

            buttonRect.Location -= new Vector2(buttonSize.Width + CodeLogicEngine.Constants.TextWindowPadding.Width, 0);
            _okButton = GameObjectFactory.Instance.CreateSimpleButton(this.Host, buttonRect, layer.MainSpriteBatch, buttonFont);
            _okButton.Text = "Ok";
            _okButton.Visible = false;
            _okButton.Tapped += Handle_OKButtonTapped;

            layer.AddEntity(_cancelButton);
            layer.AddEntity(_okButton);
        }

        #region Overrides
        protected override bool AllowAutoAccept => false;

        protected override void OnShown()
        {
            base.OnShown();

            _okButton.Visible = true;
            _cancelButton.Visible = true;
        }

        protected override void OnClosing()
        {
            base.OnClosing();

            _okButton.Visible = false;
            _cancelButton.Visible = false;
        }

        protected override void OnClearEntities()
        {
            base.OnClearEntities();

            _okButton = null;
            _cancelButton = null;
        }
        #endregion

        #region Event Handlers
        private void Handle_OKButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            this.Close(DialogResult.Ok);
        }

        private void Handle_CancelButtonTapped(object sender, ButtonEventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            this.Close(DialogResult.Cancel);
        }
        #endregion
    }
}
