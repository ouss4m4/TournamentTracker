using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;


//@PlaceNumber int,
//	@PlaceName nvarchar(50),
//	@PrizeAmount money,
//  @PrizePercentage Float,
//  @id int = 0 output


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
                team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam",p, commandType: CommandType.StoredProcedure).ToList();
            }
            return output;
        }
    }
}
