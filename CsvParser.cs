using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvView
{
    public class CsvParser
    {
        private bool _insideString = false;

        private char _separator = ',';
        private char _quoteChar = '"';
        private char _escapeChar = '\\';

        public CsvParser(char separator = ',', char quoteChar = '"', char escapeChar = '\\')
        {
            _separator = separator;
            _quoteChar = quoteChar;
            _escapeChar = escapeChar;
        }

        private List<List<string>> _data = new List<List<string>>();

        private List<string> _currentReg = null;
        StringBuilder _currentCell = null;
        
        public List<List<string>> Data
        {
            get { return _data; }
        }

        public void ParseLine(string line)
        {
            if(_currentReg == null)
            {
                _currentReg = new List<string>();
            }
            if(_currentCell == null)
            {
                _currentCell = new StringBuilder();
            }
            
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == _separator && _insideString == false)
                {
                    _currentReg.Add(_currentCell.ToString());
                    _currentCell.Clear();
                    continue;
                }
                if (c == _quoteChar && _insideString == false)
                {
                    _insideString = true;
                    continue;
                }
                if (c == _quoteChar && _insideString == true)
                {
                    _insideString = false;
                    continue;
                }
                if (c == _escapeChar && _insideString)
                {
                    i++;
                    if (i == line.Length) { break; }
                    c = line[i];
                }

                _currentCell.Append(c);
            }


            if (_insideString)
            {
                _currentCell.Append('\n');
            }
            else
            {
                _currentReg.Add(_currentCell.ToString());
                _currentCell.Clear();
                _data.Add(_currentReg);
                _currentReg = null;
            }
        }

        public void ParseFile(string file, long offset = 0, int count = 0)
        {
            _insideString = false;
            _data = new List<List<string>>();
            _currentReg = null;
            FileStream stream = new FileStream(file, FileMode.Open);
            stream.Seek(offset, SeekOrigin.Begin);
            using (StreamReader reader = new StreamReader(stream, Encoding.Default, true, 4096))
            {
                string currentLine;
                while ((currentLine = reader.ReadLine()) != null)
                {
                    ParseLine(currentLine);
                    if(count>0 && Data.Count== count)
                    {
                        break;
                    }
                }
            }
            stream.Close();
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
                if (c == _quoteChar && _insideString == true)
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
            private TextReader _baseReader;
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

    }
}
