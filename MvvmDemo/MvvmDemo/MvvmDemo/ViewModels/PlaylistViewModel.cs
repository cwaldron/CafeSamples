using System.Collections.ObjectModel;
using System.Windows.Input;
using CafeLib.Core.IoC;
using CafeLib.Mobile.ViewModels;
using Xamarin.Forms;

namespace MvvmDemo.ViewModels
{
    public class PlaylistViewModel : BaseViewModel<string>
    {
        #region Constructor

        /// <summary>
        /// Constructor of the list of play list items view model.
        /// </summary>
        public PlaylistViewModel(IServiceResolver resolver)
            :base(resolver)
        {
            Playlist = new ObservableCollection<PlaylistDetailViewModel>();
            AddPlaylistCommand = new Command(AddPlaylist);
            Title = $"{Playlist.Count} Playlist";
        }

        #endregion

        #region Properties

        private ObservableCollection<PlaylistDetailViewModel> _playlist;
        public ObservableCollection<PlaylistDetailViewModel> Playlist
        {
            get => _playlist;
            set => SetValue(ref _playlist, value);
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
            var newPlaylist = "Playlist " + (Playlist.Count + 1);
            var vm = Resolver.Resolve<PlaylistDetailViewModel>();
            vm.Title = newPlaylist;
            Playlist.Add(vm);
            Title = $"{Playlist.Count} Playlist";
        }

        #endregion
    }
}
