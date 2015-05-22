using System;
using System.Windows.Forms;
using SnagitImgur.OAuth;

namespace SnagitImgur.Dialogs
{
    public partial class AccountForm : Form
    {
        private readonly OAuthHelper oauthHelper;

        public AccountForm(OAuthHelper oauthHelper)
        {
            this.oauthHelper = oauthHelper;

            InitializeComponent();
        }

        private void AccountFormLoad(object sender, EventArgs e)
        {
            ShowAccountDetails();
        }

        private void ShowAccountDetails()
        {
            bool auth = oauthHelper.IsAuthenticated;
            lblAccountName.Text = auth ? oauthHelper.GetAccountName() : "Anonymous";
            btnAuthenticate.Text = auth ? "&Sign Out" : "Authorize";
            btnAuthenticate.Tag = auth ? "sign_out" : "auth";
        }

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string tag = (string)btnAuthenticate.Tag;
            switch (tag)
            {
                case "sign_out":
                    if (MessageBox.Show(
                        string.Format("Are you sure you want to sign out '{0}'?", oauthHelper.GetAccountName()),
                        "imgur.com",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        oauthHelper.SignOut();
                        ShowAccountDetails();
                    }
                    break;
                case "auth":
                    oauthHelper.OpenAuthorizationPage();
                    ShowPinCodePrompt();
                    break;
            }
        }

        private void ShowPinCodePrompt()
        {
            using (var pinCodeForm = new PinCodeForm(oauthHelper))
            {
                if (pinCodeForm.ShowDialog() == DialogResult.OK)
                {
                    ShowAccountDetails();
                }
            }
        }
    }
}
