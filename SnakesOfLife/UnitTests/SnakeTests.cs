using NUnit.Framework;
using SnakesOfLife.Models;

namespace UnitTests
{
    [TestFixture]
    public class SnakeTests
    {
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
    }
}
