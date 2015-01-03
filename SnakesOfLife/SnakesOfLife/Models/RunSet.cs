using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SnakesOfLife.Models
{
    public class SimulationRunner
    {
        public RunSet MaximalRunSet { get; set; }

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
                var hasImprovedOnLastRun = false;
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
            } while (_paramsChanger.MoveToNextProperty());
        }

        public bool RunSetForParams(Params currParams)
        {
            var runSet = new RunSet(currParams, _rowLength, _columnLength);

            runSet.RunAll();

            if (MaximalRunSet == null)
            {
                MaximalRunSet = runSet;
                return true;
            }

            if (runSet.AvargeRunTurns > MaximalRunSet.AvargeRunTurns)
            {
                MaximalRunSet = runSet;
                return true;
            }

            return false;
        }
    }

    public class RunSet
    {
        private readonly List<RunManager> _managers;
        private const int RunCount = 5;

        public Params Params { get; private set; }
        public double AvargeRunTurns { get; private set; }

        public RunSet(Params currParams, int rowLength, int columnLength)
        {
            Params = currParams;

            _managers = new List<RunManager>();

            for (var i = 0; i < RunCount; i++)
            {
                _managers.Add(new RunManager(currParams, rowLength, columnLength));
            }
        }

        public void RunAll()
        {
            var enumerable = _managers.Select(x => Task.Factory.StartNew(() => x.RunToEnd())).ToArray();

            Task.WaitAll(enumerable.OfType<Task>().ToArray());

            AvargeRunTurns = enumerable.Select(x => x.Result).Average();
        }
    }
}