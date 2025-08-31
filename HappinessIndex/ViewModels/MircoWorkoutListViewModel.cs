using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using HappinessIndex.Models;
using HappinessIndex.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace HappinessIndex.ViewModels
{
    public class MircoWorkoutListViewModel : ViewModelBase, INotifyPropertyChanged
    {
        bool appeared;

        private string editDate="good";
        public string EditDate
        {
            get => editDate;
            set
            {
                if (editDate == value) return;
                editDate = value;

                NotifyPropertyChanged();
            }
        }

        public User User { get; set; }
        private List<MicroWorkout> microWorkoutList;

        public List<MicroWorkout> MicroWorkoutList
        {
            get => microWorkoutList;
            set
            {
                if (microWorkoutList == value) return;
                microWorkoutList = value;

                NotifyPropertyChanged();
            }
        }

        public Command PlayInYoutubeCommand { get; set; }

        public Command PlayTimerCommand { get; set; }

        public Command StartTimerCommand { get; set; }

        public Command PauseTimerCommand { get; set; }

        public Command CloseTimerCommand { get; set; }

        public Command FavCommand { get; set; }

        public Command ShareCommand { get; set; }

        public MircoWorkoutListViewModel()
        {
            isFromYoutube = false;
            timerInProgess = false;
            PlayInYoutubeCommand = new Command(PlayInYoutubeHanlder);
            PlayTimerCommand = new Command(PlayTimerHanlder);
            SaveHighlightsCommand = new Command(SaveHighlights);
            StartTimerCommand = new Command(StartTimerHanlder);
            PauseTimerCommand = new Command(StopTimerHanlder);
            CloseTimerCommand = new Command(CloseTimerHanlder);
            FavCommand = new Command(FavCommandHandler);
            ShareCommand = new Command(ShareCommandHandler);
        }

        bool isPreviousDayAndHasRecords;bool isFromYoutube = false;
        protected async override void OnAppearing()
        {
            try
            {

                isFromPlayTimerHanlder = false;
                if (!isFromYoutube)
                {
                    IsBusy = true;
                    appeared = false;
                    EditDate = string.Empty;
                    //isPreviousDayAndHasRecords = false;

                    //if (User == null)
                    {
                        User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
                    }


                    //loadSelectedList();

                    checkNavigationAndGetWorkouts();

                    appeared = true;
                    IsBusy = false;

                }
            }
            catch (Exception)
            {

            }
        }

        public async void checkNavigationAndGetWorkouts()
        {
            var route = Shell.Current.CurrentState.Location.OriginalString;
            if (route.Contains("workout"))
            {
                MicroWorkoutList = await GetDefaultMicroWorkoutFromFirebase(User);
            }
            else
            {
                MicroWorkoutList = await GetUserFavourites();
            }
        }

        public async Task<List<MicroWorkout>> GetUserFavourites()
        {
            IsBusy = true;
            List<MicroWorkout>  MicroWorkoutList = (await CloudService.GetUserFavourites(User)).ToList();

            //if (AppSettings.isMind)
            //    MicroWorkoutList = MicroWorkoutList.Where(x => x.IsMind).ToList();
            //else
            //    MicroWorkoutList = MicroWorkoutList.Where(x => !x.IsMind).ToList();

            String dateId = AppSettings.JournalDate.ToString().Replace("/", "_");
            List<MicroWorkout> DailyWorkouts = (await CloudService.GetDailyorkouts(dateId, User)).ToList();

            foreach (var favWorkout in MicroWorkoutList)
            {
                favWorkout.IsLiked = true;
                foreach (var dailyWorkout in DailyWorkouts)
                {
                    if (favWorkout.ID == dailyWorkout.ID)
                    {
                        favWorkout.IsPlayed = true;
                    }
                }
            }


            IsBusy = false;

            return MicroWorkoutList;
        }


        public async Task<List<MicroWorkout>> GetDefaultMicroWorkoutFromFirebase(User user)
        {
            IsBusy = true;

            List<MicroWorkout> MicroWorkoutList = (await CloudService.GetDefaultWorkouts(user)).ToList();


            //Get the date and check user performed workout for the day and mark as completed.
            String dateId = AppSettings.JournalDate.ToString().Replace("/", "_");
            List<MicroWorkout> DailyWorkouts = (await CloudService.GetDailyorkouts(dateId, User)).ToList();

            foreach (var favWorkout in MicroWorkoutList)
            {
                foreach (var dailyWorkout in DailyWorkouts)
                {
                    if (favWorkout.ID == dailyWorkout.ID)
                    {
                        favWorkout.IsPlayed = true;
                    }
                }
            }

            //Get the favourites workout for the day and mark as favourite.
            List<MicroWorkout> FavouriteWorkouts = (await CloudService.GetUserFavourites(User)).ToList();

            foreach (var allWorkout in MicroWorkoutList)
            {
                foreach (var favWorkout in FavouriteWorkouts)
                {
                    if (allWorkout.ID == favWorkout.ID)
                    {
                        allWorkout.IsLiked = true;
                    }
                }
            }

            IsBusy = false;

            return MicroWorkoutList;
        }

        //public async void loadSelectedList()
        //{
        //    IsBusy = true;
        //    MicroWorkoutList = await DataService.GetAllMicroWorkout(User);

        //    if (AppSettings.isMind)
        //        MicroWorkoutList = MicroWorkoutList.Where(x =>  x.IsMind).ToList();
        //    else
        //        MicroWorkoutList = MicroWorkoutList.Where(x =>  !x.IsMind).ToList();

        //    IsBusy = false;


        //}

        private async void ShareCommandHandler(object parameter)
        {
            MicroWorkout microWorkout = MicroWorkoutList.Where(x => x.ID == (string)parameter).FirstOrDefault();
            await Share.RequestAsync(new ShareTextRequest
            {

                Text = "I completed my "+ microWorkout.Name + " workout!, You can find workouts here - https://play.google.com/store/apps/details?id=com.sanhabits.app  " +
                "/n Checkout the video -"+microWorkout.YoutubeLink,
                Title = "Share Workout Completion"
            });
        }


        private async void FavCommandHandler(object parameter)
        {
            IsBusy = true;
            MicroWorkout microWorkout = MicroWorkoutList.Where(x => x.ID == (string)parameter).FirstOrDefault();
            //List<MicroWorkout> microWorkouts = MicroWorkoutList.Where(x => x.ID == (string)parameter).ToList();
            if(microWorkout.IsLiked)
            {
                microWorkout.IsLiked = false;
            }
            else
            {
                microWorkout.IsLiked = true;
            }
            await CloudService.CreateOrUpdateFavourites(User, microWorkout);


            IsBusy = false;
        }


        private async void PlayInYoutubeHanlder(object Id)
        {
            try
            {

                isFromYoutube = true;
                var microWorkout = MicroWorkoutList.Where(x => x.ID == (string)Id).FirstOrDefault();
                // await Launcher.OpenAsync(new Uri("https://www.youtube.com/watch?v=pLlI2N9z-cQ"));
                if (!string.IsNullOrEmpty(microWorkout.YoutubeLink))
                {
                    await Browser.OpenAsync(microWorkout.YoutubeLink, BrowserLaunchMode.SystemPreferred);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "No video to play", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }


        System.Timers.Timer timer;
        int DurationMin = 0, DurationSec = 30;
        int breakMins = 0, breakSecs = 10;
        bool timerInProgess = false;
        bool isWorkout = true;
        bool isPause = false;
        bool isFromPlayTimerHanlder;

        private bool isWorkoutNotStarted=true;
        public bool IsWorkoutNotStarted
        {
            get => isWorkoutNotStarted;
            set
            {
                if (isWorkoutNotStarted == value) return;
                isWorkoutNotStarted = value;

                NotifyPropertyChanged();
            }
        }

        private MicroWorkout microWorkout;
        public MicroWorkout MicroWorkout
        {
            get => microWorkout;
            set
            {
                if (microWorkout == value) return;
                microWorkout = value;

                NotifyPropertyChanged();
            }
        }

      

        private async void PlayTimerHanlder(object Id)
        {
            //isFromPlayTimerHanlder = true;
            //if (timerInProgess)
            //{
            //    await Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
            //    return;
            //}

            //microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
            //mins = microWorkout.WorkoutDurationMin;
            //secs = microWorkout.WorkoutDurationSec;
            //timer = new System.Timers.Timer();
            //timer.Interval = 1000; // 1 sec  
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();

            //isFromYoutube = true;

            //if (!string.IsNullOrEmpty(MicroWorkout.YoutubeLink))
            //{
            //    await Browser.OpenAsync(MicroWorkout.YoutubeLink, BrowserLaunchMode.SystemPreferred);
            //}



            try
            {

                microWorkout = MicroWorkoutList.Where(x => x.ID == (string)Id).FirstOrDefault();
                var popup = new MicroWorkoutTimerPage(microWorkout);

                popup.TaskCompleted += (sender, arg) =>
                {
                    Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
                };

                await PopupNavigation.Instance.PushAsync(popup);
            }
            catch (Exception ex)
            {

            }

        }


        private async void StartTimerHanlder(object Id)
        {
            // microWorkout.YoutubeLink = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4";

            try
            {

                if (isPause)
                {
                    microWorkout.IsPaused = false;
                    isPause = false;
                    timer.Start();
                }
                else
                {
                    IsWorkoutNotStarted = false;
                    //getYotubeStream();
                    microWorkout.IsPaused = false;
                    isFromPlayTimerHanlder = true;
                    if (timerInProgess)
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Workout In-Progress.", "OK");
                        return;
                    }

                    //microWorkout = SelectedFactors.Where(x => x.ID == (int)Id).FirstOrDefault();
                    DurationMin = microWorkout.WorkoutDurationMin;
                    DurationSec = microWorkout.WorkoutDurationSec;
                    breakMins = microWorkout.BreakDurationMin;
                    breakSecs = microWorkout.BreakDurationSec;
                    timer = new System.Timers.Timer();
                    timer.Interval = 1000; // 1 sec  
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal async Task getYotubeStream()
        {
            try
            {
                var youtube = new YoutubeClient();

                // You can specify either video ID or URL
                var video = await youtube.Videos.GetAsync(microWorkout.YoutubeLink);

                var title = video.Title; // "Collections - Blender 2.80 Fundamentals"
                var author = video.Author.ChannelTitle; // "Blender"
                var duration = video.Duration; // 00:07:20

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(microWorkout.YoutubeLink);
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestBitrate();


                microWorkout.YoutubeExtracted = streamInfo.Url;
            }
            catch(Exception e)
            {
                try
                {
                    microWorkout.YoutubeExtracted = microWorkout.YoutubeLink;
                }
                catch(Exception ex){}

            }

        }


        private async void StopTimerHanlder(object Id)
        {
            try
            {

                if (timer != null)
                {
                    if (microWorkout.IsPaused)
                    {
                        isPause = false;
                        microWorkout.IsPaused = false;
                        timer.Start();
                    }
                    else
                    {
                        microWorkout.IsPaused = true;

                        isPause = true;
                        timer.Stop();
                    }

                }
            }
            catch (Exception ex)
            {

            }
            
        }

        private async void CloseTimerHanlder(object Id)
        {

            

            if (timer != null)
            {
                microWorkout.NoOfSetsCompleted = 1;
                microWorkout.IsPaused = true;
                microWorkout.WorkoutDurationMin = DurationMin;
                microWorkout.WorkoutDurationSec = DurationSec;
                microWorkout.BreakDurationMin = breakMins;
                microWorkout.BreakDurationSec = breakSecs;
                timer.Stop();
            }

            await PopupNavigation.Instance.PopAllAsync();

        }



        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerInProgess = true;
            microWorkout.WorkoutDurationSec--;

            if (microWorkout.WorkoutDurationSec == 0)
            {
                if (microWorkout.WorkoutDurationMin == 0)
                {

                    if (isWorkout && microWorkout.NoOfSets>1 && microWorkout.NoOfSetsCompleted!=microWorkout.NoOfSets)
                    {
                        isWorkout = false;
                        microWorkout.IsBreakTime = true;
                        microWorkout.IsWorkoutTime = false ;
                        microWorkout.WorkoutDurationMin = breakMins;
                        microWorkout.WorkoutDurationSec = breakSecs;
                    }
                    else
                    {
                        isWorkout = true;
                        microWorkout.IsBreakTime = false;
                        microWorkout.IsWorkoutTime = true;
                        microWorkout.WorkoutDurationMin = DurationMin;
                        microWorkout.WorkoutDurationSec = DurationSec;
                        if (microWorkout.NoOfSets <= microWorkout.NoOfSetsCompleted)
                        {
                            //Make it 1 once sets completed.
                            microWorkout.NoOfSetsCompleted = 1;
                            microWorkout.IsPaused = true;
                            timer.Stop();
                            //show alert
                            timerInProgess = false;

                            microWorkout.IsPlayed = true;


                            //  await DataService.UpdateMicroWoroutAsync(microWorkout);
                            microWorkout.WorkoutDate = AppSettings.JournalDate;
                            User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
                            await CloudService.CreateDailyWorkouts(User, microWorkout);


                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //PopupNavigation.Instance.PopAllAsync();
                                Application.Current.MainPage.DisplayAlert("", "Duration completed", "OK");
                            });
                            
                            await PopupNavigation.Instance.PopAllAsync();

                        }
                        else
                        {
                            microWorkout.NoOfSetsCompleted++;
                        }
                    }


                }
                else if (microWorkout.WorkoutDurationMin > 0)
                {
                    microWorkout.WorkoutDurationMin--;
                    microWorkout.WorkoutDurationSec = 60;
                }
            }
        }


        private bool showWorkoutList = true;
        public bool ShowWorkoutList
        {
            get => showWorkoutList;
            set
            {
                showWorkoutList = value;
                NotifyPropertyChanged();
            }
        }

        private bool showExitingView = false;
        public bool ShowExitingView
        {
            get => showExitingView;
            set
            {
                showExitingView = value;
                NotifyPropertyChanged();
            }
        }


        public Command SegmentChange;

        #region segment 2
        public Command SaveHighlightsCommand { get; set; }

        private Highlights highlights;

        public Highlights Highlights
        {
            get => highlights;
            set
            {
                if (highlights == value) return;
                highlights = value;
                NotifyPropertyChanged();
            }
        }

        private async void SaveHighlights()
        {
            IsBusy = true;
            await DataService.UpdateHighlightsAsync(Highlights);
            IsBusy = false;

            await PopupNavigation.Instance.PushAsync(new Views.Popup.CommonMessage("", Resx.AppResources.SavedSuccessfully, Resx.AppResources.Ok));
        }

        #endregion


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //going out from the page
            if (!isFromPlayTimerHanlder && timer != null)
            {
                timerInProgess = false;
                isFromYoutube = false;
                //isFromPlayTimerHanlder = false;
                timer.Stop();
                timer.Elapsed -= Timer_Elapsed;
                timer = null;
                //if (MicroWorkout != null)
                //{
                //    MicroWorkout.WorkoutDurationMin = mins;
                //    MicroWorkout.WorkoutDurationSec = secs;
                //    MicroWorkout.IsPlayed = false;
                //}
            }

        }
    }
}

