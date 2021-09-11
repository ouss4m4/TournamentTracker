using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrackerLibrary.Models;



namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public string db = "Tournaments";
        public PersonModel CreatePerson(PersonModel model)
        {
            try
            {
                using IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db));
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellphoneNumber", model.CellPhoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);
                model.id = p.Get<int>("@id");
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            try
            {


                using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("spPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                model.id = p.Get<int>("@id");
                return model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
            var p = new DynamicParameters();
            p.Add("@TeamName", model.TeamName);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            connection.Execute("spTeams_Insert", p, commandType: CommandType.StoredProcedure);
            model.id = p.Get<int>("@id");

            foreach (PersonModel person in model.TeamMembers)
            {
                var member = new DynamicParameters();
                member.Add("@TeamId", model.id);
                member.Add("@PersonId", person.id);
                connection.Execute("spTeamMembers_Insert", member, commandType: CommandType.StoredProcedure);

            }

            return model;

        }

        public void CreateTournament(TournamentModel model)
        {
            using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
            // dbo.Tournaments_Insert
            SaveTournament(model, connection);
            SaveTournamentPrizes(model, connection);
            SaveEnteredTeams(model, connection);
            SaveTournamentRounds(model, connection);
        }

        private static void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {
            // List<List<MatchupModel> rounds
            // List<MatchupEntryModel> Entries
            // 
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    DynamicParameters p = new();
                    p.Add("@TournamentId", model.id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);
                    matchup.id = p.Get<int>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        DynamicParameters pm = new();
                        pm.Add("@MatchupId", matchup.id);
                        if (entry.ParentMatchup == null)
                        {
                            pm.Add("@ParentMatchupId", null);
                        }
                        else
                        {

                            pm.Add("@ParentMatchupId", entry.ParentMatchup.id);
                        }

                        if (entry.TeamCompeting == null)
                        {
                            pm.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            pm.Add("@TeamCompetingId", entry.TeamCompeting.id);
                        }

                        connection.Execute("dbo.spMatchupEntries_Insert", pm, commandType: CommandType.StoredProcedure);
                        // matchup.id = p.Get<int>("@id");
                    }
                }
            }
        }
        private static void SaveTournament(TournamentModel model, IDbConnection connection)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.Tournaments_Insert", p, commandType: CommandType.StoredProcedure);
            model.id = p.Get<int>("@id");
        }

        private static void SaveTournamentPrizes(TournamentModel model, IDbConnection connection)
        {
            foreach (PrizeModel pz in model.Prizes)
            {
                DynamicParameters p = new();
                p.Add("@TournamentId", model.id);
                p.Add("@PrizeId", pz.id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);

            }

        }

        private static void SaveEnteredTeams(TournamentModel model, IDbConnection connection)
        {
            foreach (TeamModel tm in model.EnteredTeams)
            {
                DynamicParameters p = new();
                p.Add("@TournamentId", model.id);
                p.Add("@Teamid", tm.id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);

            }

        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;
            using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
            output = connection.Query<PersonModel>("spPeople_GetAll").ToList();
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;
            using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
            output = connection.Query<TeamModel>("spTeam_GetAll").ToList();
            foreach (TeamModel team in output)
            {
                var p = new DynamicParameters();
                p.Add("@TeamId", team.id);
                team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> output = new();
            using IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db));
            output = connection.Query<TournamentModel>("dbo.spTournament_GetAll").ToList();
            foreach (TournamentModel tm in output)
            {
                // populate prizes
                DynamicParameters p = new();
                p.Add("@TournamentId", tm.id);
                tm.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                // populate teams

                tm.EnteredTeams = connection.Query<TeamModel>("spTeam_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                foreach (TeamModel team in tm.EnteredTeams)
                {
                    DynamicParameters p1 = new();
                    p1.Add("@TeamId", team.id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p1, commandType: CommandType.StoredProcedure).ToList();

                }
                DynamicParameters p2 = new();
                p2.Add("@TournamentId", tm.id);

                // populate rounds // dbo.spMatchups_GetByTournament;
                List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchups_GetByTournament", p2, commandType: CommandType.StoredProcedure).ToList();

                foreach (MatchupModel m in matchups)
                {
                    DynamicParameters p3 = new();
                    p3.Add("@MatchupId", m.id);

                    m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntries_GetByMatchup", p3, commandType: CommandType.StoredProcedure).ToList();
                    List<TeamModel> allTeams = GetTeam_All();

                    if (m.WinnerId > 0)
                    {
                        m.Winner = allTeams.Where(x => x.id == m.WinnerId).First();
                    }

                    foreach (MatchupEntryModel entry in m.Entries)
                    {
                        if (entry.TeamCompetingId > 0)
                        {
                            entry.TeamCompeting = allTeams.Where(x => x.id == entry.TeamCompetingId).First();
                        }

                        if (entry.ParentMatchupId > 0)
                        {
                            entry.ParentMatchup = matchups.Where(x => x.id == entry.ParentMatchupId).First();
                        }
                    }
                }

                List<MatchupModel> currRow = new();
                int currRound = 1;
                foreach (MatchupModel m in matchups)
                {
                    if (m.MatchupRound > currRound)
                    {
                        tm.Rounds.Add(currRow);
                        currRow = new();
                        currRound += 1;
                    }
                    currRow.Add(m);
                }
                tm.Rounds.Add(currRow);
            }


            return output;

        }
    }
}
