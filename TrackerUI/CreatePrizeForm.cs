using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        public CreatePrizeForm()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Invalid form data");
                return;
            }
            PrizeModel model = new(placeNameValue.Text, placeNumberValue.Text, prizeAmountValue.Text, PrizePercentageValue.Text);
            GlobalConfig.Connection.CreatePrize(model);
            MessageBox.Show($"{model.PlaceName} prize created");
            placeNameValue.Text = "";
            placeNumberValue.Text = "";
            prizeAmountValue.Text = "0";
            PrizePercentageValue.Text = "0";
        }

        private bool ValidateForm()
        {
            bool isFormValid = true;
            bool placeNumberValid = int.TryParse(placeNumberValue.Text, out int placeNumber);

            if (!placeNumberValid && placeNumber <= 0)
            {
                return false;
            }
            if (placeNameValue.Text.Length <= 0)
            {
                return false;
            }

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out decimal prizeAmount);
            bool prizePercentageValid = double.TryParse(PrizePercentageValue.Text, out double prizePercentage);

            if (!prizeAmountValid || !prizePercentageValid)
            {
                return false;
            }
            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                return false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                return false;
            }
            if (prizeAmount > 0 && prizePercentage > 0)
            {
                return false;
            }

            return isFormValid;
        }
    }
}
