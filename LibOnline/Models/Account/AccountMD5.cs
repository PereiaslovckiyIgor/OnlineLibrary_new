using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace LibOnline.Models.Account
{
    public class AccountMD5
    {
        public string ConvertPasswosdToMD5(string pass) {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            //Guid guid = new Guid(hash);

            return new Guid(hash).ToString();
        }//GetPasswosdMD5

    }
}
