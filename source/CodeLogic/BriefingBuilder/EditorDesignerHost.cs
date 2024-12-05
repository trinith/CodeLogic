using ArbitraryPixel.Platform2D.Engine;
using BriefingBuilder.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BriefingBuilder
{
    /// <summary>
    /// An implementation of IDesignerHost to provide to objects when using the editor.
    /// </summary>
    public class EditorDesignerHost : IDesignerHost
    {
        #region Singleton Implementation
        private static EditorDesignerHost _instance = null;
        public static IDesignerHost Instance
        {
            get
            {
                bool isDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

                if (_instance == null && isDesignMode)
                    _instance = new EditorDesignerHost();

                return _instance;
            }
        }
        #endregion

        private Form _dummyForm;

        public EditorDesignerHost()
        {
            _dummyForm = new Form();
            this.Components = DependencyCreator.SetupComponentContainer(this);
        }

        #region IDesignerHost Implementation
        public IComponentContainer Components { get; private set; }

        public IEditorControl[] EditorControls => null;

        public string Namespace { get; set; } = "";

        public Graphics CreateGraphics()
        {
            return _dummyForm.CreateGraphics();
        }
        #endregion
    }
}
