namespace OpenTorch
{
    using Android.Hardware.Camera2;
    using Android.Widget;

    public class TorchListener : CameraManager.TorchCallback
    {
        private readonly Torch _torch;

        public TorchListener(Torch torch)
        {
            this._torch = torch;
        }
        public override void OnTorchModeChanged(string cameraId, bool enabled)
        {
            //base.OnTorchModeChanged(cameraId, enabled);
            this._torch.Toggle(cameraId, enabled);
        }

        public override void OnTorchModeUnavailable(string cameraId)
        {
            //base.OnTorchModeUnavailable(cameraId);
            this._torch.OnTorchModeUnavailable(cameraId);
        }
    }
}