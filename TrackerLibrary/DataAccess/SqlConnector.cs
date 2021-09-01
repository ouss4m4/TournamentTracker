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
        public PersonModel CreatePerson(PersonModel model)
        {
            try
            {
                using IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments"));
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


                using IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments"));
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
    }
}
