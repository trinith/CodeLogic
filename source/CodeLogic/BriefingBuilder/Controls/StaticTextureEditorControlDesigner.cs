using BriefingBuilder.DesignEditors;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BriefingBuilder.Controls
{
    public class StaticTextureEditorControlDesigner : ControlDesigner
    {
        private enum VerbType
        {
            ResetSize,
            SetWidthForAR,
            SetHeightForAR,
            SetTexture,
        }

        private DesignerVerbCollection _verbs = null;
        private TexturePickerDialog _dialog = null;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (_verbs == null)
                {
                    _verbs = new DesignerVerbCollection();
                    _verbs.Add(new DesignerVerb("Reset Size to Texture Dimensions", (sender, e) => HandleVerb(VerbType.ResetSize)));
                    _verbs.Add(new DesignerVerb("Set Width for Aspect Ratio", (sender, e) => HandleVerb(VerbType.SetWidthForAR)));
                    _verbs.Add(new DesignerVerb("Set Height for Aspect Ratio", (sender, e) => HandleVerb(VerbType.SetHeightForAR)));
                    _verbs.Add(new DesignerVerb("Set Texture", (sender, e) => HandleVerb(VerbType.SetTexture)));
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

        private void UpdatePropertyInDesigner(string propertyName, StaticTextureEditorControl control, object originalValue, object currentValue)
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
                StaticTextureEditorControl control = this.GetComponentAs<StaticTextureEditorControl>();

                switch (verbType)
                {
                    case VerbType.ResetSize:
                        {
                            Size originalSize = control.Size;
                            control.ResetSizeToTextureDimensions();
                            UpdatePropertyInDesigner("Size", control, originalSize, control.Size);
                        }
                        break;
                    case VerbType.SetWidthForAR:
                        {
                            Size originalSize = control.Size;
                            control.SetWidthForTextureAspectRatio();
                            UpdatePropertyInDesigner("Size", control, originalSize, control.Size);
                        }
                        break;
                    case VerbType.SetHeightForAR:
                        {
                            Size originalSize = control.Size;
                            control.SetHeightForTextureAspectRatio();
                            UpdatePropertyInDesigner("Size", control, originalSize, control.Size);
                        }
                        break;
                    case VerbType.SetTexture:
                        {
                            if (_dialog == null)
                                _dialog = new TexturePickerDialog();

                            string originalAsset = control.TextureAsset;
                            _dialog.TextureAsset = originalAsset;

                            if (_dialog.ShowDialog() == DialogResult.OK)
                            {
                                control.TextureAsset = _dialog.TextureAsset;
                                UpdatePropertyInDesigner("TextureAsset", control, originalAsset, control.TextureAsset);
                            }
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
