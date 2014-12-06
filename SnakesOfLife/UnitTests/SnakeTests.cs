using NUnit.Framework;
using SnakesOfLife.Models;

namespace UnitTests
{
    [TestFixture]
    public class SnakeTests
    {
        [SetUp]
        public void SetUp()
        {
            Params.Current = new Params
            {
                NeededAliveNeighborsTurnsToGrow = 3,
                SnakeCellsForGrow = 2,
                SnakeTurnsToShrink = 2,
                SnakeLengthForSplit = 6,
                SnakeLengthToStop = 2,
                SnakeTurnToDie = 3
            };
        }

        [Test]
        public void SplitEvenLength()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();
            var cell3 = new GrassCell();
            var cell4 = new GrassCell();
            var cell5 = new GrassCell();
            var cell6 = new GrassCell();
            
            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);
            snake.Locations.Enqueue(cell3);

            snake.Locations.Enqueue(cell4);
            snake.Locations.Enqueue(cell5);
            snake.Locations.Enqueue(cell6);

            var splitSnake = snake.SplitSnake();

            Assert.AreEqual(3, snake.Locations.Count);
            Assert.AreEqual(3, splitSnake.Locations.Count);

            Assert.AreEqual(cell4, snake.Locations.Dequeue());
            Assert.AreEqual(cell5, snake.Locations.Dequeue());
            Assert.AreEqual(cell6, snake.Locations.Dequeue());

            Assert.AreEqual(cell1, splitSnake.Locations.Dequeue());
            Assert.AreEqual(cell2, splitSnake.Locations.Dequeue());
            Assert.AreEqual(cell3, splitSnake.Locations.Dequeue());
        }
        
        [Test]
        public void SplitOddLength()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();
            var cell3 = new GrassCell();
            var cell4 = new GrassCell();
            var cell5 = new GrassCell();
            var cell6 = new GrassCell();
            var cell7 = new GrassCell();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);
            snake.Locations.Enqueue(cell3);
            snake.Locations.Enqueue(cell4);

            snake.Locations.Enqueue(cell5);
            snake.Locations.Enqueue(cell6);
            snake.Locations.Enqueue(cell7);

            var splitSnake = snake.SplitSnake();

            Assert.AreEqual(3, snake.Locations.Count);
            Assert.AreEqual(4, splitSnake.Locations.Count);

            Assert.AreEqual(cell5, snake.Locations.Dequeue());
            Assert.AreEqual(cell6, snake.Locations.Dequeue());
            Assert.AreEqual(cell7, snake.Locations.Dequeue());

            Assert.AreEqual(cell1, splitSnake.Locations.Dequeue());
            Assert.AreEqual(cell2, splitSnake.Locations.Dequeue());
            Assert.AreEqual(cell3, splitSnake.Locations.Dequeue());
            Assert.AreEqual(cell4, splitSnake.Locations.Dequeue());
        }

        [Test]
        public void SnakeShortens()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();
            var cell3 = new GrassCell();
            var cell4 = new GrassCell();

            var cell5 = new GrassCell();
            cell5.EnteredBySnake();

            var cell6 = new GrassCell();
            cell6.EnteredBySnake();

            var cell7 = new GrassCell();
            cell7.EnteredBySnake();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);
            snake.Locations.Enqueue(cell3);
            snake.Locations.Enqueue(cell4);

            snake.MoveSnake(cell5);
            snake.MoveSnake(cell6);
            snake.MoveSnake(cell7);

            Assert.AreEqual(3, snake.Locations.Count);

            Assert.AreEqual(cell5, snake.Locations.Dequeue());
            Assert.AreEqual(cell6, snake.Locations.Dequeue());
            Assert.AreEqual(cell7, snake.Locations.Dequeue());
        } 
        
        [Test]
        public void SnakeGrows()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();
            var cell3 = new GrassCell();
            var cell4 = new GrassCell();

            var cell5 = new GrassCell();
            var cell6 = new GrassCell();
            var cell7 = new GrassCell();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);
            snake.Locations.Enqueue(cell3);
            snake.Locations.Enqueue(cell4);

            snake.MoveSnake(cell5);
            snake.MoveSnake(cell6);
            snake.MoveSnake(cell7);

            Assert.AreEqual(5, snake.Locations.Count);

            Assert.AreEqual(cell3, snake.Locations.Dequeue());
            Assert.AreEqual(cell4, snake.Locations.Dequeue());
            Assert.AreEqual(cell5, snake.Locations.Dequeue());
            Assert.AreEqual(cell6, snake.Locations.Dequeue());
            Assert.AreEqual(cell7, snake.Locations.Dequeue());
        }

        [Test]
        public void SnakeGetsToMinimalLength()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();
            var cell3 = new GrassCell();

            var cell5 = new GrassCell();
            cell5.EnteredBySnake();

            var cell6 = new GrassCell();
            cell6.EnteredBySnake();

            var cell7 = new GrassCell();
            cell7.EnteredBySnake();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);
            snake.Locations.Enqueue(cell3);

            snake.MoveSnake(cell5);
            snake.MoveSnake(cell6);
            snake.MoveSnake(cell7);

            Assert.AreEqual(Params.Current.SnakeLengthToStop, snake.Locations.Count);

            Assert.That(snake.IsStarving);

            Assert.AreEqual(cell6, snake.Locations.Dequeue());
            Assert.AreEqual(cell7, snake.Locations.Dequeue());
        }
        
        [Test]
        public void SnakeDying()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();

            var cell3 = new GrassCell();
            cell3.EnteredBySnake();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);

            Assert.That(!snake.IsStarving);

            snake.MoveSnake(cell3);

            Assert.That(snake.IsStarving);

            var starvingSnakeTurn = snake.StarvingSnakeTurn();

            Assert.That(!starvingSnakeTurn);

            starvingSnakeTurn = snake.StarvingSnakeTurn();
            
            Assert.That(starvingSnakeTurn);

            Assert.AreEqual(cell2, snake.Locations.Dequeue());
            Assert.AreEqual(cell3, snake.Locations.Dequeue());
        }

        [Test]
        public void StarvingSnakeEating()
        {
            var snake = new Snake();

            var cell1 = new GrassCell();
            var cell2 = new GrassCell();

            var cell3 = new GrassCell();
            cell3.EnteredBySnake();

            snake.Locations.Enqueue(cell1);
            snake.Locations.Enqueue(cell2);

            Assert.That(!snake.IsStarving);

            snake.MoveSnake(cell3);

            Assert.That(snake.IsStarving);

            cell3.UpdateGrowth(Params.Current.NeededAliveNeighborsTurnsToGrow);

            snake.StarvingSnakeTurn();

            Assert.That(!snake.IsStarving);
        }
    }
}
