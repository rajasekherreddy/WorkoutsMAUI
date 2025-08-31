using System;
using System.Linq;
using System.Globalization;
using System.IO;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using HappinessIndex.ViewModels;
using Xamarin.Forms;
using HappinessIndex.Models;
using System.Collections.Generic;
using Syncfusion.SfCarousel.XForms;
using FFImageLoading.Forms;

namespace HappinessIndex.Views
{
    public partial class DashboardPageNew : ContentPage
    {
        public DashboardPageNew()
        {
            InitializeComponent();
            //DateLabel.Text = AppResources.ReadyMessage + DateTime.Now.ToString("MMM-dd");
            if (Device.RuntimePlatform == "iOS")
            {
                //List.EnableVirtualization = true;
            }
        }

        protected override void OnAppearing()
        {
            //AppSettings.JournalDate = DateTime.Now;
            base.OnAppearing();
            mediaSource.Stop();


            //List.SetBinding(SfCarousel.ItemsSourceProperty, "Highlights");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            mediaSource.Stop();
        }

        void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
          //  if (BindingContext is DashboardPageNewViewModel viewModel && viewModel.Highlights != null)
            {
                var selectedDate = e.NewDate.Date;

                AppSettings.JournalDate = selectedDate;

                //  var item = viewModel.Highlights.Where(item => item.Date.Date == selectedDate).FirstOrDefault();
                //  if (item == null) return;

                //  int index = viewModel.Highlights.IndexOf(item);
                //List.SelectedItem = item;
                // List.ScrollTo(index);
            }
        }

        void List_SelectionChanged(object sender, Syncfusion.SfCarousel.XForms.SelectionChangedEventArgs e)
        {
            var highlight = e.SelectedItem as Highlights;

            if (BindingContext is DashboardPageViewModel viewModel && viewModel != null && highlight != null)
            {
                viewModel.SelectedDate = highlight.Date.Date;
            }
        }

        private async void WorkoutClicked(object sender, EventArgs e)
        {
            await App.GetShell().GoToAsync("//workoutmain");
        }
        private async void FavouriteClicked(object sender, EventArgs e)
        {
            await App.GetShell().GoToAsync("//favouritepage");
        }
        private async void EnablersClicked(object sender, EventArgs e)
        {
            await App.GetShell().GoToAsync("//dayscore");
        }
        private async void InhibitorsClicked(object sender, EventArgs e)
        {
            await App.GetShell().GoToAsync("//dashboard");
        }
        private async void JournalClicked(object sender, EventArgs e)
        {
             await App.GetShell().GoToAsync("//myjournal");
            //Application.Current.MainPage = new NotesPage();

        }
        private async void ReportsClicked(object sender, EventArgs e)
        {
            await App.GetShell().GoToAsync("//report");
        }
        private void PlayClicked(object sender, EventArgs e)
        {
            var viewModel = (DashboardPageNewViewModel)this.BindingContext;
            viewModel.IsVideoNotPlaying=false;
            mediaSource.Play();
        }

    }

    public class GreetingUserWithTimeConverterNew : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeHelper.TimeToGreeting(DateTime.Now.Hour) + value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStringConverterNew : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return AppResources.Edit;
            }
            else
            {
                return AppResources.View;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageToBoolConverterNew : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}