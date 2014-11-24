using CityWars.ViewModels;
using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWars.Pages
{
    public class AllFightersPageViewModel : ViewModelBase
    {
        private ObservableCollection<FighterViewModel> allFighters;

        public AllFightersPageViewModel()
        {
            this.loadAllFighters();
        }

        private async Task loadAllFighters()
        {
            var currentUserId = ParseUser.CurrentUser.ObjectId;
            var currentUserFighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == currentUserId).FirstOrDefaultAsync();
            var currentUserFighterCity = currentUserFighter.City;

            // all figthers except current user fighter and not from the same city
            var fighters = await new ParseQuery<FighterViewModel>().Where(f => f.UserId != currentUserId && f.City != currentUserFighterCity && f.Health == 100 ).FindAsync();
            this.AllFighters = fighters; 
        }
        public IEnumerable<FighterViewModel> AllFighters
        {
            get
            {
                if (this.allFighters == null)
                {
                    this.allFighters = new ObservableCollection<FighterViewModel>();
                }
                return this.allFighters;
            }
            set
            {
                if (this.allFighters == null)
                {
                    this.allFighters = new ObservableCollection<FighterViewModel>();
                }
                this.allFighters.Clear();
                foreach (var item in value)
                {
                    this.allFighters.Add(item);
                }
            }
        }
    }
}
