using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtilAssembly;
using Exceptionless;
using RestSharp;
using SnagitImgur.Dialogs;
using SnagitImgur.OAuth;
using SnagitImgur.Plugin.ImageService;
using SnagitImgur.Properties;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("681D1A5C-A78F-4D27-86A2-A07AAC89B8FE")]
    public class PackageOutput : MarshalByRefObject, 
        IComponentInitialize, 
        IOutput, 
        IComponentWantsCategoryPreferences, 
        IOutputMenu, 
        IPackageOptionsUI
    {
        public const string Version = "1.5.0";

        public static string IconPath;

        private static string PackageDirectory;
        private IWin32Window snagitWindow;
        private ShareController shareController;

        public void InitializeComponent(object pExtensionHost, IComponent pComponent, componentInitializeType initType)
        {
            ExceptionlessClient.Default.Register();
            ExceptionlessClient.Default.Configuration.ApiKey = "rMZZehkm8bq2HH0J9d4YV7pMYhkZPpHIfKcDsvSa";

            var snagitHost = pExtensionHost as ISnagIt;
            if (snagitHost == null)
            {
                throw new InvalidOperationException("Unable to communicate with Snagit");
            }

            snagitWindow = new Win32HWndWrapper(new IntPtr(snagitHost.TopLevelHWnd));
            shareController = new ShareController(snagitHost, Settings.Default);
            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

            new WebClient().DownloadStringTaskAsync("https://raw.githubusercontent.com/hmemcpy/SnagitImgur/master/latest.txt")
              .ContinueWith(task => CheckVersion(task.Result),
                  CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CheckVersion(string versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString)) return;

            var latest = new Version(versionString.Trim());
            var current = new Version(Version);

            if (latest > current)
            {
                const string latestUrl = "https://github.com/hmemcpy/SnagitImgur/releases/latest";

                ToasterWrapper.DisplayToaster("New SnagitImgur version available!", "Click to download...", IconPath, () => Process.Start(latestUrl));
            }
        }

        public void Output()
        {
            IImageService imageService = GetSelectedImageService();
            shareController.ShareImage(imageService);
        }

        private IImageService GetSelectedImageService()
        {
            return new ImgurService(Settings.Default);
        }

        public string GetOutputMenuData()
        {
            return "<menu> " +
                      "<menuitem label=\"&Send to imgur.com\" id=\"1\" />" +
                      "<menuseparator />" +
                      "<menuitem label=\"&Account...\" id=\"2\" />" +
                      "<menuitem label=\"&Options\" id=\"3\" />" +
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

        private void ShowAbout()
        {
            MessageBox.Show(string.Format("SnagitImgur v{0}\n\nby Igal Tabachnik", Version), "About SnagitImgur",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowPackageOptionsUI()
        {
            using (var optionsForm = new OptionsForm(Settings.Default))
            {
                if (optionsForm.ShowDialog(snagitWindow) == DialogResult.OK)
                {
                    Settings.Default.Save();
                }
            }
        }

        public void SetComponentCategoryPreferences(SnagItOutputPreferences prefs)
        {
            PackageDirectory = prefs.PackageDir;
            IconPath = Path.Combine(PackageDirectory, "imgur.com.ico");
        }
    }
}