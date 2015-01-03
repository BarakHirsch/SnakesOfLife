using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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
        private const int GridSize = 10;

        public Params CurrentParams { get; set; }
        private RunManager _runManager;

        // 100 millisecs.
        private readonly TimeSpan _timerInterval = new TimeSpan(0, 0, 0, 0, 50);

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private readonly List<UIElement> _currentSnakeParts;
        private readonly Image[,] _imageGrid;


        public MainWindow()
        {
            CurrentParams = new Params
            {
                NeededAliveNeighborsTurnsToGrow = 40,
                SnakeCellsForGrow = 5,
                SnakeLengthForSplit = 8,
                SnakeLengthToStop = 2,
                SnakeTurnToDie = 2,
                SnakeTurnsToShrink = 2
            };

            InitializeComponent();
            Loaded += Window_Loaded;
            _currentSnakeParts = new List<UIElement>();
            _imageGrid = new Image[GridSize, GridSize];


            InitializeGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Determines what method should be called on every timer tick.
            _timer.Tick += TimerTick;

            //Sets the timer interval.
            _timer.Interval = _timerInterval;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var haveMoreSnakes = _runManager.RunTurn();

            DrawSnakes(_runManager.Snakes);

            if (!haveMoreSnakes)
            {
                MessageBox.Show("There are no more snakes alive, Game over");

                _timer.Stop();
            }
        }

        private void InitializeGrid()
        {
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

                    image.Style = Resources["GrassCellStyle"] as Style;

                    _imageGrid[row, column] = image;

                    MainGrid.Children.Add(image);
                }
            }
        }

        private void DrawSnakes(IEnumerable<Snake> snakes)
        {
            foreach (var uiElement in _currentSnakeParts)
            {
                MainGrid.Children.Remove(uiElement);
            }

            _currentSnakeParts.Clear();

            foreach (var snake in snakes)
            {
                foreach (var grassCell in snake.Locations)
                {
                    var snakePart = new Ellipse();

                    Grid.SetRow(snakePart, grassCell.RowIndex);
                    Grid.SetColumn(snakePart, grassCell.ColumnIndex);

                    snakePart.Style = Resources["SnakePartStyle"] as Style;

                    MainGrid.Children.Add(snakePart);
                    _currentSnakeParts.Add(snakePart);
                }
            }
        }

        private void SimulationButtonClick(object sender, RoutedEventArgs e)
        {
            var simulationRunner = new SimulationRunner(CurrentParams, GridSize, GridSize);

            simulationRunner.RunSimulation();
        }

        private void StartNewRun(Params currParams)
        {
            _runManager = new RunManager(currParams, GridSize, GridSize);

            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    _imageGrid[row, column].DataContext = _runManager.GrassBoard.GrassCells[row][column];
                }
            }
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            StartNewRun(CurrentParams);

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