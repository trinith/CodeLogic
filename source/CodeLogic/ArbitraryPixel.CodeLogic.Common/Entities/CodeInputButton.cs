using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Common.Input;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Theme;
using ArbitraryPixel.Platform2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public enum CodeInputButtonSelectorMode
    {
        Gesture,
        Select
    }

    public enum CodeInputButtonSelectorState
    {
        Opening,
        Open,
        Closing,
        Closed
    }

    public class SelectorClosedEventArgs : EventArgs
    {
        public bool ValueChanged { get; }

        public SelectorClosedEventArgs(bool valueChanged)
        {
            this.ValueChanged = valueChanged;
        }
    }

    public interface ICodeInputButton : IButton
    {
        event EventHandler<EventArgs> SelectorOpened;
        event EventHandler<SelectorClosedEventArgs> SelectorClosed;

        ICodeInputButtonModel Model { get; }

        void OpenSelector();
        void CloseSelector();
    }

    public class CodeInputButton : ButtonBase, ICodeInputButton
    {
        private const float OPEN_TIME = 0.125f;
        private const float DEADZONE = 50f;

        private ISpriteBatch _spriteBatch;
        private int _index;

        private IDeviceTheme _theme;
        private RectangleF _imageBounds;

        public event EventHandler<EventArgs> SelectorOpened;
        public event EventHandler<SelectorClosedEventArgs> SelectorClosed;
        
        public ICodeInputButtonModel Model { get; private set; }

        public CodeInputButton(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, ICodeInputButtonModel model, int index) 
            : base(host, bounds)
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            this.Model = model ?? throw new ArgumentNullException();
            _index = (index >= 0 && index < this.Model.DeviceModel.InputSequence.Length) ? index : throw new IndexOutOfRangeException("Index is out of range for IDeviceModel.InputSequence.Length");

            _imageBounds = new RectangleF(Vector2.Zero, new SizeF(this.Bounds.Width / 2f, this.Bounds.Height / 2f));
            _imageBounds.Location = this.Bounds.Centre - _imageBounds.Size.Centre;

            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        #region Private Methods
        private bool UpdateModelGestureAngle(Vector2 origin, Vector2 location, float deadzone)
        {
            Vector2 gestureDir = location - origin;

            if (gestureDir.Length() >= deadzone)
            {
                gestureDir.Normalize();
                float angle = (float)Math.Atan2(gestureDir.Y, gestureDir.X) + (float)(Math.PI / 2f);

                if (angle < 0)
                    angle += MathHelper.TwoPi;
                else if (angle >= MathHelper.TwoPi)
                    angle -= MathHelper.TwoPi;

                float inc = MathHelper.ToRadians(360f / 5f);
                int mod = (int)(angle / inc);
                angle = mod * inc + inc / 2f;

                this.Model.GestureAngle = angle;
            }
            else
            {
                this.Model.GestureAngle = null;
            }

            return (this.Model.GestureAngle != null);
        }

        private void UpdateForInput()
        {
            float angleInc = 360f / 5f;
            float angleCentre = angleInc / 2f;
            this.Model.SelectedHighlightAngle = MathHelper.ToRadians((int)this.Model.DeviceModel.InputSequence[_index] * angleInc - angleCentre);

            SurfaceState surfaceState = this.Host.InputManager.GetSurfaceState();
            if (surfaceState.IsTouched == true)
            {
                Vector2 worldSurfaceLocation = this.Host.ScreenManager.PointToWorld(surfaceState.SurfaceLocation);
                if (UpdateModelGestureAngle(this.Bounds.Centre, worldSurfaceLocation, DEADZONE))
                {
                    this.Model.MovedOutOfDeadzone = true;
                }
            }
        }
        #endregion

        #region Protected Methods
        protected virtual void OnSelectorOpened()
        {
            if (this.SelectorOpened != null)
                this.SelectorOpened(this, new EventArgs());
        }

        protected virtual void OnSelectorClosed()
        {
            if (this.SelectorClosed != null)
                this.SelectorClosed(this, new SelectorClosedEventArgs(this.Model.GestureAngle != null));
        }
        #endregion

        #region Override Methods
        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            switch (this.Model.SelectorState)
            {
                case CodeInputButtonSelectorState.Opening:
                    UpdateForInput();
                    this.Model.ScaleValue += (1f / OPEN_TIME) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.Model.ScaleValue >= 1)
                    {
                        this.Model.ScaleValue = 1;
                        this.Model.SelectorState = CodeInputButtonSelectorState.Open;
                        OnSelectorOpened();
                    }
                    break;
                case CodeInputButtonSelectorState.Open:
                    UpdateForInput();
                    break;
                case CodeInputButtonSelectorState.Closing:
                    this.Model.ScaleValue -= (1f / OPEN_TIME) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (this.Model.ScaleValue <= 0)
                    {
                        this.Model.ScaleValue = 0;
                        this.Model.SelectorState = CodeInputButtonSelectorState.Closed;
                        OnSelectorClosed();
                    }
                    break;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IAssetBank bank = this.Host.AssetBank;

            if (this.Model.SelectorState != CodeInputButtonSelectorState.Closed)
            {
                Vector2 centre = this.Bounds.Centre;
                Vector2 origin = centre - this.Bounds.Location;
                Vector2 scale = new Vector2(this.Model.ScaleValue);

                _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonSelectorHighlight"), centre, null, _theme.InputButtonHighlightPreviousSelectionMask, this.Model.SelectedHighlightAngle + (float)Math.PI, origin, scale, SpriteEffects.None, 0f);

                if (this.Model.GestureAngle != null)
                    _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonSelectorHighlight"), centre, null, _theme.InputButtonHighlightCurrentSelectionMask, this.Model.GestureAngle.Value + (float)Math.PI, origin, scale, SpriteEffects.None, 0f);

                _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonSelectorFill"), centre, null, _theme.InputButtonBackgroundMask, 0f, origin, scale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonSelectorColours"), centre, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonSelectorBorder"), centre, null, _theme.InputButtonBorderMask, 0f, origin, scale, SpriteEffects.None, 0f);
            }

            _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonFill"), _imageBounds, _theme.InputButtonBackgroundMask);
            _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonIcon"), _imageBounds, this.Model.DeviceModel.CodeColourMap.GetColour(this.Model.DeviceModel.InputSequence[_index]));
            _spriteBatch.Draw(bank.Get<ITexture2D>("HexButtonBorder"), _imageBounds, _theme.InputButtonBorderMask);
        }

        protected override void OnTouched(ButtonEventArgs e)
        {
            base.OnTouched(e);

            OpenSelector();
        }

        protected override void OnReleased(ButtonEventArgs e)
        {
            base.OnReleased(e);

            bool acceptAndClose = false;

            if (this.Model.SelectorMode == CodeInputButtonSelectorMode.Gesture)
            {
                if (this.Model.GestureAngle != null || this.Model.MovedOutOfDeadzone)
                {
                    acceptAndClose = true;
                }
                else
                {
                    this.Model.SelectorMode = CodeInputButtonSelectorMode.Select;
                }
            }
            else
            {
                acceptAndClose = true;
            }

            if (acceptAndClose)
            {
                CodeValue newValue = this.Model.DeviceModel.InputSequence[_index];

                if (this.Model.GestureAngle != null)
                {
                    float inc = MathHelper.ToRadians(360f / 5f);
                    int newValueInt = (int)(this.Model.GestureAngle.Value / inc) + 1;
                    if (newValueInt >= 5)
                        newValueInt -= 5;
                    this.Model.DeviceModel.InputSequence[_index] = (CodeValue)newValueInt;
                }

                this.Model.MovedOutOfDeadzone = false;
                this.Model.SelectorMode = CodeInputButtonSelectorMode.Gesture;

                CloseSelector();
            }
        }

        protected override bool IsPointInBounds(Vector2 p)
        {
            bool inBounds = false;

            if (this.Model.SelectorState == CodeInputButtonSelectorState.Closed || this.Model.SelectorState == CodeInputButtonSelectorState.Closing)
            {
                inBounds = _imageBounds.Contains(p);
            }
            else
            {
                inBounds = ((new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World)).Contains(p));
            }

            return inBounds;
        }
        #endregion

        #region Public Methods
        public void OpenSelector()
        {
            this.Model.OpenSelector();
        }

        public void CloseSelector()
        {
            this.Model.CloseSelector();
        }
        #endregion
    }
}
