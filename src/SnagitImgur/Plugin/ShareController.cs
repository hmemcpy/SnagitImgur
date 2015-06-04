using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtilAssembly;
using Exceptionless;
using SnagitImgur.Plugin.ImageService;
using SnagitImgur.Properties;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    public class ShareController
    {
        private readonly ISnagIt snagitHost;
        private readonly Settings settings;
        private readonly ISnagItAsyncOutput asyncOutput;

        public ShareController(ISnagIt snagitHost, Settings settings)
        {
            this.snagitHost = snagitHost;
            this.settings = settings;
            asyncOutput = snagitHost as ISnagItAsyncOutput;
        }

        public void ShareImage(IImageService service)
        {
            string imagePath = GetCapturedImage();

            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

            var worker = Task.Run(() =>
            {
                StartAsyncOutput();
                var uploadTask = service.UploadImage(imagePath);
                uploadTask.ContinueWith(task => HandleError(task.Exception), 
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);
                return uploadTask.Result;
            });
            worker.ContinueWith(previousTask => HandleResult(previousTask.Result),
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(task =>
                {
                    FinishAsyncOutput();
                    File.Delete(imagePath);
                });
        }

        private void HandleError(AggregateException ae)
        {
            if (ae.InnerException != null)
            {
                MessageBox.Show("An error occurred while uploading the image to imgur.com. Please try again.", "SnagitImgur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ae.InnerException.ToExceptionless().Submit();
            }
        }

        private void HandleResult(ImageInfo result)
        {
            if (settings.CopyToClipboard)
                CopyToClipboard(result);

            if (settings.ShowPopup)
                ToasterWrapper.DisplayToaster("URL copied to clipboard!", "Open in browser...", PackageOutput.IconPath,
                    () => Process.Start(result.Url));

            if (settings.OpenInBrowser)
                Process.Start(result.Url);
        }

        private static void CopyToClipboard(ImageInfo result)
        {
            try
            {
                RunAsSTAThread(() => Clipboard.SetText(result.Url));
            }
            catch(Exception ex)
            {
                ex.ToExceptionless()
                    .SetMessage("A thread exception occurred while copying to clipboard")
                    .SetProperty("ThreadAppartment", Thread.CurrentThread.GetApartmentState())
                    .SetProperty("SynchronizationContext", SynchronizationContext.Current)
                    .SetProperty(".NET45", IsNet45OrNewer())
                    .SetProperty("Runtime", Environment.Version)
                    .Submit();
            }
        }

        private static void RunAsSTAThread(Action action)
        {
            var thread = new Thread(
                () =>
                {
                    action();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private static bool IsNet45OrNewer()
        {
            // Class "ReflectionContext" exists from .NET 4.5 onwards.
            return Type.GetType("System.Reflection.ReflectionContext", false) != null;
        }

        private void StartAsyncOutput()
        {
            if (asyncOutput != null)
            {
                asyncOutput.StartAsyncOutput();
            }
        }

        private void FinishAsyncOutput()
        {
            if (asyncOutput != null)
            {
                asyncOutput.FinishAsyncOutput(true);
            }
        }

        private string GetCapturedImage()
        {
            ISnagItDocument snagItDocument = snagitHost.SelectedDocument;
            var imageDocumentSave = snagItDocument as ISnagItImageDocumentSave;
            if (imageDocumentSave == null)
            {
                throw new InvalidOperationException("Unable to get image saving facility of Snagit");
            }

            string tempFileName = Path.GetTempFileName() + ".png";
            imageDocumentSave.SaveToFile(ref tempFileName, snagImageFileType.siftPNG, null);

            return tempFileName;
        }
    }
}