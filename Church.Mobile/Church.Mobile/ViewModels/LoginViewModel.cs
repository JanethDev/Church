using Church.Mobile.DataLayer.ApiModels;
using Church.Mobile.DataLayer.AuxiliaryModels;
using Church.Mobile.DataLayer.Models;
using Church.Mobile.DataServices;
using Church.Mobile.Helpers;
using Church.Mobile.Localization;
using Church.Mobile.Views;
using Newtonsoft.Json;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Church.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
  
        private AccountServices AccountServices = new AccountServices();
        PermissionsManager PermissionsManager = new PermissionsManager();
        public string UserName
        {
            get ;
            set;
           
        }

        public string Password
        {
            get;
            set;
            
        }

        public ICommand LogInCommand => new AsyncCommand(LogInAsync);

      

        //public Command PasswordRecovery => new Command(async () =>
        //{
        //    await App.Current.MainPage.Navigation.PushAsync(new PasswordRecoveryPage(), false);
        //});

        public LoginViewModel()
        {
           // PermissionsNeeded();
            IsBusy = false;
        }


        private async Task<bool> PermissionsNeeded()
        {
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                return await PermissionsManager.CheckPermissionsAsync(Permission.Location, Permission.LocationAlways, Permission.LocationWhenInUse, Permission.Storage);
            }
            else
            {
                return true;
            }
        }

        public async Task LogInAsync()
        {

            //if (!await PermissionsNeeded())
            //{
            //    // XSnackService.ShowMessage(Resources.error_permissions);
            //    Alerts.ShowAlert("Title","MSG");

            //    return;
            //}
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                IsBusy = false;
                Alerts.ShowAlert("Notificación","Favor de ingresar datos validos en su usuario/contraseña");
                return;
            }

            await Alerts.ShowLoadingPageAsync();
            tblLogin Credentials = new tblLogin() { UserName = UserName, Password = Password };



            var response = await AccountServices.UserLoginAsync(Credentials);

            if (response.Result != Result.NETWORK_UNAVAILABLE)
            {
                await Alerts.HideLoadingPageAsync();
                if (response.Result == Result.ERROR_GETTING_DATA || response.Result == Result.EXCEPTION)
                {
                    Alerts.ShowAlert(string.Empty, Resources.error_getting_data);
                }
                if (response.Result == Result.NOT_FOUND)
                {
                    Alerts.ShowAlert(string.Empty, Resources.error_user_not_found);
                }
                else if (response.Result == Result.SERVICE_EXCEPTION)
                {
                    Alerts.ShowAlert(string.Empty, Resources.error_getting_data);
                }
                else if (response.Result == Result.INVALID_PASSWORD)
                {
                    Alerts.ShowAlert(string.Empty, Resources.error_invalid_password);
                }
                else if (response.Result == Result.OK)
                {
                    tblUsersDTO LoggedUser = JsonConvert.DeserializeObject<tblUsersDTO>(response.Data);
                    App.UsersB.Create(LoggedUser);
                    App.FirebaseManagerService.SendRegistrationToServerAsync();
                    HelperMethods.OpenNewPage(new Home());
                }
            }
            else
            {
                await Alerts.HideLoadingPageAsync();
                Alerts.ShowNetworkError();
            }

        }
    }
}
