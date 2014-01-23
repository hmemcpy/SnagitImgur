using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SnagitImgur.Plugin.ImageService;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("681D1A5C-A78F-4D27-86A2-A07AAC89B8FE")]
    public class PackageOutput : MarshalByRefObject, IComponentInitialize, IOutput
    {
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

        public async void Output()
        {
            IImageService imageService = GetSelectedImageService();
            try
            {
                await shareController.ShareImage(imageService);
            }
            catch (Exception)
            {
                MessageBox.Show("an error occurred!");
            }
        }

        private IImageService GetSelectedImageService()
        {
            // todo implement others
            // todo move to config
            return new ImgurService("d9c6c0bfd99b470");
        }
    }
}