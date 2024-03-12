using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Church.Mobile.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
      //  protected static readonly IXSnack XSnackService;

        private bool isBusy;
        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual Task UninitializeAsync() => Task.CompletedTask;

        public event PropertyChangedEventHandler PropertyChanged;

        public Color InvalidColor { get; set; } = Color.FromHex("#c0392b");
        public Color ValidColor = Color.FromHex("#ddd");

        public Command BackCommand
        {
            get
            {
                return new Command(() =>
                {
                    App.Current.MainPage.Navigation.PopAsync();
                });
            }
        }

        static BaseViewModel()
        {
            //AuthenticationService = DependencyService.Get<IAuthenticationService>();
            //LoggingService = DependencyService.Get<ILoggingService>();
            //RestPoolService = DependencyService.Get<IRestPoolService>();
         //   XSnackService = DependencyService.Get<IXSnack>();
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
            }
        }





    }
}
