using System;
using System.Windows.Forms;

namespace LAB_1_1
{
    public partial class Form1 : Form
    {
        private string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        public Form1()
        {
            InitializeComponent();
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            var original = encodeBox.Text;
            var ciphertext = Convert(original);

            decodeBox.Text = ciphertext;
            encodeBox.Text = string.Empty;
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            var ciphertext = decodeBox.Text;
            var original = Convert(ciphertext);

            encodeBox.Text = original;
            decodeBox.Text = string.Empty;
        }

        private string Convert(string original)
        {
            var ciphertext = string.Empty;
            original = original.ToLower();

            for (int i = 0; i < original.Length; i++)
            {
                var index = _alphabet.IndexOf(original[i]);
                ciphertext += (index >= 0) ? _alphabet[_alphabet.Length - index - 1].ToString() : " ";
            }

            return ciphertext;
        }
    }
}
