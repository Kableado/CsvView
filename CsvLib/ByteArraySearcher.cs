namespace CsvLib;

public class ByteArraySearcher
{
    private readonly byte[] _needle;

    public ByteArraySearcher(byte[] needle)
    {
        _needle = needle;
    }

    public bool Contains(byte[] haystack)
    {
        return Contains(haystack, haystack.Length);
    }

    public bool Contains(byte[] haystack, int length)
    {
        // TODO: Implement the Boyer-Moore algorithm
        for (int i = 0; i <= length - _needle.Length; i++)
        {
            bool found = true;

            for (int j = 0; j < _needle.Length; j++)
            {
                if (haystack[i + j] != _needle[j])
                {
                    found = false;
                    break;
                }
            }

            if (found) { return true; }
        }

        return false;
    }
}
