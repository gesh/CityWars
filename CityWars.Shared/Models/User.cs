using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWars.Models
{
    [ParseClassName("User")]
    public class User : ParseUser
    {
        public User(string username, string password, Fighter fighter)
        {
            this.Username = username;
            this.Password = password;
            this.Fighter = fighter;
        }


        [ParseFieldName("Username")]
        public string Username
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Password")]
        public string Password
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Fighter")]
        public Fighter Fighter
        {
            get { return GetProperty<Fighter>(); }
            set { SetProperty<Fighter>(value); }
        }

    }
}
