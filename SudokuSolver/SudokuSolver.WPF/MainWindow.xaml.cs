using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SudokuSolver.WPF.Entities;

namespace SudokuSolver.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[, ,] board = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            //Inializing the board parameter


            board = new int[9, 9, 10];
            //getting the number of rows ,columns and z values (probality of getting a number)
            var x = board.GetLength(0);
            var y = board.GetLength(1);
            var z = board.GetLength(2);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < z; k++)
                    {
                        if (k != 0)
                        {
                            board[i, j, k] = 1;
                        }
                        else
                        {
                            board[i, j, k] = 0;
                        }
                    }
                }
            }


            //polulating the value from text box in to board

            for (int i = 0; i < grdBoard.Children.Count; i++)
            {
                var row = (int)(i / 9);
                var col = i % 9;

                int number;
                bool result = Int32.TryParse((grdBoard.Children[i] as TextBox).Text, out number);
                if (result)
                {
                    board[row, col, 0] = number;
                }
                else
                {
                    //board[row, col, 0] =0;
                }
            }


            //seting the probailities
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (board[i, j, 0] != 0)
                    {
                        var boardcell = new BoardCell()
                        {
                            Row = i,
                            Column = j,
                            Value = board[i, j, 0]
                        };
                        setProbability(boardcell);
                    }
                }
            }

            UpdateTooltip();

            while (true)
            {
            var boardcells = ApplyChecks();
            if (boardcells.Count != 0)
            {
                foreach (var boardcell in boardcells)
                {
                    board[boardcell.Row, boardcell.Column, 0] = boardcell.Value;
                    updategridcell(boardcell);
                    setProbability(boardcell);
                }
            }
            else
            {
                break;
            }
            }

            UpdateTooltip();


        }

        //needs optimization
        private void updategridcell(BoardCell boardcell)
        {
            for (int i = 0; i < grdBoard.Children.Count; i++)
            {
                var row = (int)(i / 9);
                var col = i % 9;
                if ((row == boardcell.Row) && (col == boardcell.Column))
                {
                    var tx = (grdBoard.Children[i] as TextBox);
                    tx.Text = board[row, col, 0].ToString();
                }
            }
        }



        private void UpdateTooltip()
        {
            for (int i = 0; i < grdBoard.Children.Count; i++)
            {
                var row = (int)(i / 9);
                var col = i % 9;
                var tx = (grdBoard.Children[i] as TextBox);
                tx.ToolTip = "";
                for (int j = 1; j < 10; j++)
                {
                    if (board[row, col, j] == 0)
                    {
                        tx.ToolTip = tx.ToolTip.ToString() + 0 + "  ";
                    }
                    else if (board[row, col, j] == 1)
                    {
                        tx.ToolTip = tx.ToolTip.ToString() + j + "  ";
                    }

                }

            }
        }

        private List<BoardCell> ApplyChecks()
        {
            var boardcells = new List<BoardCell>();

            //row checking
            for (int i = 1; i < 10; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    int probcount = 0;
                    BoardCell cell = null;
                    for (int k = 0; k < 9; k++)
                    {
                        if (board[j, k, i] == 1)
                        {
                            probcount += 1;
                            cell = new BoardCell
                            {
                                Row = j,
                                Column = k,
                                Value = i
                            };
                        }
                    }
                    if (probcount == 1)
                    {
                        if (board[cell.Row, cell.Column, 0] == 0)
                        {
                            boardcells.Add(cell);
                        }
                    }
                }
            }

            //Column checking
            for (int i = 1; i < 10; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int probcount = 0;
                    BoardCell cell = null;
                    for (int k = 0; k < 9; k++)
                    {
                        if (board[k, j, i] == 1)
                        {
                            probcount += 1;
                            cell = new BoardCell
                            {
                                Row = k,
                                Column = j,
                                Value = i
                            };
                        }
                    }
                    if (probcount == 1)
                    {
                        if (board[cell.Row, cell.Column, 0] == 0)
                        {
                            boardcells.Add(cell);
                        }
                    }
                }
            }


            //Grid checking
            for (int k = 1; k < 10; k++)
            {
                for (int row = 0; row < 9; row += 3)
                {
                    for (int col = 0; col < 9; col += 3)
                    {
                        int rowstart = row - (row % 3);
                        int colstart = col - (col % 3);

                        int probcount = 0;
                        BoardCell cell = null;

                        for (int i = rowstart; i < (rowstart + 3); i++)
                        {
                            for (int j = colstart; j < (colstart + 3); j++)
                            {
                                if (board[i, j, k] == 1)
                                {
                                    probcount += 1;
                                    cell = new BoardCell
                                    {
                                        Row = i,
                                        Column = j,
                                        Value = k
                                    };
                                }


                            }
                        }

                        if (probcount == 1)
                        {
                            if (board[cell.Row, cell.Column, 0] == 0)
                            {
                                boardcells.Add(cell);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int probcount = 0;
                    BoardCell cell = null;
                    for (int k = 1; k < 10; k++)
                    {
                        if (board[i, j, k] == 1)
                        {
                            probcount += 1;
                            cell = new BoardCell
                            {
                                Row = i,
                                Column = j,
                                Value = k
                            };
                        }
                    }
                    if (probcount == 1)
                    {
                        if (board[cell.Row, cell.Column, 0] == 0)
                        {
                            boardcells.Add(cell);
                        }
                    }
                }
            }


            return boardcells;
        }

        private void setProbability(BoardCell boardcell)
        {
            int row = boardcell.Row;
            int col = boardcell.Column;
            int value = boardcell.Value;
            //setting the probality along the row and column
            for (int i = 0; i < 9; i++)
            {
                board[row, i, value] = 0;
                board[i, col, value] = 0;
            }
            //setting the probalitity inside the group
            int rowstart = row - (row % 3);
            int colstart = col - (col % 3);
            for (int i = rowstart; i < (rowstart + 3); i++)
            {
                for (int j = colstart; j < (colstart + 3); j++)
                {
                    board[i, j, value] = 0;
                }
            }
            //along the same cell
            for (int i = 1; i < 10; i++)
            {
                board[row, col, i] = 0;
            }

            board[row, col, value] = 1;
        }

    }
}
