using CsvLib;

namespace CvsLib;

[TestSubject(typeof(CsvParser))]
public class CsvParserTest
{
    #region Parse
    
    [Fact]
    public void Parse__Empty__Empty()
    {
        // --- Arrange
        StringReader sr = new(string.Empty);

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Empty(parser.Data);
    }
    
    [Fact]
    public void Parse__PlainText__OneRowOneColumn()
    {
        // --- Arrange
        StringReader sr = new("Hello World");

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Single(parser.Data);
        Assert.Single(parser.Data[0]);
        Assert.Equal("Hello World", parser.Data[0][0]);
    }


    [Fact]
    public void Parse__TwoLinesOfPainText__TwoRows()
    {
        // --- Arrange
        StringReader sr = new(
            """
            Hello World
            Hello World
            """);

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Equal(2, parser.Data.Count);
        Assert.Single(parser.Data[0]);
        Assert.Equal("Hello World", parser.Data[0][0]);
        Assert.Single(parser.Data[1]);
        Assert.Equal("Hello World", parser.Data[1][0]);
    }

    [Fact]
    public void Parse__TwoLinesOfQuotedText__TwoRows()
    {
        // --- Arrange
        StringReader sr = new(
            """
            "Hello World"
            "Hello World"
            """);

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Equal(2, parser.Data.Count);
        Assert.Single(parser.Data[0]);
        Assert.Equal("Hello World", parser.Data[0][0]);
        Assert.Single(parser.Data[1]);
        Assert.Equal("Hello World", parser.Data[1][0]);
    }

    [Fact]
    public void Parse__TwoLinesWithTwoQuotedColumns__TwoRowsTwoFields()
    {
        // --- Arrange
        StringReader sr = new(
            """
            "Hello","World"
            "Hello","World"
            """);

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Equal(2, parser.Data.Count);
        Assert.Equal(2, parser.Data[0].Count);
        Assert.Equal("Hello", parser.Data[0][0]);
        Assert.Equal("World", parser.Data[0][1]);
        Assert.Equal(2, parser.Data[1].Count);
        Assert.Equal("Hello", parser.Data[1][0]);
        Assert.Equal("World", parser.Data[1][1]);
    }

    [Fact]
    public void GenerateIndex__TwoLinesWithTwoQuotedColumnsWithUnicode__TwoRowsTwoFields()
    {
        // --- Arrange
        StringReader sr = new(
            """
            "Hélló","Wórld"
            "Hélló","Wórld"
            """);

        // --- Act
        CsvParser parser = new();
        parser.Parse(sr);

        // --- Assert
        Assert.Equal(2, parser.Data.Count);
        Assert.Equal(2, parser.Data[0].Count);
        Assert.Equal("Hélló", parser.Data[0][0]);
        Assert.Equal("Wórld", parser.Data[0][1]);
        Assert.Equal(2, parser.Data[1].Count);
        Assert.Equal("Hélló", parser.Data[1][0]);
        Assert.Equal("Wórld", parser.Data[1][1]);
    }

    #endregion Parse
}
