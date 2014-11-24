using CityWars.APIs;
using CityWars.Common;
using CityWars.Enums;
using CityWars.Models;
using CityWars.ViewModels;
using GalaSoft.MvvmLight.Views;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
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
    public sealed partial class LoginPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public LoginPage()
            : this(new LoginPageViewModel())
        {
        }

        public LoginPage(LoginPageViewModel viewModel)
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
            //this.DataContext = e.Parameter;
            //this.ViewModel = e.Parameter as LoginPageViewModel;

            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void onLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                if (this.ViewModel == null)
                {
                    return;
                }

                var isLoginSuccessful = await this.ViewModel.Login();


                if (isLoginSuccessful)
                {
                    this.LoginResponseMessage.Text = "Login Successful";

                    var userId = ParseUser.CurrentUser.ObjectId.ToString();
                    var fighterToSend = await new ParseQuery<FighterViewModel>().Where(f => f.Get<string>("userId") == userId).FirstOrDefaultAsync();

                    this.Frame.Navigate(typeof(UserFighterPage), fighterToSend);
                }
                else
                {
                    this.LoginResponseMessage.Text = "Login Failed";
                }
            }
        }


        public LoginPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private async void onRegisterButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!ConnectionInspector.IsOnline())
            {
                MessageDialog msgbox = new MessageDialog("Check Your Internet Connection!");
                await msgbox.ShowAsync();
            }
            else
            {
                this.Frame.Navigate(typeof(RegisterPage), new RegisterPageViewModel());
            }
        }

        //private void Test_Click(object sender, RoutedEventArgs e)
        //{
        //    showStoryBoard.Begin();
        //    showStoryBoard.Stop();
        //    showStoryBoard.Begin();
        //}
    }
}
