
namespace AppPhone.ViewModels
{
    using AppPhone.Models;
    using AppPhone.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class PhoneBookViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private ObservableCollection<Phone> phones;
        #endregion

        #region Properties
    
        ObservableCollection<Phone> Phones
        {
            get { return this.phones; }
            set { SetValue(ref this.phones, value); }
        }
        #endregion
        #region Constructor
        public PhoneBookViewModel()
        {
            this.apiService = new ApiService();
            this.LoadPhone();
        }
        #endregion

        #region Methods
        private async void LoadPhone()
        {
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Internet Error Connection",
                    connection.Message,
                    "Acept"
                    );
                return;
            }
            var response = await apiService.GetList<Phone>(
                "http://localhost:50129/", //base
                "api/",//prefijo
                "Phones"//controlador
                );
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Phone Service Error",
                    response.Message,
                    "Acept"
                    );
                return;
            }
            MainViewModel main = MainViewModel.GetInstance();
            main.ListPhone = (List<Phone>) response.Result;
            this.Phones = new ObservableCollection<Phone>(ToPhoneCollect());
        }

        private IEnumerable<Phone> ToPhoneCollect()
        {
            ObservableCollection<Phone> collection = new ObservableCollection<Phone>();
            MainViewModel main = MainViewModel.GetInstance();
            foreach(var lista in main.ListPhone)
            {
                Phone phone = new Phone();
                phone.PhoneID = lista.PhoneID;
                phone.Name = lista.Name;
                phone.Type = lista.Type;
                phone.Contact = lista.Contact;
                collection.Add(phone);
                
            }
            return (collection);
        }
        #endregion
    }
}