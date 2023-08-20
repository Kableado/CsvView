using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvLib
{
    public class CsvFieldIndexer
    {
        private bool _insideString;

        private Encoding _currentEncoding = Encoding.Default;
        
        private readonly char _separator;
        private readonly char _quoteChar;
        private readonly char _escapeChar;

        public CsvFieldIndexer(char separator = ',', char quoteChar = '"', char escapeChar = '\\')
        {
            _separator = separator;
            _quoteChar = quoteChar;
            _escapeChar = escapeChar;
        }

        private List<long> _index = new();

        public List<long> Index { get { return _index; } }

        private List<List<long>> _fieldIndex = new();

        public List<List<long>> FieldIndex { get { return _fieldIndex; } }

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
                    if (fieldStartPosition != null)
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
                if (fieldStartPosition != null)
                {
                    fieldPositions.Add((long)fieldStartPosition);
                    fieldPositions.Add((long)fieldEndPosition);
                }
            }
            return fieldPositions;
        }

        public void GenerateIndex(string file)
        {
            using FileStream stream = new(file, FileMode.Open);
            using StreamReader streamReader = new(stream, Encoding.Default, true, 4096);
            GenerateIndex(streamReader);
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
            string currentLine;
            while ((currentLine = reader.ReadLine()) != null)
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

        private void Index_SaveFile(string indexFile)
        {
            if (indexFile == null) { return; }
            if (File.Exists(indexFile))
            {
                File.Delete(indexFile);
            }
            Stream streamOut = File.Open(indexFile, FileMode.Create);
            using (BinaryWriter binWriter = new(streamOut))
            {
                binWriter.Write(_index.Count);
                for (int i = 0; i < _index.Count; i++)
                {
                    binWriter.Write(_index[i]);
                }
                binWriter.Write("test");
            }
            streamOut.Close();
        }

        private void Index_LoadFile(string indexFile)
        {
            if (File.Exists(indexFile) == false)
            {
                return;
            }
            List<long> tempIndex = new();
            Stream streamIn = File.Open(indexFile, FileMode.Open);
            using (BinaryReader binReader = new(streamIn))
            {
                int numRegs = binReader.ReadInt32();
                for (int i = 0; i < numRegs; i++)
                {
                    long value = binReader.ReadInt64();
                    tempIndex.Add(value);
                }
            }
            streamIn.Close();
            _index = tempIndex;
        }

        public void LoadIndexOfFile(string file)
        {
            DateTime dtFile = File.GetCreationTime(file);
            string indexFile = $"{file}.idx2";
            if (File.Exists(indexFile) && File.GetCreationTime(indexFile) > dtFile)
            {
                Index_LoadFile(indexFile);
            }
            else
            {
                // Generate index
                DateTime dtNow = DateTime.UtcNow;
                GenerateIndex(file);
                TimeSpan tsGenIndex = DateTime.UtcNow - dtNow;

                // Save Index if expensive generation
                if (tsGenIndex.TotalSeconds > 2)
                {
                    Index_SaveFile(indexFile);
                }
            }
        }
    }
}
