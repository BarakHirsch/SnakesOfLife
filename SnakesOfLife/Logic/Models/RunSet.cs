using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Logic.Models
{
    public class RunSet
    {
        private readonly int _rowLength;
        private readonly int _columnLength;

        private readonly Random _random;

        public RunSet(Params currParams, int rowLength, int columnLength)
        {
            Params = currParams;
            _rowLength = rowLength;
            _columnLength = columnLength;

            _random = new Random();
        }

        public Params Params { get; set; }
        public double AverageTurns { get; set; }
        
        public void Run(CancellationToken cancellationToken)
        {
            var runManagers = new List<RunManager>();

            for (var i = 0; i < 5; i++)
            {
                runManagers.Add(new RunManager(Params, _rowLength, _columnLength, new Random(_random.Next())));
            }

            foreach (var runManager in runManagers)
            {
                runManager.RunToEnd(cancellationToken);
            }

            AverageTurns = runManagers.Average(x => x.TurnsCount);
        }
    }
}