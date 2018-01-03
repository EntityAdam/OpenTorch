using Android.App;
using Android.Widget;
using Android.OS;

namespace OpenTorch
{
    [Activity(Label = "OpenTorch", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Main);
            ToggleButton button = this.FindViewById<ToggleButton>(Resource.Id.toggleButton1);
            var torch = new Torch(this);
            torch.Start();
            button.Click += (sender, e) => { torch.Toggle(); };
        }
    }
}

