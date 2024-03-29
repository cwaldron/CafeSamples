﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeLib.Mobile.Views;
using MvvmDemo.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmDemo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlaylistDetailPage : BaseContentPage<PlaylistDetailViewModel>
	{
		public PlaylistDetailPage()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
	}
}