using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Week03Homework
{
    public partial class FormMain : Form
    {
        String temp = "";
        public FormMain()
        {
            InitializeComponent();
        }

        List<String> list = new List<String>();
        private void btnNum_Click(object sender, EventArgs e)
        {

            lblExpression.Text = lblExpression.Text + (sender as Button).Text;
            //list.Add((sender as Button).Text);
            temp = temp + (sender as Button).Text;
        }

        private void btnOp_Click(object sender, EventArgs e)
        {
            lblExpression.Text = lblExpression.Text + (sender as Button).Text;
            list.Add(temp);
            list.Add((sender as Button).Text);
            temp = "";
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            lblExpression.Text = lblExpression.Text + (sender as Button).Text;
            list.Add(temp);
            int sum = 0;
            String op = "";
            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];

                if (d == "+" || d == "-" || d == "*" || d == "/")
                {
                    op = d;
                } else {
                    if (op == "+") {
                        sum += int.Parse(d);                         
                        op = "";
                     } else if (op == "-") {
                        sum -= int.Parse(d); 
                        op = "";
                    } else if (op == "*") {
                        sum *= int.Parse(d); 
                        op = "";
                    } else if (op == "/") {
                        sum /= int.Parse(d);
                        op = "";
                    } else {
                        sum = int.Parse(d);
                    }
                }
                //num1 = sum;
            }
            lblNumbers.Text = sum.ToString();
            lblExpression.Text = lblExpression.Text + sum.ToString();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            lblExpression.Text = "";
            lblNumbers.Text = "";
        }
    }
}
