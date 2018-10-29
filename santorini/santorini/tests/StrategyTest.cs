using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace santorini.tests
{
    [TestFixture()]
    public class StrategyTest
    {
        [Test()]
        // 1. a worker can win in one move - all directions
        public void TestWhite1WinsInOneMove()
        {
            string validBoard = @"[
                                [[0, 'White1'], 3, 0, 0, [0, 'Blue']]," +
                                "[3, 0, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[1, 0, 0, 0, 0]," +
                                "[[0, 'White2'], 0, 0, 0, [0, 'Blue2']]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();

            List<dynamic> answer = new List<dynamic>
            {
                new List<dynamic> { "White1", new List<dynamic> {"E", "S"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "E"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "N"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "W"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "NW"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "SW"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "NE"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "SE"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "S"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "E"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "N"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "W"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "NW"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "SW"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "NE"} },
                new List<dynamic> { "White1", new List<dynamic> {"S", "SE"} }
            };
            CollectionAssert.AreEquivalent(Strategy.WinInOneTurn(newBoard, "White"), answer);
        }


        [Test()]
        // 2. two workers can win in one move - all directions
        public void TestWhite1White2WinsInOneMove()
        {
            string validBoard = @"[
                                [[0, 'White1'], 3, 0, 0, [0, 'Blue']]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[3, 0, 0, 0, 0]," +
                                "[[0, 'White2'], 2, 0, 0, [0, 'Blue2']]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();

            List<dynamic> answer = new List<dynamic>
            {
                new List<dynamic> { "White1", new List<dynamic> {"E", "S"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "E"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "N"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "W"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "NW"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "SW"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "NE"} },
                new List<dynamic> { "White1", new List<dynamic> {"E", "SE"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "S"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "E"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "N"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "W"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "NW"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "SW"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "NE"} },
                new List<dynamic> { "White2", new List<dynamic> {"N", "SE"} }
            };
            CollectionAssert.AreEquivalent(Strategy.WinInOneTurn(newBoard, "White"), answer);
        }

        //[Test()]
        //// 3. a workers can prevent lose in one move by building a level 4
        //public void TestBlue1PreventLoseInOne()
        //{
        //    string validBoard = @"[
        //                        [[0, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
        //                        "[1, 0, 0, 0, 0]," +
        //                        "[0, 0, 0, 0, 0]," +
        //                        "[3, 0, 0, 0, 0]," +
        //                        "[[0, 'White2'], 2, 0, 0, [0, 'Blue2']]" +
        //                         "]";
        //    Board newBoard = new Board(JArray.Parse(validBoard));
        //    Player newPlayer = new Player();

        //    List<dynamic> answer = new List<dynamic>
        //    {
        //        new List<dynamic> {"Blue1", new List<dynamic> {"E", "S"}}
        //    };
        //    CollectionAssert.AreEqual(Strategy.PreventLoseInOneTurn(newBoard, "White"), answer);
        //}

        //[Test()]
        //// 4. two workers can prevent lose in one move by building a level 4
        //public void TestBlue1Blue2PreventLoseInOne()
        //{
        //    string validBoard = @"[
        //                        [[0, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
        //                        "[1, 3, 0, 0, 0]," +
        //                        "[0, 0, 0, 0, 0]," +
        //                        "[[0, 'Blue2'], 0, 0, 0, 0]," +
        //                        "[[0, 'White2'], 2, 0, 0, 0]" +
        //                         "]";
        //    Board newBoard = new Board(JArray.Parse(validBoard));
        //    Player newPlayer = new Player();

        //    List<dynamic> answer = new List<dynamic>
        //    {
        //        new List<dynamic> {"Blue1", new List<dynamic> {"W", "W"}},
        //        new List<dynamic> {"Blue1", new List<dynamic> {"W", "SW"}},
        //        new List<dynamic> {"Blue1", new List<dynamic> {"SW", "W"}},
        //        new List<dynamic> {"Blue2", new List<dynamic> {"N", "NE"}},
        //        new List<dynamic> {"Blue2", new List<dynamic> {"NE", "N"}}
        //    };
        //    CollectionAssert.AreEqual(Strategy.PreventLoseInOneTurn(newBoard, "White"), answer);
        //}


        // 2. the player can win in one move - one direction
        // 3. the player can block opponent from winning in one move - one direction
    }
}
