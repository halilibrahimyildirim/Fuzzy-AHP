using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHP
{
    class RFile
    {
        static public string path;
        static StreamReader sr;
        private static void rec(Criteria parent)//rekürsif okuma
        {
            string[] subTemp;
            string[] temp = sr.ReadLine().Split(';');
            Criteria current = new Criteria(parent, Convert.ToInt32(temp[1]), temp[0]);
            int limit;
            if (current.childCriteria == 0) limit = Criteria.firmCount;
            else limit = current.childCriteria;
            for (int i = 0; i < limit; i++)//matris oku
            {
                temp = sr.ReadLine().Split(';');
                for (int j = 0; j < limit; j++)
                {
                    subTemp = temp[j].Split(' ');
                    FuzzyData data = new FuzzyData(Convert.ToDouble(subTemp[0]), Convert.ToDouble(subTemp[1]), Convert.ToDouble(subTemp[2]));
                    current.matrix[i, j] = data;
                }
            }
            /* Alt kriter sayısı kadar içeri gir
             * tüm alt kriterleri okuduktan sonra 
             * bir üst kritere kendini alt kriter 
             * olarak ekle.Eğer ki alt kriterin yoksa
             * for dönmeyeceğinden direkt olarak ekle
             */
            for (int i = 0; i < current.childCriteria; i++)
            {
                rec(current);
            }
            parent.subCriteria.Add(current);
        }

        public static Criteria ReadFile()
        {
            sr = new StreamReader(path, Encoding.Default);
            CultureInfo.CurrentCulture = new CultureInfo("en-GB",false);//küsüratlı değer ayraçı için
            string[] subTemp;
            Criteria.firmCount = Convert.ToInt32(sr.ReadLine());
            for(int i=0;i<Criteria.firmCount;i++)//firmalara oku
            {
                Criteria.firms.Add(sr.ReadLine());
            }
            string[] temp=sr.ReadLine().Split(';');
            Criteria goal = new Criteria(null, Convert.ToInt32(temp[1]), temp[0]);
            for(int i=0;i<goal.childCriteria;i++)//goal matrisini oku
            {
                temp = sr.ReadLine().Split(';');
                for(int j=0;j<goal.childCriteria;j++)
                {
                    subTemp = temp[j].Split(' ');
                    FuzzyData data = new FuzzyData(Convert.ToDouble(subTemp[0]), Convert.ToDouble(subTemp[1]), Convert.ToDouble(subTemp[2]));
                    goal.matrix[i, j] = data;
                }
            }
            for (int i = 0; i < goal.childCriteria; i++)//goal'a bağlı bir şekilde kriterleri oku
            {
                rec(goal);
            }
            return goal;
        }
    }
}
