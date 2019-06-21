using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeLib.Mobile.Services;
using CafeLib.Mobile.Views;
using MvvmDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterUserPage : BaseContentPage<EnterUserViewModel>
    {
        public EnterUserPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);

            UserNameEntry.SetBinding(Entry.ReturnCommandProperty, nameof(EnterUserViewModel.NavigateToPlaylistPageCommand));

            NextButton.SetBinding(Button.CommandProperty, nameof(EnterUserViewModel.NavigateToPlaylistPageCommand));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Do this to work around https://bugzilla.xamarin.com/show_bug.cgi?id=44774
            Resolver.Resolve<IDeviceService>().RunOnMainThread(() =>
            {
                ForceLayout();
                UserNameEntry.Focus();
            });
        }

        protected override bool OnBackButtonPressed() => true;
    }
}