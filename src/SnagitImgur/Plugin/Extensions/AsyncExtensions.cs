using System;
using System.Threading.Tasks;
using RestSharp;

namespace SnagitImgur.Plugin.Extensions
{
    public static class AsyncExtensions
    {
        public static Task<TResult> ExecuteAsyncTask<TResult>(this IRestClient client, IRestRequest request, Func<IRestResponse, TResult> selector)
        {
            var tcs = new TaskCompletionSource<TResult>();
            client.ExecuteAsync(request, response =>
            {
                if (response.ErrorException == null)
                {
                    TResult result = selector(response);
                    tcs.SetResult(result);
                }
                else
                {
                    tcs.SetException(response.ErrorException);
                }
            });
            return tcs.Task;
        }
    }
}