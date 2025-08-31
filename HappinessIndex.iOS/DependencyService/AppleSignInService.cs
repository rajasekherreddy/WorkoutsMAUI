﻿using System;
using System.Threading.Tasks;
using AuthenticationServices;
using Foundation;
using HappinessIndex.DependencyService;
using HappinessIndex.iOS.DependencyService;
using UIKit;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(AppleSignInService))]
namespace HappinessIndex.iOS.DependencyService
{
    public class AppleSignInService : NSObject, IAppleSignInService, IASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
    {
        public bool IsAvailable => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        TaskCompletionSource<ASAuthorizationAppleIdCredential> tcsCredential;

        public async Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId)
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var credentialState = await appleIdProvider.GetCredentialStateAsync(userId);
            switch (credentialState)
            {
                case ASAuthorizationAppleIdProviderCredentialState.Authorized:
                    // The Apple ID credential is valid.
                    return AppleSignInCredentialState.Authorized;
                case ASAuthorizationAppleIdProviderCredentialState.Revoked:
                    // The Apple ID credential is revoked.
                    return AppleSignInCredentialState.Revoked;
                case ASAuthorizationAppleIdProviderCredentialState.NotFound:
                    // No credential was found, so show the sign-in UI.
                    return AppleSignInCredentialState.NotFound;
                default:
                    return AppleSignInCredentialState.Unknown;
            }

        }

        public async Task<AppleAccount> SignInAsync()
        {
            var appleIdProvider = new ASAuthorizationAppleIdProvider();
            var request = appleIdProvider.CreateRequest();

            //await AppleSignInAuthenticator.AuthenticateAsync();

            request.RequestedScopes = new[] { ASAuthorizationScope.Email, ASAuthorizationScope.FullName };

            var authorizationController = new ASAuthorizationController(new[] { request });
            authorizationController.Delegate = this;
            authorizationController.PresentationContextProvider = this;
            authorizationController.PerformRequests();

            tcsCredential = new TaskCompletionSource<ASAuthorizationAppleIdCredential>();

            var creds = await tcsCredential.Task;

            if (creds == null)
                return null;

            var appleAccount = new AppleAccount();
            appleAccount.Token = new NSString(creds.IdentityToken, NSStringEncoding.UTF8).ToString();
            if(creds.Email!=null)
                appleAccount.Email = creds.Email;
            else
                appleAccount.Email = creds.User;
            appleAccount.UserId = creds.User;
            appleAccount.Name = NSPersonNameComponentsFormatter.GetLocalizedString(creds.FullName, NSPersonNameComponentsFormatterStyle.Default, NSPersonNameComponentsFormatterOptions.Phonetic);
            appleAccount.RealUserStatus = creds.RealUserStatus.ToString();

            return appleAccount;
        }

        #region IASAuthorizationController Delegate

        [Export("authorizationController:didCompleteWithAuthorization:")]
        public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            var creds = authorization.GetCredential<ASAuthorizationAppleIdCredential>();
            tcsCredential?.TrySetResult(creds);
        }

        [Export("authorizationController:didCompleteWithError:")]
        public void DidComplete(ASAuthorizationController controller, NSError error)
        {
            // Handle error
            tcsCredential?.TrySetResult(null);
            Console.WriteLine(error);
        }

        #endregion

        #region IASAuthorizationControllerPresentation Context Providing

        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            return UIApplication.SharedApplication.KeyWindow;
        }

        #endregion
    }
}