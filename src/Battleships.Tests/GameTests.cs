using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleships.Domain;

namespace Battleships.Tests
{
    [TestClass]
    public class GameTests
    {
        private Board BuildSimpleBoard()
        {
            var lines = new[]
            {
                "Carrier,A1,A2,A3,A4,A5",
                "Battleship,B1,B2,B3,B4",
                "Cruiser,C1,C2,C3",
                "Submarine,D1,D2,D3",
                "Destroyer,E1,E2"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.IsTrue(res.IsValid);
            return res.Board!;
        }

        [TestMethod]
        public void MissIsReported()
        {
            var board = BuildSimpleBoard();
            var game = new Game(board);
            var coord = new Coordinate(9,9); // J10
            var r = game.ProcessShot(coord);
            Assert.AreEqual(ShotResult.Miss, r);
        }

        [TestMethod]
        public void HitAndSunkBehavior()
        {
            var board = BuildSimpleBoard();
            var game = new Game(board);
            var r1 = game.ProcessShot(new Coordinate(0,0)); // A1 -> Carrier
            Assert.AreEqual(ShotResult.Hit, r1);
            // hit remaining carrier cells
            game.ProcessShot(new Coordinate(0,1));
            game.ProcessShot(new Coordinate(0,2));
            game.ProcessShot(new Coordinate(0,3));
            var r5 = game.ProcessShot(new Coordinate(0,4));
            Assert.AreEqual(ShotResult.Sunk, r5);
        }

        [TestMethod]
        public void CannotEnterSameCoordinateTwice()
        {
            var board = BuildSimpleBoard();
            var game = new Game(board);
            var c = new Coordinate(9,9);
            var r1 = game.ProcessShot(c);
            Assert.AreEqual(ShotResult.Miss, r1);
            var r2 = game.ProcessShot(c);
            Assert.AreEqual(ShotResult.AlreadyShot, r2);
        }

        [TestMethod]
        public void RepeatedCoordinateDoesNotCountAsHit()
        {
            var board = BuildSimpleBoard();
            var game = new Game(board);
            var c = new Coordinate(0,0);
            var r1 = game.ProcessShot(c);
            Assert.AreEqual(ShotResult.Hit, r1);
            var r2 = game.ProcessShot(c);
            Assert.AreEqual(ShotResult.AlreadyShot, r2);
        }

        [TestMethod]
        public void GameEndsWhenAllSunk()
        {
            var board = BuildSimpleBoard();
            var game = new Game(board);
            // sink all ships by firing every occupied cell
            foreach (var ship in board.Ships)
            {
                foreach (var cell in ship.Cells)
                {
                    var r = game.ProcessShot(cell);
                }
            }
            Assert.IsTrue(game.IsGameOver);
        }
    }
}
