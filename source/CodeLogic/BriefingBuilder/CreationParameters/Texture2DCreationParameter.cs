using ArbitraryPixel.Common.Graphics;
using BriefingBuilder.DrawingImplementations;

namespace BriefingBuilder.CreationParameters
{
    public class Texture2DCreationParameter : EntityCreationParameter<ITexture2D>
    {
        public Texture2DCreationParameter(ITexture2D objectToWrap)
            : base(objectToWrap)
        {
        }

        protected override string GetCreateString()
        {
            return string.Format("this.Host.AssetBank.Get<ITexture2D>(\"{0}\")", this.GetWrappedObject<Win32Texture2D>().Texture2DAsset);
        }
    }
}
