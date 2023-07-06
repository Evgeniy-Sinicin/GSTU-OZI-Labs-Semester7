using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace LAB_2_2
{
    public partial class Form1 : Form
    {
        private int _matrixSize;
        private string _alphabet = " абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        private Random _random = new Random();
        private Matrix<double> _matrix;
        private Matrix<double> _x;
        private Matrix<double> _c;
        private Matrix<double> _y;

        public Form1()
        {
            InitializeComponent();
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            var original = encodeBox.Text;
            var length = original.Length;
            
            _x = DenseMatrix.OfArray(new double[length, 1]);

            for (int i = 0; i < length; i++)
            {
                _x[i, 0] = _alphabet.IndexOf(original[i]);
            }

            if (string.IsNullOrEmpty(elementsBox.Text))
            {
                _c = GenerateC(length, length);
                elementsBox.Text = ToString(_c);
                ShowInfo("Матрица C сгенерирована успешно!", Color.DarkGreen);
            }
            else
            {
                // Получаем матрицу
                var size = (int)Math.Sqrt(elementsBox.Text.Split(' ').Length);
                _c = ToMatrix(elementsBox.Text, size, size);

                if (_c == null)
                {
                    return;
                }
            }

            // Проверяем размерность
            if (!IsCorrectMatrixLength(_c, length, length))
            {
                // Выкидываем ошибку
                ShowInfo("Ошибка! Матрица C некорректной размерности.", Color.DarkRed);

                return;
            }

            sizeBox.Text = $"{_c.RowCount} x {_c.ColumnCount}";

            // Шифруем и получаем матрицу Y = C * X
            _y = _c * _x;

            decodeBox.Text = ToString(_y);

            ShowInfo($"Сообщение '{encodeBox.Text}' зашифровано успешно!", Color.DarkGreen);
            encodeBox.Text = "";
        }

        private Matrix<double> GenerateC(int rows, int cols)
        {
            var c = DenseMatrix.OfArray(new double[rows, cols]);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    c[i, j] = _random.Next(0, 10);
                }
            }

            return c;
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            // Читаем матрицу C
            var size = (int)Math.Sqrt(elementsBox.Text.Split(' ').Length);
            _c = ToMatrix(elementsBox.Text, size, size);

            // Читаем матрицу Y
            _y = ToMatrix(decodeBox.Text, size, 1);

            // Находим индексы букв
            _x = _c.Inverse() * _y;

            // Выводим сообщение
            for (int i = 0; i < _x.RowCount; i++)
            {
                encodeBox.Text += _alphabet[(int)Math.Round(_x[i, 0])];
            }

            ShowInfo($"Закодированное сообщение '{decodeBox.Text}' расшифровано успешно!", Color.DarkGreen);
            decodeBox.Text = "";
        }

        private void OnFormClickEvent(object sender, EventArgs e)
        {
            HideInfo();
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

        private bool IsCorrectMatrixLength(Matrix<double> mtx, int rows, int cols)
        {
            if (mtx.RowCount == rows &&
                mtx.ColumnCount == cols)
            {
                return true;
            }

            return false;
        }

        private Matrix<double> ToMatrix(string line, int rows, int cols)
        {
            var elementsStrArr = line.Split(' ');
            var length = elementsStrArr.Length;
            var elements = new double[length];
            var mtx = DenseMatrix.OfArray(new double[rows, cols]);

            for (int i = 0; i < length; i++)
            {
                if (!double.TryParse(elementsStrArr[i], out elements[i]))
                {
                    ShowInfo("Ошибка! Некорректные значения матрицы C.", Color.DarkRed);

                    return null;
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    mtx[i, j] = elements[i * cols + j];
                }
            }

            return mtx;
        }

        private string ToString(Matrix<double> mtx)
        {
            var line = string.Empty;
            var iter = 0;

            for (int i = 0; i < mtx.RowCount; i++)
            {
                for (int j = 0; j < mtx.ColumnCount; j++, iter++)
                {
                    line += mtx[i, j];

                    if (iter != mtx.RowCount * mtx.ColumnCount - 1)
                    {
                        line += " ";
                    }
                }
            }

            return line;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            elementsBox.Text = "";
            sizeBox.Text = "";
        }
    }
}
