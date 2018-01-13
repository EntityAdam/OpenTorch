namespace OpenTorch
{
    using System;

    using Android.Content;
    using Android.Hardware.Camera2;

    public class Torch
    {
        //private readonly Context _context;
        private readonly CameraManager _cm;
        private string[] _cameras;

        public Torch(Context context)
        {
            //_context = context;
            _cm = (CameraManager)context.GetSystemService(Context.CameraService);
            TorchOn = false;
        }

        private string RearFacingCamera { get; set; }
        public bool TorchOn { get; set; }

        public void Start()
        {
            _cameras = _cm.GetCameraIdList();
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
        }

        public void Toggle()
        {
            if (string.IsNullOrEmpty(RearFacingCamera)) return;
            TorchOn = !TorchOn;
            _cm.SetTorchMode(RearFacingCamera, TorchOn);
        }

        private bool CheckFlashSupported(string rearFacingCamera)
        {
            var chars = GetCharactaristics(rearFacingCamera);
            return (bool)chars.Get(CameraCharacteristics.FlashInfoAvailable);
        }

        private string GetRearFacingCamera()
        {
            foreach (var cameraId in _cameras)
            {
                var chars = GetCharactaristics(cameraId);

                var cameraFacing = (int)chars.Get(CameraCharacteristics.LensFacing);
                const int rearFacingValue = (int)LensFacing.Back;
                if (cameraFacing == rearFacingValue)
                {
                    return cameraId;
                }
            }
            return string.Empty;
        }

        private CameraCharacteristics GetCharactaristics(string cameraId)
        {
            return _cm.GetCameraCharacteristics(cameraId);
        }
    }
}