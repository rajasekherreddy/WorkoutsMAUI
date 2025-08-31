using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildHappiness.Core.Models;
using Firebase.Auth;
using HappinessIndex.DependencyService;
using HappinessIndex.Models;
using HappinessIndex.Resx;
using HappinessIndex.Views.Popup;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using User = HappinessIndex.Models.User;

namespace HappinessIndex.ViewModels
{
    public class MicroWorkoutPageViewModel : ViewModelBase
    {
        private string editDate;


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

        public Command SaveCommand { get; set; }

        public Command AddFactorCommand { get; set; }
        public Command AddMindFactorCommand { get; set; }

        public Command ValidateFactorsCommand { get; set; }
        public Command ShareCommand { get; set; }
        

        public bool HasInvalidName
        {
            get => hasInvalidName;
            set
            {
                if (hasInvalidName == value) return;
                hasInvalidName = value;
                NotifyPropertyChanged();
            }
        }

        public User User { get; set; }

        public MicroWorkout AddNewMicroWorkout { get; set; }

        private List<MicroWorkout> microWorkoutList;

        public List<MicroWorkout> MicroWorkoutList
        {
            get => microWorkoutList;
            set
            {
               // if (microWorkoutList == value) return;
                microWorkoutList = value;
                NotifyPropertyChanged();
            }
        }

        private List<int> durationPickerMinutes;

        public List<int> DurationPickerMinutes
        {
            get
            {
                durationPickerMinutes = new List<int>();
                for (int i = 0; i <= 60; i++)
                    durationPickerMinutes.Add(i);
                return durationPickerMinutes;
            }

        }

        public int DurationPickerMinutesSelected { get; set; }


        private List<int> durationPickerSeconds;

        public List<int> DurationPickerSeconds
        {
            get
            {
                durationPickerSeconds = new List<int>();
                for (int i = 0; i <= 60; i++)
                    durationPickerSeconds.Add(i);
                return durationPickerSeconds;
            }

        }

        public int DurationPickerSecondsSelected { get; set; }

        public TimeSpan SelectedReminderTime { get; set; }

        public Command EditCommand { get; set; }

        public Command DateSelectionCommand { get; set; }

        public Command RemoveRemainderCommand { get; set; }

        public MicroWorkoutPageViewModel()
        {
           // AddNewMicroWorkout = new MicroWorkout();
            SaveCommand = new Command(SaveFactors);
            ValidateFactorsCommand = new Command(ValidateFactors);
            AddFactorCommand = new Command(AddFactor);
            AddMindFactorCommand = new Command(AddMindFactor);
            HomeCommand = new Command(HomeFactor);

            EditCommand = new Command(EditCommandHandler);
            //ShareCommand = new Command(ShareCommandHandler);
            DateSelectionCommand = new Command(DateSelectionHandler);
            RemoveRemainderCommand = new Command(RemoveRemainderHandlerAsync);


        }


        private async void DateSelectionHandler(object Id)
        {
            MicroWorkout workoutItem = MicroWorkoutList.Where(x => x.ID == (string)Id).FirstOrDefault();

            if (workoutItem.ReminderEnabled == 0)
            {
                workoutItem.IsReminder1 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 1)
            {
                workoutItem.IsReminder2 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 2)
            {
                workoutItem.IsReminder3 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 3)
            {
                workoutItem.IsReminder4 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 4)
            {
                workoutItem.IsReminder5 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 5)
            {
                workoutItem.IsReminder6 = true;
                ++workoutItem.ReminderEnabled;
            }
            else if (workoutItem.ReminderEnabled == 6)
            {
                await Application.Current.MainPage.DisplayAlert("", "Max 6 remainders can set.", "OK");
            }

            
        }

        private async void RemoveRemainderHandlerAsync(object Id)
        {
            try
            {
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder1 = false;
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder2 = false;
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder3 = false;
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder4 = false;
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder5 = false;
                //MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).IsReminder6 = false;


                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 1).ID + "" + 1));
                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 2).ID + "" + 1));
                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 3).ID + "" + 1));
                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 4).ID + "" + 1));
                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 5).ID + "" + 1));
                //App.CancelNotificationWithId(Int16.Parse(MicroWorkoutList.ElementAt(Int16.Parse(Id + "") - 6).ID + "" + 1));




                MicroWorkout workoutItem = MicroWorkoutList.Where(x => x.ID == (string)Id).FirstOrDefault();

                if (workoutItem.ReminderEnabled == 0)
                {
                    
                    await Application.Current.MainPage.DisplayAlert("", "No remainders set.", "OK");

                }
                else if (workoutItem.ReminderEnabled == 1)
                {
                    workoutItem.IsReminder1 = false;
                    --workoutItem.ReminderEnabled;
                }
                else if (workoutItem.ReminderEnabled == 2)
                {
                    workoutItem.IsReminder2 = false;
                    --workoutItem.ReminderEnabled;
                }
                else if (workoutItem.ReminderEnabled == 3)
                {
                    workoutItem.IsReminder3 = false;
                    --workoutItem.ReminderEnabled;
                }
                else if (workoutItem.ReminderEnabled == 4)
                {
                    workoutItem.IsReminder4 = false;
                    --workoutItem.ReminderEnabled;
                }
                else if (workoutItem.ReminderEnabled == 5)
                {
                    workoutItem.IsReminder5 = false;
                    --workoutItem.ReminderEnabled;
                }
                else if (workoutItem.ReminderEnabled == 6)
                {
                    workoutItem.IsReminder6 = false;
                    --workoutItem.ReminderEnabled;
                }

            }
            catch (Exception e)
            {

            }
            

           
        }

        public Command HomeCommand { get; set; }
        private async void HomeFactor(object parameter)
        {
            try
            {
                await App.GetShell().GoToAsync("//main");
            }
            catch (Exception ex)
            {

            }
        }

        bool isPreviousDayAndHasRecords;

        protected async override void OnAppearing()
        {
            IsBusy = true;
            appeared = false;
            EditDate = string.Empty;
            isPreviousDayAndHasRecords = false;

            //if (User == null)
            {
                User = await DataService.GetUserAsync(Preferences.Get(AppSettings.EmailKey, ""));
            }

            MicroWorkoutList = await GetDefaultMicroWorkoutFromFirebase(User);

          

            appeared = true;
            IsBusy = false;
        }

        public async Task<List<MicroWorkout>> GetDefaultMicroWorkoutFromFirebase(User user)
        {
            IsBusy = true;

            MicroWorkoutList = (await CloudService.GetDefaultWorkouts(user)).ToList();

           

            if (AppSettings.isMind)
                MicroWorkoutList = MicroWorkoutList.Where(x =>  x.IsMind).ToList();
            else
                MicroWorkoutList = MicroWorkoutList.Where(x => !x.IsMind).ToList();

            IsBusy = false;

            return MicroWorkoutList;
        }

        private async void EditCommandHandler(object parameter)
        {
           MicroWorkout workoutItem = MicroWorkoutList.Where(x => x.ID == (string)parameter).FirstOrDefault();
           workoutItem.AllowEdit = true;
           workoutItem.IsUpdated = true;
        }

        bool appeared;
        private bool hasInvalidName;

        private async void ValidateFactors(object Id)
        {
            if (appeared)
            {
                var workoutItem = MicroWorkoutList.Where(x => x.ID == (string)Id).FirstOrDefault();
                if (MicroWorkoutList.Where(x => x.IsSelected).ToList().Count > 7)
                {
                    await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.PleaseSelectAMaximumOf6Factors, AppResources.Ok));
                    workoutItem.IsSelected = false;
                    return;
                }

                workoutItem.AllowEdit = true;
                workoutItem.IsUpdated = true;

            }
        }

        private async void AddFactor(object parameter)
        {
            try
            {
                if (parameter == null)
                {
                    AddNewMicroWorkout = new MicroWorkout();
                    //CustomFactor = new MicroWorkout();
                    await PopupNavigation.Instance.PushAsync(new AddFactorMicroWorkout() { BindingContext = this });
                }
                else
                {
                    if(String.IsNullOrEmpty(AddNewMicroWorkout.Name))
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Workout name is required.", "OK");
                        return;
                    }
                    int customWorkoutCount =  MicroWorkoutList.Where(x => x.IsCustomWorkout).ToList().Count;
                    AddNewMicroWorkout.IsCustomWorkout = true;

                    AddNewMicroWorkout.WorkoutIcon =(customWorkoutCount<11)? string.Format("CW{0}", ++customWorkoutCount): "CW1";
                        await DataService.AddMicroWoroutAsync(AddNewMicroWorkout);
                        await PopupNavigation.Instance.PopAllAsync();
                        //await DataService.UpdateUserAsync(User);
                        Xamarin.Forms.DependencyService.Get<IToast>().Show(AppResources.AddedSuccessfully, 1.5);
                        OnAppearing();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private async void AddMindFactor(object parameter)
        {
            try
            {
                if (parameter == null)
                {
                    AddNewMicroWorkout = new MicroWorkout();
                    //CustomFactor = new MicroWorkout();
                    await PopupNavigation.Instance.PushAsync(new AddMindFactorMicroWorkout() { BindingContext = this });
                }
                else
                {
                    if (String.IsNullOrEmpty(AddNewMicroWorkout.Name))
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Workout name is required.", "OK");
                        return;
                    }
                    int customWorkoutCount = MicroWorkoutList.Where(x => x.IsCustomWorkout).ToList().Count;
                    AddNewMicroWorkout.IsCustomWorkout = true;
                    AddNewMicroWorkout.IsMind = true;

                    AddNewMicroWorkout.WorkoutIcon = (customWorkoutCount < 11) ? string.Format("CW{0}", ++customWorkoutCount) : "CW1";
                    await DataService.AddMicroWoroutAsync(AddNewMicroWorkout);
                    await PopupNavigation.Instance.PopAllAsync();
                    //await DataService.UpdateUserAsync(User);
                    Xamarin.Forms.DependencyService.Get<IToast>().Show(AppResources.AddedSuccessfully, 1.5);
                    OnAppearing();
                }
            }
            catch (Exception ex)
            {

            }
        }



        private async void SaveFactors()
        {
            //TODO: Refactor this code.
            IsBusy = true;

            List<MicroWorkout> microWorkouts = MicroWorkoutList.Where(x => x.IsUpdated).ToList();

            setAsFavouriteRemaindersAsync(microWorkouts);

            

            //Previous days

            #region commented code
            //if (isPreviousDayAndHasRecords)
            //{
            //    await ValidateAndDeletePreviousRecords();
            //}
            //else
            //{
            //    canUpdateDB = true;
            //}

            //if (canUpdateDB)
            //{
            //    await ValidateAndDeletePreviousRecords();

            //    if (canUpdateDB)
            //    {
            //        User.SelectedFactors = "";
            //        foreach (var factor in MicroWorkout)
            //        {
            //            if (factor.IsSelected)
            //            {
            //                User.SelectedFactors += factor.ID + ",";
            //            }


            //        }

            //        var result = await DataService.UpdateUserAsync(User);

            //        if (result == 1)
            //        {
            //            //await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.FactorChangeWarning, AppResources.Ok));
            //            await App.GetShell().GoToAsync("//favouritepage");
            //        }
            //    }
            //}

            #endregion


        }

        private async void setAsFavouriteRemaindersAsync(List<MicroWorkout> microWorkouts)
        {
            try
            {
            

            
            for(int i = 0; i < microWorkouts.Count; i++)
            {
                MicroWorkout microWorkout = microWorkouts.ElementAt(i);
                if (microWorkout.IsSelected)
                {
                    string content = AppResources.ItsTimeforYourWorkout + microWorkout.Name;

                    if (microWorkout.IsReminder1)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder1,
                            Int16.Parse(microWorkout.ID + "" + 1));
                    }
                    if (microWorkout.IsReminder2)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder2,
                            Int16.Parse(microWorkout.ID + "" + 2));
                    }
                    if (microWorkout.IsReminder3)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder3,
                            Int16.Parse(microWorkout.ID + "" + 3));
                    }
                    if (microWorkout.IsReminder4)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder4,
                            Int16.Parse(microWorkout.ID + "" + 4));
                    }
                    if (microWorkout.IsReminder5)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder5,
                            Int16.Parse(microWorkout.ID + "" + 5));
                    }
                    if (microWorkout.IsReminder6)
                    {
                        await App.RegisterWorkoutNotification(content, microWorkout.WorkoutReminder6,
                            Int16.Parse(microWorkout.ID + "" + 6));
                    }
                }
                else
                {
                    deleteRemainders(microWorkout);
                }
            }

         //   var result = await DataService.UpdateAllMicroWoroutAsync(microWorkouts);
            //if (result == 1)
            //{
            //    //await PopupNavigation.Instance.PushAsync(new CommonMessage("", AppResources.FactorChangeWarning, AppResources.Ok));

            //}


            //Local database
            //Drop and update today workouts
            //await DataService.DropAndUpdateToday(User);

            // From Firebase
         //   await CloudService.CreateOrUpdateFavourites(User, microWorkouts);



            await App.GetShell().GoToAsync("//favouritepage");
            IsBusy = false;

            }
            catch (Exception e)
            {

            }
        }

        

        private async void deleteRemainders(MicroWorkout microWorkout)
        {
            try
            {
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 1));
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 2));
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 3));
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 4));
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 5));
                await App.CancelNotifications(Int16.Parse(microWorkout.ID + "" + 6));
            }
            catch (Exception ex) { }
        }

       
    }
}