using System.Windows.Input;
using CafeLib.Core.IoC;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

namespace MvvmDemo.ViewModels
{
    public class EnterUserViewModel : BaseViewModel
    {
        public EnterUserViewModel(IServiceResolver resolver)
            : base(resolver)
        {
            NavigateToPlaylistPageCommand = new Command(NavigateToPlaylist);
        }

        public bool CanChangePage => !string.IsNullOrEmpty(UserName);

        private string _userNameCode = string.Empty;
        public string UserName
        {
            get => _userNameCode;
            set
            {
                if (SetValue(ref _userNameCode, value))
                {
                    OnPropertyChanged(nameof(CanChangePage));
                }
            }
        }

        //private ICommand _navigateToPlaylistPageCommand;
        //public ICommand NavigateToPlaylistPageCommand => _navigateToPlaylistPageCommand ?? (_navigateToPlaylistPageCommand = new Command(NavigateToPlaylist));

        private ICommand _navigateToPlaylistPageCommand;
        public ICommand NavigateToPlaylistPageCommand
        {
            get => _navigateToPlaylistPageCommand;
            set => SetValue(ref _navigateToPlaylistPageCommand, value);
        }


        private void NavigateToPlaylist()
        {
            if (!CanChangePage) return;
            NavigationService.Navigate<PlaylistViewModel, string>(UserName);
        }
    }
}
