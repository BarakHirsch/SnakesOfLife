using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Logic.Models;

namespace Web.Controllers
{
    public class SimulationController : ApiController
    {
        public IHttpActionResult Get(string id)
        {
            id = id.Replace("\"", "");
            var gameId = Guid.Parse(id).ToString();

            var simulationRunnerHolder = HttpContext.Current.Application[gameId] as SimulationRunnerHolder;

            if (simulationRunnerHolder == null)
            {
                return NotFound();
            }

            if (simulationRunnerHolder.RunningAction.IsCompleted)
            {
                simulationRunnerHolder.StartNewRun();
            }

            return Json(simulationRunnerHolder.SimulationRunner.RanOptimizations.Select(x => x.MaximalRun).OrderBy(x => x.AverageTurns).ToArray());
        }

        public IHttpActionResult Put()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var simulationRunner = new SimulationRunner(GameComstants.GridSize, GameComstants.GridSize,
                cancellationTokenSource.Token);

            var simulationRunnerHolder = new SimulationRunnerHolder(simulationRunner, cancellationTokenSource);

            simulationRunnerHolder.StartNewRun();

            var gameId = Guid.NewGuid().ToString();

            HttpContext.Current.Application.Add(gameId, simulationRunnerHolder);

            return Ok(gameId);
        }

        public class SimulationRunnerHolder
        {
            public SimulationRunnerHolder(SimulationRunner simulationRunner, CancellationTokenSource cancellationTokenSource)
            {
                SimulationRunner = simulationRunner;
                CancellationTokenSource = cancellationTokenSource;
            }

            public SimulationRunner SimulationRunner { get; }
            public Task RunningAction { get; set; }
            public CancellationTokenSource CancellationTokenSource { get; }

            public void StartNewRun()
            {
                RunningAction = Task.Factory.StartNew(() => SimulationRunner.LocateMaximalPoint());
            }
        }
    }
}