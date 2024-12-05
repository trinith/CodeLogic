namespace BriefingBuilder.DesignEditors
{
    partial class TexturePickerDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btnOk = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._lbAvailableTextures = new System.Windows.Forms.ListBox();
            this._scSplitContainer = new System.Windows.Forms.SplitContainer();
            this._pbPreview = new System.Windows.Forms.PictureBox();
            this._cbPreviewBackground = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._scSplitContainer)).BeginInit();
            this._scSplitContainer.Panel1.SuspendLayout();
            this._scSplitContainer.Panel2.SuspendLayout();
            this._scSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pbPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOk.Location = new System.Drawing.Point(604, 292);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 0;
            this._btnOk.Text = "Ok";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.Handle_OkButtonClick);
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(685, 292);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 1;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _lbAvailableTextures
            // 
            this._lbAvailableTextures.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lbAvailableTextures.FormattingEnabled = true;
            this._lbAvailableTextures.Location = new System.Drawing.Point(0, 0);
            this._lbAvailableTextures.Name = "_lbAvailableTextures";
            this._lbAvailableTextures.Size = new System.Drawing.Size(383, 274);
            this._lbAvailableTextures.TabIndex = 2;
            this._lbAvailableTextures.SelectedIndexChanged += new System.EventHandler(this.Handle_ListBoxSelectedIndexChanged);
            // 
            // _scSplitContainer
            // 
            this._scSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._scSplitContainer.Location = new System.Drawing.Point(12, 12);
            this._scSplitContainer.Name = "_scSplitContainer";
            // 
            // _scSplitContainer.Panel1
            // 
            this._scSplitContainer.Panel1.Controls.Add(this._lbAvailableTextures);
            // 
            // _scSplitContainer.Panel2
            // 
            this._scSplitContainer.Panel2.Controls.Add(this._pbPreview);
            this._scSplitContainer.Size = new System.Drawing.Size(748, 274);
            this._scSplitContainer.SplitterDistance = 383;
            this._scSplitContainer.TabIndex = 3;
            // 
            // _pbPreview
            // 
            this._pbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pbPreview.Location = new System.Drawing.Point(0, 0);
            this._pbPreview.Name = "_pbPreview";
            this._pbPreview.Size = new System.Drawing.Size(361, 274);
            this._pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._pbPreview.TabIndex = 0;
            this._pbPreview.TabStop = false;
            // 
            // _cbPreviewBackground
            // 
            this._cbPreviewBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._cbPreviewBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbPreviewBackground.FormattingEnabled = true;
            this._cbPreviewBackground.Location = new System.Drawing.Point(157, 292);
            this._cbPreviewBackground.Name = "_cbPreviewBackground";
            this._cbPreviewBackground.Size = new System.Drawing.Size(238, 21);
            this._cbPreviewBackground.TabIndex = 4;
            this._cbPreviewBackground.SelectedIndexChanged += new System.EventHandler(this.Handle_ComboBoxSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Preview Background Colour";
            // 
            // TexturePickerDialog
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(772, 327);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this._cbPreviewBackground);
            this.Controls.Add(this._scSplitContainer);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.Name = "TexturePickerDialog";
            this.Text = "TexturePickerForm";
            this._scSplitContainer.Panel1.ResumeLayout(false);
            this._scSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._scSplitContainer)).EndInit();
            this._scSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pbPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.ListBox _lbAvailableTextures;
        private System.Windows.Forms.SplitContainer _scSplitContainer;
        private System.Windows.Forms.PictureBox _pbPreview;
        private System.Windows.Forms.ComboBox _cbPreviewBackground;
        private System.Windows.Forms.Label label1;
    }
}