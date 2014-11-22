using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWars.Models
{
    [ParseClassName("Cities")]
    public class City : ParseObject
    {
        public City(string name,double score)
        {
            this.Name = name;
            this.Score = score;
        }

        [ParseFieldName("Name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("Score")]
        public double Score
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }

    }
}
