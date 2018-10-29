using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
namespace santorini.tests
{
    [TestFixture()]
    public class PlayerTest
    {
        [Test()]
        // 1. valid board - empty building & no players
        public void TestEmptyBuildingNoPlayer()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 0 },
                new List<int>() { 0, 4 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "White"), answer);
            Assert.AreEqual(newBoard.Board_[0, 0].Worker, "White1");
            Assert.AreEqual(newBoard.Board_[0, 4].Worker, "White2");
        }

        [Test()]
        // 2. valid board - empty building & two other colored players on top-left and top-right corner
        public void TestEmptyBuildingOtherPlayersTLTR()
        {
            string validBoard = @"[
                                [[0, 'Blue1'], 0, 0, 0, [0, 'Blue2']]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 4, 4 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "White"), answer);
            Assert.AreEqual(newBoard.Board_[4, 4].Worker, "White1");
            Assert.AreEqual(newBoard.Board_[4, 0].Worker, "White2");
        }


        [Test()]
        // 3. valid board - empty building & two other colored players on top-left and bottom-right corner
        public void TestEmptyBuildingOtherPlayerTLBR()
        {
            string validBoard = @"[
                                [[0, 'Blue1'], 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, [0, 'Blue2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 4 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "White"), answer);
            Assert.AreEqual(newBoard.Board_[0, 4].Worker, "White1");
            Assert.AreEqual(newBoard.Board_[4, 0].Worker, "White2");
        }

        [Test()]
        // 4. valid board - empty building & two other colored players on top-right and bottom-right corner
        public void TestEmptyBuildingOtherPlayerTRBR()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, [0, 'Blue1']]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, [0, 'Blue2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 0 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "White"), answer);
            Assert.AreEqual(newBoard.Board_[0, 0].Worker, "White1");
            Assert.AreEqual(newBoard.Board_[4, 0].Worker, "White2");
        }

        [Test()]
        // 5. valid board - empty building & two other colored players on bottom-right and bottom-left corner
        public void TestEmptyBuildingOtherPlayerBRBL()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'Blue1'], 0, 0, 0, [0, 'Blue2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 0 },
                new List<int>() { 0, 4 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "White"), answer);
            Assert.AreEqual(newBoard.Board_[0, 0].Worker, "White1");
            Assert.AreEqual(newBoard.Board_[0, 4].Worker, "White2");
        }

        [Test()]
        // 6. valid board - empty building & two other colored players on top-left and bottom-left corner
        public void TestEmptyBuildingOtherPlayerTLBL()
        {
            string validBoard = @"[
                                [[0, 'White1'], 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'White2'], 0, 0, 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 4, 4 },
                new List<int>() { 0, 4 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkers(newBoard, "Blue"), answer); // asserts item not in order
            Assert.AreEqual(newBoard.Board_[0, 4].Worker, "Blue1");
            Assert.AreEqual(newBoard.Board_[4, 4].Worker, "Blue2");
        }

        [Test()]
        public void TestRegisterPlayer()
        {
            Player newPlayer = new Player();
            List<string> answer = new List<string>
            {
                newPlayer.Color, newPlayer.OppColor
            };
            List<string> result = new List<string>
            {
                newPlayer.RegisterPlayer(), newPlayer.GetOpponentColor()
            };
            CollectionAssert.AreEquivalent(result, answer);
        }

        // seems like these are invalid inputs from the start
        // 4. invalid board - empty buliding & two same colored players are already on the board
        // 5. invalid board - non-empy building & 
        [Test()]
        public void TestCase()
        {

        }
    }
}
