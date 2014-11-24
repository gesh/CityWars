using CityWars.APIs;
using CityWars.Common;
using CityWars.ViewModels;
using Newtonsoft.Json;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
//using Windows.Services.Maps;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CityWars.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpponentDetailsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public OpponentDetailsPage()
            : this(new FighterViewModel())
        {

        }

        public OpponentDetailsPage(FighterViewModel viewModel)
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //this.DataContext = viewModel;
            this.ViewModel = viewModel;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            this.DataContext = e.Parameter;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void onFightButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.PerformFight();
            }
        }


        public FighterViewModel ViewModel
        {
            get
            {
                return this.DataContext as FighterViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private async void PerformFight()
        {
            this.FightMessage.Text = "Fighting ..";
            var currentUserId = ParseUser.CurrentUser.ObjectId.ToString();
            var currentUserFighter = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == currentUserId).FirstOrDefaultAsync();

            var opponentFighter = this.ViewModel;
 #if WINDOWS_PHONE_APP
            var currentUserGPSLocationCity = await this.getCurrentCityName();

            // cursed! damage decrease on opponent teritory
            if (currentUserGPSLocationCity.ToLower() == opponentFighter.City.ToLower())
            {
                currentUserFighter.Damage = 0.9 * currentUserFighter.Damage;
                currentUserFighter.Armor = 0.9 * currentUserFighter.Armor;
            }
#endif
            if (currentUserFighter.Health < 100 || opponentFighter.Health < 100)
            {
                this.FightMessage.Text = "Both fighters must be regenerated!";
                return;
            }
            else
            {

                // fight logic
                while (currentUserFighter.Health > 0 && opponentFighter.Health > 0)
                {
                    // current user fighter attacks
                    opponentFighter.Health = opponentFighter.Health - currentUserFighter.Damage * ((100 - opponentFighter.Armor) / 100);
                    if (opponentFighter.Health <= 0)
                    {
                        opponentFighter.Health = 0;
                        break;
                    }

                    // opponent fighter attacks
                    currentUserFighter.Health = currentUserFighter.Health - opponentFighter.Damage * ((100 - currentUserFighter.Armor) / 100);
                    if (currentUserFighter.Health <= 0)
                    {
                        currentUserFighter.Health = 0;
                        break;
                    }
                }

                // current user fighter wins
                if (currentUserFighter.Health > 0 && opponentFighter.Health <= 0)
                {
                    currentUserFighter.Experience += 10;

                    // beat bigger level fighters = gain more reputation
                    if (currentUserFighter.Level < opponentFighter.Level)
                    {
                        currentUserFighter.Experience += 5;
                        currentUserFighter.Reputation += 3;
                        currentUserFighter.Money += 60;
                    }
                    else
                    {
                        currentUserFighter.Reputation += 1;
                        currentUserFighter.Money += 40;
                    }

                    // check for level up
                    if (currentUserFighter.Experience >= 100)
                    {
                        currentUserFighter.Experience = currentUserFighter.Experience - 100;
                        currentUserFighter.Level++;
                    }

                    // add points to winner's city
                    //var query = from city in ParseObject.GetQuery("Cities")
                    //            where city.Get<string>("Name") == currentUserFighter.City
                    //            select city;
                    //var currentUserCityFromDb = await query.FirstOrDefaultAsync();
                    var curCity = await new ParseQuery<CityViewModel>().Where(c => c.Name == currentUserFighter.City).FirstAsync();
                    if (curCity != null)
                    {
                        curCity.Score += 10;
                        //currentUserCityFromDb["Score"] = int.Parse(currentUserCityFromDb["Score"].ToString()) + 10;
                    }

                    // messages for both fighters
                    currentUserFighter.Message = string.Format("Contrats! You beat {0} - the {1} from {2}", opponentFighter.FighterName, opponentFighter.FighterType, opponentFighter.City);
                    opponentFighter.Message = string.Format("Damn! You were beaten by {0} - the {1} from {2}", currentUserFighter.FighterName, currentUserFighter.FighterType, currentUserFighter.City);

                    // update  cities db
                    await curCity.SaveAsync();

                    this.FightMessage.Text = "Congrats!\nYou win!";
                }

                // opponent fighter wins
                if (opponentFighter.Health > 0 && currentUserFighter.Health <= 0)
                {
                    opponentFighter.Experience += 10;

                    // beat bigger level fighters = gain more reputation
                    if (opponentFighter.Level < currentUserFighter.Level)
                    {
                        opponentFighter.Experience += 5;
                        opponentFighter.Reputation += 3;
                        opponentFighter.Money += 60;
                    }
                    else
                    {
                        opponentFighter.Reputation += 1;
                        opponentFighter.Money += 40;
                    }

                    // check for level up
                    if (opponentFighter.Experience >= 100)
                    {
                        opponentFighter.Experience = opponentFighter.Experience - 100;
                        opponentFighter.Level++;
                    }

                    // add points to winner's city
                    var curCity = await new ParseQuery<CityViewModel>().Where(c => c.Name == opponentFighter.City).FirstAsync();
                    //var query = from city in ParseObject.GetQuery("Cities")
                    //            where city.Get<string>("Name") == opponentFighter.City
                    //            select city;
                    //var currentUserCityFromDb = await query.FirstOrDefaultAsync();
                    if (curCity != null)
                    {
                        curCity.Score += 10;
                        //currentUserCityFromDb["Score"] = int.Parse(currentUserCityFromDb["Score"].ToString()) + 10;
                    }

                    // messages for both fighters
                    opponentFighter.Message = string.Format("Contrats! You beat {0} - the {1} from {2}", currentUserFighter.FighterName, currentUserFighter.FighterType, currentUserFighter.City);
                    currentUserFighter.Message = string.Format("Damn! You were beaten by {0} - the {1} from {2}", opponentFighter.FighterName, opponentFighter.FighterType, opponentFighter.City);

                    // update cities db
                    await curCity.SaveAsync();
                    this.FightMessage.Text = "Damn!\nYou lose!";

                    Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    localSettings.Values["wins"] = int.Parse(localSettings.Values["wins"].ToString()) + 1;
                }

                // both fighters gain exp and money
                currentUserFighter.Experience += 3;
                currentUserFighter.Money += 5;
                opponentFighter.Experience += 3;
                opponentFighter.Money += 5;

                // update db
                await currentUserFighter.SaveAsync();
                await opponentFighter.SaveAsync();

            }
        }

        private async void onMyFighterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(UserFighterPage));
            }
        }

        private async void onFightersButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(AllFightersPage));
            }
        }

        private async void onTopCitiesButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(TopCitiesPage));
            }
        }

 

        private async Task<string> getCurrentCityName()
        {
            var geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;
            Geoposition position = await geolocator.GetGeopositionAsync();

            // reverse geocoding
            BasicGeoposition myLocation = new BasicGeoposition
            {
                Longitude = position.Coordinate.Longitude,
                Latitude = position.Coordinate.Latitude
            };
            Geopoint pointToReverseGeocode = new Geopoint(myLocation);

            //MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

            // here also it should be checked if there result isn't null and what to do in such a case
            string town = "";
            //if (result.Locations[0].Address.Town != null)
            //{
            //    town = result.Locations[0].Address.Town;
            //}
            var b = 5;


            return town;
 

        }
    }
}
