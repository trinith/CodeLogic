using ArbitraryPixel.CodeLogic.Common.BriefingContent;
using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Credits;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.CodeLogic.BriefingContent;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Layer;
using ArbitraryPixel.Platform2D.Text;
using ArbitraryPixel.Platform2D.UI.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IMainMenuContentLayerFactory
    {
        IMenuSinglePlayerContentLayer CreateMenuSinglePlayerContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds);
        IMenuExitContentLayer CreateMenuExitContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds);
        IMenuSettingsContentLayer CreateMenuSettingsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds);
        IMenuCreditsContentLayer CreateMenuCreditsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds);
        IMenuContentLayer CreateMenuBriefingContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds);
        IMenuStatsContentLayer CreateMenuStatsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IGameStatsData gameStatsData);
    }

    public class MainMenuContentLayerFactory : IMainMenuContentLayerFactory
    {
        public IMenuExitContentLayer CreateMenuExitContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
        {
            return new MenuExitContentLayer(host, mainSpriteBatch, contentBounds);
        }

        public IMenuSinglePlayerContentLayer CreateMenuSinglePlayerContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
        {
            return new MenuSinglePlayerContentLayer(host, mainSpriteBatch, contentBounds);
        }

        public IMenuSettingsContentLayer CreateMenuSettingsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
        {
            return new MenuSettingsContentLayer(host, mainSpriteBatch, contentBounds, new AudioControlsFactory(host, contentBounds));
        }

        public IMenuStatsContentLayer CreateMenuStatsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds, IGameStatsData gameStatsData)
        {
            return new MenuStatsContentLayer(host, mainSpriteBatch, contentBounds, gameStatsData);
        }

        public IMenuCreditsContentLayer CreateMenuCreditsContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
        {
            return new MenuCreditsContentLayer(host, mainSpriteBatch, contentBounds, CreditsContent.Credits);
        }

        public IMenuContentLayer CreateMenuBriefingContentLayer(IEngine host, ISpriteBatch mainSpriteBatch, RectangleF contentBounds)
        {
            ITextObjectBuilder builder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );
            builder.RegisterFont("BriefingNormalFont", host.AssetBank.Get<ISpriteFont>("BriefingNormalFont"));
            builder.RegisterFont("BriefingTitleFont", host.AssetBank.Get<ISpriteFont>("BriefingTitleFont"));
            builder.DefaultFont = builder.GetRegisteredFont("BriefingNormalFont");

            ITextObjectRendererFactory textObjectRendererFactory = GameObjectFactory.Instance.CreateTextObjectRendererFactory();
            IUIObjectFactory uiObjectFactory = GameObjectFactory.Instance.CreateUIObjectFactory();

            RectangleF pageBounds = contentBounds;
            pageBounds.Inflate(-CodeLogicEngine.Constants.TextWindowPadding);

            // Add Pages
            Dictionary<GameObjectFactory.BriefingPage, ILayer> contentPages = new Dictionary<GameObjectFactory.BriefingPage, ILayer>();
            foreach (GameObjectFactory.BriefingPage pageType in Enum.GetValues(typeof(GameObjectFactory.BriefingPage)))
            {
                contentPages.Add(
                    pageType,
                    GameObjectFactory.Instance.CreateBriefingPage(
                        pageType,
                        host,
                        mainSpriteBatch,
                        pageBounds,
                        builder,
                        textObjectRendererFactory,
                        uiObjectFactory
                    )
                );
            }

            // Create the content layer.
            IMenuBriefingContentLayer briefingContentLayer = new MenuBriefingContentLayer(host, mainSpriteBatch, contentBounds, contentPages.Values.ToArray());

            // Hook the contents page to the contents layer.
            IBriefingContentsPage briefintContentsPage = (IBriefingContentsPage)contentPages[GameObjectFactory.BriefingPage.Contents];
            briefintContentsPage.BriefingContentLayer = briefingContentLayer;

            return briefingContentLayer;
        }
    }
}
