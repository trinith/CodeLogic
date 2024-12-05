using System;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public interface IMenuListView : IMenuView
    {
        IMenuItem SelectedItem { get; set; }

        event EventHandler<EventArgs> MenuBackTapped;
        event EventHandler<MenuItemEventArgs> MenuItemTapped;
        event EventHandler<MenuItemEventArgs> SelectedItemChanged;
    }
}
