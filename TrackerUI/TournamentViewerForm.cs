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
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        List<int> rounds = new();
        List<MatchupModel> selectedMatchups = new();
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();
            tournament = tournamentModel;

            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;
            LoadFormDate();
            LoadRounds();


        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            this.Close();
        }

        private void LoadFormDate()
        {
            tournamentName.Text = tournament.TournamentName;


        }

        private void wireUpRoundsLists()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = rounds;

        }
        private void wireUpMatchupLists()
        {
            matchupListbox.DataSource = null;
            matchupListbox.DataSource = selectedMatchups;
            matchupListbox.DisplayMember = "DisplayName";
        }
        private void LoadRounds()
        {
            rounds = new();
            rounds.Add(1);
            int currRound = 1;
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);

                }
            }
            wireUpRoundsLists();

        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void LoadMatchups()
        {
            int round = (int)roundDropDown.SelectedItem;
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    if (UnplayedOnlyCheckbox.Checked)
                    {
                        selectedMatchups = matchups.Where(mu => mu.Winner == null).ToList();
                    }
                    else
                    {
                        selectedMatchups = matchups;
                    }
                }
            }
            wireUpMatchupLists();
            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = selectedMatchups.Count > 0;
            teamOnelabel.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;
            teamTwoLabel.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }
        private void loadMatchup()
        {

            MatchupModel m = (MatchupModel)matchupListbox.SelectedItem;
            if (m == null)
            {
                return;
            }
            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        teamOnelabel.Text = m.Entries[0].TeamCompeting.TeamName;
                        teamOneScoreValue.Text = m.Entries[0].Score.ToString();

                        teamTwoLabel.Text = "<Bye>";
                        teamTwoScoreValue.Text = "0";
                    }
                    else
                    {
                        teamOnelabel.Text = "Not set";
                        teamOneScoreValue.Text = "";

                    }
                }

                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {

                        teamTwoLabel.Text = m.Entries[1].TeamCompeting.TeamName;
                        teamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                    }
                    else
                    {
                        teamTwoLabel.Text = "Not set";
                        teamTwoScoreValue.Text = "";
                    }
                }
            }
        }

        private void matchupListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadMatchup();
        }

        private void UnplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }
        private string ValidateData()
        {
            bool scoreOneValid = double.TryParse(teamOneScoreValue.Text, out double teamOneScore);
            bool scoreTwoValid = double.TryParse(teamTwoScoreValue.Text, out double teamTwoScore);

            if (!scoreOneValid || !scoreTwoValid)
            {
                return "Invalid Score";
            }

            if (teamOneScore == 0 && teamTwoScore == 0)
            {
                return "Please enter a score";
            }
            if (teamOneScore == teamTwoScore)
            {
                return "Tie games not allowed";
            }
            return "";
        }
        private void scoreButton_Click(object sender, EventArgs e)
        {
            if (ValidateData().Length > 0)
            {
                MessageBox.Show($"input error: {ValidateData()}");
                return;
            }
            MatchupModel m = (MatchupModel)matchupListbox.SelectedItem;
            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        m.Entries[0].Score = double.Parse(teamOneScoreValue.Text);
                    }
                }

                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        m.Entries[1].Score = double.Parse(teamTwoScoreValue.Text);
                    }

                }
            }

            try
            {

                TournamentLogic.UpdateTournamentsResults(tournament);
                LoadMatchups();

            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
                return;
            }


        }
    }
}
