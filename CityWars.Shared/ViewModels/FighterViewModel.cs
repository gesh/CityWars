using System;
using System.Collections.Generic;
using System.Text;

namespace CityWars.ViewModels
{
    public class FighterViewModel
    {
        public FighterViewModel(string fighterId, string userId, string fighterName, int health, int level, int reputation, int damage, int armor,
            int experience, double money, string fighterType, string city, string message)
        {
            this.FighterId = fighterId;
            this.UserId = userId;
            this.FighterName = fighterName;
            this.Health = health;
            this.Level = level;
            this.Reputation = reputation;
            this.City = city;
            this.FighterType = fighterType;
            this.Experience = experience;
            this.Money = money;
            this.Message = message;
            this.Damage = damage;
            this.Armor = armor;

        }

        public string UserId { get; set; }

        public string FighterName { get; set; }

        public int Health { get; set; }

        public int Level { get; set; }

        public int Reputation { get; set; }

        public string City { get; set; }

        public string FighterType { get; set; }

        public int Experience { get; set; }

        public double Money { get; set; }

        public string Message { get; set; }

        public int Damage { get; set; }

        public int Armor { get; set; }

        public string FighterId { get; set; }
    }
}
