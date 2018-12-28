using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using static santorini_remote.JSONEncoder;

namespace santorini_remote.tests
{
    [TestFixture()]
    public class RefereeTest
    {
        [Test()]
        public void TestRefereeConstructor()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);

            string initBoardString = @"[
                                [0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";
            Board initBoard = new Board(JArray.Parse(initBoardString));
            Console.WriteLine(initBoard.Board_[0, 0]);
            Console.WriteLine(referee.InternalBoard.Board_[0, 0]);
            Assert.IsTrue(initBoard.IsEqual(referee.InternalBoard));
        }

        // Deprecated
        //[Test()]
        //public void TestReferee_playerOrder()
        //{

        //    Player one = new Player("San");
        //    Player two = new Player("Stan");

        //    Referee referee = new Referee(one, two);

        //    List<Player> answer = new List<Player>() { };
        //    answer.Add(one);
        //    answer.Add(two);
        //    CollectionAssert.AreEqual(answer, referee.Order);

        //    Player three = new Player("Alice");
        //    Player four = new Player("Bob");
        //    Referee referee2 = new Referee(three, four);

        //    List<Player> answer2 = new List<Player>() { };
        //    answer2.Add(four);
        //    answer2.Add(three);
        //    CollectionAssert.AreEqual(answer2, referee2.Order);
        //}

        // Deprecated
        //[Test()]
        //public void TestRefereeConstructor_sameColorPlayers()
        //{

        //    Player one = new Player("blue", "San");
        //    Player two = new Player("blue", "Stan");

        //    Assert.Throws<FormatException>(
        //    delegate
        //    {
        //        Referee referee = new Referee(one, two);
        //    });
        //}

        [Test()]
        public void TestRefereeName()
        {
            Player one = new Player("blue");
            Player two = new Player("white");

            Referee referee = new Referee(one, two);
            Assert.AreEqual("blue", referee.Name("San"));
            Assert.AreEqual("white", referee.Name("Stan"));
        }

        [Test()]
        public void TestPlacement()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);
            string initBoardString = @"[
                                [0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";
            Board initBoard = new Board(JArray.Parse(initBoardString));

            referee.Name("San");
            referee.Name("Stan");

            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);

            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 4, 0 });
            whiteCoords.Add(new JArray { 4, 4 });
            referee.PlaceWorkers(whiteCoords);

            string placementBoardString = @"[
                                [[0, 'blue1'],0,0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[[0, 'white1'], 0,0,0,[0, 'white2']]" +
                                 "]";

            Board placementBoard = new Board(JArray.Parse(placementBoardString));
            Console.WriteLine(JSONEncoder.DumpJson(placementBoard.DumpBoard()));
            Console.WriteLine(JSONEncoder.DumpJson(referee.InternalBoard.DumpBoard()));
            Assert.IsTrue(placementBoard.IsEqual(referee.InternalBoard));
        }

        [Test()]
        // white player violates the placement (occupied)
        public void TestPlacement_InvalidPlace_occupied()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);
            string initBoardString = @"[
                                [0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";

            string bluePlacementBoardString = @"[
                                [[0, 'blue1'],0,0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";


            Board initBoard = new Board(JArray.Parse(initBoardString));
            Board placementBoard = new Board(JArray.Parse(bluePlacementBoardString));

            referee.Name("San");
            referee.Name("Stan");

            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);
            Assert.IsTrue(placementBoard.IsEqual(referee.InternalBoard));
            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 4, 0 });
            whiteCoords.Add(new JArray { 4, 4 });
            referee.PlaceWorkers(whiteCoords);
            Assert.IsFalse(placementBoard.IsEqual(referee.InternalBoard));
        }

        [Test()]
        // white player violates the placement (OutOfBound)
        public void TestPlacement_InvalidPlace_OutOfBound()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);
            string initBoardString = @"[
                                [0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";

            string bluePlacementBoardString = @"[
                                [[0, 'blue1'],0,0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]" +
                                 "]";


            Board initBoard = new Board(JArray.Parse(initBoardString));
            Board placementBoard = new Board(JArray.Parse(bluePlacementBoardString));

            referee.Name("San");
            referee.Name("Stan");

            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);
            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 0, 2 });
            whiteCoords.Add(new JArray { 5, 4 }); 
            referee.PlaceWorkers(whiteCoords);
            Assert.IsTrue(placementBoard.IsEqual(referee.InternalBoard));
        }

        [Test()]
        // test for basic Direction command
        public void TestDirection()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);

            // place the workers in the corners
            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);
            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 4, 0 });
            whiteCoords.Add(new JArray { 4, 4 });
            referee.PlaceWorkers(whiteCoords);

            referee.ExecutePlay("blue1", new String[] { "E", "W" });
            referee.ExecutePlay("white1", new String[] { "N", "E" });

            // expected board after 1 turn
            string afterPlayBoardString = @"[
                                [1,[0, 'blue1'],0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[[0, 'white1'],1,0,0,0]," +
                               "[0, 0,0,0,[0, 'white2']]" +
                                 "]";
            Board afterPlayBoard = new Board(JArray.Parse(afterPlayBoardString));

            Assert.IsTrue(afterPlayBoard.IsEqual(referee.InternalBoard));
        }

        [Test()]
        // test for basic Direction command
        public void TestDirection_ConsecutiveExecutions()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);

            // place the workers in the corners
            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);
            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 4, 0 });
            whiteCoords.Add(new JArray { 4, 4 });
            referee.PlaceWorkers(whiteCoords);

            referee.ExecutePlay("blue1", new string[] { "E", "W" });
            referee.ExecutePlay("white1", new string[] { "N", "E" });
            // expected board after 1 turn
            string afterPlayOneBoardString = @"[
                                [1,[0, 'blue1'],0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[[0, 'white1'],1,0,0,0]," +
                               "[0, 0,0,0,[0, 'white2']]" +
                                 "]";
            Board afterOnePlay = new Board(JArray.Parse(afterPlayOneBoardString));
            Assert.IsTrue(afterOnePlay.IsEqual(referee.InternalBoard));

            referee.ExecutePlay("blue2", new string[] { "W", "S" });
            referee.ExecutePlay("white2", new string[] { "N", "N" });
            // expected board after 2 turn
            string afterPlayTwoBoardString = @"[
                                 [1,[0, 'blue1'],0,[0, 'blue2'],0]," +
                                "[0,0,0,1,0]," +
                                "[0,0,0,0,1]," +
                                "[[0, 'white1'],1,0,0,[0, 'white2']]," +
                                "[0,0,0,0,0]" +
                                "]";
            Board afterTwoPlay = new Board(JArray.Parse(afterPlayTwoBoardString));
            Assert.IsTrue(afterTwoPlay.IsEqual(referee.InternalBoard));

            referee.ExecutePlay("blue1", new string[] { "SE", "SE" });
            referee.ExecutePlay("white2", new string[] { "W", "N" });
            // expected board after 2 turn
            string afterPlayThreeBoardString = @"[
                                 [1,0,0,[0, 'blue2'],0]," +
                                "[0,0,[0, 'blue1'],1,0]," +
                                "[0,0,0,2,1]," +
                                "[[0, 'white1'],1,0,[0, 'white2'],0]," +
                                "[0,0,0,0,0]" +
                                "]";
            Board afterThreePlay = new Board(JArray.Parse(afterPlayThreeBoardString));
            Assert.IsTrue(afterThreePlay.IsEqual(referee.InternalBoard));
        }

        [Test()]
        // winning move -- how to check the return value;
        public void TestDirection_Winning()
        {
            Player one = new Player("San");
            Player two = new Player("Stan");

            Referee referee = new Referee(one, two);
            string placementBoardString = @"[
                                [[0, 'blue1'],0,0,0,[0, 'blue2']]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[[0, 'white1'], 0,0,0,[0, 'white2']]" +
                                 "]";
            Board placedBoard = new Board(JArray.Parse(placementBoardString));
            JArray blueCoords = new JArray { };
            blueCoords.Add(new JArray { 0, 0 });
            blueCoords.Add(new JArray { 0, 4 });
            referee.PlaceWorkers(blueCoords);
            JArray whiteCoords = new JArray { };
            whiteCoords.Add(new JArray { 4, 0 });
            whiteCoords.Add(new JArray { 4, 4 });
            referee.PlaceWorkers(whiteCoords);

            referee.ExecutePlay("blue1", new string[] { "S", "N" });
            referee.ExecutePlay("blue1", new string[] { "N", "S" });
            referee.ExecutePlay("blue1", new string[] { "S", "N" });
            referee.ExecutePlay("blue1", new string[] { "N", "S" });
            referee.ExecutePlay("blue1", new string[] { "S", "N" });

            // any way to create a already almost-winning-board to test the funcionality not the scenario?
            string blueWinBoardString = @"[
                                [3,0,0,0,[0, 'blue2']]," +
                               "[[2, 'blue1'],0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[0,0,0,0,0]," +
                               "[[0, 'white1'], 0,0,0,[0, 'white2']]" +
                                 "]";
            Board blueWinBoard = new Board(JArray.Parse(blueWinBoardString));
            Assert.AreEqual(null,referee.ExecutePlay("blue1", new string[] { "N" }));
        }

        //[Test()]
        //public void TestDirection_Losing()
        //{

        //}
    }
}
