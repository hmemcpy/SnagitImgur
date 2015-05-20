using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtilAssembly;
using SnagitImgur.Plugin.ImageService;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    public class ShareController
    {
        private readonly ISnagIt snagitHost;
        private readonly ISnagItAsyncOutput asyncOutput;

        public ShareController(ISnagIt snagitHost)
        {
            this.snagitHost = snagitHost;
            asyncOutput = snagitHost as ISnagItAsyncOutput;
        }

        public void ShareImage(IImageService service)
        {
            string imagePath = GetCapturedImage();
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            var worker = Task.Factory.StartNew(() =>
            {
                StartAsyncOutput();
                var uploadTask = service.UploadAsync(imagePath);
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
                MessageBox.Show("An error occurred while uploading the image to imgur.com");
            }
        }

        private void HandleResult(ImageInfo result)
        {
            Clipboard.SetText(result.Url);

            ToasterWrapper.DisplayToaster("URL copied to clipboard!", "Click to open in browser", "", () => Process.Start(result.Url));
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