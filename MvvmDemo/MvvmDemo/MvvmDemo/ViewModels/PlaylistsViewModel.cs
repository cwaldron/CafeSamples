using System.Collections.ObjectModel;
using System.Windows.Input;
using CafeLib.Core.IoC;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

namespace MvvmDemo.ViewModels
{
    public class PlaylistsViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor of the list of play list items view model.
        /// </summary>
        public PlaylistsViewModel(IServiceResolver resolver)
            :base(resolver)
        {
            Playlists = new ObservableCollection<PlaylistDetailViewModel>();
            AddPlaylistCommand = new Command(AddPlaylist);
            Title = $"{Playlists.Count} Playlists";
        }

        #endregion

        #region Properties

        private ObservableCollection<PlaylistDetailViewModel> _playlists;
        public ObservableCollection<PlaylistDetailViewModel> Playlists
        {
            get => _playlists;
            set => SetValue(ref _playlists, value);
        }

        #endregion

        #region Commands

        private ICommand _addPlaylistCommand;
        public ICommand AddPlaylistCommand
        {
            get => _addPlaylistCommand;
            set => SetValue(ref _addPlaylistCommand, value);
        }

        #endregion

        #region Properties

        private PlaylistDetailViewModel _selectedPlaylist;
        public PlaylistDetailViewModel SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                if (!SetValue(ref _selectedPlaylist, value)) return;
                _selectedPlaylist.IsFavorite = !_selectedPlaylist.IsFavorite;
                NavigationService.PushAsync(_selectedPlaylist);
                SetValue(ref _selectedPlaylist, null);
            }
        }

        #endregion

        #region Command Handlers

        private void AddPlaylist()
        {
            var newPlaylist = "Playlist " + (Playlists.Count + 1);
            var vm = Resolver.Resolve<PlaylistDetailViewModel>();
            vm.Title = newPlaylist;
            Playlists.Add(vm);
            Title = $"{Playlists.Count} Playlists";
        }

        #endregion
    }
}
