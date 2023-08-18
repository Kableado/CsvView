using System.IO;
using System.Text;

namespace CsvLib
{
    public class TrackingTextReader : TextReader
    {
        private readonly TextReader _baseReader;
        private int _position;
        private readonly Encoding _currentEncoding = Encoding.Default;

        public TrackingTextReader(TextReader baseReader)
        {
            _baseReader = baseReader;
            if (baseReader is StreamReader streamReader)
            {
                _currentEncoding = streamReader.CurrentEncoding;
            }
        }

        public override int Read()
        {
            int read = _baseReader.Read();
            if (read > 127)
            {
                int count = _currentEncoding.GetByteCount(((char)read).ToString());
                _position += count;
            }
            else
            {
                _position++;
            }
            return read;
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
