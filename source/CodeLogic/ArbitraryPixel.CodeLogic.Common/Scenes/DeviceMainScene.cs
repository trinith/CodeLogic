using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Audio;
using ArbitraryPixel.Common.Drawing;
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
    public class DeviceMainScene : SceneBase
    {
        private IDeviceModel _deviceModel = null;
        private ILogPanelModel _logPanelModel = null;

        private ILayer _windowLayer = null;
        private bool _gameOver = false;

        public DeviceMainScene(IEngine host, IDeviceModel deviceModel, ILogPanelModel logPanelModel)
            : base(host)
        {
            _deviceModel = deviceModel ?? throw new ArgumentNullException();
            _logPanelModel = logPanelModel ?? throw new ArgumentNullException();
            this.Host.ExternalActionOccurred += Handle_ExternalActionOccurred;
        }

        #region Override Methods
        protected override void OnStarting()
        {
            if (_deviceModel.Stopwatch.IsPaused)
                _deviceModel.Stopwatch.Start();
        }

        protected override void OnReset()
        {
            base.OnReset();

            // If we switch back to this scene, we are no longer complete.
            this.SceneComplete = false;
            _gameOver = false;
        }

        protected override void OnEnding()
        {
            base.OnEnding();

            if (_gameOver)
            {
                this.Host.AudioManager.MusicController.FadeVolumeAttenuation(0, CodeLogicEngine.Constants.FadeSceneTransitionTime);
                _deviceModel.Stopwatch.Stop();
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            IDeviceBackground background;
            ILayer bgLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            bgLayer.AddEntity(background = GameObjectFactory.Instance.CreateDeviceBackground(this.Host, new RectangleF(Vector2.Zero, (SizeF)this.Host.ScreenManager.World), bgLayer.MainSpriteBatch));
            background.Colour = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>().BackgroundImageMask;
            this.AddEntity(bgLayer);

            IDeviceMainUILayer uiLayer = GameObjectFactory.Instance.CreateDeviceMainUILayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), _deviceModel, _logPanelModel);
            uiLayer.SubmitSequence += Handle_SequenceSubmit;
            uiLayer.MenuButtonTapped += Handle_MenuButtonTapped;
            this.AddEntity(uiLayer);

            ICodeInputLayer inputLayer = GameObjectFactory.Instance.CreateCodeInputLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), _deviceModel);
            this.AddEntity(inputLayer);

            _windowLayer = GameObjectFactory.Instance.CreateGenericLayer(this.Host, this.Host.GrfxFactory.SpriteBatchFactory.Create(this.Host.Graphics.GraphicsDevice), SpriteSortMode.Deferred);
            this.AddEntity(_windowLayer);
        }
        #endregion

        #region Event Handlers
        private void Handle_SequenceSubmit(object sender, EventArgs e)
        {
            ISequenceComparer comparer = GameObjectFactory.Instance.CreateSequenceComparer();
            ISequenceCompareResult result = comparer.Compare(_deviceModel.InputSequence, _deviceModel.TargetSequence);

            _deviceModel.Attempts.Add(GameObjectFactory.Instance.CreateSequenceAttemptRecord((CodeValue[])_deviceModel.InputSequence.Code.Clone(), (SequenceIndexCompareResult[])result.Result.Clone()));

            // Add a text window to the UI layer
            SizeF borderSize = CodeLogicEngine.Constants.TextWindowBorderSize;
            SizeF padding = CodeLogicEngine.Constants.TextWindowPadding;

            SizeF windowSize = (SizeF)this.Host.ScreenManager.World * 0.9f;
            RectangleF windowBounds = new RectangleF(
                new Vector2(this.Host.ScreenManager.World.X / 2f - windowSize.Width / 2f, this.Host.ScreenManager.World.Y / 2f - windowSize.Height / 2f),
                windowSize
            );

            RectangleF textBounds = windowBounds;
            textBounds.Inflate(-borderSize.Width - padding.Width, -borderSize.Height - padding.Height);

            IConsoleWindow consoleWindow = GameObjectFactory.Instance.CreateConsoleWindow(
                this.Host,
                windowBounds,
                _windowLayer.MainSpriteBatch,
                GameObjectFactory.Instance.CreateTextObjectBuilderWithConsoleFonts(
                    GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
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
            consoleWindow.BorderSize = borderSize;
            consoleWindow.Padding = padding;

            _windowLayer.AddEntity(consoleWindow);

            consoleWindow.SetTextFormat(CreateWindowText(result));

            consoleWindow.Disposed +=
                (x, y) =>
                {
                    _deviceModel.CurrentTrial++;

                    if (result.IsEqual || _deviceModel.CurrentTrial > CodeLogicEngine.Constants.MaximumTrials)
                    {
                        _deviceModel.GameWon = result.IsEqual;
                        _gameOver = true;
                        this.ChangeScene(GameObjectFactory.Instance.CreateFadeSceneTransition(this.Host, this, this.Host.Scenes["MissionDebriefing"], FadeSceneTransitionMode.OutIn, CodeLogicEngine.Constants.FadeSceneTransitionTime));
                    }
                };
        }

        private void Handle_MenuButtonTapped(object sender, EventArgs e)
        {
            this.Host.AssetBank.Get<ISoundResource>("ButtonPress").Play();
            ChangeToDeviceMenuScene();
        }

        private void Handle_ExternalActionOccurred(object sender, ExternalActionEventArgs e)
        {
            if (e.Data != null && e.Data.Equals(CodeLogicEngine.Constants.ExternalActions.BackPressed) && this.Host.CurrentScene == this)
            {
                ChangeToDeviceMenuScene();
            }
        }
        #endregion

        #region Private Methods
        private string CreateWindowText(ISequenceCompareResult result)
        {
            StringBuilder windowText = new StringBuilder();

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

            Func<string, string> InsertParam = (paramName) => $"{cResponseParam}{paramName}{cResponse}";
            Func<int, string> InsertReportLine =
                (index) =>
                {
                    StringBuilder s = new StringBuilder();
                    s.Append($"{cResponse}Line {index.ToString()}..... ");
                    s.Append($"{cMain}[");
                    switch (result.Result[index])
                    {
                        case SequenceIndexCompareResult.Equal:
                            s.Append("{Color:Green}Equal");
                            break;
                        case SequenceIndexCompareResult.PartialEqual:
                            s.Append("{Color:Orange}Partial");
                            break;
                        case SequenceIndexCompareResult.NotEqual:
                            s.Append("{Color:Red}Not Equal");
                            break;
                    }
                    s.Append($"{cMain}]");

                    return s.ToString();
                };

            Func<string> InsertResult = () =>
                ""
                + $"{cMain}["
                + ((result.IsEqual) ? cSuccess : cFail)
                + "" + ((result.IsEqual) ? "Accepted" : "Rejected")
                + $"{cMain}]"
                + "";

            windowText.AppendLine("{Font:Heading}{Alignment:Centre}submit.run");
            windowText.Append("{Font:Normal}{Alignment:Left}");

            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}bindAlias alias:@sequence value:{_deviceModel.InputSequence.ToString().Replace("{", "\\{").Replace("}", "\\}")}{pauseToZero}");
            windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}analyzeSequence @sequence /report:report.text{pauseToZero}");
            windowText.Append($"  {cResponse}Testing sequence: \\{{");
            for (int i = 0; i < _deviceModel.InputSequence.Length; i++)
            {
                if (i != 0)
                    windowText.Append($"{cResponse}, ");
                windowText.Append($"{InsertParam(_deviceModel.InputSequence[i].ToString())}");
            }
            windowText.AppendLine($"\\}} => Processing{tpcProcessing}.....{tpcZero} {InsertResult()}");

            if (result.IsEqual == false)
            {
                windowText.AppendLine($"  {cResponse}Sequence rejected. Analysis written to {InsertParam("report.text")}{pauseToZero}");
                windowText.AppendLine($"{cMain}> {tpcCommand}{cCommand}outputReport report.text{pauseToZero}");
                windowText.AppendLine($"  {cResponse}Printing report, {InsertParam("report.text")}.");
                windowText.AppendLine($"    - NOTE: Results in no particular order.");
                for (int i = 0; i < result.Result.Length; i++)
                {
                    windowText.AppendLine($"  {InsertReportLine(i)}");
                }
                windowText.AppendLine($"{pauseToZero}");

                if (_deviceModel.CurrentTrial == CodeLogicEngine.Constants.MaximumTrials)
                {
                    windowText.AppendLine($"  {tpcCommand}{cFail}CRITICAL UPDATE: {cResponse}Maximum attempts reached, alarm has been tripped. Aborting mission!");
                }
            }
            else
            {
                windowText.AppendLine();
                windowText.AppendLine($"  {cResponse}{tpcCommand}Sequence has been correctly identified. Mission successful, great work, agent!");
            }

            windowText.AppendLine($"{pauseToZero}");

            return windowText.ToString();
        }

        private void ChangeToDeviceMenuScene()
        {
            this.ChangeScene(GameObjectFactory.Instance.CreatePanSceneTransition(this.Host, this, this.Host.Scenes["DeviceMenu"], PanSceneTransitionMode.PanRight, CodeLogicEngine.Constants.DeviceSceneTransitionTime));
        }
        #endregion
    }
}