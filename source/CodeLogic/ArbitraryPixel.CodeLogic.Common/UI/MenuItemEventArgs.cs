using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuItemEventArgs : EventArgs
    {
        public IMenuItem Item { get; private set; }

        public MenuItemEventArgs(IMenuItem item)
        {
            this.Item = item;
        }
    }
}
