using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonUtilAssembly;
using RestSharp;
using SnagitImgur.Dialogs;
using SnagitImgur.Plugin.ImageService;
using SnagitImgur.Properties;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("681D1A5C-A78F-4D27-86A2-A07AAC89B8FE")]
    public class PackageOutput : MarshalByRefObject, IComponentInitialize, IOutput, IComponentWantsCategoryPreferences, IOutputMenu, IPackageOptionsUI
    {
        internal static string IconPath;
        private static string PackageDirectory;
        private IWin32Window snagitWindow;
        private ShareController shareController;

        public void InitializeComponent(object pExtensionHost, IComponent pComponent, componentInitializeType initType)
        {
            var snagitHost = pExtensionHost as ISnagIt;
            if (snagitHost == null)
            {
                throw new InvalidOperationException("Unable to communicate with Snagit");
            }

            snagitWindow = new Win32HWndWrapper(new IntPtr(snagitHost.TopLevelHWnd));
            shareController = new ShareController(snagitHost);
        }

        public void Output()
        {
            IImageService imageService = GetSelectedImageService();
            shareController.ShareImage(imageService);
        }

        private IImageService GetSelectedImageService()
        {
            return new ImgurService(
                CreateAuthenticator(Settings.Default)
                );
        }

        private IAuthenticator CreateAuthenticator(Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.AccessToken))
            {
                return new OAuth2AuthorizationRequestHeaderAuthenticator(settings.AccessToken, "Bearer");
            }

            return new AnonymousClientAuthenticator(settings.ClientID);
        }

        public string GetOutputMenuData()
        {
            return "<menu> " +
                      "<menuitem label=\"Send to imgur.com\" id=\"1\" />" +
                      "<menuseparator />" +
                      "<menuitem label=\"Account\" id=\"2\" />" +
                      "<menuitem label=\"Options\" id=\"3\" />" +
                      "<menuseparator />" +
                      "<menuitem label=\"About\" id=\"4\" />" +
                   "</menu>";
        }

        public void SelectOutputMenuItem(string id)
        {
            switch (id)
            {
                case "1":
                    Output();
                    break;
                case "2":
                    ShowAccount(Settings.Default);
                    break;
                case "3":
                    ShowPackageOptionsUI();
                    break;
                case "4":
                    ShowAbout();
                    break;
            }
        }

        private void ShowAccount(Settings settings)
        {
            using (var accountForm = new AccountForm(new OAuthHelper(settings)))
            {
                var dialogResult = DialogResult.Retry;
                while (dialogResult != DialogResult.Cancel)
                {
                    dialogResult = accountForm.ShowDialog(snagitWindow);
                }
            }
        }

        private bool IsAutenticated(Settings settings)
        {
            return !string.IsNullOrWhiteSpace(settings.AccessToken);
        }

        private void ShowAbout()
        {
            MessageBox.Show("About");

        }

        public void ShowPackageOptionsUI()
        {
            MessageBox.Show("Settings");
            
        }
        public void SetComponentCategoryPreferences(SnagItOutputPreferences prefs)
        {
            PackageDirectory = prefs.PackageDir;
            IconPath = Path.Combine(PackageDirectory, "imgur.com.ico");
        }
    }
}