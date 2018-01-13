namespace OpenTorch
{
    using System;

    using Android.App;
    using Android.OS;
    using Android.Widget;

    [Activity(Label = "OpenTorch", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public ToggleButton Button { get; set; }

        public Torch Torch { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            Button = FindViewById<ToggleButton>(Resource.Id.toggleButton1);

            Torch = new Torch(this);

            if (savedInstanceState != null)
            {
                RestoreState(savedInstanceState);
            }

            Button.Click += (sender, e) => { Torch.Toggle(); };
        }

        protected override void OnResume()
        {
            base.OnResume();

            try
            {
                Torch.Start();
            }
            catch (Exception e)
            {
                var toast = Toast.MakeText(this, $"{e.Message}", ToastLength.Short);
                toast.Show();
                Button.Enabled = false;
            }
            
        }

        private void RestoreState(Bundle state)
        {
            Torch.TorchOn = state.GetBoolean("torch_state");
            Button.Checked = Torch.TorchOn;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean("torch_state", Torch.TorchOn);
            base.OnSaveInstanceState(outState);
        }
    }
}

