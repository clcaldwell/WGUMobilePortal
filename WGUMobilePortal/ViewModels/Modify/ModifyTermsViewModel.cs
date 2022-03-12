using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Web;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    //[QueryProperty(nameof(strId), "id")]
    //[QueryProperty(nameof(Name), "name")]
    //[QueryProperty(nameof(strStartDate), "startDate")]
    //[QueryProperty(nameof(strEndDate), "endDate")]
    public class ModifyTermsViewModel : BaseViewModel, IQueryAttributable
    {
        public Command AddCommand { get; }
        public Command ModifyCommand { get; }
        public Command<Models.Term> SaveCommand { get; }
        public Command<Models.Term> RemoveCommand { get; }

        public Term Term { get => _term; set => SetProperty(ref _term, value); }
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }
        }
        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
       
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private Term _term;
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _id;

        public ModifyTermsViewModel()
        {

            Title = "Add/Modify Terms Page";
            RemoveCommand = new Command<Models.Term>(Remove);
            SaveCommand = new Command<Models.Term>(Save);

        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {

            Name = HttpUtility.UrlDecode(query["name"]);
            StartDate = DateTime.Parse(
                HttpUtility.UrlDecode(query["startDate"])
            );
            EndDate = DateTime.Parse(
                HttpUtility.UrlDecode(query["endDate"])
            );
            Id = int.Parse(
                HttpUtility.UrlDecode(query["id"])
            );

            Load(Id);


            // load data or do other processing based on parameter values
        }

        async void Remove(Models.Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}?", "Delete", "Cancel")) {
                await DBService.RemoveTerm(term.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        async void Save(Models.Term term)
        {
            //term.Id = Id;
            term.Name = Name;
            term.StartDate = StartDate;
            term.EndDate = EndDate;
            
            await DBService.EditTerm(term);

            await Shell.Current.GoToAsync("..");
        }

        //async void Remove(int Id)
        //{
            //if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {Id}?", "Delete", "Cancel"))
            //{
                //await DBService.RemoveTerm(Id);
            //}
        //}

        async void Load(int Id)
        {

            //int.TryParse(strId, out intId);

            Term = await DBService.GetTerm(Id);

            //Id = term.Id;
        }

        //async void Modify(Models.Term term)
        //{
        //var termsModifyPage = new Views.ModifyTermsPage();
        // termsModifyPage.BindingContext = term;
        //await AppShell.Current.Navigation.PushAsync(termsModifyPage);
        //}
    }
}