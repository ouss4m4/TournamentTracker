using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        public int id { get; set; }
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePerecentage)
        {
            PlaceName = placeName;
            PlaceNumber= int.Parse(placeNumber);
            PrizeAmount = decimal.Parse(prizeAmount);
            PrizePercentage = double.Parse(prizePerecentage);


        }

    }


}
