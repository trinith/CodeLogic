using ArbitraryPixel.CodeLogic.Common.Entities;
using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IMissionDebriefHistoryMarksBuilder
    {
        IMissionDebriefAttemptRecordMarksBuilder AttemptRecordBuilder { get; }

        ITextureEntity[] CreateAttemptHistoryMarks(IDeviceModel model, Vector2 anchor, ISpriteBatch spriteBatch);
        ITextureEntity[] CreateFinalCodeMarks(IDeviceModel model, Vector2 anchor, ISpriteBatch spriteBatch);
    }

    public class MissionDebriefHistoryMarksBuilder : IMissionDebriefHistoryMarksBuilder
    {
        private IEngine _host;
        private IRandom _random;
        private ITextureEntityFactory _factory;
        private IMissionDebriefAttemptRecordMarksBuilder _attemptRecordBuilder;

        private ITexture2D _codeMarksTexture;

        public IMissionDebriefAttemptRecordMarksBuilder AttemptRecordBuilder { get { return _attemptRecordBuilder; } }

        public MissionDebriefHistoryMarksBuilder(IEngine host, IRandom random, ITextureEntityFactory factory, IMissionDebriefAttemptRecordMarksBuilder attemptRecordBuilder)
        {
            _host = host ?? throw new ArgumentNullException();
            _random = random ?? throw new ArgumentNullException();
            _factory = factory ?? throw new ArgumentNullException();
            _attemptRecordBuilder = attemptRecordBuilder ?? throw new ArgumentNullException();

            _codeMarksTexture = _host.AssetBank.Get<ITexture2D>("MissionDebriefMarks");
        }

        public ITextureEntity[] CreateAttemptHistoryMarks(IDeviceModel model, Vector2 anchor, ISpriteBatch spriteBatch)
        {
            List<ITextureEntity> entities = new List<ITextureEntity>();

            Vector2 codeMarksPos = anchor;
            SizeF codeMarkAreaSize = new SizeF(_codeMarksTexture.Width / 4, _codeMarksTexture.Height);

            int num = 0;
            foreach (ISequenceAttemptRecord record in model.Attempts)
            {
                foreach (ITextureEntity recordMarkEntity in _attemptRecordBuilder.CreateMarksForAttemptRecord(record, model.CodeColourMap, codeMarksPos, spriteBatch))
                    entities.Add(recordMarkEntity);

                Vector2 equalityMarksAnchor = new Vector2(codeMarksPos.X + codeMarkAreaSize.Width * 4 + 1, codeMarksPos.Y);
                foreach (ITextureEntity recordEqualityMarkEntity in _attemptRecordBuilder.CreateEqualityMarksForAttemptRecord(record, equalityMarksAnchor, spriteBatch))
                    entities.Add(recordEqualityMarkEntity);

                codeMarksPos.Y += codeMarkAreaSize.Height + 1;
                if (num == 4)
                {
                    codeMarksPos.Y = anchor.Y;
                    codeMarksPos.X += codeMarkAreaSize.Width * 5 + 1 + 25 + 1 + 1;
                }

                num++;
            }

            return entities.ToArray();
        }

        public ITextureEntity[] CreateFinalCodeMarks(IDeviceModel model, Vector2 anchor, ISpriteBatch spriteBatch)
        {
            List<ITextureEntity> entities = new List<ITextureEntity>();

            Vector2 codeMarksAnchor = anchor;
            Vector2 codeMarksPos = codeMarksAnchor;
            SizeF codeMarkAreaSize = new SizeF(_codeMarksTexture.Width / 4, _codeMarksTexture.Height);

            for (int i = 0; i < model.TargetSequence.Length; i++)
            {
                entities.Add(
                    _factory.Create(
                        _host,
                        new RectangleF(codeMarksPos + new Vector2(i * codeMarkAreaSize.Width, 0), codeMarkAreaSize),
                        spriteBatch,
                        _codeMarksTexture,
                        model.CodeColourMap.GetColour(model.TargetSequence.Code[i]),
                        new Rectangle(_random.Next(0, 4) * (int)codeMarkAreaSize.Width, 0, (int)codeMarkAreaSize.Width, (int)codeMarkAreaSize.Height)
                    )
                );
            }

            return entities.ToArray();
        }
    }
}
