using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LAB_2_1
{
    public partial class Form1 : Form
    {
        private int ALPHABET_SIZE = 6;

        private string _alphabet;

        public Form1()
        {
            InitializeComponent();
        }

        public void DecodeButton_Click(object sender, EventArgs e)
        {
            var ciphertext = decodeBox.Text;
            var original = string.Empty;

            original = Decode(ciphertext);

            encodeBox.Text = original;
            decodeBox.Text = string.Empty;
        }

        public void EncodeButton_Click(object sender, EventArgs e)
        {
            var ciphertext = string.Empty;
            var original = encodeBox.Text;
            var slogan = sloganBox.Text;
            _alphabet = InitializeAlphabet(slogan);

            sloganLabel.Text = $"Слоган: {slogan}";
            
            ciphertext = Encode(original);

            decodeBox.Text = ciphertext;
            encodeBox.Text = string.Empty;
            sloganBox.Text = string.Empty;
        }

        private string Decode(string ciphertext)
        {
            var degrams = ciphertext.Split(' ').ToList();

            for (int i = 0; i < degrams.Count; i++)
            {
                degrams[i] = DecodeDegram(degrams[i]);
            }

            return DegramsToOriginal(degrams);
        }

        private string Encode(string original)
        {
            var ciphertext = string.Empty;
            var degrams = OriginalToDegrams(original);
            var degramsArr = degrams.Split(' ').ToList();

            degramsArr.ForEach(degram => ciphertext += $"{EncodeDegram(degram)} ");

            return ciphertext.Trim();
        }

        private string EncodeDegram(string degram)
        {
            var firstChar = degram[0];
            var secondChar = degram[1];

            if (firstChar.Equals(secondChar))
            {
                throw new Exception("A deg mustn't have two equal symbols");
            }

            var firstRowIndex = GetSymbolRowIndex(firstChar);
            var firstColumnIndex = GetSymbolColumnIndex(firstChar);
            var secondRowIndex = GetSymbolRowIndex(secondChar);
            var secondColumnIndex = GetSymbolColumnIndex(secondChar);

            if (firstRowIndex == secondRowIndex)
            {
                firstChar = GetSymbolByCoord(firstRowIndex, 
                                             firstColumnIndex + 1 >= ALPHABET_SIZE ? 0 : firstColumnIndex + 1);

                secondChar = GetSymbolByCoord(secondRowIndex,
                                              secondColumnIndex + 1 >= ALPHABET_SIZE ? 0 : secondColumnIndex + 1);
            }
            else if (firstColumnIndex == secondColumnIndex)
            {
                firstChar = GetSymbolByCoord(firstRowIndex + 1 >= ALPHABET_SIZE ? 0 : firstRowIndex + 1,
                                             firstColumnIndex);

                secondChar = GetSymbolByCoord(secondRowIndex + 1 >= ALPHABET_SIZE ? 0 : secondRowIndex + 1,
                                              secondColumnIndex);
            }
            else
            {
                firstChar = GetSymbolByCoord(firstRowIndex, secondColumnIndex);
                secondChar = GetSymbolByCoord(secondRowIndex, firstColumnIndex);
            }

            return $"{firstChar}{secondChar}";
        }

        private string DecodeDegram(string degram)
        {
            var firstChar = degram[0];
            var secondChar = degram[1];

            if (firstChar.Equals(secondChar))
            {
                throw new Exception("A deg mustn't have two equal symbols");
            }

            var firstRowIndex = GetSymbolRowIndex(firstChar);
            var firstColumnIndex = GetSymbolColumnIndex(firstChar);
            var secondRowIndex = GetSymbolRowIndex(secondChar);
            var secondColumnIndex = GetSymbolColumnIndex(secondChar);

            if (firstRowIndex == secondRowIndex)
            {
                firstChar = GetSymbolByCoord(firstRowIndex,
                                             firstColumnIndex - 1 < 0 ? ALPHABET_SIZE - 1 : firstColumnIndex - 1);

                secondChar = GetSymbolByCoord(secondRowIndex,
                                              secondColumnIndex - 1 < 0 ? ALPHABET_SIZE - 1 : secondColumnIndex - 1);
            }
            else if (firstColumnIndex == secondColumnIndex)
            {
                firstChar = GetSymbolByCoord(firstRowIndex - 1 < 0 ? ALPHABET_SIZE - 1 : firstRowIndex - 1,
                                             firstColumnIndex);

                secondChar = GetSymbolByCoord(secondRowIndex - 1 < 0 ? ALPHABET_SIZE - 1 : secondRowIndex - 1,
                                              secondColumnIndex);
            }
            else
            {
                firstChar = GetSymbolByCoord(firstRowIndex, secondColumnIndex);
                secondChar = GetSymbolByCoord(secondRowIndex, firstColumnIndex);
            }

            return $"{firstChar}{secondChar}";
        }

        private char GetSymbolByCoord(int rowIndex, int columnIndex)
        {
            return _alphabet[rowIndex * ALPHABET_SIZE + columnIndex];
        }

        private int GetSymbolRowIndex(char symbol)
        {
            int row = -1;

            for (int i = 0; i < _alphabet.Length; i++)
            {
                if (i % ALPHABET_SIZE == 0)
                {
                    row++;
                }

                if (_alphabet[i].Equals(symbol))
                {
                    return row;
                }
            }

            return -1;
        }

        private int GetSymbolColumnIndex(char symbol)
        {
            for (int i = 0; i < _alphabet.Length; i++)
            {
                if (_alphabet[i].Equals(symbol))
                {
                    return i % ALPHABET_SIZE;
                }
            }

            return -1;
        }

        private string InitializeAlphabet(string slogan)
        {
            var alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-12";

            slogan = new string(slogan.Distinct().ToArray());

            for (int i = slogan.Length - 1; i >= 0; i--)
            {
                alphabet = ReplaceSymbol(alphabet, slogan[i], 0);
            }

            return alphabet;
        }

        private string ReplaceSymbol(string text, char symbol, int position)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Equals(symbol))
                {
                    text = text.Remove(i, 1);
                    text = text.Insert(position, symbol.ToString());
                }
            }

            return text;
        }

        private string OriginalToDegrams(string original)
        {
            var spacesCount = 0;
            var degrams = original[0].ToString();
            
            original = SplitEqualsSymbols(RemoveSpaces(original));

            for (int i = 1; i < original.Length; i++)
            {
                if (i % 2 == 0)
                {
                    degrams += $" {original[i]}";
                    spacesCount++;
                }
                else
                {
                    degrams += original[i];
                }
            }

            if ((degrams.Length - 1 + spacesCount) % 2 == 0)
            {
                degrams += "я";
            }

            return degrams;
        }

        private string DegramsToOriginal(List<string> degrams)
        {
            var ciphertext = string.Empty;

            degrams.ForEach(degram => ciphertext += $"{degram} ");

            return DegramsToOriginal(ciphertext.Trim());
        }

        private string DegramsToOriginal(string ciphertext)
        {
            var degrams = ciphertext.Split(' ');
            var original = degrams[0];
            var spaceCount = degrams.Length;

            for (int i = 1; i < degrams.Length; i++)
            {
                var lastDegr = degrams[i - 1];
                var curDegr = degrams[i];

                if (lastDegr[1].Equals('я') &&
                    lastDegr[0].Equals(curDegr[0]))
                {
                    original = original.Remove(original.Length - 1);
                    original += curDegr;
                }
                else if (curDegr[0].Equals('я') &&
                         lastDegr[1].Equals(curDegr[1]))
                {
                    original += curDegr[1];
                }
                else
                {
                    original += degrams[i];
                }
            }

            var lastIndex = original.Length - 1;
            var lastLatter = original[lastIndex];

            if (lastLatter.Equals('я') &&
                (spaceCount) % 2 == 0)
            {
                original = original.Remove(lastIndex);
            }

            return original;
        }

        private string RemoveSpaces(string text)
        {
            return text.Replace(" ", "");
        }

        private string SplitEqualsSymbols(string text)
        {
            var result = text[0].ToString();

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i].Equals(text[i - 1]) &&
                    i % 2 == 1)
                {
                    result += $"я{text[i]}";
                }
                else
                {
                    result += text[i];
                }
            }

            return result;
        }
    }
}
