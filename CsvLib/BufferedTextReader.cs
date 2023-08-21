using System.IO;
using System.Text;

namespace CsvLib
{
    public class BufferedTextReader : TextReader
    {
        private readonly TextReader _baseReader;
        private int _position;
        private readonly StringBuilder _sbBuffer = new();

        private readonly Encoding _currentEncoding = Encoding.Default;

        public BufferedTextReader(TextReader baseReader)
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
            if (read != -1)
            {
                _sbBuffer.Append((char)read);
            }
            return read;
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
