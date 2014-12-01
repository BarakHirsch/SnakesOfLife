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
            ParametersContainer.Current = new ParametersContainer
            {
                NeededAliveNeighborsTurnsToGrow = 3,
                SnakeCellsForGrow = 1,
                SnakeLengthForSplit = 8,
                SnakeLengthToStay = 2,
                SnakeTurnToDie = 2
            };
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
            Assert.AreEqual(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow - 1);

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(1, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateToZero()
        {
            var grassCell = new GrassCell();

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateBelowZero()
        {
            var grassCell = new GrassCell();

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(ParametersContainer.Current.NeededAliveNeighborsTurnsToGrow + 1);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }
    }
}