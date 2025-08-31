using System;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using BuildHappiness.Core.Helpers;
using BuildHappiness.Core.Models;
using System.Collections.Generic;
using BuildHappiness.Core.Common;
using Firebase.Auth;
using HappinessIndex.Models;
using User = HappinessIndex.Models.User;
using System.Linq;
using Xamarin.Essentials;
using Acr.UserDialogs.Infrastructure;

namespace HappinessIndex.Data.CloudData
{
    public class FirebaseServiceNew : ICloudService
    {
        FirebaseClient _firebase;

        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAhsfDMHP1ywZledu6Ed5P01C2-QnNj3oc"));

        private readonly string SPRequestDB = "sprequest";
        private readonly string ApprovedSBDB = "approvedsp";
        private readonly string RejectedSBDB = "rejectedsp";
        private readonly string UserDataDB = "userdatasp";
        private readonly string Workouts = "Workouts";
        private readonly string DailyWorkouts = "DailyWorkouts";
        private readonly string UserFavorites = "userFavorites"; 

        private async Task<FirebaseClient> GetFirebaseClient()
        {
            if(_firebase == null)
            {
                var result = await authProvider.SignInAnonymouslyAsync();

                //_firebase = new FirebaseClient("https://build-happiness.firebaseio.com/", new FirebaseOptions { AuthTokenAsyncFactory = async () => { return result.FirebaseToken; } });

                _firebase = new FirebaseClient("https://sanhabits-fa5ca-default-rtdb.firebaseio.com/");
            }

            return _firebase;
        }

        public async Task<ServiceProvider> GetServiceProviderForEdit(string email)
        {
            try
            {
                var emailKey = email.RemoveSpecialCharacters().ToLower();
                var firebaseClient = await GetFirebaseClient();

                var requestDB = await firebaseClient.Child(SPRequestDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (requestDB != null)
                {
                    return requestDB;
                }

                var approvedDB = await firebaseClient.Child(ApprovedSBDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (approvedDB != null)
                {
                    return approvedDB;
                }

                var rejectDB = await firebaseClient.Child(RejectedSBDB).Child(emailKey).OnceSingleAsync<ServiceProvider>();
                if (rejectDB != null)
                {
                    return rejectDB;
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public async Task<List<ServiceProvider>> GetUserDataWithServiceProvider(string email)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await GetUserData(email);

                var serviceProviders = new List<ServiceProvider>();

                foreach (var item in userData.ServiceProviders)
                {
                    serviceProviders.Add(await firebaseClient.Child(ApprovedSBDB).Child(item.RemoveSpecialCharacters().ToLower()).OnceSingleAsync<ServiceProvider>());
                }
                return serviceProviders;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<UserData> GetUserData(string email)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).OnceSingleAsync<UserData>();

                return userData;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<int> SaveUserDataWithServiceProvider(string email, UserData data)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).PutAsync(data);

                return 1;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage("Unauthorized");
            }

            return 0;
        }

        public async Task<List<ServiceProvider>> GetValidServiceProvidersByCountry(string country)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var approvedProviders = await firebaseClient.Child(ApprovedSBDB).OrderBy("Country").EqualTo("India").OnceAsync<ServiceProvider>();

                var list = new List<ServiceProvider>();

                foreach (var item in approvedProviders)
                {
                    list.Add(item.Object);
                }

                return list;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }
            return null;
        }

        public async Task<List<ServiceProvider>> GetValidServiceProviders()
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var approvedProviders = await firebaseClient.Child(ApprovedSBDB).OnceAsync<ServiceProvider>();

                var list = new List<ServiceProvider>();

                foreach (var item in approvedProviders)
                {
                    list.Add(item.Object);
                }

                return list;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }
            return null;
        }

        public async Task<int> SubmitServiceProviderForReview(ServiceProvider data)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var emailKey = data.Email.RemoveSpecialCharacters();

                await firebaseClient.Child(SPRequestDB).Child(emailKey).PutAsync(data);

                return 1;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }
            return 0;
        }


        public async Task<int> SaveUser(string email, Models.User data)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).PutAsync(data);

                return 1;
            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return 0;
        }

        public async Task<Models.User> GetUser(string email)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(UserDataDB).Child(email.RemoveSpecialCharacters().ToLower()).OnceSingleAsync<Models.User>();

                return userData;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return null;
        }

        public async Task<List<Models.User>> GetAllUsers()
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(UserDataDB).OnceAsync<User>();

                var WorkoutsList = new List<User>();

                foreach (var item in WorkoutsList)
                {
                    WorkoutsList.Add(item);
                }


                return WorkoutsList;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return null;
        }

        public async Task<List<MicroWorkout>> GetDefaultWorkouts(Models.User user)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var workouts = await firebaseClient.Child(Workouts).OnceAsync<MicroWorkout>();

                var WorkoutsList = new List<MicroWorkout>();

                foreach (var item in workouts.ToList())
                {
                    item.Object.ID = item.Key;
                    WorkoutsList.Add(item.Object);
                }


                return WorkoutsList;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return null;
        }

        public async Task<List<MicroWorkout>> GetUserFavourites(User user)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(UserFavorites).Child(user.Email.RemoveSpecialCharacters().ToLower()).OnceAsync<MicroWorkout>();

                var WorkoutsList = new List<MicroWorkout>();

                foreach (var item in userData.ToList())
                {
                    WorkoutsList.Add(item.Object);
                }
                return WorkoutsList;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return null;
        }

        public async Task<List<MicroWorkout>> GetDailyorkouts(string dateId,User user)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

                var userData = await firebaseClient.Child(DailyWorkouts).Child(user.Email.RemoveSpecialCharacters().ToLower()).Child(dateId).OnceAsync<MicroWorkout>();

                var WorkoutsList = new List<MicroWorkout>();

                foreach (var item in userData.ToList())
                {
                    WorkoutsList.Add(item.Object);
                }
                return WorkoutsList;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unauthorized");
            }

            return null;
        }


        public async Task<int> CreateOrUpdateFavourites(User user, MicroWorkout workout)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();

             //   foreach (var workout in workouts)
               // {
                    if(workout.IsLiked)
                        await firebaseClient.Child(UserFavorites).Child(user.Email.RemoveSpecialCharacters().ToLower()).Child(workout.ID).PutAsync(workout);
                    else
                        await firebaseClient.Child(UserFavorites).Child(user.Email.RemoveSpecialCharacters().ToLower()).Child(workout.ID).DeleteAsync();

                //   }
                return 1;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unable to mark as favourite");
            }

            return 0;
        }

        public async Task<int> CreateDailyWorkouts(User user, MicroWorkout workout)
        {
            try
            {
                var firebaseClient = await GetFirebaseClient();
               // String dateId = workout.WorkoutDate.ToString().Replace("/","_");
                String dateId = workout.WorkoutDate.ToString("dd_MMM_yyy");
                await firebaseClient.Child(DailyWorkouts).Child(user.Email.RemoveSpecialCharacters().ToLower()).Child(dateId).Child(workout.ID).PutAsync(workout);
                return 1;

            }
            catch (Exception e)
            {
                GlobalClass.ShowToastMessage(e + "Unable to mark as complete");
            }

            return 0;
        }

    }
}