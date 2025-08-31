using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HappinessIndex.ViewModels;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;

namespace HappinessIndex.Views
{	
	public partial class MircoWorkoutList : ContentPage
	{	
		public MircoWorkoutList()
		{
			InitializeComponent ();
		}

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            
        }

        void selector_SelectionChanged(System.Object sender, Syncfusion.XForms.Buttons.SelectionChangedEventArgs e)
        {
            var viewModel = (MircoWorkoutListViewModel)this.BindingContext;

            if ( e.Index == 0)
            {
              AppSettings.isMind = false;
              //viewModel.SelectedFactors = viewModel.SelectedFactors.Where(x => x.IsSelected && !x.IsMind).ToList();

            }
            else
            {
               AppSettings.isMind = true;
               // viewModel.SelectedFactors = viewModel.SelectedFactors.Where(x => x.IsSelected && x.IsMind).ToList();

            }
            viewModel.checkNavigationAndGetWorkouts();

        }

        

        protected override void OnAppearing()
        {
            var dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(AppSettings.JournalDate.DayOfWeek);

            

            base.OnAppearing();

            Title = "Micro workouts".ToUpper();

            //if (route.Contains("workout"))
            //{
            //    Title = "Micro workouts".ToUpper();
            //}
            //else if (route.Contains("favouritepage"))
            //{
            //    Title = $"{dayName}, {AppSettings.JournalDate:MMMM dd yyyy}".ToUpper();

            //}
        }

    }

    

    public class BoolToStringConverterMicroWorkout : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "completed";
            }
            else
            {
                return "youtube";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToStringConverterFavouriteMicroWorkout : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "favgreen";
            }
            else
            {
                return "favgrey";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

