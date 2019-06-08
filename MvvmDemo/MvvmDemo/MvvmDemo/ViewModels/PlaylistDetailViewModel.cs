using CafeLib.Core.IoC;
using CafeLib.Mobile.ViewModels;
using MvvmDemo.Views;
using Xamarin.Forms;

namespace MvvmDemo.ViewModels
{
    public class PlaylistDetailViewModel : BaseViewModel
    {
        public PlaylistDetailViewModel(IServiceResolver resolver)
            :base(resolver)
        {
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                SetValue(ref _isFavorite, value);

                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Color));
            }
        }

        public Color Color => IsFavorite ? Color.Pink : Color.Black;
    }
}
