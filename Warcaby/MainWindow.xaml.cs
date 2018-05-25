
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Warcaby
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private members 
        /// <summary>
        /// Holds references to buttons
        /// </summary>
        private Button[] buttons;
        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        private MarkType[] mResults;
        /// <summary>
        /// True if it is player 1's turn (white) or player 2's turn (black)
        /// </summary>
        private bool mPlayer1Turn;
        /// <summary>
        /// True if game ended
        /// </summary>
        private bool mGameEnded;
        /// <summary>
        /// index to create first pawns
        /// </summary>
        private int[] index = { 1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23, 24, 26, 28, 30};
        /// <summary>
        /// index of previous clicked button
        /// </summary>
        private int mPrevious;
        #endregion
        #region Contructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }
        #endregion

        /// <summary>
        /// Starts a newe game and clear all values back to stay
        /// </summary>
        private void NewGame()
        {
            ///Create a new blan array of free cells
            mResults = new MarkType[64];
            for (var i = 0; i < 64; i++)
                    mResults[i] = MarkType.Free;


            ///Make sure player 1 starts the game
            mPlayer1Turn = true;

            //cast every bi
            buttons = Container.Children.Cast<Button>().ToArray();
            for(var i=0 ; i < 12;i++)
            {
                buttons[index[i]].Content = "B";
                buttons[index[i]].Background = Brushes.DarkGray;
                mResults[index[i]] = MarkType.Black;

                buttons[64- index[i]-1].Content = "W";
                buttons[64-index[i]-1].Background = Brushes.DarkGray;
                mResults[64 - index[i]-1] = MarkType.White;
            }

            //cast every bi
            for (var i = 0; i < 16; i++)
            {
                buttons[index[i]].Background = Brushes.DarkGray;
                buttons[64 - index[i] - 1].Background = Brushes.DarkGray;
            }
            //Make sure the game hasn't finished
            mGameEnded = false;
        }


        /// <summary>
        /// handles button click event
        /// </summary>
        /// <param name="sender"> clicked button </param>
        /// <param name="e"> the events of the click </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Start a new game on the click after it finished
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            //Cast the sended to a button
            var button = (Button)sender;
            //finds buttons position in the array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            var position = column + (row * 8);

            // check if it's your pawn
            if(mPlayer1Turn)
                if ( mResults[position] != MarkType.Free)
                        return;


        }
    }
}
