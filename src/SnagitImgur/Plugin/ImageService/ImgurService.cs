using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using SnagitImgur.OAuth;
using SnagitImgur.Plugin.Extensions;
using SnagitImgur.Properties;

namespace SnagitImgur.Plugin.ImageService
{
    /// <summary>
    /// API wrapper for imgur API v3 
    /// </summary>
    public class ImgurService : IImageService
    {
        private readonly Settings settings;
        private const string apiBaseUrl = "https://api.imgur.com/3/";
        private readonly IRestClient client;

        public ImgurService(Settings settings)
        {
            this.settings = settings;

            client = new RestClient(apiBaseUrl) { Authenticator = CreateAuthenticator() };
        }

        private IAuthenticator CreateAuthenticator()
        {
            if (!string.IsNullOrWhiteSpace(settings.AccessToken))
            {
                return new OAuth2AuthorizationRequestHeaderAuthenticator(settings.AccessToken, "Bearer");
            }

            return new AnonymousClientAuthenticator(settings.ClientID);
        }


        /// <summary>
        /// Uploads the image asynchronously to imgur.com 
        /// </summary>
        /// <param name="imagePath">Path to the image file.</param>
        /// <returns>A <c>Task</c> object containing the <see cref="ImageInfo"/>.</returns>
        /// <exception cref="WebException">Thrown if imgur.com returns any status code other than <see cref="HttpStatusCode.OK"/>.</exception>
        public Task<ImageInfo> UploadImage(string imagePath)
        {
            // http://api.imgur.com/endpoints/image#image-upload

            var request = new RestRequest("image", Method.POST);
            request.AddParameter("image", Convert.ToBase64String(File.ReadAllBytes(imagePath)), ParameterType.RequestBody);
            
            return client.ExecuteAsyncTask<dynamic>(request, response =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new JsonFx.Json.JsonReader().Read(response.Content);
                }
                if (response.StatusCode == HttpStatusCode.Forbidden && Regex.IsMatch(response.Content, "access token.*?invalid|expired"))
                {
                    client.Authenticator = RefreshToken();
                    return UploadImage(imagePath).Result;
                }

                throw new WebException(response.Content);
            }).ContinueWith(previousTask =>
            {
                var info = previousTask.Result as ImageInfo;
                if (info != null)
                    return info;

                dynamic uploadResponse = previousTask.Result;

                return new ImageInfo
                {
                    Id = uploadResponse.data.id,
                    Url = uploadResponse.data.link,
                    DeleteHash = uploadResponse.data.deletehash,
                };
            });
        }

        private IAuthenticator RefreshToken()
        {
            var helper = new OAuthHelper(settings);
            helper.RefreshAccessToken().Wait();
            return CreateAuthenticator();
        }
    }

    public class ImageInfo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string DeleteHash { get; set; }
    }
}