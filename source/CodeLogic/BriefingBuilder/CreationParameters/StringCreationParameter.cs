namespace BriefingBuilder.CreationParameters
{
    public class StringCreationParameter : EntityCreationParameter<string>
    {
        public StringCreationParameter(string objectToWrap)
            : base(objectToWrap)
        {
        }

        protected override string GetCreateString()
        {
            return string.Format("\"{0}\"", this.WrappedObject); ;
        }
    }
}
