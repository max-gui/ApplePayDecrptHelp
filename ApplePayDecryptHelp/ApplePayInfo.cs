using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApplePayDecryptHelp
{
    public class ApplePayInfo4ECC : ApplePayBase
    {
        public byte[] merchantIdentifierField { get; set; }
        public ECPrivateKeyParameters merchantPrivateKey { get; set; }
        public ECPublicKeyParameters ephemeralPublicKey { get; set; }
        public byte[] sharedSecret { get; set; }

        public string base64PubKey { get; set; }

        protected override void GetOthersFromP12(Stream fs, string passWord)
        {
            GetMerchantIdentifierField(fs, certPassWord);
        }

        public override void PriorToGetStreamFromP12()
        {
            GetPubKey(base64PubKey);
        }

        public override void SuccessorForGetStreamFromP12()
        {
            GetShardSecret();
        }
        
        protected override void SetKey(AsymmetricKeyParameter key)
        {
            this.merchantPrivateKey = key as ECPrivateKeyParameters;
        }

        private void GetPubKey(string base64PubKeyStr)
        {
            var keyInBytes = Convert.FromBase64String(base64PubKeyStr);

            this.ephemeralPublicKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(keyInBytes);
        }
        
        private void GetMerchantIdentifierField(Stream fs, string passWord)
        {
            var rawTmp = new byte[fs.Length];
            fs.Read(rawTmp, 0, (Int32)fs.Length);

            //var rawTmp = File.ReadAllBytes(path);

            var x509cert = new X509Certificate2(rawTmp, passWord,
                    X509KeyStorageFlags.Exportable |
                    X509KeyStorageFlags.PersistKeySet);
            //new X509Certificate2(path, passWord,
            //    X509KeyStorageFlags.Exportable |
            //    X509KeyStorageFlags.PersistKeySet);
            rawTmp = null;

            rawTmp = x509cert.Extensions[7].RawData;

            using (var stream = new MemoryStream(64))
            using (var sr = new BinaryWriter(stream))
            {
                sr.Write(rawTmp, 2, 64);
                sr.Flush();
                rawTmp = null;

                //var streamTmp = stream.GetBuffer();

                var ascStr = Encoding.ASCII.GetString(stream.GetBuffer());

                rawTmp = new byte[ascStr.Length / 2];
                for (int i = 0; i < rawTmp.Length; i++)
                {
                    rawTmp[i] = Convert.ToByte(ascStr.Substring(i * 2, 2), 16);
                }

                this.merchantIdentifierField = rawTmp;
            }
        }
                
        private void GetShardSecret()
        {
            IBasicAgreement aKeyAgree = AgreementUtilities.GetBasicAgreement("ECDH");
            aKeyAgree.Init(this.merchantPrivateKey);
            BigInteger SharedSecret = aKeyAgree.CalculateAgreement(this.ephemeralPublicKey);
            byte[] tmpSharedSecret = SharedSecret.ToByteArray();

            if (tmpSharedSecret.Length > 32)
            {
                this.sharedSecret = new byte[tmpSharedSecret.Length - 1];
                Array.Copy(tmpSharedSecret, 1, this.sharedSecret, 0, tmpSharedSecret.Length - 1);
            }
            else {
                this.sharedSecret = tmpSharedSecret;
            }
        }

        protected override void GetSymmetricKey()
        {
            var infoToWrite = GetInfoReady(this.merchantIdentifierField);

            IDigest digest = new Sha256Digest();
            this.symmetricKey = new byte[digest.GetDigestSize()];
            digest.Update((byte)(1 >> 24));
            digest.Update((byte)(1 >> 16));
            digest.Update((byte)(1 >> 8));
            digest.Update((byte)1);
            digest.BlockUpdate(this.sharedSecret, 0, this.sharedSecret.Length);
            digest.BlockUpdate(infoToWrite, 0, infoToWrite.Length);
            digest.DoFinal(symmetricKey, 0);

            kp = new KeyParameter(symmetricKey);
        }

        private static byte[] GetInfoReady(byte[] mid)
        {
            var algorithmId = Encoding.ASCII.GetBytes(((char)0x0D + "id-aes256-GCM"));//.getBytes("ASCII");
            var partyUInfo = Encoding.ASCII.GetBytes("Apple");//.getBytes("ASCII");
            var partyVInfo = mid;// extractMerchantIdFromCertificateOid("1.2.840.113635.100.6.32");

            //byte[] otherInfo = new byte[algorithmId.Length + partyUInfo.Length + partyVInfo.Length];
            var stream = new MemoryStream(algorithmId.Length + partyUInfo.Length + partyVInfo.Length);
            using (var sr = new BinaryWriter(stream))
            {
                sr.Write(algorithmId);
                sr.Flush();
                sr.Write(partyUInfo);
                sr.Flush();
                sr.Write(partyVInfo);
                sr.Flush();
                stream.Position = 0;
                var o2 = stream.GetBuffer();
                return o2;
            }
        }
    }
}
