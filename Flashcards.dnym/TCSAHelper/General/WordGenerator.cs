namespace TCSAHelper.General;

public static class WordGenerator
{
    // Basically a port of https://stackoverflow.com/a/1891422
    private static readonly string[] _vowels = { "a", "e", "i", "o", "u" };
    private static readonly string[] _consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m",
                                                        "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };

    private static string V(Random random)
    {
        return _vowels[random.Next(0, _vowels.Length)];
    }

    private static string C(Random random)
    {
        return _consonants[random.Next(0, _consonants.Length)];
    }

    private static string CV(Random random)
    {
        return C(random) + V(random);
    }

    private static string CVC(Random random)
    {
        return C(random) + V(random) + C(random);
    }

    private static string Syllable(Random random)
    {
        string[] syllables = { V(random), CV(random), CVC(random) };
        return syllables[random.Next(0, syllables.Length)];
    }

    public static string CreateFakeWord(Random random)
    {
        string word = string.Empty;
        int syllables = random.Next(1, 4);
        for (int i = 0; i < syllables; i++)
        {
            word += Syllable(random);
        }
        return word;
    }
}
