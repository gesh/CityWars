﻿using CityWars.Common;
using CityWars.ViewModels;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
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
    public sealed partial class AllFightersPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string CurrentFighterId { get; set; }

        public AllFightersPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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

            this.CurrentFighterId = e.Parameter.ToString();

            var allFighters = this.LoadFighters();
            var b = 5;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

        }

        private async Task<IEnumerable<FighterViewModel>> LoadFighters()
        {
            ParseQuery<ParseObject> query = ParseObject.GetQuery("Fighters");
            ParseObject currentFighter = await query.GetAsync(CurrentFighterId);

            var currentFighterLocation = currentFighter.Get<string>("City");

            var secondQuery = from figh in ParseObject.GetQuery("Fighters")
                        where figh.Get<string>("City") != currentFighterLocation
                        select figh;
            IEnumerable<ParseObject> results = await secondQuery.FindAsync();
            var allFightersFromDb = results.ToList();
            //var fighters = new ParseQuery<ParseObject>().Where(f => f["City"] != currentFighterLocation).FindAsync().Result.ToList();
            var returnFighters = new List<FighterViewModel>();


            for (int i = 0; i < allFightersFromDb.Count; i++)
            {

                var fighterId = allFightersFromDb[i].ObjectId;
                var fighterName = allFightersFromDb[i]["FighterName"].ToString();
                var fighterTypeToShow = allFightersFromDb[i]["FighterType"].ToString();
                var fighterCity = allFightersFromDb[i]["City"].ToString();
                var fighterHealth = int.Parse(allFightersFromDb[i]["Health"].ToString());
                var fighterLevel = int.Parse(allFightersFromDb[i]["Level"].ToString());
                var fighterReputation = int.Parse(allFightersFromDb[i]["Reputation"].ToString());
                var fighterDamage = int.Parse(allFightersFromDb[i]["Damage"].ToString());
                var fighterExp = int.Parse(allFightersFromDb[i]["Experience"].ToString());
                var fighterMoney = double.Parse(allFightersFromDb[i]["Money"].ToString());
                var fighterArmor = int.Parse(allFightersFromDb[i]["Armor"].ToString());
                string fighterMessage = null;
                if (allFightersFromDb[i]["Message"] != null)
                {
                    fighterMessage = allFightersFromDb[i]["Message"].ToString();
                }

                var fighterToAdd = new FighterViewModel(fighterId, ParseUser.CurrentUser.ObjectId, fighterName, fighterHealth, fighterLevel, fighterReputation, fighterDamage, fighterArmor,
                    fighterExp, fighterMoney, fighterTypeToShow, fighterCity, fighterMessage);

                returnFighters.Add(fighterToAdd);
            }
            return returnFighters;
        }


        #endregion
    }
}