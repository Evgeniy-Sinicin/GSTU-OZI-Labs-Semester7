using System;
using System.Windows.Forms;

namespace LAB_3_1
{
    public partial class Form1 : Form
    {
        private int _blockSize;
        private int _roundsCount;
        private int[] _keys = { 245689, 985214 };

        public Form1()
        {
            InitializeComponent();
            _blockSize = 64;
            _roundsCount = 16;
            encodeBox.Text = "Белый снег, серый лёд на растрескавшейся земле...";
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            encodeBox.Text = new string(Crypt(decodeBox.Text.ToCharArray(), false));
            decodeBox.Text = string.Empty;
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            decodeBox.Text = new string(Crypt(encodeBox.Text.ToCharArray(), true));
            encodeBox.Text = string.Empty;
        }

        private int H(int r)
        {
            return ((_keys[0] << r) ^ (_keys[1] >> r));
        }

        private int Vi(int x1, int r)
        {
            return x1 ^ H(r);
        }

        private int F(int x1, int vir)
        {
            return x1 + vir;
        }

        private int[] Rounder(int[] xn)
        {
            var old = new int[xn.Length];

            for (int i = 0; i < _blockSize; i++)
            {
                xn.CopyTo(old, 0);
                xn[0] = old[1] ^ F(old[0], Vi(old[0], i));

                for (int n = 1; n < xn.Length; n++)
                {
                    xn[n] = old[(n + 1) % (xn.Length)];
                }
            }

            return xn;
        }

        private int[] Derounder(int[] xn)
        {
            var old = new int[xn.Length];

            for (int i = (_blockSize - 1); i > (-1); i--)
            {
                xn.CopyTo(old, 0);
                xn[1] = old[0] ^ F(old[xn.Length - 1], Vi(old[xn.Length - 1], i));

                for (int n = 1; n < xn.Length; n++)
                {
                    xn[(n + 1) % (xn.Length)] = old[n];
                }
            }

            return xn;
        }

        private char[] Crypt(char[] msg, bool f)
        {
            var i = 0;
            var res = new char[msg.Length];

            if (msg.Length >= _roundsCount)
            {
                for (i = 0; i < (msg.Length - _roundsCount + 1); i += _roundsCount)
                {
                    var tmp = new int[_roundsCount];

                    for (int n = 0; n < _roundsCount; n++)
                    {
                        tmp[n] = msg[i + n];
                    }

                    if (f)
                    {
                        tmp = Rounder(tmp);
                    }
                    else
                    {
                        tmp = Derounder(tmp);
                    }

                    for (int n = 0; n < _roundsCount; n++)
                    {
                        res[i + n] = (char)tmp[n];
                    }
                }
            }

            for (int n = 0; n < (msg.Length % _roundsCount); n++)
            {
                res[i + n] = msg[i + n];
            }

            return res;
        }
    }
}