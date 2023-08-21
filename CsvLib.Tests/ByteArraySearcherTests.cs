using CsvLib;

namespace CvsLib;

public class ByteArraySearcherTests
{
    [Fact]
    public void Contains__EmptyNeedle_ReturnsTrue()
    {
        byte[] haystack = { 1, 2, 3, 4, 5 };
        byte[] needle = Array.Empty<byte>();
        ByteArraySearcher searcher = new(needle);

        bool result = searcher.Contains(haystack);

        Assert.True(result);
    }

    [Fact]
    public void Contains__NeedleAtBeginning_ReturnsTrue()
    {
        byte[] haystack = { 1, 2, 3, 4, 5 };
        byte[] needle = { 1, 2, 3 };
        ByteArraySearcher searcher = new(needle);

        bool result = searcher.Contains(haystack);

        Assert.True(result);
    }

    [Fact]
    public void Contains__NeedleInMiddle_ReturnsTrue()
    {
        byte[] haystack = { 1, 2, 3, 4, 5 };
        byte[] needle = { 3, 4 };
        ByteArraySearcher searcher = new(needle);

        bool result = searcher.Contains(haystack);

        Assert.True(result);
    }

    [Fact]
    public void Contains__NeedleAtEnd_ReturnsTrue()
    {
        byte[] haystack = { 1, 2, 3, 4, 5 };
        byte[] needle = { 4, 5 };
        ByteArraySearcher searcher = new(needle);

        bool result = searcher.Contains(haystack);

        Assert.True(result);
    }

    [Fact]
    public void Contains__NeedleNotPresent_ReturnsFalse()
    {
        byte[] haystack = { 1, 2, 3, 4, 5 };
        byte[] needle = { 5, 6, 7 };
        ByteArraySearcher searcher = new(needle);

        bool result = searcher.Contains(haystack);

        Assert.False(result);
    }
}
