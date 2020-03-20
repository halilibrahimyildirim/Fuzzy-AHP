using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP
{
    class calcAHP
    {
        public static FuzzyData RowSum(FuzzyData[] row)
        {
            //satır toplamalını hesaplar
            FuzzyData rs = new FuzzyData(0, 0, 0);
            for (int i = 0; i < row.Length; i++)
            {
                rs.l += row[i].l;
                rs.m += row[i].m;
                rs.u += row[i].u;
            }
            return rs;
        }

        public static List<FuzzyData> Normalize(List<FuzzyData> rsList)
        {
            //Formüle uygulayarak normalize eder
            List<FuzzyData> SList = new List<FuzzyData>();
            for (int i = 0; i < rsList.Count; i++)
            {
                FuzzyData Si = new FuzzyData(0, 0, 0);
                double SiL = rsList[i].l;
                double SiM = 0;
                double SiU = rsList[i].u;
                for (int j = 0; j < rsList.Count; j++)
                {
                    SiM += rsList[j].m;
                    if (i != j)
                    {
                        SiL += rsList[j].u;
                        SiU += rsList[j].l;
                    }
                }
                Si.l = rsList[i].l / SiL;
                Si.m = rsList[i].m / SiM;
                Si.u = rsList[i].u / SiU;
                SList.Add(Si);
            }
            return SList;
        }

        public static double Compare(FuzzyData S1, FuzzyData S2)
        {
            if (S1.m >= S2.m)
            {
                return 1;
            }
            else if (S2.l <= S1.u)
            {
                return (S1.u - S2.l) / (S1.u - S1.m + S2.m - S2.l);
            }
            else
            {
                return 0;
            }
        }
        public static double[] Weight(List<FuzzyData> SList)
        {
            //Formüle uygun ağırlık hesaplar
            double[] w = new double[SList.Count];
            double wSum = 0;
            for (int i = 0; i < SList.Count; i++)
            {
                double min = double.MaxValue;
                for (int j = 0; j < SList.Count; j++)
                {
                    if (i != j)
                    {
                        double temp = Compare(SList[i], SList[j]);
                        if (temp < min)
                        {
                            min = temp;
                        }
                    }
                }
                w[i] = min;
                wSum += min;
            }
            for (int i = 0; i < SList.Count; i++)//Normalize
            {
                w[i] /= wSum;
            }
            return w;
        }

        public static void calcWeight(Criteria current)//Rekürsif ağırlık hesaplama
        {
            List<FuzzyData> rs = new List<FuzzyData>();
            List<FuzzyData> normalized = new List<FuzzyData>();
            for (int i = 0; i < current.matrix.GetLength(0); i++)
            {
                rs.Add(RowSum(current.getRow(i)));//satır toplamları hesaplandı
            }
            normalized = Normalize(rs);//normalize edildi
            current.weight = Weight(normalized);//şuanki kriterin ağırlığı hesaplandı
            //varsa tüm alt kategorilerininde 
            //ağırlığının hesaplanması istendi
            for (int i = 0; i < current.childCriteria; i++)
            {
                calcWeight(current.subCriteria[i]);
            }
        }

        //Rekürsif öncelik hesabı
        public static void calcPrio(Criteria Parent, Criteria Current, int index)
        {
            /* (1)Eğerki ana kriterin alt kriteri var ise inebildiği kadar
             * derine iner daha fazla alt kriteri yoksa en derindeyizdir.
             * 
             * (2) Bu alt kriterin öncelik vektörü bağlı olduğu kriterin ona
             * olan ağırlıyla kendi ağırlığı çarpılır ve bir üst katmana çıkar.
             *  
             * 
             * (3)Bir üst katmandaki ana kriterin tüm çocuklarının öncelik vektörü hesaplandıktan
             * sonra bu vektörler toplanarak ana kriterin öncelik vektörü hesaplanmış olur.
             * 
             * (4)Ana kriterinde bir üst ana kriteri var ise 2 numaradaki aynı hesapla yeni
             * öncelik vektörü hesaplanmış olur.
             */
            for (int i = 0; i < Current.childCriteria; i++)
            {
                calcPrio(Current, Current.subCriteria[i], i);//(1)
                for(int j=0;j<Criteria.firmCount;j++)
                {
                    Current.priorityVector[j] += Current.subCriteria[i].priorityVector[j];//(3)
                }
            }
            if(Current.childCriteria>0)
            {
                for (int i = 0; i < Criteria.firmCount; i++)
                {
                    Current.priorityVector[i] = Parent.weight[index] * Current.priorityVector[i];//(4)
                }
            }
            else//(2)
            {
                for (int i = 0; i < Criteria.firmCount; i++)
                {
                    Current.priorityVector[i] = Parent.weight[index] * Current.weight[i];
                }
            }
            
        }

        public static double[] AHP(Criteria C)
        {
            calcWeight(C);//tüm kriterler için ağırlık hesapla
            for(int i=0;i<C.childCriteria;i++)
            {
                //Goal'un tüm alt kriterleri için öncelik vektörü hesaplanır
                calcPrio(C, C.subCriteria[i], i);
                for (int j = 0; j < Criteria.firmCount; j++)
                {
                    //toplama işlemi ile goal'un ki de hesaplanmış olur
                    C.priorityVector[j] += C.subCriteria[i].priorityVector[j];
                }
            }
            return C.priorityVector;
            //En son tüm firmaların öncelik vektörü tamamlanmış olur ve döndürülür

        }

    }
}
