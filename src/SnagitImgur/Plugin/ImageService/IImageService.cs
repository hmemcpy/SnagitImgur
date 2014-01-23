using System.Threading.Tasks;

namespace SnagitImgur.Plugin.ImageService
{
    public interface IImageService
    {
        Task<ImageInfo> UploadAsync(string imagePath);
    }
}