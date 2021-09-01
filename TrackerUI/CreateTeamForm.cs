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

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        public CreateTeamForm()
        {
            InitializeComponent();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {


            if (!ValidateForm())
            {
                MessageBox.Show("Invalid form data");
                return;
            }
            PersonModel member = new();
            member.FirstName = firstNameValue.Text;
            member.LastName = lastNameValue.Text;
            member.EmailAddress = emailValue.Text;
            member.CellPhoneNumber = cellPhoneValue.Text;
            GlobalConfig.Connection.CreatePerson(member);
            MessageBox.Show($"{member.FirstName} added");

            firstNameValue.Text = "";
            lastNameValue.Text = "";
            emailValue.Text = "";
            cellPhoneValue.Text = "";

   

        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length <= 0)
            {
                return false;
            }
            if (lastNameValue.Text.Length <= 0)
            {
                return false;
            }
            if (emailValue.Text.Length <= 0)
            {
                return false;
            }
            if (cellPhoneValue.Text.Length <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
