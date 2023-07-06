using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LAB_2_3
{
    public partial class Form1 : Form
    {
        private const int SIZE = 5;

        private string _defaultMessage = "guhoe " +
                                         "llxoe " +
                                         "bnce. " +
                                         "utp!w " +
                                         "nqwmp";

        private string _defaultStencil = "00101 " +
                                         "11010 " +
                                         "00000 " +
                                         "00010 " +
                                         "00000";

        private TextBox[,] _mBoxes;
        private TextBox[,] _sBoxes;
        private TextBox[,] _rBoxes;

        private TextBox[,] GetMessageBoxes()
        {
            return new TextBox[,] { { mBox0, mBox1, mBox2, mBox3, mBox4 },
                                    { mBox5, mBox6, mBox7, mBox8, mBox9 },
                                    { mBox10, mBox11, mBox12, mBox13, mBox14 },
                                    { mBox15, mBox16, mBox17, mBox18, mBox19 },
                                    { mBox20, mBox21, mBox22, mBox23, mBox24 }};
        }

        private TextBox[,] GetStencilBoxes()
        {
            return new TextBox[,] { { sBox0, sBox1, sBox2, sBox3, sBox4 },
                                    { sBox5, sBox6, sBox7, sBox8, sBox9 },
                                    { sBox10, sBox11, sBox12, sBox13, sBox14 },
                                    { sBox15, sBox16, sBox17, sBox18, sBox19 },
                                    { sBox20, sBox21, sBox22, sBox23, sBox24 }};
        }

        private TextBox[,] GetResultBoxes()
        {
            return new TextBox[,] { { rBox0, rBox1, rBox2, rBox3, rBox4 },
                                    { rBox5, rBox6, rBox7, rBox8, rBox9 },
                                    { rBox10, rBox11, rBox12, rBox13, rBox14 },
                                    { rBox15, rBox16, rBox17, rBox18, rBox19 },
                                    { rBox20, rBox21, rBox22, rBox23, rBox24 }};
        }

        private void HideInfo()
        {
            label5.Visible = false;
            hintLabel.Visible = false;
        }

        private void ShowInfo(string message, Color color)
        {
            label5.Visible = true;
            hintLabel.Visible = true;
            hintLabel.Text = message;
            hintLabel.ForeColor = color;
        }

        private void OnFormClickEvent(object sender, EventArgs e)
        {
            HideInfo();
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            const int SIZE = 5;
            string[] buf = new string[SIZE] {"guhoe",
                                             "llxoe",
                                             "bnce.",
                                             "utp!w",
                                             "nqwmp"};

            int[,] grid = new int[SIZE, SIZE]{{0, 0, 1, 0, 1},
                                              {1, 1, 0, 1, 0},
                                              {0, 0, 0, 0, 0},
                                              {0, 0, 0, 1, 0},
                                              {0, 0, 0, 0, 0}};

            // вывод зашифрованного сообщения
            for (int i = 0; i < SIZE; i++)
            {
                var res = buf[i];
            }

            // прямой обход решетки 0 degrs
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (grid[i, j] == 1)
                    {
                        var res = buf[i][j];
                    }
                }
            }

            // поворот решетки на 90 degrs по часовой стрелке
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (grid[SIZE - j - 1, i] == 1)
                    {
                        var res = buf[i][j];
                    }
                }
            }

            // поворот решетки на 180 degrs по часовой стрелке
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (grid[SIZE - i - 1, SIZE - j - 1] == 1)
                    {
                        var res = buf[i][j];
                    }
                }
            }

            // поворот решетки на 270 degrs по часовой стрелке
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (grid[j, SIZE - i - 1] == 1)
                    {
                        var res = buf[i][j];
                    }
                }
            }
        }

        private void RotateButton_Click(object sender, EventArgs e)
        {
            Rotate(messageBox, stencilBox, resultBox, _rBoxes);
        }

        private void Rotate(TextBox input, TextBox own, TextBox output, TextBox[,] rBoxes)
        {
            var buf = input.Text.Split(' ');
            var grid = GetGrid();

            own.Text = "";
            output.Text = "";

            ClearBoxes(_rBoxes);

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (grid[SIZE - j - 1, i] == 1)
                    {
                        output.Text += buf[i][j];
                        rBoxes[i, j].Text = buf[i][j].ToString();
                    }
                    own.Text += grid[SIZE - j - 1, i];

                    if (j == SIZE - 1)
                    {
                        own.Text += " ";
                    }
                }
            }

            FillBoxes(stencilBox, _sBoxes);
        }

        private int[,] GetGrid()
        {
            var grid = new int[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    grid[i, j] = int.Parse(_sBoxes[i, j].Text);
                }
            }

            return grid;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            messageBox.Text = "";
            stencilBox.Text = "";
            resultBox.Text = "";

            ClearBoxes(_mBoxes);
            ClearBoxes(_sBoxes);
            ClearBoxes(_rBoxes);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            messageBox.Text = _defaultMessage;
            stencilBox.Text = _defaultStencil;

            FillBoxes(messageBox, _mBoxes);
            FillBoxes(stencilBox, _sBoxes);

            Rotate(messageBox, stencilBox, resultBox, _rBoxes);
            Rotate(messageBox, stencilBox, resultBox, _rBoxes);
            Rotate(messageBox, stencilBox, resultBox, _rBoxes);
            Rotate(messageBox, stencilBox, resultBox, _rBoxes);
        }

        private void ClearBoxes(TextBox[,] boxes)
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    boxes[i, j].Text = "";
                }
            }
        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            FillBoxes(messageBox, _mBoxes);
        }

        private void FillBoxes(TextBox messageBox, TextBox[,] mBoxes)
        {
            var messages = messageBox.Text.Split(' ');

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    mBoxes[i, j].Text = messages[i][j].ToString();
                }
            }
        }

        private void StencilBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ResultBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            _mBoxes = GetMessageBoxes();
            _sBoxes = GetStencilBoxes();
            _rBoxes = GetResultBoxes();

            ResetButton_Click(null, null);
        }
    }
}
