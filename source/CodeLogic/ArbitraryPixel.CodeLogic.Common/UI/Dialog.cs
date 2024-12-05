using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.UI
{
    public class Dialog : GameEntityBase, IDialog
    {
        #region Private Members
        private ILayer _hostLayer;

        private RectangleF _windowBounds;
        private ITextObjectBuilder _textBuilder;
        private string _textFormat;

        private IConsoleWindow _consoleWindow;

        private DialogResult? _currentResult = null;

        private Color _backgroundColour = new Color(32, 32, 32);
        private Color _borderColour = Color.White;

        private List<ILayer> _contentLayers = new List<ILayer>();
        #endregion

        #region Constructor
        public Dialog(IEngine host, RectangleF windowBounds, ITextObjectBuilder textBuilder, string textFormat)
            : base(host, windowBounds)
        {
            _windowBounds = windowBounds;
            _textBuilder = textBuilder ?? throw new ArgumentNullException();
            _textFormat = textFormat;

            _hostLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice));
            _hostLayer.AddEntity(GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice)));
            _hostLayer.AddEntity(GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice)));
            _hostLayer.AddEntity(GameObjectFactory.Instance.CreateGenericLayer(this.Host, _hostLayer.MainSpriteBatch));

            this.Visible = false;
            this.Enabled = false;
        }
        #endregion

        #region Private / Protected Methods
        protected void Close(DialogResult result)
        {
            _currentResult = result;
            Close();
        }

        private void ClearParentLayer()
        {
            foreach (ILayer layer in _hostLayer.Entities)
                layer.ClearEntities();
            _consoleWindow = null;

            OnClearEntities();
        }

        private RectangleF GetBorderInteriorRectangle(RectangleF exteriorBounds)
        {
            exteriorBounds.Inflate(-CodeLogicEngine.Constants.TextWindowBorderSize.Width, -CodeLogicEngine.Constants.TextWindowBorderSize.Height);
            return exteriorBounds;
        }

        private RectangleF GetClientRectangle(RectangleF exteriorBounds)
        {
            RectangleF borderInterior = GetBorderInteriorRectangle(exteriorBounds);
            borderInterior.Inflate(-CodeLogicEngine.Constants.TextWindowPadding.Width, -CodeLogicEngine.Constants.TextWindowPadding.Height);
            return borderInterior;
        }

        private void ToggleContentLayer(bool value)
        {
            _hostLayer.Entities[1].Enabled = value;
            _hostLayer.Entities[1].Visible = value;
        }

        private void ToggleThis(bool value)
        {
            this.Enabled = value;
            this.Visible = value;
        }
        #endregion

        #region IDialog Implementation
        public Color BackgroundColour
        {
            get { return _backgroundColour; }
            set
            {
                _backgroundColour = value;
                if (_consoleWindow != null)
                    _consoleWindow.BackgroundColour = _backgroundColour;
            }
        }

        public Color BorderColour
        {
            get { return _borderColour; }
            set
            {
                _borderColour = value;
                if (_consoleWindow != null)
                    _consoleWindow.BorderColour = _borderColour;
            }
        }

        public RectangleF ClientRectangle { get { return GetClientRectangle(this.Bounds); } }

        public bool IsOpen
        {
            get { return (_consoleWindow != null && _consoleWindow.IsDisposed == false); }
        }

        public event EventHandler<DialogClosedEventArgs> DialogClosed;

        public void Show()
        {
            ClearParentLayer();

            OnShowing();

            _currentResult = null;

            ILayer backgroundLayer = (ILayer)_hostLayer.Entities[0];

            ITextureEntity backgroundTexture = GameObjectFactory.Instance.CreateTextureEntity(
                this.Host,
                new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World),
                backgroundLayer.MainSpriteBatch,
                this.Host.AssetBank.Get<ITexture2D>("Pixel"),
                new Color(0, 0, 0, 128)
            );

            SizeF borderSize = CodeLogicEngine.Constants.TextWindowBorderSize;
            SizeF padding = CodeLogicEngine.Constants.TextWindowPadding;
            RectangleF textBounds = _windowBounds;
            textBounds.Inflate(-padding.Width - borderSize.Width, -padding.Height - borderSize.Height);

            ITextObjectRenderer textRenderer = GameObjectFactory.Instance.CreateTextObjectRenderer(this.Host.GrfxFactory.RenderTargetFactory, this.Host.Graphics.GraphicsDevice, backgroundLayer.MainSpriteBatch, (Rectangle)textBounds);

            _consoleWindow = GameObjectFactory.Instance.CreateConsoleWindow(this.Host, _windowBounds, backgroundLayer.MainSpriteBatch, _textBuilder, textRenderer);
            _consoleWindow.AutoAdvanceOnTap = false;
            _consoleWindow.SetTextFormat(_textFormat);
            _consoleWindow.Tapped += Handle_ConsoleWindowTapped;
            _consoleWindow.Disposed += Handle_ConsoleWindowDisposed;
            _consoleWindow.WindowStateChanged += Handle_ConsoleWindowStateChanged;
            _consoleWindow.Padding = padding;
            _consoleWindow.BorderSize = borderSize;
            _consoleWindow.BackgroundColour = _backgroundColour;
            _consoleWindow.BorderColour = _borderColour;

            backgroundLayer.AddEntity(backgroundTexture);
            backgroundLayer.AddEntity(_consoleWindow);

            ILayer contentLayer = (ILayer)_hostLayer.Entities[1];
            ToggleContentLayer(false);
            foreach (ILayer layer in _contentLayers)
            {
                contentLayer.AddEntity(layer);
            }

            OnCreateAdditionalEntities((ILayer)_hostLayer.Entities[2], _consoleWindow.ClientRectangle);

            ToggleThis(true);
        }

        public void Close()
        {
            OnClosing();
            ToggleContentLayer(false);
            _consoleWindow?.CloseWindow();
        }

        public void AddContentLayer(ILayer layer)
        {
            _contentLayers.Add(layer);
        }
        #endregion

        #region Event Handlers
        private void Handle_ConsoleWindowStateChanged(object sender, StateChangedEventArgs<WindowState> e)
        {
            if (e.PreviousState == WindowState.Opening)
            {
                ToggleContentLayer(true);
                OnShown();
            }
        }

        private void Handle_ConsoleWindowDisposed(object sender, EventArgs e)
        {
            ClearParentLayer();

            OnClosed();

            ToggleThis(false);
        }

        private void Handle_ConsoleWindowTapped(object sender, ButtonEventArgs e)
        {
            if (_consoleWindow.WindowState == WindowState.Processing)
                _consoleWindow.FlushText();
            else if (this.AllowAutoAccept && _windowBounds.Contains(e.Location) == false)
                this.Close(DialogResult.Ok);
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// If true, window will automatically closed when tapped outside the window bounds. DialogResult will be set to Ok.
        /// </summary>
        protected virtual bool AllowAutoAccept => true;

        /// <summary>
        /// Called when the dialog window starts to show.
        /// </summary>
        protected virtual void OnShowing() { }

        /// <summary>
        /// Called when the dialog window has fully opened.
        /// </summary>
        protected virtual void OnShown() { }

        /// <summary>
        /// Called when the dialog window starts to close.
        /// </summary>
        protected virtual void OnClosing() { }

        /// <summary>
        /// Called when the dialog window has fully closed.
        /// </summary>
        protected virtual void OnClosed()
        {
            if (this.DialogClosed != null)
                this.DialogClosed(this, new DialogClosedEventArgs((_currentResult != null) ? _currentResult.Value : DialogResult.Cancel));
        }

        /// <summary>
        /// Create any additional entities this dialog will display.
        /// </summary>
        /// <param name="layer">The layer the entities will be added to.</param>
        /// <param name="clientRectangle">The client rectangle for the dialog window.</param>
        protected virtual void OnCreateAdditionalEntities(ILayer layer, RectangleF clientRectangle) { }

        /// <summary>
        /// Called when the dialog window is clearing its entities. The dialog layer will be automatically cleared; however, any handles to entities created during OnCreateAdditionalEntities should be released here.
        /// </summary>
        protected virtual void OnClearEntities() { }
        #endregion

        #region Override Methods
        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            _hostLayer.PreDraw(gameTime);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            _hostLayer.Update(gameTime);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            _hostLayer.Draw(gameTime);
        }
        #endregion
    }
}
