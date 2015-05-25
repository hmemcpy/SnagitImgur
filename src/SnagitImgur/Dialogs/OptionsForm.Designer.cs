namespace SnagitImgur.Dialogs
{
    partial class OptionsForm
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
            this.chkCopyToClipboard = new System.Windows.Forms.CheckBox();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkOpenInBrowser = new System.Windows.Forms.CheckBox();
            this.chkShowPopup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkCopyToClipboard
            // 
            this.chkCopyToClipboard.AutoSize = true;
            this.chkCopyToClipboard.Checked = true;
            this.chkCopyToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyToClipboard.Location = new System.Drawing.Point(13, 13);
            this.chkCopyToClipboard.Name = "chkCopyToClipboard";
            this.chkCopyToClipboard.Size = new System.Drawing.Size(165, 17);
            this.chkCopyToClipboard.TabIndex = 0;
            this.chkCopyToClipboard.Text = "Copy image URL to Clipboard";
            this.chkCopyToClipboard.UseVisualStyleBackColor = true;
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAccept.Location = new System.Drawing.Point(196, 87);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 3;
            this.btnAccept.Text = "&OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(277, 87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkOpenInBrowser
            // 
            this.chkOpenInBrowser.AutoSize = true;
            this.chkOpenInBrowser.Checked = true;
            this.chkOpenInBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenInBrowser.Location = new System.Drawing.Point(13, 36);
            this.chkOpenInBrowser.Name = "chkOpenInBrowser";
            this.chkOpenInBrowser.Size = new System.Drawing.Size(206, 17);
            this.chkOpenInBrowser.TabIndex = 1;
            this.chkOpenInBrowser.Text = "Open the browser with the image URL";
            this.chkOpenInBrowser.UseVisualStyleBackColor = true;
            // 
            // chkShowPopup
            // 
            this.chkShowPopup.AutoSize = true;
            this.chkShowPopup.Checked = true;
            this.chkShowPopup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowPopup.Location = new System.Drawing.Point(13, 59);
            this.chkShowPopup.Name = "chkShowPopup";
            this.chkShowPopup.Size = new System.Drawing.Size(280, 17);
            this.chkShowPopup.TabIndex = 5;
            this.chkShowPopup.Text = "Show notification window on successful image upload";
            this.chkShowPopup.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(364, 122);
            this.Controls.Add(this.chkShowPopup);
            this.Controls.Add(this.chkOpenInBrowser);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.chkCopyToClipboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCopyToClipboard;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkOpenInBrowser;
		private System.Windows.Forms.CheckBox chkShowPopup;
	}
}