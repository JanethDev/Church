using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Church.Business
{
    public class PasswordSecurity
    {

        //Clave principal para garantizar la seguridad
        static readonly string PasswordHash = "GruP0L@NS3CretK3y";

        //Clave adicional para evitar que pueda ser descifrada con algoritmos existentes
        static readonly string SaltKey = "S@LT&KEYL@N";


        //Llave simétrica
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";


        /// <summary>
        /// Este método recibe string en texto plano y realiza la encriptación del mismo 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns>Regresa el string encriptado</returns>

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Este método recibe string en encriptado y realiza la desencriptación  del mismo
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns>Regresa el string en texto plano</returns>
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }


        public static bool ValidatePassword(string password, string correctHash)
        {
            bool valid = false;
            string encryptedPassword = Encrypt(password);            
            if (encryptedPassword.Equals(correctHash))
            {
                valid = true;
            }
            return valid;
        }
    }
}
