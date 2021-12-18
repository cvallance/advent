namespace Day16;

public static class PacketFactory
{
    public static Packet CreatePacket(string bitStr, int start)
    {
        var header = Header.Create(bitStr, start);
        // Console.WriteLine(header);
        return header.TypeId switch
        {
            4 => LiteralValuePacket.Create(bitStr, start, header),
            _ => OperatorPacket.Create(bitStr, start, header)
        };
    }
}

public class Header
{
    public int Version { get; set; }
    public int TypeId { get; set; }

    public static Header Create(string bitStr, int start = 0)
    {
        var version = Convert.ToInt32(bitStr.Substring(start, 3), 2);
        var typeId = Convert.ToInt32(bitStr.Substring(start + 3, 3), 2);
        return new Header {Version = version, TypeId = typeId};
    }

    public override string ToString()
    {
        return $"Header(version={Version},typeId={TypeId})";
    }
}

public abstract class Packet
{
    public Header Header { get; }
    public int Length { get; }
    public virtual int VersionSum => Header.Version;
    public abstract long Value { get; }

    protected Packet(Header header, int length)
    {
        Header = header;
        Length = length;
    }

    public abstract override string ToString();
}

public class LiteralValuePacket : Packet
{
    public override long Value { get; }

    private LiteralValuePacket(long value, Header header, int length) : base(header, length)
    {
        Value = value;
    }
    
    public static LiteralValuePacket Create(string bitStr, int start, Header header)
    {
        Console.WriteLine($"LiteralValuePacket.Create start={start} {header}");
        var totalLength = 6;
        var lastDigitDone = false;
        var numberStr = string.Empty;
        while (!lastDigitDone)
        {
            var literalStart = Convert.ToInt32(bitStr.Substring(start + totalLength, 1), 2);
            numberStr += bitStr.Substring(start + totalLength + 1, 4);
            if (literalStart == 0) lastDigitDone = true;
            totalLength += 5;
        }
        return new LiteralValuePacket(Convert.ToInt64(numberStr, 2), header, totalLength);
    }

    public override string ToString()
    {
        return $"LiteralPacket(value={Value}) {Header}";
    }
}

public class OperatorPacket : Packet
{
    public List<Packet> SubPackets { get; }

    public override long Value
    {
        get
        {
            return Header.TypeId switch
            {
                0 => SubPackets.Sum(x => x.Value),
                1 => SubPackets.Aggregate(1L, (current, subPacket) => current * subPacket.Value),
                2 => SubPackets.Min(x => x.Value),
                3 => SubPackets.Max(x => x.Value),
                5 => SubPackets[0].Value > SubPackets[1].Value ? 1 : 0,
                6 => SubPackets[0].Value < SubPackets[1].Value ? 1 : 0,
                7 => SubPackets[0].Value == SubPackets[1].Value ? 1 : 0,
                _ => throw new Exception($"Unexpected TypeId{Header.TypeId}")
            };
        }
    }

    public override int VersionSum
    {
        get { return base.VersionSum + SubPackets.Sum(x => x.VersionSum); }
    }

    private OperatorPacket(List<Packet> subPackets, Header header, int totalLength) : base(header, totalLength)
    {
        SubPackets = subPackets;
    }

    public static OperatorPacket Create(string bitStr, int start, Header header)
    {
        Console.WriteLine($"OperatorPacket.Create start={start} {header}");
        
        var subPackets = new List<Packet>();
        var lengthType = bitStr[start + 6];
        var totalLength = 7;
        switch (lengthType)
        {
            // The next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
            case '0':
                var lengthOfBits = Convert.ToInt32(bitStr.Substring(start + totalLength, 15), 2);
                Console.WriteLine($"Length of bits {lengthOfBits}");
                totalLength += 15;
                var subPacketLength = 0;
                while (subPacketLength < lengthOfBits)
                {
                    Console.WriteLine("HERE 1");
                    var subPacket = PacketFactory.CreatePacket(bitStr, start + totalLength);
                    totalLength += subPacket.Length;
                    subPacketLength += subPacket.Length;
                    Console.WriteLine($"subPacketLength {subPacketLength}");
                    subPackets.Add(subPacket);
                }
                break;
            // The next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
            case '1':
                var numOfSubPackets = Convert.ToInt32(bitStr.Substring(start + totalLength, 11), 2);
                totalLength += 11;
                for (var i = 0; i < numOfSubPackets; i++)
                {
                    Console.WriteLine("HERE 2");
                    var subPacket = PacketFactory.CreatePacket(bitStr, start + totalLength);
                    totalLength += subPacket.Length;
                    subPackets.Add(subPacket);
                }
                break;
            default:
                throw new Exception($"Unexpected lengthType {lengthType}");
        }
        
        return new OperatorPacket(subPackets, header, totalLength);
    }

    public override string ToString()
    {
        return $"OperatorPacket(subPackets={SubPackets.Count}) {Header}";
    }
}