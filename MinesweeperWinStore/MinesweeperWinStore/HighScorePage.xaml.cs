using MinesweeperWinStore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MinesweeperWinStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class HighScorePage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private HighScore highScore;

        private Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public HighScorePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
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
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string diff;
            if (e.Parameter != null)
            {
                highScore = (HighScore)e.Parameter;
                Grid.SetRow(nameInputBox, highScore.Place);
                nameInputBox.Text = "";
                nameInputBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
                saveNameBtn.Visibility = Windows.UI.Xaml.Visibility.Visible;
                diff = highScore.Difficulty;
                if (highScore.Place != 10)
                {
                    for (int i = 10; i > highScore.Place; i--)
                    {
                        localSettings.Values[diff + i + "Name"] = localSettings.Values[diff + (i - 1) + "Name"];
                        localSettings.Values[diff + i + "Time"] = localSettings.Values[diff + (i - 1) + "Time"];
                    }
                }
                if (highScore.Difficulty == "beginner")
                {
                    beginnerBtn.IsEnabled = false;
                }
                else if (highScore.Difficulty == "intermediate")
                {
                    intermediateBtn.IsEnabled = false;
                }
                else
                {
                    expertBtn.IsEnabled = false;
                }
            }
            else
            {
                diff = "beginner";
                beginnerBtn.IsEnabled = false;
            }
            updateHighScores(diff);
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void saveNameBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (nameInputBox.Text == "" || nameInputBox.Text == null || nameInputBox.Text.Length > 10)
            {
                MessageDialog message = new MessageDialog("Please enter a valid name in the name field.  Names cannot be blank and must have fewer than 10 characters.", "Invalid Name");
                await message.ShowAsync();
            }
            else
            {
                string diff = highScore.Difficulty;
                
                localSettings.Values[diff + highScore.Place + "Name"] = nameInputBox.Text;
                localSettings.Values[diff + highScore.Place + "Time"] = highScore.Time.ToString();

                updateHighScores(diff);

                nameInputBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                saveNameBtn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void updateHighScores(string diff)
        {
            firstNameTxt.Text = localSettings.Values[diff + "1Name"].ToString();
            secondNameTxt.Text = localSettings.Values[diff + "2Name"].ToString();
            thirdNameTxt.Text = localSettings.Values[diff + "3Name"].ToString();
            fourthNameTxt.Text = localSettings.Values[diff + "4Name"].ToString();
            fifthNameTxt.Text = localSettings.Values[diff + "5Name"].ToString();
            sixthNameTxt.Text = localSettings.Values[diff + "6Name"].ToString();
            seventhNameTxt.Text = localSettings.Values[diff + "7Name"].ToString();
            eighthNameTxt.Text = localSettings.Values[diff + "8Name"].ToString();
            ninthNameTxt.Text = localSettings.Values[diff + "9Name"].ToString();
            tenthNameTxt.Text = localSettings.Values[diff + "10Name"].ToString();

            firstTimeTxt.Text = localSettings.Values[diff + "1Time"].ToString();
            secondTimeTxt.Text = localSettings.Values[diff + "2Time"].ToString();
            thirdTimeTxt.Text = localSettings.Values[diff + "3Time"].ToString();
            fourthTimeTxt.Text = localSettings.Values[diff + "4Time"].ToString();
            fifthTimeTxt.Text = localSettings.Values[diff + "5Time"].ToString();
            sixthTimeTxt.Text = localSettings.Values[diff + "6Time"].ToString();
            seventhTimeTxt.Text = localSettings.Values[diff + "7Time"].ToString();
            eighthTimeTxt.Text = localSettings.Values[diff + "8Time"].ToString();
            ninthTimeTxt.Text = localSettings.Values[diff + "9Time"].ToString();
            tenthTimeTxt.Text = localSettings.Values[diff + "10Time"].ToString();
        }

        private void beginnerBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateHighScores("beginner");
            intermediateBtn.IsEnabled = true;
            expertBtn.IsEnabled = true;
            beginnerBtn.IsEnabled = false;
        }

        private void intermediateBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateHighScores("intermediate");
            intermediateBtn.IsEnabled = false;
            expertBtn.IsEnabled = true;
            beginnerBtn.IsEnabled = true;
        }

        private void expertBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            updateHighScores("expert");
            intermediateBtn.IsEnabled = true;
            expertBtn.IsEnabled = false;
            beginnerBtn.IsEnabled = true;
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
