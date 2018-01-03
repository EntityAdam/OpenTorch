namespace OpenTorch
{
    using System;
    using System.Runtime.CompilerServices;

    using Android.Content;
    using Android.Hardware.Camera2;
    using Android.OS;
    using Android.Widget;

    public class Torch
    {
        private readonly Context _context;
        private readonly CameraManager _cm;
        private string[] cameras;
        
        private CameraManager.TorchCallback mTorchState;

        private Handler TorchEventHandler { get; set; }

        private string RearFacingCamera { get; set; }
        public bool FlashSupported { get; set; }

        public bool IsAvailable { get; set; }

        public Torch(Context context)
        {
            this._context = context;
            this._cm = (CameraManager)context.GetSystemService(Context.CameraService);
        }

        public void Start()
        {
            this.cameras = this._cm.GetCameraIdList();
            this.mTorchState = new TorchListener(this);
            this.TorchEventHandler = new Handler();
            this._cm.RegisterTorchCallback(this.mTorchState, this.TorchEventHandler);
            this.RearFacingCamera = this.GetRearFacingCamera();
            this.FlashSupported = this.CheckFlashSupported(this.RearFacingCamera);
        }

        private bool CheckFlashSupported(string rearFacingCamera)
        {
            var chars = this.GetCharactaristics(rearFacingCamera);
            return (bool)chars.Get(CameraCharacteristics.FlashInfoAvailable);
        }

        private string GetRearFacingCamera()
        {
            foreach (var cameraId in this.cameras)
            {
                var chars = this.GetCharactaristics(cameraId);

                var cameraFacing = (int)chars.Get(CameraCharacteristics.LensFacing);
                const int RearFacingValue = (int)LensFacing.Back;
                if (cameraFacing == RearFacingValue)
                {
                    return cameraId;
                }
            }
            return string.Empty;
        }

        private CameraCharacteristics GetCharactaristics(string cameraId)
        {
            return this._cm.GetCameraCharacteristics(cameraId);
        }

        public void Toggle(string cameraId, bool enabled)
        {
            //throw new NotImplementedException();
            var toast = Toast.MakeText(this._context, $"Camera {cameraId} is {enabled}", ToastLength.Short);
            toast.Show();
        }

        public void OnTorchModeUnavailable(string cameraId)
        {
            var toast = Toast.MakeText(this._context, $"Camera {cameraId} Torch is unavailable", ToastLength.Short);
            toast.Show();
        }

        public void Toggle()
        {
            var toast = Toast.MakeText(this._context, $"Camera Id {this.RearFacingCamera} and is flash supported? {this.FlashSupported}", ToastLength.Short);
            toast.Show();
        }
    }
}