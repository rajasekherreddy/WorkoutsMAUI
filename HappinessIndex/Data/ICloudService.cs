using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildHappiness.Core.Models;
using HappinessIndex.Models;
using HappinessIndex.ViewModels;

namespace HappinessIndex.Data
{
    public interface ICloudService
    {
        Task<int> SubmitServiceProviderForReview(ServiceProvider data);

        Task<ServiceProvider> GetServiceProviderForEdit(string email);

        Task<List<ServiceProvider>> GetValidServiceProvidersByCountry(string country);

        Task<List<ServiceProvider>> GetValidServiceProviders();

        Task<int> SaveUserDataWithServiceProvider(string email, UserData data);

        Task<int> SaveUser(string email, User data);

        Task<List<ServiceProvider>> GetUserDataWithServiceProvider(string email);

        Task<UserData> GetUserData(string email);
        Task<User> GetUser(string email);
        Task<List<MicroWorkout>> GetDefaultWorkouts(User user);
        Task<List<MicroWorkout>> GetUserFavourites(User user); 
        Task<List<User>> GetAllUsers();
        Task<int> CreateOrUpdateFavourites(User user,MicroWorkout microWorkouts);
        Task<int> CreateDailyWorkouts(User user, MicroWorkout microWorkout);
        Task<List<MicroWorkout>> GetDailyorkouts(string date,User user);


    }
}