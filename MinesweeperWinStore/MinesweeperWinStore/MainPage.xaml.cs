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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MinesweeperWinStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;

        bool newGame = false;

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

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            fillSettingsComboBoxes();
            heightComboBox.SelectedValue = 8;
            widthComboBox.SelectedValue = 8;
            minesComboBox.SelectedValue = 10;

            if (localSettings.Values["beginner1Name"] == null)
            {
                string diff;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 0)
                    {
                        diff = "beginner";
                    }
                    else if (i == 1)
                    {
                        diff = "intermediate";
                    }
                    else
                    {
                        diff = "expert";
                    }
                    localSettings.Values[diff + "1Name"] = "Billy";
                    localSettings.Values[diff + "2Name"] = "Sally";
                    localSettings.Values[diff + "3Name"] = "Margo";
                    localSettings.Values[diff + "4Name"] = "Timmy";
                    localSettings.Values[diff + "5Name"] = "George";
                    localSettings.Values[diff + "6Name"] = "Gertrude";
                    localSettings.Values[diff + "7Name"] = "Albert";
                    localSettings.Values[diff + "8Name"] = "Tabitha";
                    localSettings.Values[diff + "9Name"] = "Elise";
                    localSettings.Values[diff + "10Name"] = "Bill";

                    localSettings.Values[diff + "1Time"] = ((i + 1) * 10);
                    localSettings.Values[diff + "2Time"] = ((i + 1) * 20);
                    localSettings.Values[diff + "3Time"] = ((i + 1) * 30);
                    localSettings.Values[diff + "4Time"] = ((i + 1) * 40);
                    localSettings.Values[diff + "5Time"] = ((i + 1) * 50);
                    localSettings.Values[diff + "6Time"] = ((i + 1) * 60);
                    localSettings.Values[diff + "7Time"] = ((i + 1) * 70);
                    localSettings.Values[diff + "8Time"] = ((i + 1) * 80);
                    localSettings.Values[diff + "9Time"] = ((i + 1) * 90);
                    localSettings.Values[diff + "10Time"] = ((i + 1) * 100);
                }
            }
        }

        private void startGameBtn_Click(object sender, RoutedEventArgs e)
        {
            BoardConfiguration boardConfig;
            int h = (int)heightComboBox.SelectedValue;
            int w = (int)widthComboBox.SelectedValue;
            int m = (int)minesComboBox.SelectedValue;
            boardConfig = new BoardConfiguration(h, w, m, true);
            this.Frame.Navigate(typeof(GamePage), boardConfig);
        }

        private void beginnerRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 8;
            widthComboBox.SelectedValue = 8;
            minesComboBox.SelectedValue = 10;
        }

        private void intermediateRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 16;
            widthComboBox.SelectedValue = 16;
            minesComboBox.SelectedValue = 40;
        }

        private void ExpertRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            disableSettingsComboBoxes();
            heightComboBox.SelectedValue = 16;
            widthComboBox.SelectedValue = 30;
            minesComboBox.SelectedValue = 99;
        }

        private void customRdoBtn_Click(object sender, RoutedEventArgs e)
        {
            enableSettingsComboBoxes();
            changeMineComboBoxRange();
        }

        private void disableSettingsComboBoxes()
        {
            heightComboBox.IsEnabled = false;
            widthComboBox.IsEnabled = false;
            minesComboBox.IsEnabled = false;
        }

        private void enableSettingsComboBoxes()
        {
            heightComboBox.IsEnabled = true;
            widthComboBox.IsEnabled = true;
            minesComboBox.IsEnabled = true;
        }

        private void fillSettingsComboBoxes()
        {
            for (int i = 8; i <= 30; i++)
            {
                heightComboBox.Items.Add(i);
                widthComboBox.Items.Add(i);
            }
            for (int m = 10; m <= 600; m++)
                minesComboBox.Items.Add(m);
        }

        private void heightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeMineComboBoxRange();
        }

        private void widthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeMineComboBoxRange();
        }

        private void changeMineComboBoxRange()
        {
            if (heightComboBox.SelectedValue != null && widthComboBox.SelectedValue != null && minesComboBox.SelectedValue != null)
            {
                int height = (int)heightComboBox.SelectedValue;
                int width = (int)widthComboBox.SelectedValue;
                int maxMines = (height * width);

                int selectedNumMines = (int)minesComboBox.SelectedValue;

                minesComboBox.Items.Clear();
                for (int m = 10; m < maxMines; m++)
                    minesComboBox.Items.Add(m);

                if (selectedNumMines > maxMines - 1)
                    minesComboBox.SelectedIndex = minesComboBox.Items.Count - 1;
                else
                    minesComboBox.SelectedValue = selectedNumMines;
            }
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
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void AppBarHelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void viewHighScoresBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HighScorePage));
        }
    }
}
