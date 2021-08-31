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

        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int nextId = 1;
            if(prizes.Count > 0)
            {
                nextId = prizes.OrderByDescending(x => x.id).First().id + 1;
            } 
            model.id = nextId;
            prizes.Add(model);
            prizes.SaveToPrizeFile(PrizesFile);
            return model;
        }
    }
}
