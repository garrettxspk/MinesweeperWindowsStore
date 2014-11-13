using MinesweeperWinStore.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace MinesweeperWinStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GamePage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        
        private BoardConfiguration boardConfig;

        private const char MINE = 'M';
        
        private bool gameOver;
        private int gameBoardWidth;
        private int gameBoardHeight;
        private int numOfMines;
        private char[,] gameBoard;

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


        //public GamePage(int boardWidth, int boardHeight, int numberMines)
        public GamePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            /*gameBoardHeight = boardHeight;
            gameBoardWidth = boardWidth;
            numOfMines = numberMines;*/
            //gameBoard = new char[gameBoardWidth, gameBoardHeight];

            //GenerateGameBoard();
        }



        private void GenerateGameBoard()
        {
            int numberOfMinesPlaced = 0;
            Random random = new Random();
            double mineDensityDecimal = (double)numOfMines / (double)(gameBoardWidth * gameBoardHeight);
            int mineDensity = (int)(mineDensityDecimal * 100);
            //int mineDensity = (int)(((double)((gameBoardWidth * gameBoardHeight) / numOfMines)) * 100);
            bool allMinesPlaced = false;

            while (numberOfMinesPlaced != numOfMines)
            {
                for(int r = 0; r < gameBoardHeight && !allMinesPlaced; r++)
                    for (int c = 0; c < gameBoardWidth && !allMinesPlaced; c++)
                    {
                        if (numberOfMinesPlaced == numOfMines)
                            allMinesPlaced = true;
                        else
                        {
                            int minePlacement = random.Next(1, 101);
                            if (minePlacement <= mineDensity)
                            {
                                gameBoard[r, c] = MINE;
                                numberOfMinesPlaced++;
                            }
                        }
                    }
            }

            for (int r = 0; r < gameBoardHeight; r++)
                for (int c = 0; c < gameBoardWidth; c++)
                {
                    int numberOfNeighborMines = 0;
                    if (gameBoard[r, c] != MINE)
                    {
                        //examine neighbors for number of mines
                        for (int neighborR = -1; neighborR < 2; neighborR++)
                            for (int neighborC = -1; neighborC < 2; neighborC++)
                            {
                                int rowToCheck = r + neighborR;
                                int colToCheck = c + neighborC;

                                if (rowToCheck >= 0 && rowToCheck < gameBoardHeight && colToCheck >= 0 && colToCheck < gameBoardWidth)
                                    if (gameBoard[r + neighborR, c + neighborC] == MINE)
                                    {
                                        numberOfNeighborMines++;
                                    }
                            }

                        gameBoard[r, c] = numberOfNeighborMines.ToString()[0];
                    }
                }
            PrintGameBoard();
        }

        private void PrintGameBoard()
        {
            // Create sample file; replace if exists.
            //string board = "";
            //for (int r = 0; r < gameBoardHeight; r++)
            //{
            //    string row = "";
            //    for (int c = 0; c < gameBoardWidth; c++)
            //    {
            //        row += gameBoard[r, c];
            //        row += " | ";
            //    }
            //    board += row;
            //    board += "\n";
            //}
            //testBoardPrint.Text = board;

            for (int i = 0; i < gameBoardWidth; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = GridLength.Auto;
                gameBoardGrid.ColumnDefinitions.Add(col);
            }
            for (int j = 0; j < gameBoardHeight; j++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                gameBoardGrid.RowDefinitions.Add(row);
            }
            for (int r = 0; r < gameBoardHeight; r++)
                for (int c = 0; c < gameBoardWidth; c++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///images/mineTest.jpg", UriKind.Absolute));
                    img.Tapped += img_Tapped;
                    gameBoardGrid.Children.Add(img);
                    Grid.SetRow(img, r);
                    Grid.SetColumn(img, c);
                }
        }

        void img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((Image)sender);
            int col = Grid.GetColumn((Image)sender);
            Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog(row.ToString() + " " + col.ToString());
            ((Image)sender).Source = new BitmapImage(new Uri("ms-appx:///images/secondImg.jpg", UriKind.Absolute));
            msg.ShowAsync();
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
            gameBoardHeight = boardConfig.Height;
            gameBoardWidth = boardConfig.Width;
            numOfMines = boardConfig.NumberOfMines;
            gameBoard = new char[gameBoardHeight, gameBoardWidth];

            GenerateGameBoard();
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
            boardConfig = (BoardConfiguration)e.Parameter;
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
