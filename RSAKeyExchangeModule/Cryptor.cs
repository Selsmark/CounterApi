using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RSAKeyExchangeModule
{
    public class Cryptor
    {
        private static readonly string PublicPemFilename = "public.pem";
        private static readonly string PrivatePemFilename = "private.pem";

        public static byte[] EncryptData(string data, string publicKey)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            publicKey = Encoding.UTF8.GetString(publicKeyBytes);

            byte[] dataEncrypted;

            using (RSACryptoServiceProvider rsa = new(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportFromPem(publicKey);

                dataEncrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
            }

            return dataEncrypted;
        }

        internal static string Encrypt(string text)
        {
            byte[] publicPemBytes = File.ReadAllBytes(PublicPemFilename);
            using X509Certificate2 publicX509 = new X509Certificate2(publicPemBytes);
            RSA? rsa = publicX509.GetRSAPublicKey();

            if (rsa == null)
            {
                throw new Exception("Unable to retrieve RSA public key.");
            }

            byte[] encrypted = rsa.Encrypt(System.Text.Encoding.Default.GetBytes(text), RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string base64Text)
        {
            using X509Certificate2 certificate = CombinePublicAndPrivateCerts();
            RSA? rsa = certificate.GetRSAPrivateKey();

            if (rsa == null)
            {
                throw new Exception("Unable to retrieve RSA private key.");
            }

            byte[] textBytes = Convert.FromBase64String(base64Text);
            byte[] decrypted = rsa.Decrypt(textBytes, RSAEncryptionPadding.Pkcs1);
            return System.Text.Encoding.Default.GetString(decrypted);
        }

        private static X509Certificate2 CombinePublicAndPrivateCerts()
        {
            byte[] publicPemBytes = File.ReadAllBytes(PublicPemFilename);
            using X509Certificate2 publicX509 = new X509Certificate2(publicPemBytes);

            string privateKeyText = File.ReadAllText(PrivatePemFilename);
            string[] privateKeyBlocks = privateKeyText.Split("-", StringSplitOptions.RemoveEmptyEntries);
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBlocks[1]);

            using RSA rsa = RSA.Create();
            if (privateKeyBlocks[0] == "BEGIN PRIVATE KEY")
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            }
            else if (privateKeyBlocks[0] == "BEGIN RSA PRIVATE KEY")
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            }

            X509Certificate2 keyPair = publicX509.CopyWithPrivateKey(rsa);
            return keyPair;
        }
    }
}
