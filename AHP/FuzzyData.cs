using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP
{
    class FuzzyData
    {
        public double l, m, u;

        public FuzzyData(double l,double m,double u)
        {
            this.l = l;
            this.m = m;
            this.u = u;
        }
    }
}
