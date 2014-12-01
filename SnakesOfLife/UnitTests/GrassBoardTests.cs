using NUnit.Framework;
using SnakesOfLife.Models;

namespace UnitTests
{
    [TestFixture]
    public class GrassBoardTests
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
        public void EnterCell()
        {
            var grassBoard = new GrassBoard(5, 5);

            grassBoard.CellEntered(0, 0);

            Assert.That(!grassBoard.GrassCells[0][0].IsAlive);
        }

        [Test]
        public void GrowInOneTurn()
        {
            var grassBoard = new GrassBoard(5, 5);

            grassBoard.CellEntered(0, 0);

            grassBoard.UpdateGrass();

            Assert.That(grassBoard.GrassCells[0][0].IsAlive);
        }

        [Test]
        public void GrowInTwoTurns()
        {
            var grassBoard = new GrassBoard(5, 5);

            grassBoard.CellEntered(0, 0);
            grassBoard.CellEntered(0, 1);

            grassBoard.UpdateGrass();

            Assert.That(!grassBoard.GrassCells[0][0].IsAlive);
            Assert.That(grassBoard.GrassCells[0][1].IsAlive);

            grassBoard.UpdateGrass();

            Assert.That(grassBoard.GrassCells[0][0].IsAlive);
            Assert.That(grassBoard.GrassCells[0][1].IsAlive);
        }

        [Test]
        public void NoGrass()
        {
            var grassBoard = new GrassBoard(2, 2);

            grassBoard.CellEntered(0, 0);
            grassBoard.CellEntered(0, 1);
            grassBoard.CellEntered(1, 0);
            grassBoard.CellEntered(1, 1);

            grassBoard.UpdateGrass();
            grassBoard.UpdateGrass();
            grassBoard.UpdateGrass();
            grassBoard.UpdateGrass();

            Assert.That(!grassBoard.GrassCells[0][0].IsAlive);
            Assert.That(!grassBoard.GrassCells[0][1].IsAlive);
            Assert.That(!grassBoard.GrassCells[1][0].IsAlive);
            Assert.That(!grassBoard.GrassCells[1][1].IsAlive);
        }
    }
}