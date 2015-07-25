using Logic.Models;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class GrassCellTests
    {
        public Params CurrentParams { get; set; }

        [SetUp]
        public void SetUp()
        {
            CurrentParams = new Params
            {
                NeededAliveNeighborsTurnsToGrow = 3,
                SnakeCellsForGrow = 1,
                SnakeLengthForSplit = 8,
                SnakeLengthToStop = 2,
                SnakeTurnToDie = 2
            };
        }

        [Test]
        public void NewGrassCell()
        {
            var grassCell = new GrassCell(CurrentParams);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateAboveZero()
        {
            var grassCell = new GrassCell(CurrentParams);

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(CurrentParams.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(CurrentParams.NeededAliveNeighborsTurnsToGrow - 1);

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(1, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateToZero()
        {
            var grassCell = new GrassCell(CurrentParams);

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(CurrentParams.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(CurrentParams.NeededAliveNeighborsTurnsToGrow);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }

        [Test]
        public void UpdateBelowZero()
        {
            var grassCell = new GrassCell(CurrentParams);

            grassCell.EnteredBySnake();

            Assert.That(!grassCell.IsAlive);
            Assert.AreEqual(CurrentParams.NeededAliveNeighborsTurnsToGrow, grassCell.NeededAliveNeighborsTurnsToGrow);

            grassCell.UpdateGrowth(CurrentParams.NeededAliveNeighborsTurnsToGrow + 1);

            Assert.That(grassCell.IsAlive);
            Assert.AreEqual(0, grassCell.NeededAliveNeighborsTurnsToGrow);
        }
    }
}