using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SudokuSolver.Silverlight.Entities
{
    public class BoardCell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Value { get; set; }
    }
}
