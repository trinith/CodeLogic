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
    public interface IMissionDebriefAttemptRecordMarksBuilder
    {
        ITextureEntity[] CreateMarksForAttemptRecord(ISequenceAttemptRecord record, ICodeValueColourMap colourMap, Vector2 anchor, ISpriteBatch spriteBatch);
        ITextureEntity[] CreateEqualityMarksForAttemptRecord(ISequenceAttemptRecord record, Vector2 anchor, ISpriteBatch spriteBatch);
    }

    public class MissionDebriefAttemptRecordMarksBuilder : IMissionDebriefAttemptRecordMarksBuilder
    {
        private IEngine _host;
        private IRandom _random;
        private ITextureEntityFactory _factory;
        private ITexture2D _codeMarksTexture;
        private ITexture2D _equalityMarksTexture;

        public MissionDebriefAttemptRecordMarksBuilder(IEngine host, IRandom random, ITextureEntityFactory factory)
        {
            _host = host ?? throw new ArgumentNullException();
            _random = random ?? throw new ArgumentNullException();
            _factory = factory ?? throw new ArgumentNullException();

            _codeMarksTexture = _host.AssetBank.Get<ITexture2D>("MissionDebriefMarks");
            _equalityMarksTexture = _host.AssetBank.Get<ITexture2D>("MissionDebriefEqualityMarks");
        }

        public ITextureEntity[] CreateMarksForAttemptRecord(ISequenceAttemptRecord record, ICodeValueColourMap colourMap, Vector2 anchor, ISpriteBatch spriteBatch)
        {
            List<ITextureEntity> entities = new List<ITextureEntity>();
            SizeF codeMarkAreaSize = new SizeF(_codeMarksTexture.Width / 4, _codeMarksTexture.Height);

            for (int i = 0; i < record.Code.Length; i++)
            {
                entities.Add(
                    _factory.Create(
                        _host,
                        new RectangleF(anchor + new Vector2(i * codeMarkAreaSize.Width, 0), codeMarkAreaSize),
                        spriteBatch,
                        _codeMarksTexture,
                        colourMap.GetColour(record.Code[i]),
                        new Rectangle(_random.Next(0, 4) * (int)codeMarkAreaSize.Width, 0, (int)codeMarkAreaSize.Width, (int)codeMarkAreaSize.Height)
                    )
                );
            }

            return entities.ToArray();
        }

        public ITextureEntity[] CreateEqualityMarksForAttemptRecord(ISequenceAttemptRecord record, Vector2 anchor, ISpriteBatch spriteBatch)
        {
            List<ITextureEntity> entities = new List<ITextureEntity>();
            SizeF equalityMarkSize = new SizeF(_equalityMarksTexture.Width / 2, _equalityMarksTexture.Height);

            for (int y = 0; y < record.Result.Length / 2; y++)
            {
                for (int x = 0; x < record.Result.Length / 2; x++)
                {
                    int i = 2 * y + x;
                    int resultInt = (int)record.Result[i];

                    if (resultInt > 0)
                    {
                        entities.Add(
                            _factory.Create(
                                _host,
                                new RectangleF(anchor + new Vector2(x * (equalityMarkSize.Width + 1), y * (equalityMarkSize.Height + 1)), equalityMarkSize),
                                spriteBatch,
                                _equalityMarksTexture,
                                Color.White,
                                new Rectangle((resultInt - 1) * (int)equalityMarkSize.Width, 0, (int)equalityMarkSize.Width, (int)equalityMarkSize.Height)
                            )
                        );
                    }
                }
            }

            return entities.ToArray();
        }
    }
}
