using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_not_Super_Reduct_only_Algorithm.CS
{
  public   class FoundItemsetsProperties
    {
      public int testIndex = 0;
      public ArrayList itemsetIndexInTrainingSet =new ArrayList();
      public ArrayList itemsetIndexInTrainingSetClassLabel = new ArrayList();
      //public string classlabel = "";
      public double nFeatures=0;
      public double nObjects = 0;
      public double FxO = 0;
    }
}
