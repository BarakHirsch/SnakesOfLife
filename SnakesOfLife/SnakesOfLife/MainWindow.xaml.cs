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
using System.Windows.Threading;
using SnakesOfLife.Models;

namespace SnakesOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int MaxGridSize = 80;
        
        // 100 millisecs.
        readonly TimeSpan _timerInterval = new TimeSpan(0, 0, 0, 0, 100);
        
        // The cells that make up the grid.
        private GrassBoard _cells;
        
        readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Sets up the display grid.
            InitializeGrid();

            //Determines what method should be called on every timer tick.
            _timer.Tick += TimerTick;

            //Sets the timer interval.
            _timer.Interval = _timerInterval;

        }

        private void TimerTick(object sender, EventArgs e)
        {
            // Update the grid and cells.
            _cells.UpdateGrass();
        }

        private void InitializeGrid()
        {
            Params.Instance.NeededAliveNeighborsTurnsToGrow = 3;
            Params.Instance.SnakeCellsForGrow = 1;
            Params.Instance.SnakeLengthForSplit = 8;
            Params.Instance.SnakeLengthToStay = 2;
            Params.Instance.SnakeTurnToDie = 2;

            _cells = new GrassBoard(MaxGridSize, MaxGridSize);
            
            //Generate the grid
            for (int i = 0; i < MaxGridSize; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int row = 0; row < MaxGridSize; row++)
            {
                for (int column = 0; column < MaxGridSize; column++)
                {
                    //Create a new Ellipse shape.
                    Ellipse ellipse = new Ellipse();

                    //Set the value of the Row/Column (e.g. Cell) with the Ellipse UI element. (e.g. (0,0), (35,35), (40,40) 
                    Grid.SetColumn(ellipse, column);
                    Grid.SetRow(ellipse, row);

                    ellipse.DataContext = _cells.GrassCells[row][column];

                    // Add the ellipse to the grid cell.
                    MainGrid.Children.Add(ellipse);

                    // Set the style of the ellipse using the style information defined in the XAML file.
                    ellipse.Style = Resources["GrassCellStyle"] as Style;
                }
            }
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }
    }
}
