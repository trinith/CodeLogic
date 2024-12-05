using ArbitraryPixel.CodeLogic.Common;
using ArbitraryPixel.Common.Graphics;
using ArbitraryPixel.Platform2D.Assets;
using ArbitraryPixel.Platform2D.Engine;
using ArbitraryPixel.Platform2D.Text;
using BriefingBuilder.DrawingImplementations;
using System.Drawing;
using System.IO;

namespace BriefingBuilder
{
    public static class DependencyCreator
    {
        public static string ContentPath { get; set; } = @"C:\ArbitraryPixel\Code Logic\source\CodeLogic\CodeLogic_Content";

        static DependencyCreator()
        {
            GameObjectFactory.SetInstance(new GameObjectFactory());
        }

        public static IComponentContainer SetupComponentContainer(IDesignerHost host)
        {
            IComponentContainer container = ComponentContainer.Create();
            IAssetBank assetBank;

            container.RegisterComponent<Graphics>(host.CreateGraphics());
            container.RegisterComponent<IAssetBank>(assetBank = CreateAssetBank());

            container.RegisterComponent<ITextObjectBuilder>(CreateTextObjectBuilder(assetBank));

            return container;
        }

        #region Dependency Creation Methods
        private static ITextObjectBuilder CreateTextObjectBuilder(IAssetBank assetBank)
        {
            ITextObjectBuilder builder = GameObjectFactory.Instance.CreateTextObjectBuilder(
                GameObjectFactory.Instance.CreateTextFormatProcessor(GameObjectFactory.Instance.CreateTextFormatValueHandlerManager()),
                GameObjectFactory.Instance.CreateTextObjectFactory()
            );

            builder.DefaultFont = assetBank.Get<ISpriteFont>("BriefingNormalFont");

            foreach (ISpriteFont font in assetBank.GetAllAssets<ISpriteFont>())
                builder.RegisterFont(((Win32SpriteFont)font).SpriteFontAsset, font);

            return builder;
        }

        private static IAssetBank CreateAssetBank()
        {
            AssetBank assetBank = new AssetBank();

            PopulateTextures(assetBank, DependencyCreator.ContentPath, @"Textures");
            PopulateFonts(assetBank, DependencyCreator.ContentPath, @"Fonts\Briefing");

            return assetBank;
        }
        #endregion

        #region Other Methods
        private static void PopulateTextures(IAssetBank assetBank, string contentPath, string subFolder)
        {
            string folder = Path.Combine(contentPath, subFolder);

            string[] imageFiles = Directory.GetFiles(folder, "*.png", SearchOption.AllDirectories);
            foreach (string imageFile in imageFiles)
            {
                string assetName = imageFile.Substring(contentPath.Length + 1);
                var texture = Win32Texture2D.FromFile(contentPath, assetName);
                assetBank.Put<ITexture2D>(texture.Texture2DAsset, texture);
            }
        }

        private static void PopulateFonts(IAssetBank assetBank, string contentPath, string subFolder)
        {
            string folder = Path.Combine(contentPath, subFolder);

            string[] imageFiles = Directory.GetFiles(folder, "*.spritefont", SearchOption.AllDirectories);
            foreach (string imageFile in imageFiles)
            {
                string assetName = imageFile.Substring(contentPath.Length + 1);
                var font = Win32SpriteFont.FromFile(contentPath, assetName);
                assetBank.Put<ISpriteFont>(font.SpriteFontAsset, font);
            }
        }
        #endregion
    }
}
