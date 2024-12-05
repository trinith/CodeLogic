using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface IStatusIndicator : IGameEntity
    {
        IDeviceTheme Theme { get; }
        ISpriteBatch SpriteBatch { get; }
        IDeviceModel Model { get; }
        Color CurrentAlarmMask { get; }
    }

    public class StatusIndicator : GameEntityBase, IStatusIndicator
    {
        private double _blinkAccum = 0.0;
        private bool _drawAlarmColour = true;
        private AlarmLevel _previousAlarmLevel;

        public IDeviceTheme Theme { get; private set; }
        public ISpriteBatch SpriteBatch { get; private set; }
        public IDeviceModel Model { get; private set; }
        public Color CurrentAlarmMask
        {
            get { return GetAlarmColour(this.Model.AlarmLevel); }
        }

        public StatusIndicator(IEngine host, RectangleF bounds, ISpriteBatch spriteBatch, IDeviceModel model)
            : base(host, bounds)
        {
            SpriteBatch = spriteBatch ?? throw new ArgumentNullException();
            Model = model ?? throw new ArgumentNullException();

            Theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (Model.AlarmLevel == AlarmLevel.Critical)
            {
                if (_previousAlarmLevel != AlarmLevel.Critical)
                {
                    _blinkAccum = 0;
                    _drawAlarmColour = true;
                }
                else
                {
                    _blinkAccum += gameTime.ElapsedGameTime.TotalSeconds;
                    if (_blinkAccum >= Theme.AlarmCriticalBlinkFrequency)
                    {
                        _blinkAccum -= Theme.AlarmCriticalBlinkFrequency;
                        _drawAlarmColour = !_drawAlarmColour;
                    }
                }
            }
            else
            {
                _drawAlarmColour = true;
            }

            _previousAlarmLevel = Model.AlarmLevel;
        }

        private Color GetAlarmColour(AlarmLevel level)
        {
            Color alarmColour = Color.Magenta;

            switch (level)
            {
                case AlarmLevel.Low:
                    alarmColour = Theme.AlarmLowMask;
                    break;
                case AlarmLevel.Medium:
                    alarmColour = Theme.AlarmMediumMask;
                    break;
                case AlarmLevel.High:
                case AlarmLevel.Critical:
                    alarmColour = (_drawAlarmColour) ? Theme.AlarmHighMask : Theme.StatusIndicatorProgressBackgroundMask;
                    break;
            }

            return alarmColour;
        }
    }
}
