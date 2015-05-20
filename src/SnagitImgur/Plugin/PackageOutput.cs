using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SnagitImgur.Plugin.ImageService;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("681D1A5C-A78F-4D27-86A2-A07AAC89B8FE")]
    public class PackageOutput : MarshalByRefObject, IComponentInitialize, IOutput, IComponentWantsCategoryPreferences
    {
        internal static string IconPath;
        private static string PackageDirectory;

        private ShareController shareController;

        public void InitializeComponent(object pExtensionHost, IComponent pComponent, componentInitializeType initType)
        {
            var snagitHost = pExtensionHost as ISnagIt;
            if (snagitHost == null)
            {
                throw new InvalidOperationException("Unable to communicate with Snagit");
            }

            shareController = new ShareController(snagitHost);
        }

        public void Output()
        {
            IImageService imageService = GetSelectedImageService();
            shareController.ShareImage(imageService);
        }

        private IImageService GetSelectedImageService()
        {
            // todo implement others
            // todo move to config
            return new ImgurService("d9c6c0bfd99b470");
        }

        public void SetComponentCategoryPreferences(SnagItOutputPreferences prefs)
        {
            PackageDirectory = prefs.PackageDir;
            IconPath = Path.Combine(PackageDirectory, "imgur.com.ico");
        }
    }
}