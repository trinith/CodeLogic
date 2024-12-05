using ArbitraryPixel.CodeLogic.Common.Entities;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface ICodeInputButtonModel
    {
        IDeviceModel DeviceModel { get; }
        float ScaleValue { get; set; }
        float SelectedHighlightAngle { get; set; }
        float? GestureAngle { get; set; }
        CodeInputButtonSelectorState SelectorState { get; set; }
        CodeInputButtonSelectorMode SelectorMode { get; set; }
        bool MovedOutOfDeadzone { get; set; }

        void OpenSelector();
        void CloseSelector();
    }

    public class CodeInputButtonModel : ICodeInputButtonModel
    {
        public IDeviceModel DeviceModel { get; private set; }
        public float ScaleValue { get; set; } = 0f;
        public float SelectedHighlightAngle { get; set; } = 0f;
        public float? GestureAngle { get; set; } = null;
        public CodeInputButtonSelectorState SelectorState { get; set; } = CodeInputButtonSelectorState.Closed;
        public CodeInputButtonSelectorMode SelectorMode { get; set; } = CodeInputButtonSelectorMode.Gesture;
        public bool MovedOutOfDeadzone { get; set; } = false;

        public CodeInputButtonModel(IDeviceModel deviceModel)
        {
            this.DeviceModel = deviceModel ?? throw new ArgumentNullException();
        }

        public void OpenSelector()
        {
            if (this.SelectorState != CodeInputButtonSelectorState.Open && this.SelectorState != CodeInputButtonSelectorState.Opening)
            {
                this.SelectorState = CodeInputButtonSelectorState.Opening;
            }
        }

        public void CloseSelector()
        {
            this.SelectorState = CodeInputButtonSelectorState.Closing;
        }
    }
}
