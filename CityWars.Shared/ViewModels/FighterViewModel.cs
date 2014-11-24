using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWars.ViewModels
{
    [ParseClassName("Fighters")]
    public class FighterViewModel : ParseObject
    {

        //public FighterViewModel(string fighterId, string userId, string fighterName, int health, int level, int reputation, int damage, int armor,
        //    int experience, double money, string fighterType, string city, string message)
        //{
        //    this.FighterId = fighterId;
        //    this.UserId = userId;
        //    this.FighterName = fighterName;
        //    this.Health = health;
        //    this.Level = level;
        //    this.Reputation = reputation;
        //    this.City = city;
        //    this.FighterType = fighterType;
        //    this.Experience = experience;
        //    this.Money = money;
        //    this.Message = message;
        //    this.Damage = damage;
        //    this.Armor = armor;

        //}
        //[ParseFieldName("objectId")]
        //public string ObjectId
        //{
        //    get { return GetProperty<string>(); }
        //    set { SetProperty<string>(value); }
        //}

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

        //public string FighterId { get; set; }
    }
}
