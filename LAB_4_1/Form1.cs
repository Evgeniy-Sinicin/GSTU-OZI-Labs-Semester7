using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace LAB_4_1
{
    public partial class Form1 : Form
    {
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        private UnicodeEncoding _byteConverter;
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public Form1()
        {
            InitializeComponent();
            
            _privateKey = RSA.ExportParameters(true);
            _publicKey = RSA.ExportParameters(false);

            _byteConverter = new UnicodeEncoding();

            encodeBox.Text = "Белый снег, серый лёд на растрескавшейся земле...";
        }

        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }

        private void EncodeButton_Click(object sender, EventArgs e)
        {
            try
            {
                var toEncrypt = encodeBox.Text;
                var encBytes = RSAEncrypt(_byteConverter.GetBytes(toEncrypt), _publicKey, false);

                decodeBox.Text = _byteConverter.GetString(encBytes);
                decodeBytesBox.Text = string.Join(" ", encBytes);

                encodeBox.Text = "";
                encodeBytesBox.Text = "";
            }
            catch (Exception) { }
        }

        private void DecodeButton_Click(object sender, EventArgs e)
        {
            try
            {
                var decBytes_2 = decodeBytesBox.Text.Split(' ').Select(x => byte.Parse(x)).ToArray();
                var decrypt_2 = RSADecrypt(decBytes_2, _privateKey, false);

                encodeBox.Text = _byteConverter.GetString(decrypt_2);
                encodeBytesBox.Text = string.Join(" ", decBytes_2);
            }
            catch (Exception) { }
        }
    }
}
