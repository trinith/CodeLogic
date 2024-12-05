using ArbitraryPixel.CodeLogic.Common.Model;
using ArbitraryPixel.CodeLogic.Common.Theme;
using ArbitraryPixel.CodeLogic.Common.Theme.Device;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Entity;
using ArbitraryPixel.Platform2D.Theme;
using Microsoft.Xna.Framework;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Entities
{
    public interface ISequenceAttemptRecordView : IGameEntity
    {
    }

    public class SequenceAttemptRecordView : GameEntityBase, ISequenceAttemptRecordView
    {
        private ISpriteBatch _spriteBatch;
        private IDeviceTheme _theme;
        private IDeviceModel _model;
        private ISpriteFont _font;
        private int _targetIndex;
        private SizeF _targetIndexSize;

        public override RectangleF Bounds
        {
            get { return base.Bounds; }
            set
            {
                base.Bounds = new RectangleF(value.Location, base.Bounds.Size);
            }
        }

        public SequenceAttemptRecordView(IEngine host, ISpriteBatch spriteBatch, Vector2 location, IDeviceModel model, int targetIndex)
            : base(host, new RectangleF(location, new SizeF(1, 1)))
        {
            _spriteBatch = spriteBatch ?? throw new ArgumentNullException();
            _model = model ?? throw new ArgumentNullException();
            _targetIndex = targetIndex;

            _theme = this.Host.GetComponent<IThemeManagerCollection>()[ThemeObjectType.Device].GetCurrentTheme<IDeviceTheme>();

            ITexture2D frameTexture = this.Host.AssetBank.Get<ITexture2D>("HistoryAttemptFrame");
            base.Bounds = new RectangleF(this.Bounds.Location, new SizeF(frameTexture.Width, frameTexture.Height));

            _font = this.Host.AssetBank.Get<ISpriteFont>("HistoryAttemptIndexFont");
            _targetIndexSize = _font.MeasureString(_targetIndex.ToString());
        }

        protected override void OnDraw(GameTime gameTime)
        {
            base.OnDraw(gameTime);

            IAssetBank bank = this.Host.AssetBank;

            _spriteBatch.Draw(bank.Get<ITexture2D>("HistoryAttemptFrameBackground"), this.Bounds.Location, _theme.HistoryAttemptBackgroundMask);
            _spriteBatch.Draw(bank.Get<ITexture2D>("HistoryAttemptFrame"), this.Bounds.Location, _theme.HistoryAttemptBorderMask);

            SizeF cellSize = new SizeF(30, 30);
            SizeF cellPadding = new SizeF(1, 1);
            SizeF resultSize = new SizeF(15, 15);

            // Draw Index
            Vector2 textLocation = _theme.HistoryAttemptIndexFontOffset + new Vector2(
                this.Bounds.Left + cellPadding.Width + cellSize.Width / 2f - _targetIndexSize.Width / 2f,
                this.Bounds.Top + cellPadding.Height + cellSize.Height / 2f - _targetIndexSize.Height / 2f
            );
            _spriteBatch.DrawString(_font, (_targetIndex + 1).ToString(), textLocation, _theme.HistoryAttemptBorderMask);

            if (_targetIndex < _model.Attempts.Count)
            {
                ISequenceAttemptRecord record = _model.Attempts[_targetIndex];

                // Draw Attempted Sequence
                Vector2 location = new Vector2(this.Bounds.Left + cellPadding.Width + cellSize.Width + cellPadding.Width, this.Bounds.Top + cellPadding.Height);
                for (int i = 0; i < record.Code.Length; i++)
                {
                    _spriteBatch.Draw(bank.Get<ITexture2D>("HistoryIndexChoice"), location, _model.CodeColourMap.GetColour(record.Code[i]));
                    location.X += cellSize.Width + cellPadding.Width;
                }

                // Draw Result
                Vector2 resultCellStart = new Vector2(this.Bounds.Right - cellPadding.Width - cellSize.Width, this.Bounds.Top + cellPadding.Height);
                Vector2 resultCellOffset = Vector2.Zero;
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        int index = y * 2 + x;
                        SequenceIndexCompareResult indexValue = record.Result[index];
                        if (indexValue != SequenceIndexCompareResult.NotEqual)
                        {
                            ITexture2D resultTexture =
                                (indexValue == SequenceIndexCompareResult.Equal)
                                ? bank.Get<ITexture2D>("HistoryEqual")
                                : bank.Get<ITexture2D>("HistoryPartial");
                            _spriteBatch.Draw(resultTexture, resultCellStart + resultCellOffset, _theme.HistoryAttemptBorderMask);
                        }

                        resultCellOffset.X += resultSize.Width;
                    }

                    resultCellOffset.X = 0;
                    resultCellOffset.Y += resultSize.Height;
                }
            }
        }
    }
}
