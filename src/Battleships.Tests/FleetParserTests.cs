using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Battleships.Domain;

namespace Battleships.Tests
{
    [TestClass]
    public class FleetParserTests
    {
        [TestMethod]
        public void ValidCompleteBoard()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,I7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.IsTrue(res.IsValid);
        }

        [TestMethod]
        public void UnknownShipClass_IsRejected()
        {
            var lines = new[]
            {
                "Boat,A1",
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,I7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Ships not recognised", res.Error);
        }

        [TestMethod]
        public void MissingShip_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Ships not recognised", res.Error);
        }

        [TestMethod]
        public void DuplicateShip_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Carrier,A1,A2,A3,A4,A5",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,I7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Ships not recognised", res.Error);
        }

        [TestMethod]
        public void IncorrectShipSize_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,I7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Incorrect ship size", res.Error);
        }

        [TestMethod]
        public void InvalidCoordinate_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,K7,K8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Invalid coordinate", res.Error);
        }

        [TestMethod]
        public void DiagonalShip_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,H7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Ships cannot sit diagonally on the board", res.Error);
        }

        [TestMethod]
        public void OverlappingShips_IsRejected()
        {
            var lines = new[]
            {
                "Carrier,C2,D2,E2,F2,G2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,G9,H9"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.AreEqual("Ships cannot overlap", res.Error);
        }

        [TestMethod]
        public void LowercaseShipNamesAndCoordinates_Accepted()
        {
            var lines = new[]
            {
                "carrier,c2,d2,e2,f2,g2",
                "battleship,d4,d5,d6,d7",
                "cruiser,g5,h5,i5",
                "submarine,e9,f9,g9",
                "destroyer,i7,i8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.IsTrue(res.IsValid);
        }

        [TestMethod]
        public void CoordinatesDifferentOrder_AcceptedIfContiguous()
        {
            var lines = new[]
            {
                "Carrier,G2,E2,F2,C2,D2",
                "Battleship,D4,D5,D6,D7",
                "Cruiser,G5,H5,I5",
                "Submarine,E9,F9,G9",
                "Destroyer,I7,I8"
            };
            var res = FleetParser.ParseLines(lines);
            Assert.IsTrue(res.IsValid);
        }
    }
}
