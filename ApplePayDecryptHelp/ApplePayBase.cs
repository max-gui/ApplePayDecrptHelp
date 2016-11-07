using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplePayDecryptHelp
{
    public abstract class ApplePayBase
    {

        public byte[] symmetricKey { get; set; }
        //RsaKeyParameters erchantPrivateKey { get; set; }
        //public string wrappedKey { get; set; }
        public KeyParameter kp { get; set; }
        public string certPath { get; set; }
        public string certPassWord { get; set; }

        public byte[] Iv { get; set; }

        protected ApplePayBase()
        {
            Iv = new byte[16];
        }

        public ApplePayBase Init()
        {
            GetBasicKey();
            GetSymmetricKey();

            return this;
        }

        public void GetBasicKey()
        {
            PriorToGetStreamFromP12();
            using (StreamReader reader = new StreamReader(certPath))
            {
                var fs = reader.BaseStream;
                fs.Position = 0;
                GetOthersFromP12(fs, certPassWord);
                fs.Position = 0;
                var keys = GetPrivateKeyFromP12(fs, certPassWord);
                SetKey(keys.FirstOrDefault());
            }
            SuccessorForGetStreamFromP12();
        }

        public abstract void PriorToGetStreamFromP12();

        public abstract void SuccessorForGetStreamFromP12();

        protected abstract void GetOthersFromP12(Stream fs, string passWord);
        
        protected abstract void SetKey(AsymmetricKeyParameter key);

        protected static IEnumerable<AsymmetricKeyParameter> GetPrivateKeyFromP12(Stream fs, string passWord)
        {

            var store = new Pkcs12Store(fs, passWord.ToCharArray());
            return store.Aliases.Cast<string>().AsParallel().Where(e => store.IsKeyEntry(e)).Select(e =>
                store.GetKey(e)).Where(e => e.Key.IsPrivate).Select(e => e.Key);            
        }

        protected abstract void GetSymmetricKey();

        public string Decrypt(string encryptedStr)
        {
            return DecryptedByAES256GCM(encryptedStr);
        }

        protected string DecryptedByAES256GCM(string encryptedStr)
        {
            var encryptedData = Convert.FromBase64String(encryptedStr);
            var cipher = WrapperUtilities.GetWrapper("AES/GCM/NoPadding");
            cipher.Init(false, new ParametersWithIV(kp, this.Iv));

            var deCryptedData = cipher.Unwrap(encryptedData, 0, encryptedData.Length);
            return Encoding.ASCII.GetString(deCryptedData);
        }
    }
}
