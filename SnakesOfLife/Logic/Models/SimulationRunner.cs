using System;
using System.ComponentModel;

namespace Logic.Models
{
    public class SimulationRunner
    {
        private readonly int _columnLength;
        private readonly ParamsCreator _paramsCreator;
        private readonly int _rowLength;

        public SimulationRunner(int rowLength, int columnLength)
        {
            _rowLength = rowLength;
            _columnLength = columnLength;
            _paramsCreator = new ParamsCreator(new Random());
        }

        public RunSet MaximalRun { get; set; }

        public void RunSimulation(BackgroundWorker cancellationToken)
        {
            while (!cancellationToken.CancellationPending)
            {
                Params currParams = _paramsCreator.Create();

                var paramsOptimizer = new ParamsOptimizer(currParams, _rowLength, _columnLength);

                paramsOptimizer.RunSimulationForParams(cancellationToken);

                if (MaximalRun == null || (paramsOptimizer.MaximalRun.AverageTurns > MaximalRun.AverageTurns))
                {
                    MaximalRun = paramsOptimizer.MaximalRun;

                    cancellationToken.ReportProgress(0, MaximalRun);
                }
            }
        }

        public class ParamsOptimizer
        {
            private readonly int _columnLength;
            private readonly ParamsChanger _paramsChanger;
            private readonly int _rowLength;

            public ParamsOptimizer(Params currParams, int rowLength, int columnLength)
            {
                _rowLength = rowLength;
                _columnLength = columnLength;
                _paramsChanger = new ParamsChanger(currParams);
            }

            public RunSet MaximalRun { get; set; }

            public bool RunSetForParams(BackgroundWorker cancellationToken, Params currParams)
            {
                var runSet = new RunSet(currParams, _rowLength, _columnLength);

                runSet.Run(cancellationToken);

                if (MaximalRun == null)
                {
                    MaximalRun = runSet;
                    return true;
                }

                if (runSet.AverageTurns > MaximalRun.AverageTurns)
                {
                    MaximalRun = runSet;
                    return true;
                }

                return false;
            }

            public void RunSimulationForParams(BackgroundWorker cancellationToken)
            {
                do
                {
                    bool hasImprovedOnLastRun;
                    bool hasImprovedByProperty = false;

                    do
                    {
                        hasImprovedOnLastRun = RunSetForParams(cancellationToken, _paramsChanger.GetCurrParams());

                        if (hasImprovedOnLastRun)
                        {
                            hasImprovedByProperty = true;
                        }
                    } while (!cancellationToken.CancellationPending && hasImprovedOnLastRun &&
                             _paramsChanger.IncreseCurrentPropertyValue());

                    _paramsChanger.DecreseCurrentPropertyValue();

                    do
                    {
                        hasImprovedOnLastRun = RunSetForParams(cancellationToken, _paramsChanger.GetCurrParams());

                        if (hasImprovedOnLastRun)
                        {
                            hasImprovedByProperty = true;
                        }
                    } while (hasImprovedOnLastRun && _paramsChanger.DecreseCurrentPropertyValue());

                    if (hasImprovedByProperty)
                    {
                        _paramsChanger.ResetCurrentProperty();
                    }
                } while (!cancellationToken.CancellationPending && _paramsChanger.MoveToNextProperty() &&
                         _paramsChanger.IncreseCurrentPropertyValue());
            }
        }
    }
}