using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuContentLayer : ILayer
    {
        RectangleF ContentBounds { get; }

        void Show();
        void Hide();
    }

    public class MenuContentLayerBase : LayerBase, IMenuContentLayer
    {
        public RectangleF ContentBounds { get; private set; }

        public MenuContentLayerBase(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
            : base(host, mainSpriteBatch)
        {
            this.ContentBounds = contentBounds;
        }

        #region IMenuContentLayer Implementation
        public void Hide()
        {
            OnHide();
        }

        public void Show()
        {
            OnShow();
        }
        #endregion

        #region Protected Methods
        protected virtual void OnShow() { }

        protected virtual void OnHide() { }
        #endregion
    }
}
