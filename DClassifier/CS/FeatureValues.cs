using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PWRSet.CS
{
    public class FeatureValues
    {
        public int value;
        public int count;
        public ArrayList RowsIndices = new ArrayList();
        public ArrayList NegationCatIndices = new ArrayList();
        public ArrayList UnkSet = new ArrayList();
        public int RID;
        public string GetRids()
        {
            string ids = "";
            if (RowsIndices.Count > 0)
            {

                foreach (int x in RowsIndices)
                {
                    ids = ids + "," + x.ToString();
                }
            }
            return ids;
        }
    }
}
