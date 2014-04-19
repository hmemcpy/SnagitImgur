using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using SnagitImgur.Plugin.Extensions;

namespace SnagitImgur.Plugin.ImageService
{
    /// <summary>
    /// API wrapper for imgur API v3 
    /// </summary>
    public class ImgurService : IImageService
    {
        private const string apiBaseUrl = "https://api.imgur.com/3/";
        private readonly IRestClient client;

        public ImgurService(string clientId)
        {
            client = new RestClient(apiBaseUrl);
            client.AddDefaultHeader("Authorization", "Client-ID " + clientId);
        }

        /// <summary>
        /// Uploads the image asynchronously to imgur.com 
        /// </summary>
        /// <param name="imagePath">Path to the image file.</param>
        /// <returns>A <c>Task</c> object containing the <see cref="ImageInfo"/>.</returns>
        /// <exception cref="WebException">Thrown if imgur.com returns any status code other than <see cref="HttpStatusCode.OK"/>.</exception>
        public Task<ImageInfo> UploadAsync(string imagePath)
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

                throw new WebException(response.Content);
            }).ContinueWith(previousTask =>
            {
                dynamic uploadResponse = previousTask.Result;

                return new ImageInfo
                {
                    Id = uploadResponse.data.id,
                    Url = uploadResponse.data.link,
                    DeleteHash = uploadResponse.data.deletehash,
                };
            });
        }
    }

    public class ImageInfo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string DeleteHash { get; set; }
    }
}