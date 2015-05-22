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
            this.chkOpenBrowser = new System.Windows.Forms.CheckBox();
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
            this.btnAccept.Location = new System.Drawing.Point(196, 60);
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
            this.btnCancel.Location = new System.Drawing.Point(277, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkOpenBrowser
            // 
            this.chkOpenBrowser.AutoSize = true;
            this.chkOpenBrowser.Checked = true;
            this.chkOpenBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenBrowser.Location = new System.Drawing.Point(13, 36);
            this.chkOpenBrowser.Name = "chkOpenBrowser";
            this.chkOpenBrowser.Size = new System.Drawing.Size(206, 17);
            this.chkOpenBrowser.TabIndex = 1;
            this.chkOpenBrowser.Text = "Open the browser with the image URL";
            this.chkOpenBrowser.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 95);
            this.Controls.Add(this.chkOpenBrowser);
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
        private System.Windows.Forms.CheckBox chkOpenBrowser;
    }
}