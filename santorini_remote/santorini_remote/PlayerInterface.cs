using System;
using System.Collections;
using System.Collections.Generic;

namespace santorini_remote
{
    public interface PlayerInterface
    {
        List<List<int>> Place(Board board, string playerColor);
        List<List<int>> PlacePlayerWorkers(Board board, string playerColor); // attempt to make Place() the public interface 
                                              // while keeping this method private
                                              // failed due to protection level for testing
        ArrayList Play(Board board);
        ArrayList GetNextBestPlay(Board board, string playerColor, string oppColor);
    }
}
