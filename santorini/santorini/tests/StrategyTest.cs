using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

            ArrayList answer = new ArrayList
            {
                new ArrayList { "White1", new ArrayList { "E" } },
                new ArrayList { "White1", new ArrayList { "S" } }
            };
            //string JSONresult = JsonConvert.SerializeObject(answer);
            //Console.WriteLine(JSONresult);
            CollectionAssert.AreEquivalent(answer, Strategy.WinInOneTurn(newBoard, "White"));
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


            ArrayList answer = new ArrayList
            {
                new ArrayList { "White2", new ArrayList { "N" } },
                new ArrayList { "White1", new ArrayList { "E" } }

            };
            CollectionAssert.AreEquivalent(answer, Strategy.WinInOneTurn(newBoard, "White"));
        }

        [Test()]
        // 3. a workers can prevent lose in one move by building a level 4
        // BlueForce: Blue, OppForce: White
        public void TestBlue1PreventLoseInOne()
        {
            string validBoard = @"[
                                [[0, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[3, 0, 0, 0, 0]," +
                                "[[0, 'White2'], 2, 0, 0, [0, 'Blue2']]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();

            //JArray answer2 = new JArray
            //{
            //    new JArray {"Blue1", new JArray { "W", "W" } }
            //};

            ArrayList answer = new ArrayList
            {
                new ArrayList {"Blue1", new ArrayList { "W", "W" } },
                new ArrayList {"Blue1", new ArrayList { "SW", "NW" } },
            };
            //string JSONresult = JsonConvert.SerializeObject(answer);
            //Console.WriteLine(JSONresult);
            CollectionAssert.AreEquivalent(answer, Strategy.PreventLoseInOneTurn(newBoard, "Blue", "White"));
        }

        [Test()]
        // 4. two workers can prevent lose in one move by building a level 4
        // BlueForce: Blue, OppForce: White
        public void TestBlue1Blue2PreventLoseInOne()
        {
            string validBoard = @"[
                                [[0, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
                                "[1, 3, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[[0, 'Blue2'], 0, 0, 0, 0]," +
                                "[[0, 'White2'], 2, 0, 0, 0]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            Player newPlayer = new Player();

            ArrayList answer = new ArrayList
            {
                new ArrayList {"Blue1", new ArrayList { "W", "W"}},
                new ArrayList {"Blue1", new ArrayList { "SW", "NW"}},
                new ArrayList {"Blue1", new ArrayList { "W", "SW"}},
                new ArrayList {"Blue1", new ArrayList { "SW", "W"}},
                new ArrayList {"Blue2", new ArrayList {"N", "NE"}},
                new ArrayList {"Blue2", new ArrayList { "NE", "N"}}
            };
            CollectionAssert.AreEquivalent(answer, Strategy.PreventLoseInOneTurn(newBoard, "Blue", "White"));
        }


        // 2. the player can win in one move - one direction
        // 3. the player can block opponent from winning in one move - one direction

        // helper functions test
        [Test()]
        // provide a path that a worker can go in one play(move and build)
        public void TestGetPossiblePathValid()
        {
            string validBoard = @"[
                                 [0, 2, [0, 'Blue1'], 0, 3]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 1, 0, 0]," +
                                "[0, 0, 0, 1, 0]," +
                                "[0, 2, [0, 'White1'], 0, 0]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            List<int> answer = new List<int> { 2, 0 };
            //CollectionAssert.AreEquivalent(answer, Strategy.GetPossiblePath(newBoard, "Blue1", new List<int> { 2, 2 }));
            List<int> value = Strategy.GetPossiblePath(newBoard, "Blue1", new List<int> { 2, 2 });
            Assert.IsTrue(answer.SequenceEqual(value));

            List<int> answer2 = new List<int> { -2, 0 };
            List<int> value2 = Strategy.GetPossiblePath(newBoard, "White1", new List<int> { 2, 2 });
            Assert.IsTrue(answer2.SequenceEqual(value2));

           
        }

        [Test()]
        // 
        public void TestGetPossiblePathInvalid()
        {
            string validBoard = @"[
                                 [0, 2, [0, 'Blue1'], 0, 3]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 1, 0, 0]," +
                                "[0, 0, 0, 1, 0]," +
                                "[0, 2, [0, 'White1'], 0, 0]" +
                                 "]";
            Board newBoard = new Board(JArray.Parse(validBoard));

            List<int> answer3 = new List<int>();
            List<int> value3 = Strategy.GetPossiblePath(newBoard, "White1", new List<int> { 0, 4 });
            value3.ForEach(Console.WriteLine);
            Assert.IsTrue(answer3.SequenceEqual(value3));

            //List<int> answer4 = new List<int> { 2, -2 };
            List<int> value4 = Strategy.GetPossiblePath(newBoard, "Blue1", new List<int> { 3, 0 });
            Assert.IsTrue(answer3.SequenceEqual(value4));
        }

      
    }
}
