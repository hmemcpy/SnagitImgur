using System;
using System.Diagnostics;
using System.Windows.Forms;
using SnagitImgur.Properties;

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
            if (oauthHelper.IsAuthenticated)
            {
                ShowAccountDetails();
            }
        }

        private void ShowAccountDetails()
        {
            lblAccountName.Text = oauthHelper.GetAccountName();
            btnAuthenticate.Text = "&Sign Out";
            btnAuthenticate.Tag = "sign_out";
        }

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string tag = (string)btnAuthenticate.Tag;
            switch (tag)
            {
                case "sign_out":
                    oauthHelper.SignOut();
                    ShowAccountDetails();
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

    public class OAuthHelper
    {
        private readonly Settings settings;
        private const string clientId = "d9c6c0bfd99b470";
        private const string clientSecret = "";

        public bool IsAuthenticated
        {
            get { return false; }
        }

        public string OAuthUrl { get; private set; }

        public OAuthHelper(Settings settings)
        {
            this.settings = settings;
            OAuthUrl = string.Format("https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=pin&state=", clientId);
        }


        public string GetAccountName()
        {
            throw new NotImplementedException();
        }

        public void Authenticate(string pin)
        {
            
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public void OpenAuthorizationPage()
        {
            Process.Start(OAuthUrl);
        }
    }
}
