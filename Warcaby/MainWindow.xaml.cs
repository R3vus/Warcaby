
using System;
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
        private Button[] mButtons;
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
        /// <summary>
        /// True if pawn was chosen
        /// </summary>
        private bool mIfClicked;
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

            //set pawns on their default spots
            mButtons = Container.Children.Cast<Button>().ToArray();
            for (var i = 0; i < 12; i++)
            {
                mButtons[index[i]].Content = "B";
                mResults[index[i]] = MarkType.Black;

                mButtons[64 - index[i] - 1].Content = "W";
                mResults[64 - index[i] - 1] = MarkType.White;
            }

            //Set background color to default for all buttons
            ClearAll();
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


            // return if clicked on the same pawn
            if (mIfClicked && position == mPrevious)
            {
                button.Background = Brushes.DarkGray;
                mIfClicked ^= true;
                return;
            }
            #region MarkGreen
            // check if it's player white pawn
            // mark green and add flag clicked and index of clicked element
            if (mPlayer1Turn && !mIfClicked)
                if (mResults[position] == MarkType.White)
                {
                    ClearAll();
                    button.Background = Brushes.Green;
                    mIfClicked ^= true;
                    mPrevious = position;
                }
            // check if it's player black pawn
            // mark green and add flag clicked and index of clicked element
            if (!mPlayer1Turn && !mIfClicked)
                if (mResults[position] == MarkType.Black)
                {
                    ClearAll();
                    button.Background = Brushes.Green;
                    mIfClicked ^= true;
                    mPrevious = position;
                }
            #endregion
            #region Diagonal atack
            //check if it's a diagonal atack and if it is possible
            Assault(position, button);
            if (!IfAssault())
            {
                mPlayer1Turn ^= true;    
            }
            #endregion

            #region Diagonal move
            //check if  player can atack, block move untill player atack
            if (!IfAssault())
            {
                //if clicked free space 1+ diagonally as white
                if (mPlayer1Turn && mResults[position] == MarkType.Free && position == mPrevious - 9 || position == mPrevious - 7)
                {
                    Move(position, button);
                    mPlayer1Turn ^= true;
                }
                //if clicked free space 1+ diagonally as black
                else if (!mPlayer1Turn && mResults[position] == MarkType.Free && position == mPrevious + 9 || position == mPrevious + 7)
                {
                    Move(position, button);
                    mPlayer1Turn ^= true;
                }
            }
            else
            {
                return;
            }
            #endregion




        }

        private void ClearAll()
        {
            for (var i = 0; i < 16; i++)
            {
                mButtons[index[i]].Background = Brushes.DarkGray;
                mButtons[64 - index[i] - 1].Background = Brushes.DarkGray;
            }
        }

        private bool IfAssault()
        {
            var k = 0;


            for (var i = 0; i<14; i++)
            {
                if (mResults[i] == MarkType.White && mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.Black){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }

                }
                else if (mResults[i] == MarkType.Black && !mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.White){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.White){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                }

            }
            for (var i = 14; i < 18; i++)
            {
                if (mResults[i] == MarkType.White && mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.Black){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.Black){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                }
                else if (mResults[i] == MarkType.Black && !mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.White){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.White){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                }

            }
            for (var i = 18; i < 46; i++)
            {
                if (mResults[i] == MarkType.White && mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                }
                else if (mResults[i] == MarkType.Black && !mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.White){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i + 18] == MarkType.Free && mResults[i + 9] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                }
                
            }
            for (var i = 46; i < 50; i++)
            {
                if (mResults[i] == MarkType.White && mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.Black){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.Black){ mButtons[i].Background = Brushes.DarkOrange; k++; }
                }
                else if (mResults[i] == MarkType.Black && !mPlayer1Turn)
                {
                    if (mResults[i + 14] == MarkType.Free && mResults[i + 7] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                }

            }
            for (var i = 50; i < 64; i++)
            {
                if (mResults[i] == MarkType.White && mPlayer1Turn)
                {
                    if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.Black) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                }
                else if (mResults[i] == MarkType.Black && !mPlayer1Turn)
                {
                    if (mResults[i - 14] == MarkType.Free && mResults[i - 7] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }
                    else if (mResults[i - 18] == MarkType.Free && mResults[i - 9] == MarkType.White) { mButtons[i].Background = Brushes.DarkOrange; k++; }

                    }

            }
            if (k>0) return true;
            return false;
        }

        private void Assault(int position, Button button)
        {
            if (mPlayer1Turn)
            {
                if (mResults[position] == MarkType.Free)
                {
                    if (position == mPrevious + 14 && mResults[mPrevious + 7] == MarkType.Black) { Move(position, button); Delete(mPrevious + 7);ClearAll(); }
                    if (position == mPrevious + 18 && mResults[mPrevious + 9] == MarkType.Black) { Move(position, button); Delete(mPrevious + 9);ClearAll(); }
                    if (position == mPrevious - 14 && mResults[mPrevious - 7] == MarkType.Black) { Move(position, button); Delete(mPrevious - 7);ClearAll(); }
                    if (position == mPrevious - 18 && mResults[mPrevious - 9] == MarkType.Black) { Move(position, button); Delete(mPrevious - 9);ClearAll();}
                }
            }
            else
                if (mResults[position] == MarkType.Free)
                {
                    if (position == mPrevious + 14 && mResults[mPrevious + 7] == MarkType.White) { Move(position, button); Delete(mPrevious + 7);ClearAll();}
                    if (position == mPrevious + 18 && mResults[mPrevious + 9] == MarkType.White) { Move(position, button); Delete(mPrevious + 9);ClearAll();}
                    if (position == mPrevious - 14 && mResults[mPrevious - 7] == MarkType.White) { Move(position, button); Delete(mPrevious - 7);ClearAll();}
                    if (position == mPrevious - 18 && mResults[mPrevious - 9] == MarkType.White) { Move(position, button); Delete(mPrevious - 9);ClearAll();}
                }
        }

        private void Delete(int position)
        {
            mResults[position] = MarkType.Free;
            mButtons[position].Content = "";
        }

        private void Move(int position,Button button)
        {
            mResults[position] = mResults[mPrevious];
            mResults[mPrevious] = MarkType.Free;
            button.Content = mButtons[mPrevious].Content;
            mButtons[mPrevious].Content = "";
            mButtons[mPrevious].Background = Brushes.DarkGray;
            mIfClicked ^= true;
            return;
        }
    }
}
