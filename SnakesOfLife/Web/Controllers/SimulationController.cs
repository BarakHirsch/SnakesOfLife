using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.WebSockets;
using Logic.Models;

namespace Web.Controllers
{
    [EnableCors("*", "*", "*")]
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

            simulationRunnerHolder.LastActiveGet = DateTime.Now;

            if (simulationRunnerHolder.RunningAction.IsCompleted)
            {
                simulationRunnerHolder.StartNewRun();
            }

            return Json(simulationRunnerHolder.SimulationRunner.RanOptimizations.Select(x => x.MaximalRun).OrderBy(x => x.AverageTurns).ToArray());
        }

        public IHttpActionResult Put()
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var simulationRunner = new SimulationRunner(GameConstants.GridSize, GameConstants.GridSize,
                cancellationTokenSource.Token);

            var simulationRunnerHolder = new SimulationRunnerHolder(simulationRunner, cancellationTokenSource);

            simulationRunnerHolder.StartNewRun();

            var gameId = Guid.NewGuid().ToString();

            HttpContext.Current.Application.Add(gameId, simulationRunnerHolder);

            // timer task to check that the research is alive, if not shut it down
            var timer = new System.Timers.Timer();
            timer.Elapsed += (sender, e) => TimerWorker(sender, e, simulationRunnerHolder, gameId);
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();        
            
            return Ok(gameId);
        }

        protected void TimerWorker(object sender, ElapsedEventArgs e, SimulationRunnerHolder simulationRunnerHolder, string gameId)
        {
            if (simulationRunnerHolder == null)
            {
                return;
            }

            if (simulationRunnerHolder.LastActiveGet.AddSeconds(5) < DateTime.Now)
            {
                simulationRunnerHolder.CancellationTokenSource.Cancel();
                var timer = (System.Timers.Timer)sender;
                timer.Stop();
            }
        }


        public class SimulationRunnerHolder
        {
            public SimulationRunnerHolder(SimulationRunner simulationRunner, CancellationTokenSource cancellationTokenSource)
            {
                SimulationRunner = simulationRunner;
                CancellationTokenSource = cancellationTokenSource;
            }

            public SimulationRunner SimulationRunner { get; set; }
            public Task RunningAction { get; set; }
            public CancellationTokenSource CancellationTokenSource { get; set; }
            public DateTime LastActiveGet { get; set; }

            public void StartNewRun()
            {
                RunningAction = Task.Factory.StartNew(() => SimulationRunner.LocateMaximalPoint(), TaskCreationOptions.LongRunning);
            }
        }
    }
}