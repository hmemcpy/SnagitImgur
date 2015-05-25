using System;
using System.Windows.Forms;
using Exceptionless;
using SnagitImgur.OAuth;

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

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            string pin = txtPinCode.Text.Trim();
            pictureBox1.Visible = true;
            btnAccept.Enabled = false;
            btnCancel.Enabled = false;
            Text = "Please wait...";

            try
            {
                await oauthHelper.Authenticate(pin);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred while authorizing the imgur account. Please try again.");
                ex.ToExceptionless().Submit();
                return;
            }
            finally
            {
                Text = "Authentication";
                pictureBox1.Visible = false;
                btnAccept.Enabled = true;
                btnCancel.Enabled = true;
            }
            Close();
            DialogResult = DialogResult.OK;
        }
    }
}
