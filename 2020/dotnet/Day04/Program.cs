using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    public class Program
    {
        private static readonly IEnumerable<string> RequiredFields = new[]
        {
            "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"
        };

        private static readonly ISet<string> ValidEyeColours = new HashSet<string>
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };
        
        static void Main(string[] args)
        {
            var pps = GetPassports();

            var firstCount = pps.Count(passport => RequiredFields.All(x => passport.Keys.Contains(x)));
            var secondCount = pps.Count(ValidatePassport);
            
            Console.WriteLine($"Day 4 - Part 1: {firstCount}");
            Console.WriteLine($"Day 4 - Part 2: {secondCount}");
        }

        private static List<IDictionary<string, string>> GetPassports()
        {
            var pps = new List<IDictionary<string, string>>();
            var allData = File.ReadAllText("../../inputs/day4.txt").Split("\n\n");
            foreach (var ppData in allData)
            {
                pps.Add(ParsePassport(ppData));
            }

            return pps;
        }

        public static IDictionary<string, string> ParsePassport(string passportData)
        {
            var pp = new Dictionary<string, string>();
            foreach (var ppItem in passportData.Split(null))
            {
                if (string.IsNullOrWhiteSpace(ppItem)) continue;

                var kvp = ppItem.Split(":");
                pp.Add(kvp[0], kvp[1]);
            }

            return pp;
        }

        private static Regex PassportIdRegex = new Regex("^[0-9]{9}$");
        private static Regex HairColourRegex = new Regex("^#[0-9a-f]{6}$");
        public static bool ValidatePassport(IDictionary<string, string> passport)
        {
            if (!RequiredFields.All(x => passport.Keys.Contains(x)))
            {
                return false;
            }

            foreach (var ppField in passport)
            {
                var isValid = ppField.Key switch
                {
                    "byr" => ValidateBirthYear(ppField.Value),
                    "iyr" => ValidateIssueYear(ppField.Value),
                    "eyr" => ValidateExpirationYear(ppField.Value),
                    "hgt" => ValidateHeight(ppField.Value),
                    "hcl" => ValidateHairColour(ppField.Value),
                    "ecl" => ValidateEyeColour(ppField.Value),
                    "pid" => ValidatePassportId(ppField.Value),
                    _ => true
                };
                if (!isValid) return false;
            }

            return true;
        }

        private static bool ValidateNumber(string number, int min, int max)
        {
            var value = int.Parse(number);
            return value >= min && value <= max;
        }

        public static bool ValidateBirthYear(string birthYear) => ValidateNumber(birthYear, 1920, 2002);
        public static bool ValidateIssueYear(string issueYear) => ValidateNumber(issueYear, 2010, 2020);
        public static bool ValidateExpirationYear(string expirationYear) => ValidateNumber(expirationYear, 2020, 2030);

        public static bool ValidateHeight(string height)
        {
            switch (height.Substring(height.Length - 2))
            {
                case "cm":
                    if (!ValidateNumber(height.Substring(0, height.Length - 2), 150, 193)) return false;
                    break;
                case "in":
                    if (!ValidateNumber(height.Substring(0, height.Length - 2), 59, 76)) return false;
                    break;
                default:
                    return false;
            }

            return true;
        }
        
        public static bool ValidateHairColour(string hairColour)
        {
            return HairColourRegex.IsMatch(hairColour);
        }
        
        public static bool ValidateEyeColour(string hairColour)
        {
            return ValidEyeColours.Contains(hairColour);
        }
        
        public static bool ValidatePassportId(string passportId)
        {
            return PassportIdRegex.IsMatch(passportId);
        }
    }
}
