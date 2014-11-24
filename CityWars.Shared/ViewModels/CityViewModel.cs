using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityWars.ViewModels
{
    [ParseClassName("Cities")]
    public class CityViewModel: ParseObject
    {
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
