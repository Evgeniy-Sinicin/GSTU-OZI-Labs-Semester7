namespace LAB_2_1
{
    public class Degram
    {
        public char A { get; set; }
        public char Z { get; set; }

        public Degram(char a, char z)
        {
            A = a;
            Z = z;
        }

        public override string ToString()
        {
            return $"{A}{Z}";
        }
    }
}
