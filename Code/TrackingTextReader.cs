using System;
using System.IO;

namespace CsvView.Code
{
    public class TrackingTextReader : TextReader
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

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException("Read buffered method on TrackingTextReader");
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
}
