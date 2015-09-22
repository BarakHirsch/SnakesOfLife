using System.Threading;

namespace Logic.Models
{
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

        public bool RunSetForParams(CancellationToken cancellationToken, Params currParams)
        {
            var runSet = new RunSet(currParams, _rowLength, _columnLength);

            if (MaximalRun == null)
            {
                MaximalRun = runSet;
                return true;
            }

            runSet.Run(cancellationToken);

            if (runSet.AverageTurns > MaximalRun.AverageTurns)
            {
                MaximalRun = runSet;
                return true;
            }

            return false;
        }

        public void RunSimulationForParams(CancellationToken cancellationToken)
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
                } while (!cancellationToken.IsCancellationRequested && hasImprovedOnLastRun &&
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
            } while (!cancellationToken.IsCancellationRequested && _paramsChanger.MoveToNextProperty() &&
                     _paramsChanger.IncreseCurrentPropertyValue());
        }
    }
}