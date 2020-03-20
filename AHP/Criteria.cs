using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP
{
    class Criteria
    {
        public static int firmCount;
        public static List<string> firms = new List<string>();
        public double[] weight;
        public string name;
        public Criteria parent;
        public int childCriteria;
        public FuzzyData [,] matrix;
        public List<Criteria> subCriteria;
        public double[] priorityVector;
        public Criteria(Criteria parent,int childCriteria,string name)
        {
            this.parent = parent;
            this.name = name;
            this.childCriteria = childCriteria;
            if (childCriteria == 0) this.matrix = new FuzzyData [firmCount, firmCount];
            else this.matrix = new FuzzyData[childCriteria, childCriteria];
            this.subCriteria = new List<Criteria>();
            this.priorityVector= new double[firmCount];
        }

        //Kriterin matrisinin satırını döndürür
        public FuzzyData[] getRow(int index)
        {
            FuzzyData[] temp= new FuzzyData[this.matrix.GetLength(1)];
            for(int i=0;i< this.matrix.GetLength(1);i++)
            {
                temp[i] = this.matrix[index, i];
            }
            return temp;
        }

    }
}
