using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace CityWars.ViewModels
{
    public class TopCitiesPageViewModel : ViewModelBase
    {
         private ObservableCollection<CityViewModel> allCities;

        public TopCitiesPageViewModel()
        {
            this.loadAllFighters();
        }

        private async Task loadAllFighters()
        {
            var currentUserId = ParseUser.CurrentUser.ObjectId;
            var currentUserFighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == currentUserId).FirstOrDefaultAsync();
            var currentUserFighterCity = currentUserFighter.City;

            // all figthers except current user fighter and not from the same city
            var cities = await new ParseQuery<CityViewModel>().OrderByDescending(c => c.Score).FindAsync();
            this.AllCities = cities; 
        }
        public IEnumerable<CityViewModel> AllCities
        {
            get
            {
                if (this.allCities == null)
                {
                    this.allCities = new ObservableCollection<CityViewModel>();
                }
                return this.allCities;
            }
            set
            {
                if (this.allCities == null)
                {
                    this.allCities = new ObservableCollection<CityViewModel>();
                }
                this.allCities.Clear();
                foreach (var item in value)
                {
                    this.allCities.Add(item);
                }
            }
        }
    }
}
