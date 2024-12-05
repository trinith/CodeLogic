using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public enum WindowState
    {
        Ready,
        Opening,
        Processing,
        Waiting,
        Closing,
        Complete
    }

    public interface IConsoleWindow : IButton
    {
        WindowState WindowState { get; }
        RectangleF ClientRectangle { get; }
        SizeF Padding { get; set; }
        SizeF BorderSize { get; set; }
        bool AutoAdvanceOnTap { get; set; }
        bool ShowBackground { get; set; }

        Color BackgroundColour { get; set; }
        Color BorderColour { get; set; }

        event EventHandler<StateChangedEventArgs<WindowState>> WindowStateChanged;

        void SetTextFormat(string formatString);
        void FlushText();
        void CloseWindow();
    }

    public class ConsoleWindow : ButtonBase, IConsoleWindow
    {
        private const float TIME_TO_ANIMATEWINDOW = 0.25f;

        private ISpriteBatch _spriteBatch;

        private ITexture2D _pixelTexture;
        private RectangleF _currentBounds = RectangleF.Empty;
        private Vector2 _windowAnimateVelocity;

        private ITextObjectBuilder _textBuilder;
        private ITextObjectRenderer _textRenderer;
        private ITexture2D _textTexture = null;

        public bool AutoAdvanceOnTap { get; set; } = true;
        public bool ShowBackground { get; set; } = true;

        public Color BackgroundColour { get; set; } = new Color(32, 32, 32);
        public Color BorderColour { get; set; } = Color.Red;

        public WindowState WindowState { get; private set; } = WindowState.Ready;
        public RectangleF ClientRectangle
        {
            get { return GetClientRectangle(this.Bounds); }
        }

        public SizeF Padding { get; set; } = SizeF.Empty;
        public SizeF BorderSize { get; set; } = new SizeF(1);

        public event EventHandler<StateChangedEventArgs<WindowState>> WindowStateChanged;

        public ConsoleWindow(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ITextObjectBuilder textBuilder, ITextObjectRenderer textRenderer, bool animateOpen = true)
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _textBuilder = textBuilder ?? throw new ArgumentNullException();
            _textRenderer = textRenderer ?? throw new ArgumentNullException();

            _pixelTexture = this.Host.GrfxFactory.Texture2DFactory.Create(this.Host.Graphics.GraphicsDevice, 1, 1);
            _pixelTexture.SetData<Color>(new Color[] { Color.White });
            _currentBounds = new RectangleF(bounds.Centre, SizeF.Empty);

            _windowAnimateVelocity = new Vector2(
                this.Bounds.Width / TIME_TO_ANIMATEWINDOW,
                this.Bounds.Height / TIME_TO_ANIMATEWINDOW
            );

            if (animateOpen == false)
                SetWindowState(WindowState.Processing);
        }

        public void SetTextFormat(string formatString)
        {
            List<ITextObject> textObjects = _textBuilder.Build(formatString, new RectangleF(Vector2.Zero, this.ClientRectangle.Size));
            foreach (ITextObject textObject in textObjects)
                _textRenderer.Enqueue(textObject);
        }

        public void CloseWindow()
        {
            SetWindowState(WindowState.Closing);
        }

        public void FlushText()
        {
            _textRenderer.Flush();
        }

        private void SetWindowState(WindowState newState)
        {
            // Assist with state migration.
            switch (newState)
            {
                case WindowState.Processing:
                    _currentBounds = this.Bounds;
                    break;
                case WindowState.Complete:
                    _currentBounds = new RectangleF(this.Bounds.Centre, SizeF.Empty);
                    this.Alive = false;
                    break;
            }

            WindowState previousState = this.WindowState;
            this.WindowState = newState;

            OnWindowStateChanged(new StateChangedEventArgs<WindowState>(previousState, this.WindowState));
        }

        protected virtual void OnWindowStateChanged(StateChangedEventArgs<WindowState> e)
        {
            if (e.PreviousState == WindowState.Ready && e.CurrentState == WindowState.Opening)
                this.Host.AssetBank.Get<ISoundResource>("WindowOpen").Play();
            else if (e.PreviousState == WindowState.Waiting && e.CurrentState == WindowState.Closing)
                this.Host.AssetBank.Get<ISoundResource>("WindowClose").Play();

            if (this.WindowStateChanged != null)
                this.WindowStateChanged(this, e);
        }

        protected override void OnTapped(ButtonEventArgs e)
        {
            base.OnTapped(e);

            if (this.AutoAdvanceOnTap)
            {
                switch (this.WindowState)
                {
                    case WindowState.Processing:
                        _textRenderer.Flush();
                        SetWindowState(WindowState.Waiting);
                        break;
                    case WindowState.Waiting:
                        SetWindowState(WindowState.Closing);
                        break;
                }
            }
        }

        protected override bool IsPointInBounds(Vector2 p)
        {
            // Interaction bounds should be entire screen :)
            RectangleF testBounds = new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World);
            return testBounds.Contains(p);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // First update, advance the state to opening and then process.
            if (this.WindowState == WindowState.Ready)
                SetWindowState(WindowState.Opening);

            switch (this.WindowState)
            {
                case WindowState.Opening:
                    _currentBounds.Size += (SizeF)_windowAnimateVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _currentBounds.Location = this.Bounds.Centre - _currentBounds.Size.Centre;

                    if (_currentBounds.Size.Width >= this.Bounds.Size.Width)
                    {
                        SetWindowState(WindowState.Processing);
                    }
                    break;
                case WindowState.Processing:
                    _textRenderer.Update(gameTime);
                    if (_textRenderer.IsComplete)
                    {
                        SetWindowState(WindowState.Waiting);
                    }
                    break;
                case WindowState.Closing:
                    _currentBounds.Size -= (SizeF)_windowAnimateVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _currentBounds.Location = this.Bounds.Centre - _currentBounds.Size.Centre;

                    if (_currentBounds.Size.Width <= 0)
                    {
                        SetWindowState(WindowState.Complete);
                    }
                    break;
            }
        }

        protected override void OnPreDraw(GameTime gameTime)
        {
            base.OnPreDraw(gameTime);

            switch (this.WindowState)
            {
                case WindowState.Processing:
                case WindowState.Waiting:
                    _textTexture = _textRenderer.Render();
                    break;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            if (this.ShowBackground)
            {
                // Draw window bounds.
                _spriteBatch.Draw(_pixelTexture, _currentBounds, this.BorderColour);
                _spriteBatch.Draw(_pixelTexture, GetBorderInteriorRectangle(_currentBounds), this.BackgroundColour);
            }

            // Render text.
            switch (this.WindowState)
            {
                case WindowState.Processing:
                case WindowState.Waiting:
                    if (_textTexture != null)
                    {
                        _spriteBatch.Draw(_textTexture, this.ClientRectangle.Location, Color.White);
                    }
                    break;
            }
        }

        private RectangleF GetBorderInteriorRectangle(RectangleF exteriorBounds)
        {
            exteriorBounds.Inflate(-this.BorderSize.Width, -this.BorderSize.Height);
            return exteriorBounds;
        }

        private RectangleF GetClientRectangle(RectangleF exteriorBounds)
        {
            RectangleF borderInterior = GetBorderInteriorRectangle(exteriorBounds);
            borderInterior.Inflate(-this.Padding.Width, -this.Padding.Height);
            return borderInterior;
        }
    }
}
