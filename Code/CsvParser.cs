using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvView.Code
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
            if (_currentReg == null)
            {
                _currentReg = new List<string>();
            }
            if (_currentCell == null)
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
                    if (count > 0 && Data.Count == count)
                    {
                        break;
                    }
                }
            }
            stream.Close();
        }
        
    }
}
