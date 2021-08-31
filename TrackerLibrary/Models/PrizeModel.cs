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
            int.TryParse(placeNumber, out int placeNumberValue);
            decimal.TryParse(prizeAmount, out decimal prizeAmountValue);
            double.TryParse(prizePerecentage, out double prizePercentageValue);


        }

    }


}
