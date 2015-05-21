using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            btnAuthenticate.Tag = "auth";
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
        private readonly string clientId;
        private readonly string clientSecret;

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(settings.AccessToken); }
        }

        public string OAuthUrl { get; private set; }

        public OAuthHelper(Settings settings)
        {
            this.settings = settings;
            clientId = settings.ClientID;
            clientSecret = settings.ClientSecret;
            OAuthUrl = string.Format("https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=pin&state=", clientId);
        }

        public string GetAccountName()
        {
            return settings.AccountUsername;
        }

        public async Task Authenticate(string pin)
        {
            using (var wc = new WebClient())
            {
                var c = new NameValueCollection();
                c["client_id"] = clientId;
                c["client_secret"] = clientSecret;
                c["grant_type"] = "pin";
                c["pin"] = pin;
                byte[] result = await wc.UploadValuesTaskAsync("https://api.imgur.com/oauth2/token", "POST", c);
                dynamic json = new JsonFx.Json.JsonReader().Read(Encoding.ASCII.GetString(result));

                SaveValues(json);
            }
        }

        private void SaveValues(dynamic json)
        {
            settings.AccessToken = json.access_token;
            settings.RefreshToken = json.refresh_token;
            settings.AccountID = json.account_id;
            settings.AccountUsername = json.account_username;
            settings.Save();
        }

        public void SignOut()
        {
            
        }

        public void OpenAuthorizationPage()
        {
            Process.Start(OAuthUrl);
        }
    }
}
