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
    public partial class TournamentDashboard : Form
    {
        readonly List<TournamentModel> tournaments= GlobalConfig.Connection.GetTournament_All();

        public TournamentDashboard()
        {
            InitializeComponent();
            wireLists();
        }

        private void wireLists()
        {
            loadExistingTournamentDropBox.DataSource = tournaments;
            loadExistingTournamentDropBox.DisplayMember = "TournamentName";
        }
        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new();
            frm.Show();
        }
    }
}
