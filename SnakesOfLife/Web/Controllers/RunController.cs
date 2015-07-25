using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using Logic.Models;

namespace Web.Controllers
{
    public class RunController : ApiController
    {
        private const int GridSize = 10;

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            var runManager = HttpContext.Current.Session[id] as RunManager;

            if (runManager == null)
            {
                return NotFound();
            }

            return Json(Convert(runManager));
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody] Params currParams)
        {
            if (currParams == null)
            {
                return BadRequest();
            }

            var runManager = new RunManager(currParams, GridSize, GridSize, new Random());

            var gameId = Guid.NewGuid().ToString();

            HttpContext.Current.Session.Add(gameId, runManager);

            return Ok(gameId);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(string id)
        {
            var runManager = HttpContext.Current.Session[id] as RunManager;

            if (runManager == null)
            {
                return NotFound();
            }

            runManager.RunTurn();

            return Json(Convert(runManager));
        }

        private RunManagerDto Convert(RunManager runManager)
        {
            var grassCells = new bool[runManager.GrassBoard.RowLength, runManager.GrassBoard.ColumnLength];

            for (int i = 0; i < runManager.GrassBoard.RowLength; i++)
            {
                for (int j = 0; j < runManager.GrassBoard.ColumnLength; j++)
                {
                    grassCells[i, j] = runManager.GrassBoard.GrassCells[i][j].IsAlive;
                }
            }

            var snakes = new List<SnakeDto>();

            foreach (var snake in runManager.Snakes)
            {
                var snakeDto = new SnakeDto();

                foreach (var location in snake.Locations)
                {
                    snakeDto.Locations.Add(new SnakeCellDto
                    {
                        RowIndex = location.RowIndex,
                        ColumnIndex = location.ColumnIndex
                    });
                }

                snakes.Add(snakeDto);
            }

            return new RunManagerDto
            {
                GrassCellsState = grassCells,
                Snakes = snakes,
                HasEnded = runManager.HasEnded
            };
        }
    }

    public class SnakeDto
    {
        public SnakeDto()
        {
            Locations = new List<SnakeCellDto>();
        }

        public List<SnakeCellDto> Locations { get; set; }
    }

    public class SnakeCellDto
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
    }

    public class RunManagerDto
    {
        public bool[,] GrassCellsState { get; set; }
        public List<SnakeDto> Snakes { get; set; }
        public bool HasEnded { get; set; }
    }

    public class GrassCellDto
    {
        public bool IsAlive { get; set; }
    }
}