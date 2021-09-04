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
        private List<PersonModel> availableMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new();
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            //CreateSampleData();
            WireUpList();
            callingForm = caller;
        }

        private void WireUpList()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
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
            selectedTeamMembers.Add(member);
            firstNameValue.Text = "";
            lastNameValue.Text = "";
            emailValue.Text = "";
            cellPhoneValue.Text = "";
            WireUpList();


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

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if (p != null)
            {
                selectedTeamMembers.Add(p);
                availableMembers.Remove(p);
                WireUpList();
            };


        }

        private void deleteSelectedMember_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableMembers.Add(p);
                WireUpList();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (!validTeamForm())
            {
                MessageBox.Show("Invalid form data");
                return;

            }

            TeamModel newTeam = new();
            newTeam.TeamName = teamNameValue.Text;
            newTeam.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(newTeam);
            callingForm.TeamComplete(newTeam);
            this.Close();
        }

        private bool validTeamForm()
        {
            if (teamNameValue.Text.Length <= 0)
            {
                return false;
            }
            if (selectedTeamMembers.Count <= 0)
            {

                return false;
            }
            return true;
        }
    }
}
