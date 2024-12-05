using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.UI;
using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuViewBase : ButtonBase, IMenuView
    {
        private IMenuItem _viewOf = null;

        public IMenuItem ViewOf
        {
            get { return _viewOf; }
            set
            {
                if (_viewOf != value)
                {
                    _viewOf = value;
                    OnViewOfSet();
                }
            }
        }

        public MenuViewBase(IEngine host, RectangleF bounds, IMenuItem viewOf)
            : base(host, bounds)
        {
            _viewOf = viewOf ?? throw new ArgumentNullException();
        }

        protected virtual void OnViewOfSet()
        {
        }
    }
}
