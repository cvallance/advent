using Xunit;
using Day04;

namespace UnitTests.Day04
{
    public class Day04Tests
    {
        [Fact]
        public void ValidateBirthYear_Valid()
        {
            Assert.True(Program.ValidateBirthYear("2002"));
        }
        
        [Fact]
        public void ValidateBirthYear_Invalid()
        {
            Assert.False(Program.ValidateBirthYear("2003"));
        }
 
        [Theory]
        [InlineData("60in")]
        [InlineData("190cm")]
        public void ValidateHeight_Valid(string height)
        {
            Assert.True(Program.ValidateHeight(height));
        }
 
        [Theory]
        [InlineData("190in")]
        [InlineData("190")]
        public void ValidateHeight_Invalid(string height)
        {
            Assert.False(Program.ValidateHeight(height));
        }
        
        [Fact]
        public void ValidateHairColour_Valid()
        {
            Assert.True(Program.ValidateHairColour("#123abc"));
        }
        
        [Theory]
        [InlineData("#123abz")]
        [InlineData("123abc")]
        public void ValidateHairColour_Invalid(string hairColour)
        {
            Assert.False(Program.ValidateHairColour(hairColour));
        }
        
        [Fact]
        public void ValidateEyeColour_Valid()
        {
            Assert.True(Program.ValidateEyeColour("brn"));
        }
        
        [Fact]
        public void ValidateEyeColour_Invalid()
        {
            Assert.False(Program.ValidateEyeColour("wat"));
        }
        
        [Fact]
        public void ValidatePassportId_Valid()
        {
            Assert.True(Program.ValidatePassportId("000000001"));
        }
        
        [Fact]
        public void ValidatePassportId_Invalid()
        {
            Assert.False(Program.ValidatePassportId("0123456789"));
        }
        
        [Theory]
        [InlineData(@"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
        hcl:#623a2f")]
        [InlineData(@"eyr:2029 ecl:blu cid:129 byr:1989
        iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm")]
        [InlineData(@"hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022")]
        [InlineData(@"iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719")]
        public void ValidatePassport_Valid(string passportData)
        {
            var passport = Program.ParsePassport(passportData);
            Assert.True(Program.ValidatePassport(passport));
        }
        
        [Theory]
        [InlineData(@"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926")]
        [InlineData(@"iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946")]
        [InlineData(@"hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277")]
        [InlineData(@"hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007")]
        public void ValidatePassport_Invalid(string passportData)
        {
            var passport = Program.ParsePassport(passportData);
            Assert.False(Program.ValidatePassport(passport));
        }
    }
}