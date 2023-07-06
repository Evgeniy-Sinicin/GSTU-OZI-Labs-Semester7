using System;
using System.Windows.Forms;

namespace LAB_1_2
{
    public partial class Form1 : Form
    {
        private int _tableSize;
        private string _alphabet;
        private char[,] _table;

        public Form1()
        {
            InitializeComponent();

            _alphabet = "abcdefghiklmnopqrstuvwxyz";
            _tableSize = 5;
            _table = new char[_tableSize, _tableSize];

            for (int row = 0; row < _tableSize; row++)
            {
                for (int column = 0; column < _tableSize; column++)
                {
                    _table[row, column] = _alphabet[row * _tableSize + column];
                }
            }
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            var original = encodeBox.Text;
            var ciphertext = string.Empty;
            var words = original.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (i < words.Length - 1)
                {
                    ciphertext += Encode(words[i]) + " ";
                }
                else
                {
                    ciphertext += Encode(words[i]);
                }
            }

            encodeBox.Text = string.Empty;
            decodeBox.Text = ciphertext;
        }

        private string Encode(string word)
        {
            var ciphertext = string.Empty;

            for (int i = 0; i < word.Length; i++)
            {
                ciphertext += GetColumnIndex(word[i]);
            }

            ciphertext += " ";

            for (int i = 0; i < word.Length; i++)
            {
                ciphertext += GetRowIndex(word[i]);
            }

            return ciphertext;
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            var ciphertext = decodeBox.Text;
            var original = string.Empty;
            var lines = ciphertext.Split(' ');

            for (int i = 0; i < lines.Length; i+=2)
            {
                if (i < lines.Length - 1)
                {
                    original += Decode(lines[i], lines[i + 1]) + " ";
                }
                else
                {
                    original += Decode(lines[i], lines[i + 1]);
                }
            }

            decodeBox.Text = string.Empty;
            encodeBox.Text = original;
        }

        private string Decode(string horizontalIndexes, string verticalIndexes)
        {
            var original = string.Empty;
            var symbolsCount = horizontalIndexes.Length;

            for (int i = 0; i < symbolsCount; i++)
            {
                var verticalIndex = Int32.Parse(verticalIndexes[i].ToString());
                var horizontalIndex = Int32.Parse(horizontalIndexes[i].ToString());
                original += _table[verticalIndex, horizontalIndex];
            }

            return original;
        }

        private int GetRowIndex(char symbol)
        {
            for (int row = 0; row < _tableSize; row++)
            {
                for (int column = 0; column < _tableSize; column++)
                {
                    if (_table[row, column].Equals(symbol))
                    {
                        return row;
                    }
                }
            }

            return -1;
        }

        private int GetColumnIndex(char symbol)
        {
            for (int row = 0; row < _tableSize; row++)
            {
                for (int column = 0; column < _tableSize; column++)
                {
                    if (_table[row, column].Equals(symbol))
                    {
                        return column;
                    }
                }
            }

            return -1;
        }
    }
}
