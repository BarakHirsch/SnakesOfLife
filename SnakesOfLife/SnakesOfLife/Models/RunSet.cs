namespace SnakesOfLife.Models
{
    public class SimulationRunner
    {
        public RunManager MaximalRun { get; set; }

        private readonly int _rowLength;
        private readonly int _columnLength;
        private readonly ParamsChanger _paramsChanger;

        public SimulationRunner(Params currParams, int rowLength, int columnLength)
        {
            _rowLength = rowLength;
            _columnLength = columnLength;

            _paramsChanger = new ParamsChanger(currParams);
        }

        public void RunSimulation()
        {
            do
            {
                bool hasImprovedOnLastRun;
                var hasImprovedByProperty = false;

                do
                {
                    hasImprovedOnLastRun = RunSetForParams(_paramsChanger.GetCurrParams());

                    if (hasImprovedOnLastRun)
                    {
                        hasImprovedByProperty = true;
                    }
                } while (hasImprovedOnLastRun && _paramsChanger.IncreseCurrentPropertyValue());

                do
                {
                    hasImprovedOnLastRun = RunSetForParams(_paramsChanger.GetCurrParams());

                    if (hasImprovedOnLastRun)
                    {
                        hasImprovedByProperty = true;
                    }
                } while (hasImprovedOnLastRun && _paramsChanger.DecreseCurrentPropertyValue());

                if (hasImprovedByProperty)
                {
                    _paramsChanger.ResetCurrentProperty();
                }
            } while (_paramsChanger.MoveToNextProperty() && _paramsChanger.IncreseCurrentPropertyValue());
        }

        public bool RunSetForParams(Params currParams)
        {
            var runSet = new RunManager(currParams, _rowLength, _columnLength);

            int runToEnd = runSet.RunToEnd();

            if (MaximalRun == null)
            {
                MaximalRun = runSet;
                return true;
            }

            if (runToEnd > MaximalRun.TurnsCount)
            {
                MaximalRun = runSet;
                return true;
            }

            return false;
        }
    }
}