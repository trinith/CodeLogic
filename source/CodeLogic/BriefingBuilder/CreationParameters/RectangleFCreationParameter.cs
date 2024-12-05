using System.Drawing;

namespace BriefingBuilder.CreationParameters
{
    public class RectangleFCreationParameter : EntityCreationParameter<RectangleF>
    {
        public RectangleFCreationParameter(RectangleF objectToWrap)
            : base(objectToWrap)
        {
        }

        protected override string GetCreateString()
        {
            return string.Format(
                "new RectangleF({0}, {1}, {2}, {3})",
                this.WrappedObject.X,
                this.WrappedObject.Y,
                this.WrappedObject.Width,
                this.WrappedObject.Height
            );
        }
    }
}
