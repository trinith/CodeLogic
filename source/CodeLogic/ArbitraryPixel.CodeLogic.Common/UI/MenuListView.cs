using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class MenuListView : MenuViewBase, IMenuListView
    {
        private ITexture2D _pixel;
        private ISpriteBatch _spriteBatch;
        private ISpriteFont _menuFont;
        private IMenuFactory _menuFactory;

        private List<IMenuItemView> _subViews = new List<IMenuItemView>();
        private IMenuItemView _backView = null;

        private IMenuItem _selectedItem = null;
        public IMenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnSelectedItemChanged(new MenuItemEventArgs(_selectedItem));
                }
            }
        }

        public event EventHandler<EventArgs> MenuBackTapped;
        public event EventHandler<MenuItemEventArgs> MenuItemTapped;
        public event EventHandler<MenuItemEventArgs> SelectedItemChanged;

        /// <summary>
        /// Indicates that this view has changed and will have its subviews update on the next update cycle.
        /// </summary>
        public bool ViewHasChanged { get; private set; } = false;

        public MenuListView(IEngine host, RectangleF bounds, IMenuItem viewOf, ISpriteBatch spriteBatch, ISpriteFont menuFont, IMenuFactory menuFactory)
            : base(host, bounds, viewOf)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _menuFont = menuFont ?? throw new ArgumentNullException();
            _menuFactory = menuFactory ?? throw new ArgumentNullException();

            _pixel = this.Host.AssetBank.Get<ITexture2D>("Pixel");

            RefreshText();
        }

        private void RefreshText()
        {
            _subViews.Clear();
            _backView = null;

            Vector2 pos = this.Bounds.Location;
            foreach (IMenuItem childItem in this.ViewOf.Items)
            {
                float height = Math.Max(childItem.Height, _menuFont.LineSpacing);

                if (childItem.Text != "")
                {
                    IMenuItemView subView =  _menuFactory.CreateMenuItemView(this.Host, new RectangleF(pos, new SizeF(this.Bounds.Width, height)), childItem, _spriteBatch, _menuFont, TextLineAlignment.Right);
                    subView.Tapped += HandleSubViewTapped;

                    _subViews.Add(subView);
                }

                pos.Y += height;
            }

            if (this.ViewOf.Parent != null)
            {
                _backView = _menuFactory.CreateMenuItemView(this.Host, new RectangleF(this.Bounds.Left, pos.Y + _menuFont.LineSpacing, this.Bounds.Width, _menuFont.LineSpacing), _menuFactory.CreateMenuItem("Back", CodeLogicEngine.Constants.MenuItemHeight), _spriteBatch, _menuFont, TextLineAlignment.Right);
                _backView.Tapped += HandleBackTapped;
            }
        }

        private void HandleSubViewTapped(object sender, ButtonEventArgs e)
        {
            if (sender is IMenuItemView)
            {
                OnMenuItemTapped(new MenuItemEventArgs(((IMenuItemView)sender).ViewOf));
            }
        }

        private void HandleBackTapped(object sender, ButtonEventArgs e)
        {
            OnMenuBackTapped(new EventArgs());
        }

        protected void OnMenuBackTapped(EventArgs e)
        {
            if (this.MenuBackTapped != null)
                this.MenuBackTapped(this, e);
        }

        protected void OnMenuItemTapped(MenuItemEventArgs e)
        {
            if (this.MenuItemTapped != null)
                this.MenuItemTapped(this, e);
        }

        protected void OnSelectedItemChanged(MenuItemEventArgs e)
        {
            if (this.SelectedItemChanged != null)
                this.SelectedItemChanged(this, e);
        }

        protected override void OnViewOfSet()
        {
            base.OnViewOfSet();

            // Don't immediately update the sub views, but indicate that the view has changed.
            this.ViewHasChanged = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = _subViews.Count - 1; i >= 0; i--)
            {
                _subViews[i].Update(gameTime);
                _subViews[i].IsSelected = (this.SelectedItem != null && this.SelectedItem == _subViews[i].ViewOf);
            }

            _backView?.Update(gameTime);

            // If they view has changed, update the sub views now that we have finished updating the existing ones.
            if (this.ViewHasChanged)
            {
                RefreshText();
                SelectFirstItem();
                this.ViewHasChanged = false;
            }

            base.OnUpdate(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _spriteBatch.Draw(_pixel, this.Bounds, CodeLogicEngine.Constants.ClrMenuBGMid);

            foreach (IMenuItemView subView in _subViews)
            {
                if (subView.IsSelected)
                    _spriteBatch.Draw(_pixel, subView.Bounds, CodeLogicEngine.Constants.ClrMenuBGMid);

                subView.Draw(gameTime);
            }

            _backView?.Draw(gameTime);
        }

        public void SelectFirstItem()
        {
            this.SelectedItem = null;
            foreach (IMenuItem childItem in this.ViewOf.Items)
            {
                if (childItem.Items.Length == 0)
                {
                    this.SelectedItem = childItem;
                    break;
                }
            }
        }
    }
}
