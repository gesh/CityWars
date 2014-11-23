using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Parse;
using CityWars.Common;

namespace CityWars.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<bool> Login()
        {
            try
            {
                await ParseUser.LogInAsync("kiro", "kiro");
                //await ParseUser.LogInAsync(this.Username, this.Password);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
