using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DClassifier.CS
{
   static  class StringOperations
    {
     static   public string GetAttrVal(string Attr, string myxmlStr)
       {
           string val = null;
         if (myxmlStr!="")
         { 
           string str1 = myxmlStr.Substring(myxmlStr.IndexOf(Attr) + Attr.Length + 2);
             val = str1.Substring(0, str1.IndexOf("\""));
         }
           return val;
       }
    }
}
