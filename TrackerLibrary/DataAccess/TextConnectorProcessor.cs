using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
// load the text file list<string>
// Convert the text to List<PrizeModel>
// Find the biggest ID
// Add the new record with the new ID
// Convert the prizes to list<string>
// Save the list<string> to the text file
namespace TrackerLibrary.DataAccess.TextProcessor
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel p = new();
                p.id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellPhoneNumber = cols[4];
                output.Add(p);
            }
            return output;
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PrizeModel p = new PrizeModel();
                p.id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            };
            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel t = new();
                t.id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.id == int.Parse(id)).First());

                }
                output.Add(t);

            }
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModel(
            this List<string> lines,
            string teamFileName,
            string peopleFileName,
            string prizeFileName)
        {
            List<TournamentModel> output = new();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel tm = new();
                tm.id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');
                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.id == int.Parse(id)).First());
                }

                string[] prizeIds = cols[4].Split('|');
                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.id == int.Parse(id)).First());
                }

                // TODO - Capture Round Information;
            }
            return output;

        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new();
            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new();
            foreach (PersonModel p in models)
            {
                lines.Add($"{p.id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellPhoneNumber}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new();
            foreach (TeamModel t in models)
            {
                lines.Add($"{t.id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new();
            foreach (TournamentModel tm in models)
            {
                lines.Add($@"{tm.id},
                    {tm.TournamentName},
                    {tm.EntryFee},
                    {ConvertTeamListToString(tm.EnteredTeams)},
                    {ConvertPrizeListToString(tm.Prizes)},
                    {ConvertRoundsListToString(tm.Rounds)}");
            };
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertRoundsListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";
            if (rounds.Count == 0)
            {
                return "";
            }
            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ConvertMatchupListToString(r)}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";
            if (matchups.Count == 0)
            {
                return "";
            }
            foreach (MatchupModel mu in matchups)
            {
                output += $"{mu.id}^";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";
            if (prizes.Count == 0)
            {
                return "";
            }
            foreach (PrizeModel p in prizes)
            {
                output += $"{p.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";
            if (teams.Count == 0)
            {
                return "";
            }
            foreach (TeamModel t in teams)
            {
                output += $"{t.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";
            if (people.Count == 0)
            {
                return "";
            }
            foreach (PersonModel person in people)
            {
                output += $"{person.id}|";
            }
            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
