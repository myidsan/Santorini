using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
namespace santorini
{
    [TestFixture()]
    public class SantoriniTest
    {
        [Test()]
        public void TestValidBoard()
        {
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
        public void TestInvalidMove()
        {
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
        public void TestValidBuild()
        {
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
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", n_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", s_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("white2", w_directions));
            Assert.IsTrue(RuleChecker.IsValidBuild("blue2", e_directions));

            // build on occupied
            Assert.IsFalse(RuleChecker.IsValidBuild("blue2", w_directions));
            // build on level 4 building
            Assert.IsFalse(RuleChecker.IsValidBuild("blue2", n_directions));
            // off the grid
            Assert.Throws<IndexOutOfRangeException>(
                delegate
                {
                    RuleChecker.IsValidBuild("blue1", s_directions);
                });
        }

        [Test()]
        public void TestBuild()
        {
            //var finalPosition = Board.GetDesiredPosition("white2", directions[1]);
            //Assert.AreEqual(Board.Board_[finalPosition[0], finalPosition[1]].Height, 1);
        }

    }
}
