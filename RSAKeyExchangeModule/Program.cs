namespace RSAKeyExchangeModule
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string plainText = "ClientPublicKey1234";
            Console.WriteLine($"Encrypting: {plainText}");

            string encrypted = Cryptor.Encrypt(plainText);
            Console.WriteLine("[Encrypted]");
            Console.WriteLine(encrypted);
            Console.WriteLine("-------------------");

            string decrypted = Cryptor.Decrypt(encrypted);
            Console.WriteLine("[Decrypted]");
            Console.WriteLine(decrypted);
            Console.WriteLine("-------------------");

            Console.ReadKey();
        }
    }
}
