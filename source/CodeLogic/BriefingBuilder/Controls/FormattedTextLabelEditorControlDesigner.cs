using BriefingBuilder.DesignEditors;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BriefingBuilder.Controls
{
    public class FormattedTextLabelEditorControlDesigner : ControlDesigner
    {
        private enum VerbType
        {
            EditText,
        }

        private DesignerVerbCollection _verbs;
        private FormatStringEditorDialog _dialog;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection();
                    _verbs.Add(new DesignerVerb("Edit Text", (sender, e) => HandleVerb(VerbType.EditText)));
                }

                return _verbs;
            }
        }

        private T GetComponentAs<T>() where T : class
        {
            T component = this.Component as T;
            if (component == null)
                throw new Exception(string.Format("Component is not a {0}.", typeof(T).Name));

            return component;
        }

        private void UpdatePropertyInDesigner(string propertyName, FormattedTextLabelEditorControl control, object originalValue, object currentValue)
        {
            this.RaiseComponentChanged(
                TypeDescriptor.GetProperties(control)[propertyName],
                originalValue,
                currentValue
            );
        }

        private void HandleVerb(VerbType verbType)
        {
            try
            {
                switch (verbType)
                {
                    case VerbType.EditText:
                        if (_dialog == null)
                            _dialog = new FormatStringEditorDialog();

                        FormattedTextLabelEditorControl control = this.GetComponentAs<FormattedTextLabelEditorControl>();
                        string[] originalLines = control.TextLines;

                        _dialog.FormattedText = control.Text;
                        if (_dialog.ShowDialog() == DialogResult.OK)
                        {
                            control.TextLines = _dialog.FormattedText.TrimEnd().Split(new char[] { '\n' });
                            UpdatePropertyInDesigner("TextLines", control, originalLines, control.TextLines);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
