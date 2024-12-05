using ArbitraryPixel.Platform2D.Entity;
using BriefingBuilder.CreationParameters;
using System;
using System.Text;
using System.Windows.Forms;

namespace BriefingBuilder.Controls
{
    public interface IEditorControl
    {
        IDesignerHost Host { get; }
        string GetCreationString();
    }

    public abstract class EditorControlBase<TEntity> : Control, IEditorControl where TEntity : IEntity
    {
        private const string CREATE_METHOD_PREFIX = "this.Create_";

        private IDesignerHost _host;
        private Type _wrappedType;

        #region Constructor(s)
        public EditorControlBase()
            : this(EditorDesignerHost.Instance)
        {
        }

        public EditorControlBase(IDesignerHost host)
        {
            SetHost(host);
            _wrappedType = typeof(TEntity);
        }
        #endregion

        #region Private Methods
        private void SetHost(IDesignerHost newHost)
        {
            if (newHost != null && _host != newHost)
            {
                _host = newHost;
                UpdateForNewHost(newHost);
            }
        }
        #endregion

        #region Override Methods
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            this.SetHost(this.Parent as IDesignerHost);
        }
        #endregion

        #region IEditorControl Implementation
        public IDesignerHost Host => _host;

        public string GetCreationString()
        {
            return CREATE_METHOD_PREFIX + _wrappedType.Name.ToString() + this.GetParameterString();
        }

        private string GetParameterString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            IEntityCreationParameter[] parameters = this.CreateParameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                sb.Append(parameters[i].ToString());
            }

            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region Abstract Methods
        protected abstract IEntityCreationParameter[] CreateParameters { get; }
        protected abstract void UpdateForNewHost(IDesignerHost host);
        #endregion
    }
}
