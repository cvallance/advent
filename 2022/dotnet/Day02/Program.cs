var lines = File.ReadLines("../../inputs/day2.txt").ToList();
const string rock = "rock";
const string paper = "paper";
const string scissors = "scissors";
const string lose = "X";
const string draw = "Y";
const string win = "Z";

var part1Games = new List<(string, string)>();
var part2Games = new List<(string, string)>();

var player1Map = new Dictionary<string, string>()
{
    { "A", rock },
    { "B", paper },
    { "C", scissors },
};

var player2Map = new Dictionary<string, string>()
{
    { "X", rock },
    { "Y", paper },
    { "Z", scissors },
};

string WinLoseThing(string p1, string p2)
{
    return p1 switch
    {
        rock => p2 switch
        {
            lose => scissors,
            win => paper,
            _ => rock
        },
        paper => p2 switch
        {
            lose => rock,
            win => scissors,
            _ => paper
        },
        scissors => p2 switch
        {
            lose => paper,
            win => rock,
            _ => scissors
        },
        _ => throw new Exception("Bananas")
    };
}


foreach (var line in lines)
{
    var plays = line.Split(" ");
    var player1Plays = player1Map[plays[0]]; 
    part1Games.Add((player1Plays, player2Map[plays[1]]));
    part2Games.Add((player1Plays, WinLoseThing(player1Plays, plays[1])));
}

int Points(string p1, string p2)
{
    return p1 switch
    {
        rock => p2 switch
        {
            paper => 6,
            scissors => 0,
            _ => 3
        },
        paper => p2 switch
        {
            scissors => 6,
            rock => 0,
            _ => 3
        },
        scissors => p2 switch
        {
            rock => 6,
            paper => 0,
            _ => 3
        },
        _ => throw new Exception("Jam")
    };
}

var pointMap = new Dictionary<string, int>()
{
    { rock, 1 },
    { paper, 2 },
    { scissors, 3 },
};

var part1 = 0;
foreach (var game in part1Games)
{
    part1 += pointMap[game.Item2];
    part1 += Points(game.Item1, game.Item2);
}

Console.WriteLine($"Part 1: {part1}");

var part2 = 0;
foreach (var game in part2Games)
{
    part2 += pointMap[game.Item2];
    part2 += Points(game.Item1, game.Item2);
}

Console.WriteLine($"Part 2: {part2}");
