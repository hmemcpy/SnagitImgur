using System;
using System.Windows.Forms;
using SnagitImgur.Properties;

namespace SnagitImgur.Dialogs
{
    public partial class OptionsForm : Form
    {
        private readonly Settings settings;

        public OptionsForm(Settings settings)
        {
            this.settings = settings;
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            settings.CopyToClipboard = chkCopyToClipboard.Checked;
            settings.OpenBrowser = chkOpenBrowser.Checked;

            Close();
            DialogResult = DialogResult.OK;
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            chkCopyToClipboard.Checked = settings.CopyToClipboard;
            chkOpenBrowser.Checked = settings.OpenBrowser;
        }
    }
}
