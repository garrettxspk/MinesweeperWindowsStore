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
        private const char NO_NEIGHBORS = '0';
        private const int IMAGE_SIZE = 66;
        
        private bool gameOver;
        private int gameBoardWidth;
        private int gameBoardHeight;
        private int numOfMines;
        private Cell[,] gameBoard;

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
        }

        private void FillGameBoard()
        {
            for(int r = 0; r < gameBoardHeight; r++)
                for (int c = 0; c < gameBoardWidth; c++)
                {
                    gameBoard[r, c] = new Cell('0', false, Cell.CellState.Blank);
                }
        }

        private void GenerateGameBoard()
        {
            int numberOfMinesPlaced = 0;
            Random random = new Random();
            double mineDensityDecimal = (double)numOfMines / (double)(gameBoardWidth * gameBoardHeight);
            int mineDensity = (int)(mineDensityDecimal * 100);
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
                                gameBoard[r, c].CellType = MINE;
                                numberOfMinesPlaced++;
                            }
                        }
                    }
            }

            for (int r = 0; r < gameBoardHeight; r++)
                for (int c = 0; c < gameBoardWidth; c++)
                {
                    int numberOfNeighborMines = 0;
                    if (gameBoard[r, c].CellType != MINE)
                    {
                        //examine neighbors for number of mines
                        for (int neighborR = -1; neighborR < 2; neighborR++)
                            for (int neighborC = -1; neighborC < 2; neighborC++)
                            {
                                int rowToCheck = r + neighborR;
                                int colToCheck = c + neighborC;

                                if (rowToCheck >= 0 && rowToCheck < gameBoardHeight && colToCheck >= 0 && colToCheck < gameBoardWidth)
                                    if (gameBoard[r + neighborR, c + neighborC].CellType == MINE)
                                    {
                                        numberOfNeighborMines++;
                                    }
                            }

                        gameBoard[r, c].CellType = numberOfNeighborMines.ToString()[0];
                    }
                }
            PrintGameBoard();
        }

        private void PrintGameBoard()
        {
            Rect bounds = Window.Current.Bounds;
            double appHeight = bounds.Height;
            double appWidth = bounds.Width;

            gameBoardGrid.MaxHeight = (.8 * (appHeight - 140));
            gameBoardGrid.MaxWidth = (.9 * appWidth);

            double totalHeight = gameBoardHeight * IMAGE_SIZE;
            double totalWidth = gameBoardWidth * IMAGE_SIZE;
            double heightOvershoot = totalHeight - gameBoardGrid.MaxHeight;
            double widthOvershoot = totalWidth - gameBoardGrid.MaxWidth;

            GridLength newImageDimension;
            bool auto = true;

            if (totalHeight > gameBoardGrid.MaxHeight && totalWidth > gameBoardGrid.MaxWidth)
            {
                if (widthOvershoot > heightOvershoot)
                    newImageDimension = new GridLength(Math.Floor(gameBoardGrid.MaxWidth / gameBoardWidth));     
                else
                    newImageDimension = new GridLength(Math.Floor(gameBoardGrid.MaxHeight / gameBoardHeight));
                auto = false;
            }
            else if (totalHeight > gameBoardGrid.MaxHeight)
            {
                newImageDimension = new GridLength(Math.Floor(gameBoardGrid.MaxHeight / gameBoardHeight));
                auto = false;
            }
            else if (totalWidth > gameBoardGrid.MaxWidth)
            {
                newImageDimension = new GridLength(Math.Floor(gameBoardGrid.MaxWidth / gameBoardWidth));
                auto = false;
            }

            GridLength colGridlength;
            if (!auto)
                colGridlength = newImageDimension;
            else
                colGridlength = GridLength.Auto;

            for (int i = 0; i < gameBoardWidth; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = colGridlength;
                gameBoardGrid.ColumnDefinitions.Add(col);
            }

            GridLength rowGridLength;
            if (!auto)
                rowGridLength = newImageDimension;
            else
                rowGridLength = GridLength.Auto;

            for (int j = 0; j < gameBoardHeight; j++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = rowGridLength;
                gameBoardGrid.RowDefinitions.Add(row);
            }

            for (int r = 0; r < gameBoardHeight; r++)
                for (int c = 0; c < gameBoardWidth; c++)
                {
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("ms-appx:///images/unrevealed.jpg", UriKind.Absolute));
                    img.Tapped += img_Tapped;
                    img.RightTapped += img_RightTapped;
                    gameBoardGrid.Children.Add(img);
                    Grid.SetRow(img, r);
                    Grid.SetColumn(img, c);
                }
        }

        private void img_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((Image)sender);
            int col = Grid.GetColumn((Image)sender);
            if (!gameOver && gameBoard[row, col].State != Cell.CellState.Revealed)
            {
                if (gameBoard[row, col].State == Cell.CellState.Blank)
                {
                    setFlagAtCell(row, col);
                    gameBoard[row, col].State = Cell.CellState.Flagged;
                }
                else if (gameBoard[row, col].State == Cell.CellState.Flagged)
                {
                    setGuessAtCell(row, col);
                    gameBoard[row, col].State = Cell.CellState.Guessed;
                }
                else if (gameBoard[row, col].State == Cell.CellState.Guessed)
                {
                    setBlankAtCell(row, col);
                    gameBoard[row, col].State = Cell.CellState.Blank;
                }
            }
        }

        void img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((Image)sender);
            int col = Grid.GetColumn((Image)sender);
            
            if (!gameOver && gameBoard[row, col].State == Cell.CellState.Blank)
            {
                revealCell(row, col);
            }
        }

        private void setFlagAtCell(int row, int col)
        {
            string newSource = "ms-appx:///images/flagged.jpg";
            Image thisCellImage = gameBoardGrid.Children[row * gameBoardWidth + col] as Image;
            (thisCellImage).Source = new BitmapImage(new Uri(newSource, UriKind.Absolute));
        }

        private void setGuessAtCell(int row, int col)
        {
            string newSource = "ms-appx:///images/guessed.jpg";
            Image thisCellImage = gameBoardGrid.Children[row * gameBoardWidth + col] as Image;
            (thisCellImage).Source = new BitmapImage(new Uri(newSource, UriKind.Absolute));
        }

        private void setBlankAtCell(int row, int col)
        {
            string newSource = "ms-appx:///images/unrevealed.jpg";
            Image thisCellImage = gameBoardGrid.Children[row * gameBoardWidth + col] as Image;
            (thisCellImage).Source = new BitmapImage(new Uri(newSource, UriKind.Absolute));
        }

        private void revealCell(int row, int col)
        {
            string newSource;
            if (gameBoard[row, col].CellType != MINE)
            {
                newSource = "ms-appx:///images/" + gameBoard[row, col].CellType + "Neighbor.jpg";
            }
            else
            {
                newSource = "ms-appx:///images/secondImg.jpg";
                gameOver = true;
            }

            Image thisCellImage = gameBoardGrid.Children[row * gameBoardWidth + col] as Image;
            (thisCellImage).Source = new BitmapImage(new Uri(newSource, UriKind.Absolute));
            //gameBoard[row, col].IsReaveled = true;
            gameBoard[row, col].State = Cell.CellState.Revealed;

            if (gameBoard[row, col].CellType == NO_NEIGHBORS)
            {
                for (int neighborR = -1; neighborR < 2; neighborR++)
                    for (int neighborC = -1; neighborC < 2; neighborC++)
                    {
                        int rowToCheck = row + neighborR;
                        int colToCheck = col + neighborC;

                        if (rowToCheck >= 0 && rowToCheck < gameBoardHeight && colToCheck >= 0 && colToCheck < gameBoardWidth)
                            if ((gameBoard[row + neighborR, col + neighborC].CellType != MINE) && (gameBoard[row + neighborR, col + neighborC].State == Cell.CellState.Blank))
                            {
                                //Image thisCellImage = gameBoardGrid.Children[rowToCheck * gameBoardWidth + colToCheck] as Image;
                                revealCell(rowToCheck, colToCheck);
                            }
                    }
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
            gameBoardHeight = boardConfig.Height;
            gameBoardWidth = boardConfig.Width;
            numOfMines = boardConfig.NumberOfMines;
            gameBoard = new Cell[gameBoardHeight, gameBoardWidth];

            gameOver = false;
            FillGameBoard();
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
