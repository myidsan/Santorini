﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
namespace santorini_remote.tests
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
            Player newPlayer = new Player("white");
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
            Player newPlayer = new Player("white");
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 4, 4 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkersCorners(newBoard, "White"), answer);
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
            Player newPlayer = new Player("white");
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 4 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkersCorners(newBoard, "White"), answer);
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
            Player newPlayer = new Player("white");
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 0 },
                new List<int>() { 4, 0 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkersCorners(newBoard, "White"), answer);
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
            Player newPlayer = new Player("white");
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 0, 0 },
                new List<int>() { 0, 4 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkersCorners(newBoard, "White"), answer);
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
            Player newPlayer = new Player("blue");
            List<List<int>> answer = new List<List<int>>
            {
                new List<int>() { 4, 4 },
                new List<int>() { 0, 4 }
            };
            CollectionAssert.AreEquivalent(newPlayer.PlacePlayerWorkersCorners(newBoard, "Blue"), answer); // asserts item not in order
            Assert.AreEqual(newBoard.Board_[0, 4].Worker, "Blue1");
            Assert.AreEqual(newBoard.Board_[4, 4].Worker, "Blue2");
        }

        [Test()]
        public void TestRegisterPlayer()
        {
            Player newPlayer = new Player("blue");
            List<string> answer = new List<string>
            {
                newPlayer.playerColor, newPlayer.OppColor
            };
        }

        // seems like these are invalid inputs from the start
        // 4. invalid board - empty buliding & two same colored players are already on the board
        // 5. invalid board - non-empy building & 

        [Test()]
        // Gets the one turn(move) win plays for the worker
        // BlueForce: White, OppForce: Blue
        public void TestGetNextBestPlayForInevitableLose()
        {
            string validBoard = @"[
                                [[0, 'White1'], 0, 0, 3, [2, 'Blue1']]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'White2'], 0, 0, 3, [2, 'Blue2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player("white");

            ArrayList answer = new ArrayList(){};
            ArrayList value = newPlayer.GetNextBestPlay(newBoard, "White", "Blue");
            Assert.AreEqual(0, value.Count);
        }

        [Test()]
        // Gets the one turn(move) win plays for the worker
        // should return the winning move and all valid moves
        // BlueForce: Blue, OppForce: White
        public void TestGetNextBestPlay()
        {
            string validBoard = @"[
                                [[0, 'White1'], 0, 0, 3, [2, 'Blue1']]," +
                               "[0, 0, 0, 3, 3]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'White2'], 0, 0, 3, [2, 'Blue2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player("blue");

            ArrayList answers = new ArrayList
            {
                new ArrayList { "Blue1", new ArrayList { "W" } },
                new ArrayList { "Blue1", new ArrayList { "S" } },
                new ArrayList { "Blue1", new ArrayList { "SW" } },
                new ArrayList { "Blue2", new ArrayList { "W" } },
                new ArrayList { "Blue2", new ArrayList { "N", "W" } },
                new ArrayList { "Blue2", new ArrayList { "N", "S" } },
                new ArrayList { "Blue2", new ArrayList { "N", "N" } },
                new ArrayList { "Blue2", new ArrayList { "N", "SW" } },
                new ArrayList { "Blue2", new ArrayList { "N", "NW" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "E" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "W" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "S" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "N" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "NE" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "NW" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "SE" } },
                new ArrayList { "Blue2", new ArrayList { "NW", "SW" } }
            };
            ArrayList values = newPlayer.GetNextBestPlay(newBoard, "Blue", "White");
            CollectionAssert.AreEquivalent(answers, values);
        }

        [Test()]
        // Gets the one turn(move) win plays for the worker when both can win in one move
        // BlueForce: Blue, OppForce: White
        public void TestPlayerWinPriority()
        {
            string validBoard = @"[
                                [[2, 'White1'], 3, [2, 'Blue1'], 3, 0]," +
                               "[0, 3, 0, 3, 3]," +
                               "[0, 0, [2, 'Blue2'], 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'White2'], 0, 0, 3, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player("blue");

            ArrayList answers = new ArrayList
            {
                new ArrayList { "Blue1", new ArrayList { "W" } },
                new ArrayList { "Blue1", new ArrayList { "E" } },
                new ArrayList { "Blue1", new ArrayList { "SW" } },
                new ArrayList { "Blue1", new ArrayList { "SE" } },
                new ArrayList { "Blue2", new ArrayList { "NW" } },
                new ArrayList { "Blue2", new ArrayList { "NE" } }
            };
            ArrayList values = newPlayer.GetNextBestPlay(newBoard, "Blue", "White");
            CollectionAssert.AreEquivalent(answers, values);
        }

        [Test()]
        // BlueForce: White, OppForce: Blue
        public void TestPlayerWorkerCannotMoveAtAll()
        {
            string validBoard = @"[
                                   [0, 0, 3, [2, 'Blue2'], 0]," +
                                  "[2, 2, 2, 2, 0]," +
                                  "[[0, 'White2'], 2, [0, 'White1'], 2, 0]," +
                                  "[2, 2, 2, 2, 0]," +
                                  "[0, [2, 'Blue1'], 3, 0, 0]" +
                                     "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player("white");


            ArrayList answers = new ArrayList(){};
            ArrayList values = newPlayer.GetNextBestPlay(newBoard, "White", "Blue");
            CollectionAssert.AreEquivalent(answers, values);
        }

        [Test()]
        // BlueForce: White, OppForce: Blue
        public void TestPlayerWorkerCannotPreventOppFromWinning()
        {
            string validBoard = @"[
                                   [0, 0, 3, [2, 'Blue2'], 0]," +
                                  "[2, 2, 2, 2, 0]," +
                                  "[1, 2, [0, 'White1'], 1, 0]," +
                                  "[[1, 'White2'], 2, 2, 2, 0]," +
                                  "[0, [2, 'Blue1'], 0, 0, 0]" +
                                     "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player("white");


            ArrayList answers = new ArrayList() { };
            ArrayList values = newPlayer.GetNextBestPlay(newBoard, "White", "Blue");
            CollectionAssert.AreEquivalent(answers, values);
        }
    }
}
