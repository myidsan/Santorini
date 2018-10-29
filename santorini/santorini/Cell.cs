using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace santorini
{
    public class Cell
    {
        private int height;
        private string worker;

        public int Height { get => height; set => height = value; }
        public string Worker { get => worker; set => worker = value; }

        public Cell(Dictionary<string, List<int>> playerposition, JToken candidate, int rowIndex, int colIndex)
        {
            if (candidate.GetType() == typeof(JArray))
            {
                this.Height = (int)candidate[0];
                this.worker = (string)candidate[1];
                if (this.worker != null)
                {
                    playerposition.Add(this.worker, new List<int> { rowIndex, colIndex });
                }
            }
            else
            {
                this.Height = (int)candidate;
                this.Worker = null;
            }
            if (this.Height > 4) 
            {
                throw new Exception("building height cannot be great than 4");
            }
        }

        public dynamic[] PrintCell()
        {
            dynamic[] arr = { this.Height, this.Worker };
            List<dynamic> dArr = new List<dynamic>() { this.Height, this.Worker };

            return dArr.ToArray();
        }
    }
}
