using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvView.Code
{
    public class CsvIndexer
    {
        private bool _insideString;

        private readonly char _separator;
        private readonly char _quoteChar;
        private readonly char _escapeChar;

        public CsvIndexer(char separator = ',', char quoteChar = '"', char escapeChar = '\\')
        {
            _separator = separator;
            _quoteChar = quoteChar;
            _escapeChar = escapeChar;
        }

        private List<long> _index = new List<long>();

        public List<long> Index { get { return _index; } }

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
                    c = line[i];
                }
            }
        }

        private class TrackingTextReader : TextReader
        {
            private readonly TextReader _baseReader;
            private int _position;

            public TrackingTextReader(TextReader baseReader)
            {
                _baseReader = baseReader;
            }

            public override int Read()
            {
                _position++;
                return _baseReader.Read();
            }

            public override int Peek()
            {
                return _baseReader.Peek();
            }

            public int Position
            {
                get { return _position; }
            }
        }

        public void GenerateIndex(string file)
        {
            _insideString = false;
            _index.Clear();
            FileStream stream = new FileStream(file, FileMode.Open);
            using (StreamReader streamReader = new StreamReader(stream, Encoding.Default, true, 4096))
            using (TrackingTextReader reader = new TrackingTextReader(streamReader))
            {
                string currentLine;
                if (_insideString == false)
                {
                    _index.Add(reader.Position);
                }
                while ((currentLine = reader.ReadLine()) != null)
                {
                    DummyParser(currentLine);
                    if (_insideString == false)
                    {
                        _index.Add(reader.Position);
                    }
                }
            }
            stream.Close();
        }
        
        private void Index_SaveFile(string indexFile)
        {
            if (File.Exists(indexFile))
            {
                File.Delete(indexFile);
            }
            Stream streamOut = File.Open(indexFile, FileMode.Create);
            using (BinaryWriter binWriter = new BinaryWriter(streamOut))
            {
                binWriter.Write(_index.Count);
                for (int i = 0; i < _index.Count; i++)
                {
                    binWriter.Write(_index[i]);
                }
            }
            streamOut.Close();
        }

        private static List<long> Index_LoadFile(string indexFile)
        {
            var tempIndex = new List<long>();

            Stream streamIn = File.Open(indexFile, FileMode.Open);
            using (BinaryReader binReader = new BinaryReader(streamIn))
            {
                int numRegs = binReader.ReadInt32();
                for (int i = 0; i < numRegs; i++)
                {
                    long value = binReader.ReadInt64();
                    tempIndex.Add(value);
                }
            }
            streamIn.Close();
            return tempIndex;
        }

        public void LoadIndexOfFile(string file)
        {
            DateTime dtFile = File.GetCreationTime(file);
            string indexFile = file + ".idx";
            if (File.Exists(indexFile) && File.GetCreationTime(indexFile) > dtFile)
            {
                _index = Index_LoadFile(indexFile);
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
