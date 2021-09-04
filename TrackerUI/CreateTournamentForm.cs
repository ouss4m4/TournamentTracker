﻿using System;
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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new();
        List<PrizeModel> selectedPrizes = new();
        public CreateTournamentForm()
        {
            InitializeComponent();
            InitializeLists();
        }

        private void InitializeLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel selected = (TeamModel)selectTeamDropDown.SelectedItem;
            if (selected != null)
            {
                availableTeams.Remove(selected);
                selectedTeams.Add(selected);

                InitializeLists();
            }

        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel selected = (TeamModel)tournamentTeamsListBox.SelectedItem;
            if (selected != null)
            {
                selectedTeams.Remove(selected);
                availableTeams.Add(selected);

                InitializeLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Call create prize form
            // get back the PrizeModel
            CreatePrizeForm prizeForm = new(this);
            prizeForm.Show();

        }

        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            InitializeLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            InitializeLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm teamForm = new(this);
            teamForm.Show();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel selected = (PrizeModel)prizesListBox.SelectedItem;
            if (selected != null)
            {
                selectedPrizes.Remove(selected);
                
                InitializeLists();
            }
        }
    }
}
