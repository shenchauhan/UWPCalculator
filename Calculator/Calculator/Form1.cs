using Calculation;
using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private double result;
        private string operation;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (lblResult.Text == "0" | lblResult.Text == "/" | lblResult.Text == "+" | lblResult.Text == "-" | lblResult.Text == "x")
            {
                lblResult.Text = string.Empty;
            }

            lblResult.Text += (sender as Button)?.Text;
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            lblResult.Text = "/";
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            lblResult.Text = "x";
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            lblResult.Text = "-";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            lblResult.Text = "+";
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            lblResult.Text = string.Empty;
        }
    }
}
