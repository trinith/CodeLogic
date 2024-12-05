using ArbitraryPixel.Platform2D.Engine;
using BriefingBuilder.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BriefingBuilder
{
    [ToolboxItem(false)]
    public class DesignerHostBase : UserControl, IDesignerHost
    {
        public IComponentContainer Components { get; private set; }

        public IEditorControl[] EditorControls => this.Controls.OfType<IEditorControl>().ToArray();

        public string Namespace { get; set; }

        public DesignerHostBase()
        {
            this.DoubleBuffered = true;
            this.Components = DependencyCreator.SetupComponentContainer(this);

            this.BackColor = Color.Black;
        }
    }
}
