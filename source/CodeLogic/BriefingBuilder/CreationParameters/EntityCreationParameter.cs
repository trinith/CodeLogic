using ArbitraryPixel.Common;

namespace BriefingBuilder.CreationParameters
{
    public interface IEntityCreationParameter { }

    public abstract class EntityCreationParameter<TType> : WrapperBase<TType>, IEntityCreationParameter
    {
        public EntityCreationParameter(TType objectToWrap)
            : base(objectToWrap)
        {
        }

        protected abstract string GetCreateString();

        public override string ToString()
        {
            return this.GetCreateString();
        }
    }
}
