using System.Collections;
using Day16;

// var hex = "D2FE28";
// var hex = "38006F45291200";
// var hex = "EE00D40C823060";
// var hex = "8A004A801A8002F478";
// var hex = "620080001611562C8802118E34";
// var hex = "C0015000016115A2E0802F182340";
// var hex = "A0016C880162017C3686B18A3D4780";
var hex = File.ReadAllText("../../inputs/day16.txt");

var bitString = string.Join(string.Empty,
    hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))
);

var rootPacket = PacketFactory.CreatePacket(bitString, 0);
Console.WriteLine(rootPacket);
Console.WriteLine(rootPacket.VersionSum);
Console.WriteLine(rootPacket.Value);
