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
            this.SetContentView(Resource.Layout.Main);

            this.Button = this.FindViewById<ToggleButton>(Resource.Id.toggleButton1);
            this.Torch = new Torch(this);
            this.Button.Click += (sender, e) => { this.Torch.Toggle(); };

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
                this.Button.Enabled = false;
            }
            
        }
    }
}

