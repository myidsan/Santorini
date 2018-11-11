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
                                [[2, 'White1'], 3, 0, 0, [0, 'Blue']]," +
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
        // Gets the one turn play(move and build) for the player worker
        // BlueForce: Blue, OppForce: White
        // This is the goldmine case for this method
        public void TestPreventLoseInOneMove()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, 0]," +
                               "[0, [2, 'Blue2'], [1, 'Blue1'], 0, 0]," +
                               "[0, 2, 1, [1, 'White2'], 0]," +
                               "[0, [2, 'White1'], 3, 0, 0]," +
                               "[0, 0, 0, 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));

            ArrayList answers = new ArrayList
            {
                new ArrayList { "Blue1", new ArrayList { "S", "S" } },
                new ArrayList { "Blue1", new ArrayList { "SW", "SE" } },
                new ArrayList { "Blue2", new ArrayList { "S", "SE" } },
                new ArrayList { "Blue2", new ArrayList { "SE", "S" } },
            };
            ArrayList values = Strategy.PreventLoseInOneTurn(newBoard, "Blue", "White");
            CollectionAssert.AreEquivalent(answers, values);
        }



        [Test()]
        // 2. two workers can win in one move - all directions
        // BlueForce: White, OppForce: Blue
        public void TestWhite1White2WinsInOneMove()
        {
            string validBoard = @"[
                                [[2, 'White1'], 3, 0, 0, [0, 'Blue']]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[3, 0, 0, 0, 0]," +
                                "[[2, 'White2'], 2, 0, 0, [0, 'Blue2']]" +
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
                                [[2, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
                                "[1, 0, 0, 0, 0]," +
                                "[0, 0, 0, 0, 0]," +
                                "[3, 0, 0, 0, 0]," +
                                "[[2, 'White2'], 2, 0, 0, [0, 'Blue2']]" +
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
                                [[2, 'White1'], 3, 0, [0, 'Blue1'], 0]," +
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
        // BlueForce: Blue, OppForce: White
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
        // provide a path that a worker cannot go in one play(move and build)
        // BlueForce: White, OppForce: Build
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

        [Test()]
        // Get legal and valid moves for the player when the opponent is going to win anyway
        // BlueForce: White, OppForce: Build
        public void TestDefaultPlay()
        {
            string validBoard = @"[
                                [[2, 'White1'], 0, [2, 'Blue1'], 3, [2, 'Blue2']]," +
                               "[0, 0, 0, 0, 3]," +
                               "[0, 0, 0, 0, 0]," +
                               "[0, 0, 0, 0, 0]," +
                               "[[0, 'White2'], 0, 0, 3, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));

            ArrayList answers = new ArrayList()
            {

            };
            ArrayList values = Strategy.DefaultPlay(newBoard, "White", "Blue");

        }

        [Test()]
        // BlueForce: Blue, OppForce: White
        public void TestPreventLoseInNTurn1_Board1()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, [0, 'Blue2']]," +
                               "[0, 0, 0, [1, 'Blue1'], 0]," +
                               "[0, 0, 3, 2, 0]," +
                               "[0, 2, 2, 1, 0]," +
                               "[0, [2, 'White1'], [2, 'White2'], 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            ArrayList answers = Strategy.GetNextBestPlayStrategy(newBoard, "Blue", "White");
            ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
            Console.WriteLine(JSONEncoder.DumpJson(value));
            CollectionAssert.AreEquivalent(answers, value);
            Assert.AreEqual(answers.Count, value.Count);
        }

        [Test()]
        // BlueForce: Blue, OppForce: White
        public void TestPreventLoseInNTurn2_Board1()
        {
            string validBoard = @"[
                                [0, 0, 0, 0, [0, 'Blue2']]," +
                               "[0, 0, 0, [1, 'Blue1'], 0]," +
                               "[0, 0, 3, 2, 0]," +
                               "[0, 2, 2, 1, 0]," +
                               "[0, [2, 'White1'], [2, 'White2'], 0, 0]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            ArrayList answers = new ArrayList()
            {
                //new ArrayList {"Blue1", new ArrayList { "S", "N" } },
                //new ArrayList {"Blue1", new ArrayList { "S", "E" } },
                //new ArrayList {"Blue1", new ArrayList { "S", "S" } },
                //new ArrayList {"Blue1", new ArrayList { "S", "NW" } },
                //new ArrayList {"Blue1", new ArrayList { "S", "NE" } },
                //new ArrayList {"Blue1", new ArrayList { "S", "SE" } }
            };
            ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
            Console.WriteLine(JSONEncoder.DumpJson(value));
            CollectionAssert.AreEquivalent(answers, value);
            Assert.AreEqual(answers.Count, value.Count);
        }

        [Test()]
        // BlueForce: Blue, OppForce: White
        public void TestPreventLoseInTurn2_Board5()
        {
            string validBoard = @"[
                                [4, 4, 4, 4, 4]," +
                               "[4, 2, [1, 'Blue1'], 4, [1, 'Blue2']]," +
                               "[1, 1, 1, 4, 4]," +
                               "[1, 1, 2, 1, 1]," +
                               "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            ArrayList answers = new ArrayList() 
            {
                new ArrayList {"Blue1", new ArrayList { "S", "S" } },
                new ArrayList {"Blue1", new ArrayList { "SW", "SE" } }
            };
            ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
            Console.WriteLine(JSONEncoder.DumpJson(value));
            CollectionAssert.AreEquivalent(answers, value);
            Assert.AreEqual(2, value.Count);
        }

        [Test()]
        // BlueForce: Blue, OppForce: White
        public void TestPreventLoseInTurn3_Board5()
        {
            string validBoard = @"[
                                [4, 4, 4, 4, 4]," +
                               "[4, 2, [1, 'Blue1'], 4, [1, 'Blue2']]," +
                               "[1, 1, 1, 4, 4]," +
                               "[1, 1, 2, 1, 1]," +
                               "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
                                  "]";
            Board newBoard = new Board(JArray.Parse(validBoard));
            ArrayList answers = new ArrayList()
            {
            };
            ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 3);
            Console.WriteLine(JSONEncoder.DumpJson(value));
            CollectionAssert.AreEquivalent(answers, value);
            Assert.AreEqual(0, value.Count);
        }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurn3_Board1()
        //    {
        //        string validBoard = @"[
        //                            [0, 0, 0, 0, [0, 'Blue2']]," +
        //                           "[0, 0, 0, [1, 'Blue1'], 0]," +
        //                           "[0, 0, 3, 2, 0]," +
        //                           "[0, 2, 2, 1, 0]," +
        //                           "[0, [2, 'White1'], [2, 'White2'], 0, 0]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList answers = new ArrayList()
        //        {

        //        };
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 3);
        //        Console.WriteLine(JSONEncoder.DumpJson(value));
        //        CollectionAssert.AreEquivalent(answers, value);
        //        Assert.AreEqual(answers.Count, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn1_InitBoard()
        //    {
        //        string validBoard = @"[
        //                            [[0, 'Blue1'], 0, 0, 0, [0, 'Blue2']]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[[0, 'White1'], 0, 0, 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        //Console.WriteLine(JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(36, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn2_InitBoard()
        //    {
        //        string validBoard = @"[
        //                            [[0, 'Blue1'], 0, 0, 0, [0, 'Blue2']]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[[0, 'White1'], 0, 0, 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        //Console.WriteLine(JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(36, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn3_InitBoard()
        //    {
        //        string validBoard = @"[
        //                            [[0, 'Blue1'], 0, 0, 0, [0, 'Blue2']]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[0, 0, 0, 0, 0]," +
        //                           "[[0, 'White1'], 0, 0, 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        //Console.WriteLine(JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(36, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn1_Board2()
        //    {
        //        string validBoard = @"[
        //                             [0, 0, 0, 0, 0]," +
        //                            "[0, 0, [1, 'Blue1'], 1, [0, 'Blue2']]," +
        //                            "[2, 2, 2, 2, 2]," +
        //                            "[0, 0, 2, 0, 0]," +
        //                            "[0, 0, [2, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList answers = new ArrayList()
        //        {
        //            new ArrayList {"Blue1", new ArrayList{ "S", "N" } }, new ArrayList {"Blue1", new ArrayList{ "S", "E" } },
        //            new ArrayList {"Blue1", new ArrayList{ "S", "W" } }, new ArrayList {"Blue1", new ArrayList{ "S", "NE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "S", "NW" } }, new ArrayList {"Blue1", new ArrayList{ "S", "SE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "S", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SW", "E" } }, new ArrayList {"Blue1", new ArrayList{ "SW", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SW", "S" } }, new ArrayList {"Blue1", new ArrayList{ "SW", "N" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SW", "NE" } }, new ArrayList {"Blue1", new ArrayList{ "SW", "NW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SW", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "W", "E" } }, new ArrayList {"Blue1", new ArrayList{ "W", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "W", "S" } }, new ArrayList {"Blue1", new ArrayList{ "W", "N" } },
        //            new ArrayList {"Blue1", new ArrayList{ "W", "NE" } }, new ArrayList {"Blue1", new ArrayList{ "W", "NW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "W", "SE" } }, new ArrayList {"Blue1", new ArrayList{ "W", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "NW", "E" } }, new ArrayList {"Blue1", new ArrayList{ "NW", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "NW", "S" } }, new ArrayList {"Blue1", new ArrayList{ "NW", "SE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "NW", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "N", "E" } }, new ArrayList {"Blue1", new ArrayList{ "N", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "N", "S" } }, new ArrayList {"Blue1", new ArrayList{ "N", "SE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "N", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "NE", "E" } }, new ArrayList {"Blue1", new ArrayList{ "NE", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "NE", "S" } }, new ArrayList {"Blue1", new ArrayList{ "NE", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "E", "W" } }, new ArrayList {"Blue1", new ArrayList{ "E", "S" } },
        //            new ArrayList {"Blue1", new ArrayList{ "E", "N" } }, new ArrayList {"Blue1", new ArrayList{ "E", "NE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "E", "NW" } }, new ArrayList {"Blue1", new ArrayList{ "E", "SE" } },
        //            new ArrayList {"Blue1", new ArrayList{ "E", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SE", "E" } }, new ArrayList {"Blue1", new ArrayList{ "SE", "W" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SE", "S" } }, new ArrayList {"Blue1", new ArrayList{ "SE", "N" } },
        //            new ArrayList {"Blue1", new ArrayList{ "SE", "SE" } }, new ArrayList {"Blue1", new ArrayList{ "SE", "NW" } },

        //            new ArrayList {"Blue2", new ArrayList{ "W", "E" } }, new ArrayList {"Blue2", new ArrayList{ "W", "S" } },
        //            new ArrayList {"Blue2", new ArrayList{ "W", "N" } }, new ArrayList {"Blue2", new ArrayList{ "W", "NE" } },
        //            new ArrayList {"Blue2", new ArrayList{ "W", "NW" } }, new ArrayList {"Blue2", new ArrayList{ "W", "SE" } },
        //            new ArrayList {"Blue2", new ArrayList{ "W", "SW" } },
        //            new ArrayList {"Blue2", new ArrayList{ "NW", "E" } }, new ArrayList {"Blue2", new ArrayList{ "NW", "W" } },
        //            new ArrayList {"Blue2", new ArrayList{ "NW", "S" } }, new ArrayList {"Blue2", new ArrayList{ "NW", "SE" } },
        //            new ArrayList {"Blue2", new ArrayList{ "N", "W" } }, new ArrayList {"Blue2", new ArrayList{ "N", "S" } },
        //            new ArrayList {"Blue2", new ArrayList{ "N", "SW" } }
        //        };
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        Console.WriteLine("value: " + JSONEncoder.DumpJson(value));
        //        CollectionAssert.AreEquivalent(answers, value);
        //        Assert.AreEqual(63, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn2_Board2()
        //    {
        //        string validBoard = @"[
        //                             [0, 0, 0, 0, 0]," +
        //                            "[0, 0, [1, 'Blue1'], 1, [0, 'Blue2']]," +
        //                            "[2, 2, 2, 2, 2]," +
        //                            "[0, 0, 2, 0, 0]," +
        //                            "[0, 0, [2, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList answers = new ArrayList()
        //        {
        //            // add here
        //        };
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
        //        Console.WriteLine("value: " + JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(6, value.Count);
        //        CollectionAssert.AreEquivalent(answers, value);

        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn1_Board3()
        //    {
        //        string validBoard = @"[
        //                             [0, 0, 0, 0, 0]," +
        //                            "[2, 2, [1, 'Blue1'], 2, [1, 'Blue2']]," +
        //                            "[3, 3, 3, 3, 3]," +
        //                            "[2, 2, 2, 2, 2]," +
        //                            "[0, 0, [2, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        Assert.AreEqual(43, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn2_Board3()
        //    {
        //        string validBoard = @"[
        //                             [0, 0, 0, 0, 0]," +
        //                            "[2, 2, [1, 'Blue1'], 2, [1, 'Blue2']]," +
        //                            "[3, 3, 3, 3, 3]," +
        //                            "[2, 2, 2, 2, 2]," +
        //                            "[0, 0, [2, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
        //        Assert.AreEqual(0, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn3_Board3()
        //    {
        //        string validBoard = @"[
        //                             [0, 0, 0, 0, 0]," +
        //                            "[2, 2, [1, 'Blue1'], 2, [1, 'Blue2']]," +
        //                            "[3, 3, 3, 3, 3]," +
        //                            "[2, 1, 2, 1, 2]," +
        //                            "[0, 0, [2, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 3);
        //        Assert.AreEqual(0, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn1_Board4()
        //    {
        //        string validBoard = @"[
        //                             [4, 4, 4, 4, 4]," +
        //                            "[4, 2, [1, 'Blue1'], 2, [1, 'Blue2']]," +
        //                            "[1, 1, 1, 4, 4]," +
        //                            "[1, 1, 1, 1, 1]," +
        //                            "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        //ArrayList answers = new ArrayList()
        //        //{
        //        //    new ArrayList {"Blue1", new ArrayList{ "W" , "E" } }, new ArrayList {"Blue1", new ArrayList{ "W" , "S" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "W" , "SE" } }, new ArrayList {"Blue1", new ArrayList{ "W" , "SW" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "E" , "W" } }, new ArrayList {"Blue1", new ArrayList{ "E" , "SW" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "S" , "W" } }, new ArrayList {"Blue1", new ArrayList{ "S" , "N" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "S" , "NE" } }, new ArrayList {"Blue1", new ArrayList{ "S" , "NW" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "SW" , "E" } }, new ArrayList {"Blue1", new ArrayList{ "SW" , "W" } },
        //        //    new ArrayList {"Blue1", new ArrayList{ "SW" , "N" } }, new ArrayList {"Blue1", new ArrayList{ "SW" , "NE" } },
        //        //    new ArrayList {"Blue2", new ArrayList{ "W", "SW" } }
        //        //};
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 1);
        //        Assert.AreEqual(22, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    public void TestPreventLoseInNTurnIn2_Board4()
        //    {
        //        string validBoard = @"[
        //                             [4, 4, 4, 4, 4]," +
        //                            "[4, 2, [1, 'Blue1'], 2, [1, 'Blue2']]," +
        //                            "[1, 1, 1, 4, 4]," +
        //                            "[1, 1, 1, 1, 1]," +
        //                            "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
        //        Console.WriteLine("value: " + JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(22, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    // Blue plays should not result in providing a connected 1-2-2 block to White
        //    public void TestPreventLoseInNTurnIn2_Board5()
        //    {
        //        string validBoard = @"[
        //                             [4, 4, 4, 4, 4]," +
        //                            "[4, 2, [1, 'Blue1'], 4, [1, 'Blue2']]," +
        //                            "[1, 1, 1, 4, 4]," +
        //                            "[1, 1, 2, 1, 1]," +
        //                            "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList answers = new ArrayList()
        //        {
        //            new ArrayList {"Blue1", new ArrayList { "S", "S" } }, new ArrayList {"Blue1", new ArrayList { "S", "N" } },
        //            new ArrayList {"Blue1", new ArrayList { "S", "NW" } },
        //            new ArrayList {"Blue1", new ArrayList { "SW", "W" } }, new ArrayList {"Blue1", new ArrayList { "SW", "N" } },
        //            new ArrayList {"Blue1", new ArrayList { "SW", "NE" } }, new ArrayList {"Blue1", new ArrayList { "SW", "SE" } },
        //            new ArrayList {"Blue1", new ArrayList { "SW", "SW" } },
        //            new ArrayList {"Blue1", new ArrayList { "W", "E" } }, new ArrayList {"Blue1", new ArrayList { "W", "S" } },
        //            new ArrayList {"Blue1", new ArrayList { "W", "SE" } }, new ArrayList {"Blue1", new ArrayList { "W", "SW" } }
        //        };
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 2);
        //        //Console.WriteLine("value: " + JSONEncoder.DumpJson(value));
        //        CollectionAssert.AreEquivalent(answers, value);
        //        Assert.AreEqual(12, value.Count);
        //    }

        //    [Test()]
        //    // BlueForce: Blue, OppForce: White
        //    // This case, White can created a connected 1-2-2 section on its own thus always winning on 3rd move
        //    public void TestPreventLoseInNTurnIn3_Board5()
        //    {
        //        string validBoard = @"[
        //                             [4, 4, 4, 4, 4]," +
        //                            "[4, 2, [1, 'Blue1'], 4, [1, 'Blue2']]," +
        //                            "[1, 1, 1, 4, 4]," +
        //                            "[1, 1, 2, 1, 1]," +
        //                            "[0, 0, [1, 'White1'], 0, [0, 'White2']]" +
        //                              "]";
        //        Board newBoard = new Board(JArray.Parse(validBoard));
        //        ArrayList value = Strategy.PreventLoseInNTurn(newBoard, "Blue", "White", 3);
        //        Console.WriteLine("value: " + JSONEncoder.DumpJson(value));
        //        Assert.AreEqual(0, value.Count);
        //    }
    }
}
