using System.Collections.Specialized;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using SnagitImgur.Properties;

namespace SnagitImgur.OAuth
{
    public class OAuthHelper
    {
        private readonly Settings settings;

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(settings.AccessToken); }
        }

        public string OAuthUrl { get; private set; }

        public OAuthHelper(Settings settings)
        {
            this.settings = settings;
            OAuthUrl = string.Format("https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=pin&state=", settings.ClientID);
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
                c["client_id"] = settings.ClientID;
                c["client_secret"] = settings.ClientSecret;
                c["grant_type"] = "pin";
                c["pin"] = pin;
                byte[] result = await wc.UploadValuesTaskAsync("https://api.imgur.com/oauth2/token", "POST", c);
                var token = new JsonFx.Json.JsonReader(new DataReaderSettings(new DataContractResolverStrategy()))
                    .Read<ImgurToken>(Encoding.ASCII.GetString(result));

                SaveValues(token);
            }
        }

        public async Task RefreshAccessToken()
        {
            using (var wc = new WebClient())
            {
                var c = new NameValueCollection();
                c["refresh_token"] = settings.RefreshToken;
                c["client_id"] = settings.ClientID;
                c["client_secret"] = settings.ClientSecret;
                c["grant_type"] = "refresh_token";
                byte[] result = await wc.UploadValuesTaskAsync("https://api.imgur.com/oauth2/token", "POST", c);
                var token = new JsonFx.Json.JsonReader(new DataReaderSettings(new DataContractResolverStrategy()))
                    .Read<ImgurToken>(Encoding.ASCII.GetString(result));

                SaveValues(token);
            }
        }

        private void SaveValues(ImgurToken token)
        {
            settings.AccessToken = token.AccessToken;
            settings.RefreshToken = token.RefreshToken;
            settings.AccountID = token.AccountID;
            settings.AccountUsername = token.AccountUsername;
            settings.Save();
        }

        public void SignOut()
        {
            var empty = new ImgurToken();
            SaveValues(empty);
        }

        public void OpenAuthorizationPage()
        {
            Process.Start(OAuthUrl);
        }
    }
}