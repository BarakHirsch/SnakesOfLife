using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SnakesOfLife.Annotations;
using SnakesOfLife.Models;

namespace SnakesOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        const int GridSize = 20;
        
        // 100 millisecs.
        readonly TimeSpan _timerInterval = new TimeSpan(0, 0, 0, 0, 100);
              
        readonly DispatcherTimer _timer = new DispatcherTimer();

        private RunManager _runManager;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
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
            var haveMoreSnakes = _runManager.RunTurn();

            if (!haveMoreSnakes)
            {
                MessageBox.Show("There are no more snakes alive, Game over");

                _timer.Stop();
            }
        }

        private void InitializeGrid()
        {
            Params.Current = new Params
            {
                NeededAliveNeighborsTurnsToGrow = 40,
                SnakeCellsForGrow = 5,
                SnakeLengthForSplit = 8,
                SnakeLengthToStop = 2,
                SnakeTurnToDie = 2,
                SnakeTurnsToShrink = 2
            };

            _runManager = new RunManager(GridSize, GridSize);

            //Generate the grid
            for (int i = 0; i < GridSize; i++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    //Create a new Ellipse shape.
                    var image = new Image();

                    //Set the value of the Row/Column (e.g. Cell) with the Ellipse UI element. (e.g. (0,0), (35,35), (40,40) 
                    Grid.SetColumn(image, column);
                    Grid.SetRow(image, row);

                    image.DataContext = _runManager.GrassBoard.GrassCells[row][column];

                    // Add the ellipse to the grid cell.
                    MainGrid.Children.Add(image);

                    // Set the style of the ellipse using the style information defined in the XAML file.
                    image.Style = Resources["GrassCellStyle"] as Style;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
