using System;
using System.Windows.Forms;
using SnagitImgur.Properties;

namespace SnagitImgur.Dialogs
{
    public partial class PinCodeForm : Form
    {
        private readonly OAuthHelper oauthHelper;

        public PinCodeForm(OAuthHelper oauthHelper)
        {
            this.oauthHelper = oauthHelper;
            InitializeComponent();
        }

        private void txtPinCode_TextChanged(object sender, EventArgs e)
        {
            btnAccept.Enabled = txtPinCode.Text.Length > 0;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            string pin = txtPinCode.Text.Trim();
            oauthHelper.Authenticate(pin);
        }
    }
}
