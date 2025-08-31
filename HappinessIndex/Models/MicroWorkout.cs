﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HappinessIndex.Helpers;
using HappinessIndex.Resx;
using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using HappinessIndex.ViewModels;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using HappinessIndex.Views.Popup;
using System.Text.Json.Nodes;

namespace HappinessIndex.Models
{
    public class MicroWorkout : INotifyPropertyChanged
    {
        public String ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string YoutubeLink { get; set; }

        private int workoutDurationMin;
        public int WorkoutDurationMin { get => workoutDurationMin; set { workoutDurationMin = value; NotifyPropertyChanged(); } }

        private int workoutDurationSec=30;
        public int WorkoutDurationSec { get => workoutDurationSec; set { workoutDurationSec = value; NotifyPropertyChanged(); } }

        private int breakDurationMin=0;
        public int BreakDurationMin { get => breakDurationMin; set { breakDurationMin = value; NotifyPropertyChanged(); } }

        private int breakDurationSec=10;
        public int BreakDurationSec { get => breakDurationSec; set { breakDurationSec = value; NotifyPropertyChanged(); } }

        private int noOfSets=3;
        public int NoOfSets { get => noOfSets; set { noOfSets = value; NotifyPropertyChanged(); } }

        private int noOfSetsCompleted = 1;
        public int NoOfSetsCompleted { get => noOfSetsCompleted; set { noOfSetsCompleted = value; NotifyPropertyChanged(); } }
        public int NextSet { get => noOfSetsCompleted + 1;}


        public TimeSpan WorkoutReminder1 { get; set; }

        public TimeSpan WorkoutReminder2 { get; set; }

        public TimeSpan WorkoutReminder3 { get; set; }

        public TimeSpan WorkoutReminder4 { get; set; }

        public TimeSpan WorkoutReminder5 { get; set; }

        public TimeSpan WorkoutReminder6 { get; set; }


        private bool isReminder1;
        public bool IsReminder1 { get => isReminder1; set { isReminder1 = value; NotifyPropertyChanged(); } }
        private bool isReminder2;
        public bool IsReminder2 { get => isReminder2; set { isReminder2 = value; NotifyPropertyChanged(); } }
        private bool isReminder3;
        public bool IsReminder3 { get => isReminder3; set { isReminder3 = value; NotifyPropertyChanged(); } }
        private bool isReminder4;
        public bool IsReminder4 { get => isReminder4; set { isReminder4 = value; NotifyPropertyChanged(); } }
        private bool isReminder5;
        public bool IsReminder5 { get => isReminder5; set { isReminder5 = value; NotifyPropertyChanged(); } }
        private bool isReminder6;
        public bool IsReminder6 { get => isReminder6; set { isReminder6 = value; NotifyPropertyChanged(); } }

        private int reminderEnabled = 0;
        public int ReminderEnabled { get => reminderEnabled; set { reminderEnabled = value; NotifyPropertyChanged(); } }

        //private List<TimeSpan> workoutReminders = new List<TimeSpan>();
        //public List<TimeSpan> WorkoutReminders { get => workoutReminders; set { workoutReminders = value; NotifyPropertyChanged(); } }

        private string workoutReminders;
        public string WorkoutReminders { get => workoutReminders; set { workoutReminders = value; NotifyPropertyChanged(); } }

        private bool isSelected;
        public bool IsSelected { get => isSelected; set { isSelected = value;NotifyPropertyChanged(); } }

        private bool isMind;
        public bool IsMind { get => isMind; set { isMind = value; NotifyPropertyChanged(); } }

        public string WorkoutIcon { get; set; }

        private bool isPlayed;
        public bool IsPlayed { get => isPlayed; set { isPlayed = value;NotifyPropertyChanged(); } }

        private bool isLiked;
        public bool IsLiked { get => isLiked; set { isLiked = value; NotifyPropertyChanged(); } }

        public bool IsCustomWorkout { get; set; } = false;

        private bool isWorkoutTime=true;
        public bool IsWorkoutTime { get => isWorkoutTime; set { isWorkoutTime = value; NotifyPropertyChanged(); } }

        private bool isBreakTime=false;
        public bool IsBreakTime { get => isBreakTime; set { isBreakTime = value; NotifyPropertyChanged(); } }

        private bool isPaused = true;
        public bool IsPaused { get => isPaused; set { isPaused = value; NotifyPropertyChanged(); } }

        private String youtubeExtracted ;
        public String YoutubeExtracted { get => youtubeExtracted; set { youtubeExtracted = value; NotifyPropertyChanged(); } }

        //public DateTime WorkoutDate { get; set; } = DateTime.Today;
        public DateTime WorkoutDate { get; set; } = AppSettings.DefaultDate.Date.Date;

        public string Default { get; set; } = "Default";

        [Ignore]
        private bool allowEdit { get; set; } = false;
        [Ignore]
        public bool AllowEdit
        {
            get => allowEdit;
            set
            {
                allowEdit = value;
                NotifyPropertyChanged();
            }
        } 

        [Ignore]
        public bool IsUpdated { get; set; } = false;


        #region commented code


        //private string duration { get; set; } = "00:30";

        //[Ignore]
        //public string Duration
        //{
        //    get => duration;
        //    set
        //    {
        //        duration = value;

        //        NotifyPropertyChanged();
        //    }
        //}

        //private bool durationPlayed { get; set; } = false;

        // [Ignore]
        //public bool DurationPlayed
        //{
        //    get => durationPlayed;
        //    set
        //    {
        //        durationPlayed = value;

        //        NotifyPropertyChanged();
        //    }
        //}

        //private string playedIcon { get; set; } = "play-button";

        //[Ignore]
        //public string PlayedIcon
        //{
        //    get => playedIcon;
        //    set
        //    {
        //        playedIcon = value;

        //        NotifyPropertyChanged();
        //    }
        //}


        //[Ignore]
        //public string DisplayName
        //{
        //    get
        //    {
        //        var localSting = AppResources.ResourceManager.GetString(Name);
        //        if (string.IsNullOrEmpty(localSting))
        //        {
        //            return Name;
        //        }
        //        else
        //        {
        //            return localSting;
        //        }
        //    }
        //}

        //public bool NeedToSave
        //{
        //    get => needToSave;
        //    set
        //    {
        //        needToSave = value;

        //        NotifyPropertyChanged();
        //    }
        //}



        //[Ignore]
        //public string DisplayDescription
        //{
        //    get
        //    {
        //        if (Description == null) return "";

        //        var localSting = AppResources.ResourceManager.GetString(Description);
        //        if (string.IsNullOrEmpty(localSting))
        //        {
        //            return Description;
        //        }
        //        else
        //        {
        //            return localSting;
        //        }
        //    }
        //}

        //public bool IsCustomFactor { get; set; }

        //[Ignore]
        //public bool IsSelected
        //{
        //    get => isSelect;
        //    set
        //    {
        //        if (isSelect != value)
        //        {
        //            isSelect = value;
        //            if (!pauseNotifyPropertyChanged)
        //                NotifyPropertyChanged();
        //        }
        //    }
        //}

        //Color? color = null;

        //[Ignore]
        //public Color Color
        //{
        //    get
        //    {
        //        if (color == null)
        //        {
        //            color = ColorHelper.GetColor(ID);
        //        }
        //        return color.Value;
        //    }
        //}

        //[Ignore]
        //public Command ShareCommand { get; set; }

        //public FactorWorkout()
        //{

        //}

        //private bool pauseNotifyPropertyChanged;
        //private bool needToSave;

        //public void PauseNotifyPropertyChanged()
        //{
        //    pauseNotifyPropertyChanged = true;
        //}

        //public void ResumeNotifyPropertyChanged()
        //{
        //    pauseNotifyPropertyChanged = false;
        //}

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}