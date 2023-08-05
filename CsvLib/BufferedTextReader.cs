using System;
using System.IO;
using System.Text;

namespace CsvLib
{
    public class BufferedTextReader : TextReader
    {
        private readonly TextReader _baseReader;
        private int _position;
        private readonly StringBuilder _sbBuffer = new StringBuilder();

        public BufferedTextReader(TextReader baseReader)
        {
            _baseReader = baseReader;
        }

        public override int Read()
        {
            _position++;
            int read = _baseReader.Read();
            if (read != -1)
            {
                _sbBuffer.Append((char)read);
            }
            return read;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException("Read buffered method on BufferedTextReader");
        }

        public override int Peek()
        {
            return _baseReader.Peek();
        }

        public int Position
        {
            get { return _position; }
        }

        public string GetBuffer()
        {
            return _sbBuffer.ToString();
        }

        public void CleanBuffer()
        {
            _sbBuffer.Clear();
        }
    }
}
