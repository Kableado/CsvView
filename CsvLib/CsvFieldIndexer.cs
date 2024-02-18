using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvLib;

public class CsvFieldIndexer
{
    #region Declarations
    
    private const byte FileFormatVersion = 1;

    private bool _insideString;

    private Encoding _currentEncoding = Encoding.Default;

    private readonly char _separator;
    private readonly char _quoteChar;
    private readonly char _escapeChar;

    #endregion Declarations
    
    #region Life cycle
    
    public CsvFieldIndexer(char separator = ',', char quoteChar = '"', char escapeChar = '\\')
    {
        _separator = separator;
        _quoteChar = quoteChar;
        _escapeChar = escapeChar;
    }
    
    #endregion Life cycle

    #region Properties
    
    private List<long> _index = new();

    public List<long> Index { get { return _index; } }

    private List<List<long>> _fieldIndex = new();

    public List<List<long>> FieldIndex { get { return _fieldIndex; } }

    #endregion Properties

    #region Parsing
    
    private void DummyParser(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == _separator && _insideString == false)
            {
                continue;
            }
            if (c == _quoteChar && _insideString == false)
            {
                _insideString = true;
                continue;
            }
            if (c == _quoteChar && _insideString)
            {
                _insideString = false;
                continue;
            }
            if (c == _escapeChar && _insideString)
            {
                i++;
            }
        }
    }

    private List<long> ParseLineIndex(string line, long lineOffset)
    {
        List<long> fieldPositions = new();
        long? fieldStartPosition = null;
        long? fieldEndPosition = null;
        int unicodeDelta = 0;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == _separator && _insideString == false)
            {
                if (fieldStartPosition != null && fieldEndPosition != null)
                {
                    fieldPositions.Add((long)fieldStartPosition);
                    fieldPositions.Add((long)fieldEndPosition);
                }
                fieldStartPosition = null;
                fieldEndPosition = null;
            }
            else if (c == _quoteChar && _insideString == false)
            {
                _insideString = true;
            }
            else if (c == _quoteChar && _insideString)
            {
                _insideString = false;
            }
            else if (c == _escapeChar && _insideString)
            {
                i++;
            }
            else if ((c == '\n' || c == '\r') && _insideString == false)
            {
                break;
            }
            else
            {
                if (c > 127)
                {
                    unicodeDelta += _currentEncoding.GetByteCount(c.ToString()) - 1;
                }

                long absolutePosition = lineOffset + i + unicodeDelta;
                fieldStartPosition ??= absolutePosition;
                fieldEndPosition = absolutePosition;
            }
        }
        if (_insideString == false)
        {
            if (fieldStartPosition != null && fieldEndPosition != null)
            {
                fieldPositions.Add((long)fieldStartPosition);
                fieldPositions.Add((long)fieldEndPosition);
            }
        }
        return fieldPositions;
    }

    #endregion Parsing

    #region GenerateIndex
    
    private void GenerateIndex(string file)
    {
        using FileStream stream = new(file, FileMode.Open);
        using StreamReader streamReader = new(stream, Encoding.Default, true, 4096);
        GenerateIndex(streamReader);
        stream.Close();
    }

    public void GenerateIndex(TextReader textReader)
    {
        _insideString = false;
        _index.Clear();
        _index.Add(0);
        int idxRow = 0;
        if (textReader is StreamReader streamReader)
        {
            _currentEncoding = streamReader.CurrentEncoding;
        }
        using BufferedTextReader reader = new(textReader);
        while (reader.ReadLine() is { } currentLine)
        {
            DummyParser(currentLine);
            if (_insideString) { continue; }

            string fullLine = reader.GetBuffer();
            reader.CleanBuffer();
            List<long> fieldIndexes = ParseLineIndex(fullLine, _index[idxRow]);
            _fieldIndex.Add(fieldIndexes);

            _index.Add(reader.Position);

            idxRow++;
        }
    }

    #endregion GenerateIndex

    #region Save
    
    public void Save(Stream streamOut)
    {
        using BinaryWriter binWriter = new(streamOut);

        binWriter.Write((byte)'C');
        binWriter.Write((byte)'S');
        binWriter.Write((byte)'V');

        binWriter.Write(FileFormatVersion);

        binWriter.Write(_index.Count);
        foreach (long currentIndex in _index)
        {
            binWriter.Write(currentIndex);
        }

        binWriter.Write(_fieldIndex.Count);
        foreach (List<long> currentFieldIndex in _fieldIndex)
        {
            binWriter.Write(currentFieldIndex.Count);
            foreach (long fieldIndex in currentFieldIndex)
            {
                binWriter.Write(fieldIndex);
            }
        }
    }

    private void SaveFile(string indexFile)
    {
        if (File.Exists(indexFile))
        {
            File.Delete(indexFile);
        }
        Stream streamOut = File.Open(indexFile, FileMode.Create);
        Save(streamOut);
        streamOut.Close();
    }

    #endregion Save
    
    #region Load
    
    public bool Load(Stream streamIn)
    {
        using BinaryReader binReader = new(streamIn);

        byte magik0 = binReader.ReadByte();
        byte magik1 = binReader.ReadByte();
        byte magik2 = binReader.ReadByte();
        if (magik0 != (byte)'C' || magik1 != (byte)'S' || magik2 != (byte)'V') { return false; }

        byte fileVersion = binReader.ReadByte();
        if (fileVersion != FileFormatVersion) { return false; }

        int numIndexes = binReader.ReadInt32();
        List<long> tempIndex = new(numIndexes);
        for (int i = 0; i < numIndexes; i++)
        {
            long value = binReader.ReadInt64();
            tempIndex.Add(value);
        }

        int numFieldIndexes = binReader.ReadInt32();
        List<List<long>> tempFieldIndex = new(numFieldIndexes);
        for (int j = 0; j < numFieldIndexes; j++)
        {
            int numCurrentFieldIndexes = binReader.ReadInt32();
            List<long> currentFieldIndex = new(numCurrentFieldIndexes);
            for (int i = 0; i < numCurrentFieldIndexes; i++)
            {
                long value = binReader.ReadInt64();
                currentFieldIndex.Add(value);
            }
            tempFieldIndex.Add(currentFieldIndex);
        }
        
        _index = tempIndex;
        _fieldIndex = tempFieldIndex;
        return true;
    }

    private bool LoadFile(string indexFile)
    {
        if (File.Exists(indexFile) == false)
        {
            return false;
        }
        Stream streamIn = File.Open(indexFile, FileMode.Open);
        try
        {
            if (Load(streamIn) == false) return false;
        }
        catch (Exception)
        {
            // NON NON NOM
            return false;
        }
        finally
        {
            streamIn.Close();
        }
        return true;
    }

    public void LoadIndexOfFile(string file)
    {
        DateTime dtFile = File.GetCreationTime(file);
        string indexFile = $"{file}.idx";
        if (File.Exists(indexFile) && File.GetCreationTime(indexFile) > dtFile)
        {
            if (LoadFile(indexFile)) { return; }
        }

        // Generate index
        DateTime dtNow = DateTime.UtcNow;
        GenerateIndex(file);
        TimeSpan tsGenIndex = DateTime.UtcNow - dtNow;

        // Save Index if expensive generation
        if (tsGenIndex.TotalSeconds > 2)
        {
            SaveFile(indexFile);
        }
    }

    #endregion Load

    #region Search
    
    public List<long> Search(Stream streamIn, string textToSearch, Action<float>? notifyProgress = null)
    {
        // TODO: Use MemoryMappedFile for better IO performance
        DateTime datePrevious = DateTime.UtcNow;
        List<long> newIndexes = new();
        byte[] bText = Encoding.UTF8.GetBytes(textToSearch);
        ByteArraySearcher searcher = new(bText);
        byte[] buffer = new byte[1024];
        for (int j = 0; j < _fieldIndex.Count; j++)
        {
            for (int i = 0; i < _fieldIndex[j].Count; i += 2)
            {
                TimeSpan tsElapsed = DateTime.UtcNow - datePrevious;
                if (tsElapsed.TotalMilliseconds > 200)
                {
                    datePrevious = DateTime.UtcNow;
                    notifyProgress?.Invoke(j / (float)_fieldIndex.Count);
                }

                long offset = _fieldIndex[j][i];
                int length = (int)(_fieldIndex[j][i + 1] - offset) + 1;

                if (buffer.Length < length)
                {
                    buffer = new byte[length];
                }
                streamIn.Seek(offset, SeekOrigin.Begin);
                int read = streamIn.Read(buffer, 0, length);
                if (read != length) { throw new Exception($"Search: Expected {length} bytes, but read {read}"); }

                bool matches = searcher.Contains(buffer, length);
                if (matches == false) { continue; }

                newIndexes.Add(_index[j]);
                break;
            }
        }

        return newIndexes;
    }
    
    public List<long> SearchFile(string fileName, string textToSearch, Action<float>? notifyProgress = null)
    {
        List<long> index;
        using FileStream streamIn = new(fileName, FileMode.Open);
        try
        {
            index = Search(streamIn, textToSearch, notifyProgress);
        }
        finally
        {
            streamIn.Close();
        }
        return index;
    }

    #endregion Search
}
