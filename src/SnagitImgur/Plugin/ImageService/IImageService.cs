using System.Threading.Tasks;

namespace SnagitImgur.Plugin.ImageService
{
    public interface IImageService
    {
        Task<ImageInfo> UploadImage(string imagePath);
    }

    public class TokenInfo
    {
    }
}