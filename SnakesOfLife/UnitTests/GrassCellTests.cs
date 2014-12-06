using NUnit.Framework;
using SnakesOfLife.Models;

namespace UnitTests
{
    [TestFixture]
    public class GrassCellTests
    {
        [SetUp]
        public void SetUp()
        {
            Params.Instance.NeededAliveNeighborsTurnsToGrow = 3;
            Params.Instance.SnakeCellsForGrow = 1;
            Params.Instance.SnakeLengthForSplit = 8;
            Params.Instance.SnakeLengthToStay = 2;
            Params.Instance.SnakeTurnToDie = 2;

        }

        [Test]
        public void NewGrassCell()
        {
            var grassCell = new GrassCell();

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateAboveZero()
        {
            var grassCell = new GrassCell();

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(Params.Instance.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(Params.Instance.NeededAliveNeighborsTurnsToGrow - 1);

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(1, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateToZero()
        {
            var grassCell = new GrassCell();

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(Params.Instance.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(Params.Instance.NeededAliveNeighborsTurnsToGrow);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateBelowZero()
        {
            var grassCell = new GrassCell();

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(Params.Instance.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(Params.Instance.NeededAliveNeighborsTurnsToGrow + 1);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }
    }
}