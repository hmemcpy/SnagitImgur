using RestSharp;

namespace SnagitImgur.Plugin.ImageService
{
    internal class AnonymousClientAuthenticator : IAuthenticator
    {
        private readonly string clientId;

        public AnonymousClientAuthenticator(string clientId)
        {
            this.clientId = string.Format("Client-ID {0}", clientId);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            client.AddDefaultHeader("Authorization", clientId);
        }
    }
}