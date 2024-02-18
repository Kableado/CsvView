using System.Text;

namespace CvsLib.Tests;

[TestSubject(typeof(CsvFieldIndexer))]
public class CsvFieldIndexerTests
{
    #region GenerateIndex

    [Fact]
    public void GenerateIndex__Empty()
    {
        // --- Arrange
        StringReader sr = new(string.Empty);

        // --- Act
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Single(indexer.Index);

        Assert.Equal(0, indexer.Index[0]);
        Assert.Empty(indexer.FieldIndex);
    }

    [Fact]
    public void GenerateIndex__PlainText__OneRow()
    {
        // --- Arrange
        StringReader sr = new("Hello World");

        // --- Act
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Equal(2, indexer.Index.Count);
        Assert.Equal(0, indexer.Index[0]);
        Assert.Equal(12, indexer.Index[1]);

        Assert.Single(indexer.FieldIndex);
        Assert.Equal(0, indexer.FieldIndex[0][0]);
        Assert.Equal(10, indexer.FieldIndex[0][1]);
    }

    [Fact]
    public void GenerateIndex__TwoLinesOfPainText__TwoRows()
    {
        // --- Arrange
        StringReader sr = new(
            """
              Hello World
              Hello World
              """);

        // --- Act
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Equal(3, indexer.Index.Count);
        Assert.Equal(0, indexer.Index[0]);
        Assert.Equal(12, indexer.Index[1]);
        Assert.Equal(24, indexer.Index[2]);

        Assert.Equal(2, indexer.FieldIndex.Count);
        Assert.Equal(2, indexer.FieldIndex[0].Count);
        Assert.Equal(0, indexer.FieldIndex[0][0]);
        Assert.Equal(10, indexer.FieldIndex[0][1]);
        Assert.Equal(2, indexer.FieldIndex[1].Count);
        Assert.Equal(12, indexer.FieldIndex[1][0]);
        Assert.Equal(22, indexer.FieldIndex[1][1]);
    }

    [Fact]
    public void GenerateIndex__TwoLinesOfQuotedText__TwoRows()
    {
        // --- Arrange
        StringReader sr = new(
            """
              "Hello World"
              "Hello World"
              """);

        // --- Act
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Equal(3, indexer.Index.Count);
        Assert.Equal(0, indexer.Index[0]);
        Assert.Equal(14, indexer.Index[1]);
        Assert.Equal(28, indexer.Index[2]);

        Assert.Equal(2, indexer.FieldIndex.Count);
        Assert.Equal(2, indexer.FieldIndex[0].Count);
        Assert.Equal(1, indexer.FieldIndex[0][0]);
        Assert.Equal(11, indexer.FieldIndex[0][1]);
        Assert.Equal(2, indexer.FieldIndex[1].Count);
        Assert.Equal(15, indexer.FieldIndex[1][0]);
        Assert.Equal(25, indexer.FieldIndex[1][1]);
    }

    [Fact]
    public void GenerateIndex__TwoLinesWithTwoQuotedColumns__TwoRowsTwoFields()
    {
        // --- Arrange
        StringReader sr = new(
            """
              "Hello","World"
              "Hello","World"
              """);

        // --- Act
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Equal(3, indexer.Index.Count);
        Assert.Equal(0, indexer.Index[0]);
        Assert.Equal(16, indexer.Index[1]);
        Assert.Equal(32, indexer.Index[2]);

        Assert.Equal(2, indexer.FieldIndex.Count);
        Assert.Equal(4, indexer.FieldIndex[0].Count);
        Assert.Equal(1, indexer.FieldIndex[0][0]);
        Assert.Equal(5, indexer.FieldIndex[0][1]);
        Assert.Equal(9, indexer.FieldIndex[0][2]);
        Assert.Equal(13, indexer.FieldIndex[0][3]);
        Assert.Equal(4, indexer.FieldIndex[1].Count);
        Assert.Equal(17, indexer.FieldIndex[1][0]);
        Assert.Equal(21, indexer.FieldIndex[1][1]);
        Assert.Equal(25, indexer.FieldIndex[1][2]);
        Assert.Equal(29, indexer.FieldIndex[1][3]);
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
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Assert

        Assert.Equal(3, indexer.Index.Count);
        Assert.Equal(0, indexer.Index[0]);
        Assert.Equal(19, indexer.Index[1]);
        Assert.Equal(38, indexer.Index[2]);

        Assert.Equal(2, indexer.FieldIndex.Count);
        Assert.Equal(4, indexer.FieldIndex[0].Count);
        Assert.Equal(1, indexer.FieldIndex[0][0]);
        Assert.Equal(7, indexer.FieldIndex[0][1]);
        Assert.Equal(11, indexer.FieldIndex[0][2]);
        Assert.Equal(16, indexer.FieldIndex[0][3]);
        Assert.Equal(4, indexer.FieldIndex[1].Count);
        Assert.Equal(20, indexer.FieldIndex[1][0]);
        Assert.Equal(26, indexer.FieldIndex[1][1]);
        Assert.Equal(30, indexer.FieldIndex[1][2]);
        Assert.Equal(35, indexer.FieldIndex[1][3]);
    }

    #endregion GenerateIndex

    #region Search

    [Fact]
    public void Search__TwoLinesWithTwoQuotedColumns__OneIndexFirstRow()
    {
        // --- Arrange
        const string strText = 
            """
            "Hello","test"
            "Hello","World"
            """;
        StringReader sr = new(strText);
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Act
        byte[] bText = Encoding.UTF8.GetBytes(strText);
        MemoryStream ms = new(bText);
        List<long> indexes = indexer.Search(ms, "test");

        // --- Assert

        Assert.Single(indexes);
        Assert.Equal(0, indexes[0]);
    }

    [Fact]
    public void Search__TwoLinesWithTwoQuotedColumns__OneIndexSecondRow()
    {
        // --- Arrange
        const string strText =
            """
            "Hello","World"
            "Hello","test"
            """;
        StringReader sr = new(strText);
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Act
        byte[] bText = Encoding.UTF8.GetBytes(strText);
        MemoryStream ms = new(bText);
        List<long> indexes = indexer.Search(ms, "test");

        // --- Assert

        Assert.Single(indexes);
        Assert.Equal(16, indexes[0]);
    }

    [Fact]
    public void Search__TwoLinesWithTwoQuotedColumnsTwoMatches__OneIndexSecondRow()
    {
        // --- Arrange
        const string strText =
            """
            "Hello","World"
            "test","test"
            """;
        StringReader sr = new(strText);
        CsvFieldIndexer indexer = new();
        indexer.GenerateIndex(sr);

        // --- Act
        byte[] bText = Encoding.UTF8.GetBytes(strText);
        MemoryStream ms = new(bText);
        List<long> indexes = indexer.Search(ms, "test");

        // --- Assert

        Assert.Single(indexes);
        Assert.Equal(16, indexes[0]);
    }

    #endregion Search

}
