// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraCaptureUI.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __IOS__
using UIKit;
#endif
using System;
using System.Threading.Tasks;
using InTheHand.Storage;
using System.Threading;
using System.IO;

namespace InTheHand.Media.Capture
{
    /// <summary>
    /// Provides a full window UI for capturing audio, video, and photos from a camera. 
    /// </summary>
    public sealed class CameraCaptureUI
    {
#if __IOS__
        private UIImagePickerController _pc = new UIImagePickerController();
#endif
        private EventWaitHandle _handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private string _filename;

        public CameraCaptureUI()
        {
#if __IOS__
            _pc.ModalPresentationStyle = UIModalPresentationStyle.CurrentContext;
            _pc.FinishedPickingMedia += Pc_FinishedPickingMedia;
            _pc.Canceled += Pc_Canceled;
#endif
        }

#if __IOS__
        private void Pc_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            Stream gfxStream = null;
            if (e.EditedImage != null)
            {
                gfxStream = e.EditedImage.AsJPEG().AsStream();
            }
            else if(e.OriginalImage != null)
            {
                gfxStream = e.OriginalImage.AsJPEG().AsStream();
            }

            if(gfxStream != null)
            {
                _filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DateTime.UtcNow.ToString("yyyy-mm-dd HH:mm:ss") + ".jpg");
                Stream file = File.Create(_filename);
                gfxStream.CopyTo(file);
                file.Close();
            }
           
            _pc.DismissViewController(true, null);
            _handle.Set();
        }

        private void Pc_Canceled(object sender, EventArgs e)
        {
            _pc.DismissViewController(true, null);
            _handle.Set();
        }
#endif

        /// <summary>
        /// Launches the CameraCaptureUI user interface. 
        /// </summary>
        /// <param name="mode">Specifies whether the user interface that will be shown allows the user to capture a photo, capture a video, or capture both photos and videos.</param>
        /// <returns>When this operation completes, a StorageFile object is returned.</returns>
        public Task<StorageFile> CaptureFileAsync(CameraCaptureUIMode mode)
        {
            return Task.Run<StorageFile>(async () =>
            {
#if __IOS__
               UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    _pc.SourceType = UIImagePickerControllerSourceType.Camera;
                    switch (mode)
                    {
                        case CameraCaptureUIMode.Photo:
                            _pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                            break;

                        case CameraCaptureUIMode.Video:
                            _pc.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
                            break;
                    }
                    
                    UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(_pc, true, null);
                });
#endif
                _handle.WaitOne();

                if(!string.IsNullOrEmpty(_filename))
                {
                    return await StorageFile.GetFileFromPathAsync(_filename);
                }
                return null;
            });
        }
    }

    /// <summary>
    /// Determines whether the user interface for capturing from the attached camera allows capture of photos, videos, or both photos and videos.
    /// </summary>
    public enum CameraCaptureUIMode
    {
        /// <summary>
        /// Either a photo or video can be captured.
        /// </summary>
        PhotoOrVideo = 0,
        /// <summary>
        /// The user can only capture a photo.
        /// </summary>
        Photo = 1,
        /// <summary>
        /// The user can only capture a video. 
        /// </summary>
        Video = 2,
    }
}