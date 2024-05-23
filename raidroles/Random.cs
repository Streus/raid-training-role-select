using System.Globalization;
using System.Security.Cryptography;

partial class Program {
    static Random Random { get; set; } = new();
    static int Seed { get; set; }

    static void SetSeed(string seed) {
        try {
            Seed = int.Parse(seed, NumberStyles.HexNumber);
            Random = new Random(Seed);
        } catch (Exception e) when (e is ArgumentException ||  e is FormatException || e is OverflowException) {
            Console.Error.WriteLine(e);
            return;
        }
    }

    static string GenerateSeed() {
        using var secureRandom = RandomNumberGenerator.Create();
        var seedBytes = new byte[sizeof(int)];
        secureRandom.GetBytes(seedBytes);
        Seed = BitConverter.ToInt32(seedBytes);
        Random = new Random(Seed);

        string seedString = Seed.ToString("X8");
        return seedString;
    }
}
