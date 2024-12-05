using ArbitraryPixel.CodeLogic.Common.Config;
using ArbitraryPixel.CodeLogic.Common.Layers;
using ArbitraryPixel.CodeLogic.Common.UI;
using ArbitraryPixel.Common.Drawing;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Engine;
using System;

namespace ArbitraryPixel.CodeLogic.Common.Model
{
    public interface IMainMenuModel
    {
        IMenuContentMap ContentMap { get; }

        IMenuItem RootMenu { get; }

        IMenuItem SinglePlayerMenu { get; }
        IMenuItem BriefingMenu { get; }
        IMenuItem SettingsMenu { get; }
        IMenuItem StatsMenu { get; }
        IMenuItem CreditsMenu { get; }
        IMenuItem ExitMenu { get; }

        void BuildContentMap(IEngine host, ISpriteBatch spriteBatch, RectangleF contentBounds);

        event EventHandler<EventArgs> ExitGame;
        event EventHandler<EventArgs> StartSinglePlayerGame;
        event EventHandler<EventArgs> ResetStatistics;
    }

    public class MainMenuModel : IMainMenuModel
    {
        private IMainMenuContentLayerFactory _contentFactory;

        public IMenuContentMap ContentMap { get; private set; }

        public IMenuItem RootMenu { get; private set; } = null;
        public IMenuItem SinglePlayerMenu { get; private set; } = null;
        public IMenuItem BriefingMenu { get; private set; } = null;
        public IMenuItem SettingsMenu { get; private set; } = null;
        public IMenuItem StatsMenu { get; private set; } = null;
        public IMenuItem CreditsMenu { get; private set; } = null;
        public IMenuItem ExitMenu { get; private set; } = null;

        public event EventHandler<EventArgs> ExitGame;
        public event EventHandler<EventArgs> StartSinglePlayerGame;
        public event EventHandler<EventArgs> ResetStatistics;

        public MainMenuModel(IEngine host, IMenuFactory menuFactory, IMenuContentMap contentMap, IMainMenuContentLayerFactory contentFactory)
        {
            if (host == null || menuFactory == null)
                throw new ArgumentNullException();

            this.ContentMap = contentMap ?? throw new ArgumentNullException();
            _contentFactory = contentFactory ?? throw new ArgumentNullException();

            BuildRootMenu(host, menuFactory);
        }

        private void BuildRootMenu(IEngine host, IMenuFactory menuFactory)
        {
            float height = CodeLogicEngine.Constants.MenuItemHeight;

            this.RootMenu = menuFactory.CreateMenuItem("CodeLogic", height);
            this.SinglePlayerMenu = this.RootMenu.CreateChild("Operation", height);
            this.StatsMenu = this.RootMenu.CreateChild("Statistics", height);
            this.BriefingMenu = this.RootMenu.CreateChild("Briefing", height);
            this.SettingsMenu = this.RootMenu.CreateChild("Settings", height);
            this.CreditsMenu = this.RootMenu.CreateChild("Credits", height);

            if (host.GetComponent<ITargetPlatform>().Platform == Platform.Windows)
            {
                this.ExitMenu = this.RootMenu.CreateChild("Exit", height);
            }
        }

        public void BuildContentMap(IEngine host, ISpriteBatch spriteBatch, RectangleF contentBounds)
        {
            this.ContentMap.Clear();

            IMenuSinglePlayerContentLayer singlePlayerLayer;
            this.ContentMap.RegisterContentLayer(this.SinglePlayerMenu, singlePlayerLayer = _contentFactory.CreateMenuSinglePlayerContentLayer(host, spriteBatch, contentBounds));
            singlePlayerLayer.StartButtonTapped += (sender, e) => OnStartSinglePlayerGame(new EventArgs());

            IMenuSettingsContentLayer settingsLayer;
            this.ContentMap.RegisterContentLayer(this.SettingsMenu, settingsLayer = _contentFactory.CreateMenuSettingsContentLayer(host, spriteBatch, contentBounds));

            IMenuCreditsContentLayer creditsLayer;
            this.ContentMap.RegisterContentLayer(this.CreditsMenu, creditsLayer = _contentFactory.CreateMenuCreditsContentLayer(host, spriteBatch, contentBounds));

            IMenuContentLayer briefingLayer;
            this.ContentMap.RegisterContentLayer(this.BriefingMenu, briefingLayer = _contentFactory.CreateMenuBriefingContentLayer(host, spriteBatch, contentBounds));

            IMenuStatsContentLayer statsLayer;
            this.ContentMap.RegisterContentLayer(this.StatsMenu, statsLayer = _contentFactory.CreateMenuStatsContentLayer(host, spriteBatch, contentBounds, host.GetComponent<IGameStatsData>()));
            statsLayer.ResetButtonTapped += (sender, e) => OnResetStatistics(new EventArgs());

            if (this.ExitMenu != null)
            {
                IMenuExitContentLayer exitLayer;
                this.ContentMap.RegisterContentLayer(this.ExitMenu, exitLayer = _contentFactory.CreateMenuExitContentLayer(host, spriteBatch, contentBounds));
                exitLayer.ExitButtonTapped += (sender, e) => OnExitGame(new EventArgs());
            }
        }

        protected virtual void OnStartSinglePlayerGame(EventArgs e)
        {
            if (this.StartSinglePlayerGame != null)
                this.StartSinglePlayerGame(this, e);
        }

        protected virtual void OnExitGame(EventArgs e)
        {
            if (this.ExitGame != null)
                this.ExitGame(this, e);
        }

        protected virtual void OnResetStatistics(EventArgs e)
        {
            if (this.ResetStatistics != null)
                this.ResetStatistics(this, e);
        }
    }
}
