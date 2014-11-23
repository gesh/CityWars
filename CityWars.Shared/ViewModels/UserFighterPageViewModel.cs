using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWars.ViewModels
{
    public class UserFighterPageViewModel : ViewModelBase
    {

        public string FighterName { get; set; }

        public double Health { get; set; }

        public int Level { get; set; }

        public double Reputation { get; set; }

        public string City { get; set; }

        public string FighterType { get; set; }

        public string Message { get; set; }

        private async void getDataForUserFighter()
        {
            var userId = ParseUser.CurrentUser.ObjectId.ToString();
            var query = from fighterFromDb in ParseObject.GetQuery("Fighters")
                        where fighterFromDb.Get<string>("userId") == userId
                        select fighterFromDb;
            var result = await query.FirstOrDefaultAsync();
            this.FighterName = result["FighterName"].ToString();
            this.Health = (double)result["Health"];
            this.Level = (int)result["Level"];
            this.Reputation = (double)result["Reputation"];
            this.City = result["City"].ToString();
            this.FighterType = result["FighterType"].ToString();
            this.Message = result["Message"].ToString();

        }

       
    }
}
