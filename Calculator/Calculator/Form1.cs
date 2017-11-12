using Calculation;
using System;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbHistory.SelectedIndexChanged -= new EventHandler(lbHistory_SelectedIndexChanged);
            lbHistory.DataSource = CalculationHistory.FetchEntireHistory();
            lbHistory.SelectedIndexChanged += new EventHandler(lbHistory_SelectedIndexChanged);
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (lblResult.Text == "0" | lblResult.Text == "/" | lblResult.Text == "+" | lblResult.Text == "-" | lblResult.Text == "x")
            {
                lblResult.Text = string.Empty;
            }

            lblResult.Text += (sender as Button)?.Text;
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            lblResult.Text = Calculation.NetStandard.Calculator.Calculate(lblResult.Text).ToString();

            lbHistory.SelectedIndexChanged -= new EventHandler(lbHistory_SelectedIndexChanged);
            lbHistory.DataSource = CalculationHistory.FetchEntireHistory();
            lbHistory.SelectedIndexChanged += new EventHandler(lbHistory_SelectedIndexChanged);
        }

        private void lbHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lbHistory.SelectedItem.ToString();
            lblResult.Text = CalculationHistory.FetchFromHistory(selectedItem).ToString();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
            lbHistory.SelectedIndexChanged -= new EventHandler(lbHistory_SelectedIndexChanged);
            lbHistory.DataSource = CalculationHistory.FetchEntireHistory();
            lbHistory.SelectedIndexChanged += new EventHandler(lbHistory_SelectedIndexChanged);
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
        }

        private void ClearDatabase_Click(object sender, EventArgs e)
        {
            CalculationHistory.ClearHistory();
            lbHistory.DataSource = CalculationHistory.FetchEntireHistory();
        }
    }
}
