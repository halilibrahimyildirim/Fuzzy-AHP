using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace AHP
{
    public partial class Form1 : Form
    {
        struct firm
        {
            public string name;
            public double value;
            public firm(string name, double value)
            {
                this.name = name;
                this.value = value;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        List<firm> firms=new List<firm>();

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            try
            { 
            if(ofd.CheckFileExists && ofd.FileName!="")
            {
                    firms.Clear();
                    RFile.path = ofd.FileName;
                    label1.Text = ofd.FileName;
                    button2.Enabled = true;
                    double[] values=calcAHP.AHP(RFile.ReadFile());
                    for (int i = 0; i < values.Length; i++)
                    {
                        firms.Add(new firm(Criteria.firms[i], values[i]));
                    }
                    firms.Sort((s1, s2) => s1.value.CompareTo(s2.value)*-1);
                    label2.Text = "Best firm is "+firms[0].name + " with value of " + firms[0].value;
                    chart1.Series.Clear();
                    chart1.Series.Add("Firms");
                    chart1.Series["Firms"].Color = Color.White;
                    Random rnd = new Random();
                    for(int i=0;i<firms.Count;i++)
                    { 
                        chart1.Series[0].Points.AddXY(firms[i].name, firms[i].value);
                        chart1.Series[0].Points[i].Color = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
                    }
            }
            else
            {
                throw new FileNotFoundException();
            }
            }
            catch (FileNotFoundException err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "";
            foreach(firm f in firms)
            {
                text += f.name + " :\t " + f.value+"\n";
            }
            MessageBox.Show(text, "Values");
        }
    }
}
