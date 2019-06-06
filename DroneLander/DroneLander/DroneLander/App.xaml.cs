using System;
using System.Diagnostics;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Startup;
using DroneLander.ViewModels;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DroneLander
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : CafeApplication
    {
        private readonly IServiceRegistry _registry;

        public App(IServiceRegistry registry)
            : base(registry)
        {
            _registry = registry;
            InitializeComponent();
        }

        /// <summary>
        /// Configure the application.
        /// </summary>
        public override void Configure()
        {
            Registry.AddScoped<MainViewModel, MainViewModel>();
        }

        protected override void OnStart()
        {
            try
            {
                // Initialize the root page.
                MainPage = _registry.GetResolver().Resolve<MainViewModel>().AsNavigationPage();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
