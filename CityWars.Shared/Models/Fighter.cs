using CityWars.Enums;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityWars.Models
{
    [ParseClassName("Fighters")]
    public class Fighter : ParseObject
    {

        public Fighter(string userId,string fighterName, string fighterType, string city)
        {
            this.UserId = userId;
            this.FighterName = fighterName;
            this.Health = 100;
            this.Level = 1;
            this.Reputation = 1;
            this.City = city;
            this.FighterType = fighterType;
            this.Experience = 0;
            this.Money = 200;
            this.Message = "Welcome to city wars";

            setPowerByFighterType(fighterType);
        }

        private void setPowerByFighterType(string fighterType)
        {
            switch (fighterType)
            {
                case "Nacepenata Batka": this.Damage = 22; this.Armor = 15;
                    break;
                case "Sniveller": this.Damage = 20; this.Armor = 19;
                    break;
                case "City Legend": this.Damage = 25; this.Armor = 12;
                    break;
                default: this.Damage = 0; this.Armor = 0;
                    break;
            }
        }

        [ParseFieldName("userId")]
        public string UserId
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("FighterName")]
        public string FighterName
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Health")]
        public double Health
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("Damage")]
        public double Damage
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("Armor")]
        public double Armor
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("Level")]
        public double Level
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("Reputation")]
        public double Reputation
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

        [ParseFieldName("City")]
        public string City
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Message")]
        public string Message
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("FighterType")]
        public string FighterType
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }


        [ParseFieldName("Experience")]
        public int Experience
        {
            get { return GetProperty<int>(); }
            set { SetProperty<int>(value); }
        }

        [ParseFieldName("Money")]
        public double Money
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

    }
}
