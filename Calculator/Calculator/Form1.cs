using Calculation;
using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private double result;
        private string operation;

        private readonly CalculationHistory calculationHistory = new CalculationHistory();

        public Form1()
        {
            InitializeComponent();
            calculationHistory.ClearHistory();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (result == 0 | lblResult.Text == "/" | lblResult.Text == "+" | lblResult.Text == "-" | lblResult.Text == "x")
            {
                lblResult.Text = string.Empty;
            }

            lblResult.Text += (sender as Button)?.Text;
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            operation = "/";
            lblResult.Text = operation;
            calculationHistory.AddToCalculation(result + operation);
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            operation = "x";
            lblResult.Text = operation;
            calculationHistory.AddToCalculation(result + operation);
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            operation = "-";
            lblResult.Text = operation;
            calculationHistory.AddToCalculation(result + operation);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            result = double.Parse(lblResult.Text);
            operation = "+";
            lblResult.Text = operation;
            calculationHistory.AddToCalculation(result + operation);
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            Calculate();
            calculationHistory.AddToCalculation(lblResult.Text);
            lblResult.Text = result.ToString();
            calculationHistory.AddToHistory(result);

            lbHistory.SelectedIndexChanged -= new EventHandler(lbHistory_SelectedIndexChanged);
            lbHistory.DataSource = calculationHistory.FetchEntireHistory();
            lbHistory.SelectedIndexChanged += new EventHandler(lbHistory_SelectedIndexChanged);

            result = 0;
        }

        private void Calculate()
        {
            var x = double.Parse(lblResult.Text);

            switch (operation)
            {
                case "/":
                    result /= x;
                    break;
                case "+":
                    result += x;
                    break;
                case "-":
                    result -= x;
                    break;
                case "x":
                    result *= x;
                    break;
            }
        }

        private void lbHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lbHistory.SelectedItem.ToString();
            lblResult.Text = calculationHistory.FetchFromHistory(selectedItem).ToString();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            result = 0;
            lblResult.Text = "0";
            calculationHistory.Clear();
        }
    }
}
