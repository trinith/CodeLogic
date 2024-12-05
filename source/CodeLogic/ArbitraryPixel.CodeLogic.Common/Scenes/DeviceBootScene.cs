using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.ContentManagement;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Scene;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

namespace ArbitraryPixel.CodeLogic.Common.Scenes
{
    public class DeviceBootScene : SceneBase
    {
        private IDeviceModel _deviceModel;
        private ILogPanelModel _logPanelModel;
        private IDeviceTheme _theme;
        private ILayer _mainLayer;

        public DeviceBootScene(IEngine host, IDeviceModel deviceModel, ILogPanelModel logPanelModel)
            : base(host)
        {
            _deviceModel = deviceModel ?? throw new ArgumentNullException();
            _logPanelModel = logPanelModel ?? throw new ArgumentNullException();

            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();
        }

        protected override void OnLoadAssetBank(IContentManager content, IAssetBank bank)
        {
            base.OnLoadAssetBank(content, bank);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            SetupMainLayer();
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            this.Host.AudioManager.MusicController.VolumeAttenuation = 1f;
            this.Host.AudioManager.MusicController.Play(this.Host.AssetBank.Get<ISong>("Gameplay"));
        }

        protected override void OnReset()
        {
            base.OnReset();
            this.SceneComplete = false;

            _deviceModel.Reset();
            _logPanelModel.Reset();

            this.ClearEntities();
            SetupMainLayer();
            SetupConsoleWindow();
        }

        private void SetupMainLayer()
        {
            ISpriteBatch layerSpriteBatch = this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice);

            _mainLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, layerSpriteBatch, SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            IDeviceBackground background = GameObjectFactory.Instance.CreateDeviceBackground(this.Host, new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World), _mainLayer.MainSpriteBatch);
            background.Colour = _theme.BackgroundImageMask;
            _mainLayer.AddEntity(background);

            this.AddEntity(_mainLayer);
        }

        private void SetupConsoleWindow()
        {
            IConsoleWindow consoleWindow = CreateWindow(_mainLayer);

            _mainLayer.AddEntity(consoleWindow);

            consoleWindow.SetTextFormat(CreateWindowText());
        }

        private IConsoleWindow CreateWindow(ILayer hostLayer)
        {
            SizeF borderSize = CodeLogicEngine.Constants.TextWindowBorderSize;
            SizeF padding = CodeLogicEngine.Constants.TextWindowPadding;

            SizeF windowSize = (SizeF)this.Host.ScreenManager.World * 0.9f;
            RectangleF windowBounds = new RectangleF(
                ((SizeF)this.Host.ScreenManager.World).Centre - windowSize.Centre,
                windowSize
            );

            RectangleF textBounds = windowBounds;
            textBounds.Inflate(-borderSize.Width - padding.Width, -borderSize.Height - padding.Height);

            IConsoleWindow newWindow = GameObjectFactory.Instance.CreateConsoleWindow(
                this.Host,
                windowBounds,
                hostLayer.MainSpriteBatch,
                GameObjectFactory.Instance.CreateTextObjectBuilderWithConsoleFonts(
                    GameObjectFactory.Instance.CreateTextFormatProcessor(
                        GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()
                    ),
                    GameObjectFactory.Instance.CreateTextObjectFactory(),
                    this.Host.AssetBank
                ),
                GameObjectFactory.Instance.CreateTextObjectRenderer(
                    this.Host.GrfxFactory.RenderTargetFactory,
                    this.Host.Graphics.GraphicsDevice,
                    this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice),
                    (Rectangle)textBounds
                )
            );
            newWindow.BorderSize = borderSize;
            newWindow.Padding = padding;

            newWindow.Disposed +=
                (sender, e) =>
                {
                    this.ChangeScene(this.Host.Scenes["DeviceMain"]);
                };

            return newWindow;
        }

        private string CreateWindowText()
        {
            StringBuilder windowText = new StringBuilder();

            string deviceAddress = "10.0.0.43:8080";

            string cMain = "{Color:White}";
            string cCommand = "{Color:192,192,192}";
            string cResponse = "{Color:96,96,96}";
            string cResponseParam = "{Color:64,64,255}";
            string cSuccess = "{Color:0,255,0}";
            string cFail = "{Color:Red}";

            string tpcZero = "{TPC:0}";
            string tpcCommand = "{TPC:0.01}";
            string tpcProcessing = "{TPC:0.75}";
            string tpcDelay = "{TPC:0.75}";

            string pauseToZero = $"{tpcDelay} {tpcZero}";

            Func<string> InsertSuccess = () => $"{cMain}[{cSuccess}Success{cMain}]{cResponse}";
            Func<string> InsertFail = () => $"{cMain}[{cFail}Fail{cMain}]{cResponse}";
            Func<string, string> InsertParam = (paramName) => $"{cResponseParam}{paramName}{cResponse}";

            windowText.AppendLine("{Font:Heading}{Alignment:Centre}boot.run");
            windowText.Append("{Font:Normal}{Alignment:Left}");
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}discoverDevice deviceID:0xaf38{pauseToZero}");
            windowText.AppendLine($"  {cResponse}Device with ID {InsertParam("0xaf38")} discovered at: {InsertParam(deviceAddress)}.{pauseToZero}");
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}bindAlias alias:@device value:{deviceAddress}{pauseToZero}");
            windowText.AppendLine($"  {cResponse}Successfully bound {InsertParam(deviceAddress)} to alias {InsertParam("@device")}.{pauseToZero}");
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}connectToDevice address:@device{pauseToZero}");
            windowText.AppendLine($"  {cResponse}Connected to device at address {InsertParam(deviceAddress)}.{pauseToZero}");
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}decryptCode address:@device{pauseToZero}");
            windowText.AppendLine($"  {cResponse}Attempting to decrypt device security code");
            windowText.AppendLine();
            windowText.AppendLine($"  - Preallocating{tpcProcessing}.. {tpcZero}                      {InsertSuccess()}");
            windowText.AppendLine($"  - Establishing auto-crack injection{tpcProcessing}.... {tpcZero}{InsertSuccess()}");
            windowText.AppendLine($"  - Launching auto-crack process{tpcProcessing}... {tpcZero}      {InsertSuccess()}");
            windowText.AppendLine($"  - Awaiting sequence response{tpcProcessing}...... {tpcZero}     {InsertFail()}");
            windowText.AppendLine();
            windowText.AppendLine($"  {cFail}!! {cResponse}Automatic device code decryption has failed. Recommend engaging in manual attempt. {cFail}!!{pauseToZero}");
            windowText.AppendLine();
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}launchCodeEntryInterface address:@device{pauseToZero}");
            windowText.AppendLine($"  {cResponse}Interface connected to {InsertParam(deviceAddress)}. Tap screen to launch...");

            return windowText.ToString();
        }
    }
}
