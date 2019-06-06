using Android.App;
using Android.Content.PM;
using Android.OS;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Startup;
using DroneLander.Droid.Services;
using DroneLander.Services;

namespace DroneLander.Droid
{
    [Activity(Label = "DroneLander", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ICafeStartup
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new CafeStartup<App>(this).Configure());
        }

        public void Configure(IServiceRegistry registry)
        {
            registry.AddSingleton<IAudioService, AudioService>();
        }
    }
}