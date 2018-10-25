using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
namespace santorini
{
    [TestFixture()]
    public class RuleCheckerTest
    {
        [Test()]
        public void TestValidBoard()
        {
            Console.WriteLine("---TestValidBoard---");
            string validBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Assert.IsTrue(RuleChecker.IsBoardValid());
            Board.PrintBoard();

            string inValidPlayerNameBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,'yellow3'],[0,null],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            //// board parse
            Board inValidBoard = new Board(JArray.Parse(inValidPlayerNameBoard));
            Assert.IsFalse(RuleChecker.IsBoardValid());
            Board.PrintBoard();

            string inValidPlayerCountBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,'white3'],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board inValidBoard2 = new Board(JArray.Parse(inValidPlayerCountBoard));
            Assert.IsFalse(RuleChecker.IsBoardValid());
            Board.PrintBoard();

            string inValidHeightBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[5,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Assert.Throws<Exception>(
            delegate
            {
                var invalidBoard3 = new Board(JArray.Parse(inValidHeightBoard));
            });
        }

        [Test()]
        public void TestValidMove()
        {
            Console.WriteLine("---TestValidMove---");
            string validBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "W", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "N", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "S", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "NW", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "NE", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "SW", "N" }));
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "SE", "N" }));
        }

        [Test()]
        public void TestValidMoveRegardingHeight()
        {
            Console.WriteLine("---TestValidMove---");
            string validBoard = @"[
                                [[3,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[3,null],[2,null],[0,null]]," +
                               "[[0,null],[1,null],[1,'white2'],[0,'blue2'],[3,null]]," +
                               "[[0,null],[0,null],[2,null],[1,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[2,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Assert.IsFalse(RuleChecker.IsValidMove("white2", new string[] { "N", "E" })); // 1 -> 3
            Assert.IsFalse(RuleChecker.IsValidMove("blue2", new string[] { "W", "E" })); // 0 -> 3
            Assert.IsFalse(RuleChecker.IsValidMove("blue2", new string[] { "N", "E" })); // 0 -> 2

            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "W", "E" })); // 1 -> 1
            Assert.IsTrue(RuleChecker.IsValidMove("blue1", new string[] { "W", "E" })); // 2 -> 0
            Assert.IsTrue(RuleChecker.IsValidMove("blue1", new string[] { "N", "E" })); // 2 -> 1
            Assert.IsTrue(RuleChecker.IsValidMove("white1", new string[] { "S", "E" })); // 3 -> 0
        }

        [Test()]
        public void TestInvalidMove()
        {
            Console.WriteLine("---TestInvalidMove---");
            string validBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            // off the grid
            Assert.IsFalse(RuleChecker.IsValidMove("white1", new string[] { "W", "N" }));
            Assert.IsFalse(RuleChecker.IsValidMove("white1", new string[] { "N", "N" }));
            // move to occupied cell
            Assert.IsFalse(RuleChecker.IsValidMove("white2", new string[] { "E", "N" }));
            // undefined direction
            Assert.Throws<Exception>(
              delegate
              {
                RuleChecker.IsValidMove("white2", new string[] { "NEW", "N" });
              });
        }

        [Test()]
        public void TestValidVerticalMove()
        {
            Console.WriteLine("---TestValidVerticalMove---");
            string validBoard = @"[
                                [[2,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                             "[[0, null],[0,null],[3,null],[2,null],[0,null]]," +
                             "[[0,null],[0,null],[1,'white2'],[0,'blue2'],[0,null]]," +
                             "[[0,null],[0,null],[1,null],[0,null],[0,null]]," +
                             "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                           "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Assert.IsFalse(RuleChecker.IsValidMove("white2", new string[] { "N", "N" })); // more than two up
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "W", "N" })); // going down
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "NE", "N" })); // going up one
            Assert.IsTrue(RuleChecker.IsValidMove("white2", new string[] { "S", "N" })); // same level
            Assert.IsTrue(RuleChecker.IsValidMove("white1", new string[] { "S", "N" })); // going down two
        }

        [Test()]
        public void TestValidBuild()
        {
            Console.WriteLine("---TestValidBuild---");
            string validBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[2,null],[4,null],[0,null]]," +
                               "[[0,null],[1,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[3,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            string[] n_directions = new string[] { "W", "N" };
            string[] s_directions = new string[] { "W", "S" };
            string[] e_directions = new string[] { "W", "E" };
            string[] w_directions = new string[] { "W", "W" };
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", n_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", s_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", w_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("blue2", e_directions));
        }

        [Test()]
        public void TestValidBuildRegardingHeight()
        {
            Console.WriteLine("---TestValidMove---");
            string validBoard = @"[
                                [[3,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[3,null],[2,null],[0,null]]," +
                               "[[0,null],[4,null],[1,'white2'],[0,'blue2'],[3,null]]," +
                               "[[0,null],[0,null],[2,null],[1,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[2,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Assert.IsFalse(RuleChecker.IsValidBuild("white2", new string[] { "N", "W" })); // 4 -> 5
            Assert.IsFalse(RuleChecker.IsValidBuild("white2", new string[] { "N", "E" })); // occupied by blue2

            Assert.IsTrue(RuleChecker.IsValidBuild("white2", new string[] { "E", "N" })); // 3 -> 4
            Assert.IsTrue(RuleChecker.IsValidBuild("blue1", new string[] { "E", "W" })); // 0 -> 1
            Assert.IsTrue(RuleChecker.IsValidBuild("blue1", new string[] { "E", "N" })); // 1 -> 2
            Assert.IsTrue(RuleChecker.IsValidBuild("white1", new string[] { "E", "SE" })); // 0 -> 1

        }

        [Test()]
        public void TestInvalidBuild()
        {
            Console.WriteLine("---TestInvalidBuild---");
            string validBoard = @"[
                                [[0,'white1'],[0,null],[0,null],[0,null],[0,null]], " +
                               "[[0, null],[0,null],[0,null],[4,null],[0,null]]," +
                               "[[0,null],[0,null],[0,'white2'],[0,'blue2'],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,null],[0,null]]," +
                               "[[0,null],[0,null],[0,null],[0,'blue1'],[0,null]]" +
                             "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            string[] n_directions = new string[] { "W", "N" };
            string[] s_directions = new string[] { "W", "S" };
            string[] e_directions = new string[] { "W", "E" };
            string[] w_directions = new string[] { "W", "W" };

            // build on occupied
            Assert.IsFalse(RuleChecker.IsValidBuild("blue2", w_directions));
            Assert.IsFalse(RuleChecker.IsValidBuild("white2", e_directions));
            // build on level 4 building
            Assert.IsFalse(RuleChecker.IsValidBuild("blue2", n_directions));
            // off the grid
            Assert.IsFalse(RuleChecker.IsValidBuild("blue1", s_directions));
            Assert.IsFalse(RuleChecker.IsValidBuild("white1", w_directions));
            Assert.IsFalse(RuleChecker.IsValidBuild("white1", n_directions));

            //var finalPosition = Board.GetDesiredPosition("white2", directions[1]);
            //Assert.AreEqual(Board.Board_[finalPosition[0], finalPosition[1]].Height, 1);
        }
    }
}