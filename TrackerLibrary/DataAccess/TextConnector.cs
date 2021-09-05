using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextProcessor;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";
        private const string TournamentFile = "TournamentModels.csv";
        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int nextId = 1;
            if (people.Count > 0)
            {
                nextId = people.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            people.Add(model);
            people.SaveToPeopleFile(PeopleFile);
            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int nextId = 1;
            if (prizes.Count > 0)
            {
                nextId = prizes.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            prizes.Add(model);
            prizes.SaveToPrizeFile(PrizesFile);
            return model;
        }


        public List<PersonModel> GetPerson_All()
        {
            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);
            int nextId = 1;
            if (teams.Count > 0)
            {
                nextId = teams.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            teams.Add(model);
            teams.SaveToTeamFile(TeamFile);
            return model;
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

        }

        public void CreateTournament(TournamentModel model)
        {

            List<TournamentModel> tournaments = TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModel(TeamFile, PeopleFile, PrizesFile);

            int nextId = 1;
            if (tournaments.Count > 0)
            {
                nextId = tournaments.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            tournaments.Add(model);
            tournaments.SaveToTournamentFile(TournamentFile);

        }
    }
}
