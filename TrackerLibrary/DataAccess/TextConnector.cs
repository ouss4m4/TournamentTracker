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
        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int nextId = 1;
            if (people.Count > 0)
            {
                nextId = people.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            people.Add(model);
            people.SaveToPeopleFile();
            // return model;
        }

        public void CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int nextId = 1;
            if (prizes.Count > 0)
            {
                nextId = prizes.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            prizes.Add(model);
            prizes.SaveToPrizeFile();
            // return model;
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();
            int nextId = 1;
            if (teams.Count > 0)
            {
                nextId = teams.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;
            teams.Add(model);
            teams.SaveToTeamFile();
            // return model;
        }
        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }
        public void CreateTournament(TournamentModel model)
        {

            List<TournamentModel> tournaments = GlobalConfig.TournamentFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModel();

            int nextId = 1;
            if (tournaments.Count > 0)
            {
                nextId = tournaments.OrderByDescending(x => x.id).First().id + 1;
            }
            model.id = nextId;

            model.SaveRoundsToFile();

            tournaments.Add(model);
            tournaments.SaveToTournamentFile();

        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels();

        }


        public List<TournamentModel> GetTournament_All()
        {
            return GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModel();
        }

    }
}
