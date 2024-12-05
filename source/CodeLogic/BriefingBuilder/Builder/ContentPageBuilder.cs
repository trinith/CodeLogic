using BriefingBuilder.Controls;
using System.IO;
using System.Text;

namespace BriefingBuilder.Builder
{
    public class ContentPageBuilder
    {
        private const string TEMPLATE_LOCATION = @"Template\ContentPageTemplate.cs";
        private const string TEMPLATE_NAME = "$TEMPLATE_NAME";
        private const string TEMPLATE_CLASS_NAME = "$TEMPLATE_CLASS_NAME";
        private const string TEMPLATE_CONTENT_CODE = "$TEMPLATE_CONTENT_CODE";
        private const string TEMPLATE_NAMESPACE = "$TEMPLATE_NAMESPACE";

        private const string DEFAULT_CLASS_NAME = "TEMPLATE_ContentPageTemplate";
        private const string DEFAULT_NAMESPACE = "GeneratedContent.Content";

        private string[] _baseTemplateLines;

        public ContentPageBuilder()
        {
            _baseTemplateLines = File.ReadAllLines(TEMPLATE_LOCATION);
        }

        public void GenerateContentPage(IDesignerHost contentClass, string outputDirectory)
        {
            StringBuilder outputContents = new StringBuilder();
            string templateClassName = contentClass.GetType().Name;
            string fileNamespace = (string.IsNullOrEmpty(contentClass.Namespace)) ? DEFAULT_NAMESPACE : contentClass.Namespace;

            for (int i = 0; i < _baseTemplateLines.Length; i++)
            {
                string line = _baseTemplateLines[i];
                string formattedLine = "";

                switch (line.Trim().TrimStart(new char[] { '/' }).Trim())
                {
                    case TEMPLATE_NAME:
                        formattedLine = line.Replace(TEMPLATE_NAME, templateClassName + ".cs");
                        break;
                    case TEMPLATE_NAMESPACE:
                        formattedLine = "namespace " + fileNamespace;
                        i += 1;
                        break;
                    case TEMPLATE_CLASS_NAME:
                        formattedLine = _baseTemplateLines[i + 1].Replace(DEFAULT_CLASS_NAME, templateClassName);
                        i += 1;
                        break;
                    case TEMPLATE_CONTENT_CODE:
                        formattedLine = GetEntityCreationCode(line, contentClass);
                        break;
                    default:
                        formattedLine = line;
                        break;
                }

                outputContents.AppendLine(formattedLine);
            }

            string fileName = Path.Combine(outputDirectory, templateClassName + ".cs");
            if (File.Exists(fileName))
                File.Delete(fileName);

            File.WriteAllText(fileName, outputContents.ToString());
        }

        private string GetEntityCreationCode(string templateLine, IDesignerHost host)
        {
            StringBuilder codeLines = new StringBuilder();
            string trimmedLine = templateLine.TrimStart(new char[] { ' ', '\t' });
            string paddingString = templateLine.Substring(0, templateLine.Length - trimmedLine.Length);

            foreach (IEditorControl editorControl in host.EditorControls)
            {
                codeLines.AppendLine(paddingString + "this.AddEntity(" + editorControl.GetCreationString() + ");");
            }

            return codeLines.ToString().TrimEnd(new char[] { '\n', '\r' });
        }
    }
}
