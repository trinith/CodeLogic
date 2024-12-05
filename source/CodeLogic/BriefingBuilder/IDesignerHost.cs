using ArbitraryPixel.Platform2D.Engine;
using BriefingBuilder.Controls;
using System.Drawing;

namespace BriefingBuilder
{
    public interface IDesignerHost
    {
        IComponentContainer Components { get; }
        IEditorControl[] EditorControls { get; }
        string Namespace { get; set; }

        Graphics CreateGraphics();
    }
}
