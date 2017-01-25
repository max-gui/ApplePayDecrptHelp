using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.IO;

namespace ApplePayDecryptHelp
{
    public class ApplePayInfo4ChinaRsa : ApplePayBase
    {
        RsaKeyParameters erchantPrivateKey { get; set; }
        public string wrappedKey { get; set; }

        public ApplePayInfo4ChinaRsa(CertKind fileOrString):base(fileOrString)
        {

        }

        protected override void SetKey(AsymmetricKeyParameter key)
        {
            erchantPrivateKey = key as RsaKeyParameters;
        }

        protected override void GetSymmetricKey()
        {
            var encryptedData = Convert.FromBase64String(wrappedKey);
            var cipher = WrapperUtilities.GetWrapper("RSA/ECB/OAEPWithSHA256AndMGF1Padding");
            cipher.Init(false, this.erchantPrivateKey);
            this.symmetricKey = cipher.Unwrap(encryptedData, 0, encryptedData.Length);

            kp = new KeyParameter(this.symmetricKey);
        }

        public override void PriorToGetStreamFromP12()
        {
            //throw new NotImplementedException();
        }

        public override void SuccessorForGetStreamFromP12()
        {
            //throw new NotImplementedException();
        }

        protected override void GetOthersFromP12(Stream fs, string passWord)
        {
            //throw new NotImplementedException();
        }
    }
}
