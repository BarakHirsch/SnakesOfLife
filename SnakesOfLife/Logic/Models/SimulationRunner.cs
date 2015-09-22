using System;
using System.Collections.Generic;
using System.Threading;

namespace Logic.Models
{
    public class SimulationRunner
    {

        private readonly int _columnLength;
        private readonly CancellationToken _cancellationToken;

        private readonly ParamsCreator _paramsCreator;

        private readonly int _rowLength;

        public SimulationRunner(int rowLength, int columnLength, CancellationToken cancellationToken)
        {
            _rowLength = rowLength;
            _columnLength = columnLength;
            _cancellationToken = cancellationToken;
            _paramsCreator = new ParamsCreator(new Random());

            RanOptimizations = new List<ParamsOptimizer>();
        }

        public List<ParamsOptimizer> RanOptimizations { get; }

        public RunSet LocateMaximalPoint()
        {
            Params currParams = _paramsCreator.Create();

            var paramsOptimizer = new ParamsOptimizer(currParams, _rowLength, _columnLength);

            RanOptimizations.Add(paramsOptimizer);

            paramsOptimizer.RunSimulationForParams(_cancellationToken);

            return paramsOptimizer.MaximalRun;
        }
    }
}