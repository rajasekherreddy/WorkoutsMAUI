using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Models;
using HappinessIndex.ViewModels;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Xamarin.CommunityToolkit.UI.Views;

namespace HappinessIndex.Views
{
    
    public partial class MicroWorkoutTimerPage
    {
        public EventHandler<EventArgs> SaveClicked;

        public EventHandler<EventArgs> TaskCompleted;
        private MicroWorkout microWorkout;

        MircoWorkoutListViewModel mircoWorkoutListViewModel;
       
        public MicroWorkoutTimerPage()
        {
            InitializeComponent();
            Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.StorageRead>();
        }

        public MicroWorkoutTimerPage(MicroWorkout microWorkout)
        {
            InitializeComponent();
            this.microWorkout = microWorkout;

            mircoWorkoutListViewModel = new MircoWorkoutListViewModel();
            this.BindingContext = mircoWorkoutListViewModel;


        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        void ClosePopup(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();

        }
        void SaveDetails(System.Object sender, System.EventArgs e)
        {
            CloseAllPopup();
            TaskCompleted?.Invoke(this, new EventArgs());

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            mircoWorkoutListViewModel.MicroWorkout = microWorkout;
            await mircoWorkoutListViewModel.getYotubeStream();

            //mediaSource.Source = GetYouTubeUrl("lQYPLnJtVfw");

            //mediaSource.Pause();
        }


        public string GetYouTubeUrl(string videoId)
        {
            var videoInfoUrl = $"https://www.youtube.com/get_video_info?video_id={videoId}";
            using (var client = new HttpClient())
            {
                var videoPageContent = client.GetStringAsync(videoInfoUrl).Result;
                var videoParameters = HttpUtility.ParseQueryString(videoPageContent);
                var encodedStreamsDelimited1 = WebUtility.HtmlDecode(videoParameters["player_response"]);
                JObject jObject = JObject.Parse(encodedStreamsDelimited1);
                string url = (string)jObject["streamingData"]["formats"][0]["url"];
                return url;
            }
        }

        public void PlayYoutubeVideo(System.Object sender, System.EventArgs e)
        {
            mediaSource.Play();
        }
    }

    public class PauseConverterMicroWorkout : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()))
            {
                return "playButton";
            }
            else
            {
                return "pause";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    
}

