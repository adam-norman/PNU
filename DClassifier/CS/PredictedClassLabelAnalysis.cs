using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DClassifier.CS
{
    public class  PredictedClassLabelAnalysis
    {
        public string className = "";
        public int ds_ClassCount = 0;
        public int Rules_ClassCount = 0;
        public string rowIDs = "";
        public int rowIDsCount = 0;
        public ArrayList Ocs = new ArrayList();
        public ArrayList DistinctRowsIDs = new ArrayList();
       
        public ArrayList Supp_i_to_c = new ArrayList();
        public ArrayList Supp_c_to_i = new ArrayList();
        public ArrayList Conf_c_to_i = new ArrayList();
        public double GetMax_Supp_i_to_c ()
        {
            if (Supp_i_to_c.Count == 0)
            {
                return 0;
            }
            Supp_i_to_c.Sort();
            return    double.Parse( Supp_i_to_c[Supp_i_to_c.Count - 1].ToString());
        }
        public double GetMax_Supp_c_to_i()
        {
            if (Supp_c_to_i.Count == 0)
            {
                return 0;
            }
            Supp_c_to_i.Sort();
            return double.Parse(Supp_c_to_i[Supp_c_to_i.Count - 1].ToString());
        }
        public double GetMax_Conf_c_to_i()
        {
            if (Conf_c_to_i.Count == 0)
            {
                return 0;
            }

            Conf_c_to_i.Sort();

            return double.Parse(Conf_c_to_i[Conf_c_to_i.Count - 1].ToString());
        }
       
        public ArrayList GetMutualRowsIds()
        {
            ArrayList t = new ArrayList();
            if (rowIDs!="")
            {
                foreach (string x in rowIDs.Split(','))
                {
                    if (x != "")
                    {
                        if (!t.Contains(x))
                        {
                            t.Add(x);
                        }
                    }
                }
            }
            return t;
        }
    }
}
