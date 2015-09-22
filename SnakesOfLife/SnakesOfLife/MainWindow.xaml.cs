using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using Logic.Models;
using UI.Properties;

namespace UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private const int GridSize = 10;
        private readonly BackgroundWorker _backgroudWorker;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly List<UIElement> _currentSnakeParts;
        private readonly Image[,] _imageGrid;
        private readonly ParamsCreator _paramsCreator;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly TimeSpan _timerInterval = new TimeSpan(0, 0, 0, 0, 10);

        public bool IsRunningSimulation
        {
            get { return _isRunningSimulation; }
            set
            {
                _isRunningSimulation = value;
                OnPropertyChanged();
            }
        }

        private Params _currentParams;
        private RunManager _runManager;
        private bool _isRunningSimulation;
        private RunSet _maxRun;

        public MainWindow()
        {
            _paramsCreator = new ParamsCreator(new Random());

            CurrentParams = _paramsCreator.Create();

            InitializeComponent();
            Loaded += Window_Loaded;
            _currentSnakeParts = new List<UIElement>();
            _imageGrid = new Image[GridSize, GridSize];

            _cancellationTokenSource = new CancellationTokenSource();

            _backgroudWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
                
            _backgroudWorker.RunWorkerCompleted += _backgroudWorker_RunWorkerCompleted;
            _backgroudWorker.ProgressChanged += _backgroudWorker_ProgressChanged;
            _backgroudWorker.DoWork += _backgroudWorker_DoWork;

            InitializeGrid();
        }

        public RunSet MaxRun
        {
            get { return _maxRun; }
            set
            {
                _maxRun = value;
                OnPropertyChanged();                
            }
        }

        public Params CurrentParams
        {
            get { return _currentParams; }
            set
            {
                _currentParams = value;
                OnPropertyChanged();
            }
        }

        void _backgroudWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MaxRun = e.UserState as RunSet;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void _backgroudWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var cancellationToken = _cancellationTokenSource.Token;

            IsRunningSimulation = true;
            
            var simulationRunner = new SimulationRunner(GridSize, GridSize, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                simulationRunner.LocateMaximalPoint();

                _backgroudWorker.ReportProgress(0, GetMaxRun(simulationRunner));
            }

            e.Result = GetMaxRun(simulationRunner);
        }

        private static RunSet GetMaxRun(SimulationRunner simulationRunner)
        {
            var runSets = simulationRunner.RanOptimizations.Select(x => x.MaximalRun).OrderBy(x => x.AverageTurns);
            return runSets.FirstOrDefault();
        }

        private void _backgroudWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsRunningSimulation = false;

            var runSet = e.Result as RunSet;

            if (runSet != null)
            {
                MaxRun = runSet;
            }
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
            _runManager.RunTurn();

            DrawSnakes(_runManager.Snakes);

            if (_runManager.HasEnded)
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
            foreach (UIElement uiElement in _currentSnakeParts)
            {
                MainGrid.Children.Remove(uiElement);
            }

            _currentSnakeParts.Clear();

            foreach (Snake snake in snakes)
            {
                foreach (GrassCell grassCell in snake.Locations)
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

        private void StartNewRun(Params currParams)
        {
            _runManager = new RunManager(currParams, GridSize, GridSize, new Random());

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

        private void SimulationButtonClick(object sender, RoutedEventArgs e)
        {
            _backgroudWorker.RunWorkerAsync();
        }

        private void StopResearchClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void RandomizeParamsClick(object sender, RoutedEventArgs e)
        {
            CurrentParams = _paramsCreator.Create();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}