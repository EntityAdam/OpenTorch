namespace OpenTorch
{
    using System;

    using Android.Content;
    using Android.Hardware.Camera2;

    public class Torch
    {
        private readonly Context context;
        private readonly CameraManager cm;
        private string[] cameras;

        public Torch(Context context)
        {
            this.context = context;
            cm = (CameraManager)context.GetSystemService(Context.CameraService);
        }

        private string RearFacingCamera { get; set; }

        private bool TorchOn { get; set; }

        public void Start()
        {
            cameras = cm.GetCameraIdList();
            var cam = GetRearFacingCamera();
            if (string.IsNullOrEmpty(cam))
            {
                throw new Exception("This device does not have a rear camera.");
            }

            if (!CheckFlashSupported(cam))
            {
                throw new Exception("This device does not have a rear camera that supports flash.");
            }

            RearFacingCamera = cam;
            TorchOn = false;
        }

        public void Toggle()
        {
            if (!string.IsNullOrEmpty(RearFacingCamera))
            {
                this.TorchOn = !this.TorchOn;
                cm.SetTorchMode(RearFacingCamera, TorchOn);
            }
        }

        private bool CheckFlashSupported(string rearFacingCamera)
        {
            var chars = GetCharactaristics(rearFacingCamera);
            return (bool)chars.Get(CameraCharacteristics.FlashInfoAvailable);
        }

        private string GetRearFacingCamera()
        {
            foreach (var cameraId in cameras)
            {
                var chars = GetCharactaristics(cameraId);

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
            return this.cm.GetCameraCharacteristics(cameraId);
        }
    }
}