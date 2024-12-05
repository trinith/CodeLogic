using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Layers
{
    public interface IMenuBriefingContentLayer : IMenuContentLayer
    {
        event EventHandler<EventArgs> PageChanged;

        int CurrentPage { get; }
        int TotalPages { get; }

        void NextPage();
        void PreviousPage();

        void SetPage(GameObjectFactory.BriefingPage page);
    }

    public class MenuBriefingContentLayer : MenuContentLayerBase, IMenuBriefingContentLayer
    {
        private ILayer _contentHostLayer;
        private ILayer _uiLayer;

        private List<ILayer> _contentPages = new List<ILayer>();

        public MenuBriefingContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, ILayer[] contentPages)
                : base(host, mainSpriteBatch, contentBounds)
        {
            if (contentPages == null || contentPages.Length <= 0)
                throw new ArgumentNullException("contentPages", "Content pages must not be null and must contain at least one ILayer object.");

            contentBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width, -CodeLogicEngine.Constants.TextWindowPadding.Height);

            _contentPages.AddRange(contentPages);

            // Create the content host layer.
            _contentHostLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.MainSpriteBatch);
            this.AddEntity(_contentHostLayer);
            _contentHostLayer.AddEntity(_contentPages[this.CurrentPage - 1]);

            // Create the ui layer.
            _uiLayer = GameObjectFactory.Instance.CreateMenuBriefingUILayer(this.Host, this.MainSpriteBatch, contentBounds, this);
            this.AddEntity(_uiLayer);
        }

        #region IMenuBriefingContentLayer
        public event EventHandler<EventArgs> PageChanged;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            private set
            {
                if (value != _currentPage)
                {
                    _contentHostLayer.RemoveEntity(_contentPages[_currentPage - 1]);
                    _currentPage = value;
                    _contentHostLayer.AddEntity(_contentPages[_currentPage - 1]);

                    OnPageChanged(new EventArgs());
                }
            }
        }

        public int TotalPages => _contentPages.Count;

        public void NextPage()
        {
            if (this.CurrentPage < _contentPages.Count)
                this.CurrentPage += 1;
        }

        public void PreviousPage()
        {
            if (this.CurrentPage > 1)
                this.CurrentPage -= 1;
        }

        public void SetPage(GameObjectFactory.BriefingPage page)
        {
            int targetPage = (int)page + 1;

            if (this.CurrentPage != targetPage)
                this.CurrentPage = (int)page + 1;
        }
        #endregion

        #region Protected Methods
        protected virtual void OnPageChanged(EventArgs e)
        {
            if (this.PageChanged != null)
                this.PageChanged(this, e);
        }
        #endregion
    }
}
