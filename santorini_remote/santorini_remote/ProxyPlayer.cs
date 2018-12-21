using System;
using System.Collections;
using System.Collections.Generic;

namespace santorini_remote
{
    public class ProxyPlayer : PlayerInterface
    { 
        public ProxyPlayer()
        {
        }

        public ArrayList GetNextBestPlay(Board board, string playerColor, string oppColor)
        {
            throw new NotImplementedException();
        }

        public List<List<int>> Place(Board board, string playerColor)
        {
            throw new NotImplementedException();
        }

        public List<List<int>> PlacePlayerWorkers(Board board, string playerColor)
        {
            throw new NotImplementedException();
        }

        public ArrayList Play(Board board)
        {
            throw new NotImplementedException();
        }

        // receives message back via TCP from the RemotePlayer
        public string ReceiveMessage()
        {
            return "";
        }
    }
}
