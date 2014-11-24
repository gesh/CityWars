﻿using CityWars.Common;
using CityWars.Pages;
using CityWars.ViewModels;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class RegisterPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public RegisterPage()
            : this(new RegisterPageViewModel())
        {
        }

        public RegisterPage(RegisterPageViewModel viewModel)
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

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

        public RegisterPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as RegisterPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void onRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel == null)
            {
                // throw exception
                return;
            }

            var city = this.cityCombobox.SelectionBoxItem.ToString();
            var fighterType = this.fighterTypeCombobox.SelectionBoxItem.ToString();
            var isRegisteredSuccessfull = await this.ViewModel.Register(city,fighterType);

            if (isRegisteredSuccessfull)
            {
                var isLoggedSuccessfull = await this.ViewModel.Login();
                if (isLoggedSuccessfull)
                {
                    var userId = ParseUser.CurrentUser.ObjectId.ToString();
                    //var query = from fighterFromDb in ParseObject.GetQuery("Fighters")
                    //            where fighterFromDb.Get<string>("userId").Equals(userId)
                    //            select fighterFromDb;
                    //var result = await query.FirstOrDefaultAsync();

                    var res = await new ParseQuery<FighterViewModel>().Where(f => f.UserId == userId).FirstOrDefaultAsync();


                    //var query = from fg in FighterViewModel.GetQuery("Fighters")
                    //            where fg.Get<string>("userId") == userId
                    //            select fg;

                    //var fighterToSend = await query.FirstOrDefaultAsync();

                    //var fighterId = result.ObjectId;
                    //var fighterName = result["FighterName"].ToString();
                    //var fighterTypeToShow = result["FighterType"].ToString();
                    //var fighterCity = result["City"].ToString();
                    //var fighterHealth = int.Parse(result["Health"].ToString());
                    //var fighterLevel = int.Parse(result["Level"].ToString());
                    //var fighterReputation = int.Parse(result["Reputation"].ToString());
                    //var fighterDamage = int.Parse(result["Damage"].ToString());
                    //var fighterExp = int.Parse(result["Experience"].ToString());
                    //var fighterMoney = double.Parse(result["Money"].ToString());
                    //var fighterArmor = int.Parse(result["Armor"].ToString());
                    //string fighterMessage = null;
                    //if (result["Message"] != null)
                    //{
                    //    fighterMessage = result["Message"].ToString();
                    //}
                    




                    //var fighterToSend = new FighterViewModel(fighterId,userId, fighterName, fighterHealth, fighterLevel, fighterReputation, fighterDamage, fighterArmor,
                    //    fighterExp, fighterMoney, fighterTypeToShow, fighterCity, fighterMessage);

                    this.Frame.Navigate(typeof(UserFighterPage), res);

                }
                else
                {
                    this.Frame.Navigate(typeof(LoginPage));
                }
            }
        }
    }
}
