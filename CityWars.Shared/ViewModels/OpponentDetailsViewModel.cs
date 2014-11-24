using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWars.ViewModels
{
    public class OpponentDetailsViewModel : ViewModelBase
    {
        // name
        public string FighterName { get; set; }
        //type
        public string FighterType { get; set; }
        //level
        public double Level { get; set; }
        //reputation
        public double Reputation { get; set; }
        //city
        public string City { get; set; }
    }
}
