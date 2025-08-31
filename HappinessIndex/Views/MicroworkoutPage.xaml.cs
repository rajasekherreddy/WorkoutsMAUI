using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Views
{
    public partial class MicroworkoutPage : ContentPage
    {
        public MicroworkoutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppSettings.isMind = false;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            //await App.GetShell().GoToAsync("//dayscore");
        }

        //async void Reminder_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
        //    {
        //        Preferences.Set(AppSettings.PreferredNotificationTimeKey, Reminder.Time.ToString());

        //        if (previousPreferredTime != Reminder.Time)
        //        {
        //            await App.RegisterNotification(Preferences.Get(AppSettings.NameKey, string.Empty), Reminder.Time, true);
        //            previousPreferredTime = Reminder.Time;
        //        }
        //    }
        //}
    }


    class TextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return Color.Black;
            }
            else
            {
                return Color.DarkGray;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
