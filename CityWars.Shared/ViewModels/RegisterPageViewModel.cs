using CityWars.Models;
using CityWars.Pages;
using GalaSoft.MvvmLight;
using Parse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityWars.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FighterName { get; set; }

        public string City { get; set; }

        public string FighterType { get; set; }

        public async Task<bool> Register(string city, string fighterType)
        {
            try
            {
                var user = new ParseUser();
                user.Username = this.Username;
                user.Password = this.Password;

                await user.SignUpAsync();

                this.FighterType = fighterType;
                this.City = city;

                var fighter = new Fighter(ParseUser.CurrentUser.ObjectId, this.FighterName, this.FighterType, this.City);
                await fighter.SaveAsync();


                //var isLoginSuccessful = await LoginPage.ViewModel.Login();

                // get current user fighter
                var userId = ParseUser.CurrentUser.ObjectId.ToString();
                var currentFighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == userId).FirstOrDefaultAsync();
                //var query = from fighterFromDb in FighterViewModel.GetQuery("Fighters")
                //            where fighterFromDb.Get<string>("userId") == userId
                //            select fighterFromDb;
                //var result = await query.FirstOrDefaultAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal async Task<bool> Login()
        {
            try
            {
                await ParseUser.LogInAsync(this.Username, this.Password);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
