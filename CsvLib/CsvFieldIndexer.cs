﻿using System;
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

        private void GenerateIndex(string file)
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

        private const byte FileFormatVersion = 1;

        private void SaveFile(string indexFile)
        {
            if (indexFile == null) { return; }
            if (File.Exists(indexFile))
            {
                File.Delete(indexFile);
            }
            Stream streamOut = File.Open(indexFile, FileMode.Create);
            using (BinaryWriter binWriter = new(streamOut))
            {
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
                    for (int i = 0; i < currentFieldIndex.Count; i++)
                    {
                        binWriter.Write(currentFieldIndex[i]);
                    }
                }
            }
            streamOut.Close();
        }

        private bool LoadFile(string indexFile)
        {
            if (File.Exists(indexFile) == false)
            {
                return false;
            }
            List<long> tempIndex;
            List<List<long>> tempFieldIndex;
            Stream streamIn = File.Open(indexFile, FileMode.Open);
            try
            {
                using BinaryReader binReader = new(streamIn);
                
                byte magik0 = binReader.ReadByte();
                byte magik1 = binReader.ReadByte();
                byte magik2 = binReader.ReadByte();
                if (magik0 != (byte)'C' || magik1 != (byte)'S' || magik2 != (byte)'V') { return false; }

                byte fileVersion = binReader.ReadByte();
                if (fileVersion != FileFormatVersion) { return false; }

                int numIndexes = binReader.ReadInt32();
                tempIndex = new List<long>(numIndexes);
                for (int i = 0; i < numIndexes; i++)
                {
                    long value = binReader.ReadInt64();
                    tempIndex.Add(value);
                }
                
                int numFieldIndexes = binReader.ReadInt32();
                tempFieldIndex = new List<List<long>>(numFieldIndexes);
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
            _index = tempIndex;
            _fieldIndex = tempFieldIndex;
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
    }
}
