using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplePayDecryptHelp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ApplePayDecryptHelp.Tests
{
    [TestClass()]
    public class ApplePayInfoTests
    {
        private string dataFromJson =
                "IX+6/ypLOaBt9zPqXDvtxqbdYfoKHoJuNbIUV+NiPvLUV3do8sr8CDHNGDd5kXbSisYT/9Wcj55sX4JyRf/O1DM4TmvmuhUjCgDOjxuPbRILTY3J3aHoU4FGfRnmEp6KPVVGeYsb6ZdFk8kNeGH4nBKItBMgIrVwBgkkvTn3dt3hcDFr+s+hC6j/o4FajeKeQb/20141XVo8EG5LF1bZWcXTccOMpKpPDaCsEWxnhslcbJig/ANNCrQHFFi348/kjB5AwrMQW2Npk4krARuysVinTXtma0+D6XurbvFL+8lExxAc4f2sRaNUn8sIMbipKh56TYtQAo4xfSQxqwcdxrV7TiJdXAydTm8e6rnB491vRiHuTwSNLW64QooNEOtBQLPbktidWsXyGnm7QFAjCE2y+BhVHrzojBHgWxWNqxqkDqjWXcmnlLg=";
        private string dataFromJson4China =
                "juTLgE+0gQPg3RSe5DdJj1/w4amEvJOWSIka+nHNGFmFVd028omhkwMNNqG0exHgT39DAXnKqVNBh4ExGvIEgO7yi97JhDOmwq0nTyvPF/U393wgizrmoe8zX1FpUzI7e2co8PIYCJLkC6uTIuuumsE//503nDhvnz9frzmiMYVhdquf56couB028QBhJiQAupLM+NawVJ41i7e7WJIfyVhYEEn1Qw0TKZKy+Y65PkhAgdwlUGkKUI6r2IpHCc/l4EWvpn1tcVQvVeoG6qJxUdszgL6qrJBLtaT+/8teg9/jfn0iQwipEgYfTjalgHwXx3nop0dK2ZxzcOzdclfTY3uguM6HBNK6rK3hL2B/LnidAuWE0EWMp5/kqNunDJsSXsUM4+g6zg7ceVjlndZ0YLrQozxRkhPkgHHGtjFxn01PpeGSMMdoAVc8iOgpGKrjx/AAIp2jNY1fZZ1G4cYzW+gsJo5PrmPV7gGxA+M+HG5xdwMVfa9/cxS0qVU2eMw92OB7hdgaIW6cwONETgrd0Y2vF/oHiw7AbRiX/3GxkOJfJ9/5S0P6cxJF4JsvVHkrgit6gZPMCMXFQxDQsK8DlmQ9YxzDRIJCb9jxKE0=";

        private string b64PubKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEW23BB1xHIc5SwZ3d36lr8olhbbNKlXjrWQQxYFU5LJ9mi/YTxAPfWZBKw7B41HWzMzkOmKk2MQGGVyetwLUJbQ==";
        private string wrappedKey = "l6w4oBmvLF/f/6Gj7idhO5aIFlwZ5qZrqSLxR+mLqsjJqHLf2OUTObn+UOO/Iaupc+nc6Kuz1ZTQbBMG2w6/KC8F07lZTPnCOcHVxxBP03UQn6gNkNV8DLNNqJ9GoBDzbGy/dWgKBBPNEdgA59jKY+8H/XQzvNuZqiFDM4LTr+O3/FdqGy6PxfFHRwRNV15WoKtsfQ91xPf++MI4GoTZSdxpc63ewm/8l5Q81AqnZMH03PMRyu8POT92tl10tg8GRmQFCXMgBm7GM+6nz0tebOoLndspqHbe9xtAGmxzseuFCi4A2q8WaqzPgnQ917bvuUnxNHAkXoPTVIUCsXzxXA==";
        private string path = "API.p12";// @"C:\Users\jimmy\Desktop\API.p12";
        private string pwd = "";

        private string merchantIdentifierField = "67-E6-B8-51-AA-6C-20-8B-31-03-05-09-B2-19-03-79-95-93-D4-62-B2-46-7B-10-F1-E1-BD-41-E7-95-C3-AF";
        private string sharedSecret = "5F-48-03-B6-1F-25-25-35-E8-49-26-F4-71-CF-6C-D3-10-78-68-2D-6B-46-A7-AD-17-81-29-AF-61-E0-53-FE";
        private string symmetricKey = "74-43-16-39-61-58-E2-A3-F2-6B-E6-79-CC-2F-35-DC-75-6E-57-9D-32-3C-C8-A7-9A-CB-C8-D0-08-2D-B6-F0";
        private string iv = "00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00";

        [TestMethod()]
        public void InitTest()
        {
            var encryptedData = Convert.FromBase64String(dataFromJson);

            var applePayInfo = new ApplePayInfo4ECC(CertKind.file) { base64PubKey = b64PubKey, certPath = path, certPassWord = pwd }.Init() as ApplePayInfo4ECC;

            Assert.IsInstanceOfType(applePayInfo, typeof(ApplePayInfo4ECC));
            Assert.AreEqual(merchantIdentifierField, BitConverter.ToString(applePayInfo.merchantIdentifierField));
            Assert.AreEqual(sharedSecret, BitConverter.ToString(applePayInfo.sharedSecret));
            Assert.AreEqual(symmetricKey, BitConverter.ToString(applePayInfo.symmetricKey));
            Assert.AreEqual(iv, BitConverter.ToString(applePayInfo.Iv));
        }

        [TestMethod()]
        public void DecryptedByAES256GCMTest()
        {
            var encryptedData = Convert.FromBase64String(dataFromJson);

            var applePayInfo = new ApplePayInfo4ECC(CertKind.file) { base64PubKey = b64PubKey, certPath = path, certPassWord = pwd }.Init();

            var jsonStr = applePayInfo.Decrypt(dataFromJson);
            Assert.IsFalse(string.IsNullOrEmpty(jsonStr));
        }

        [TestMethod]
        public void DecryptedByAES256GCM4ChinaTest()
        {
            var encryptedData = Convert.FromBase64String(dataFromJson);
            path = "private.p12";
            pwd = "123456";

            var applePayInfo = new ApplePayInfo4ChinaRsa(CertKind.file) { wrappedKey = wrappedKey, certPath = path, certPassWord = pwd }.Init();
            //var jsonStr = applePayInfo.Decrypt(dataFromJson);
            //dataFromJson = "juTLgE+0gQPg3RSe5DdJj1/w4amEvJOWSIka+nHNGFmFVd028omhkwMNNqG0exHgT39DAXnKqVNBh4ExGvIEgO7yi97JhDOmwq0nTyvPF/U393wgizrmoe8zX1FpUzI7e2co8PIYCJLkC6uTIuuumsE//503nDhvnz9frzmiMYVhdquf56couB028QBhJiQAupLM+NawVJ41i7e7WJIfyVhYEEn1Qw0TKZKy+Y65PkhAgdwlUGkKUI6r2IpHCc/l4EWvpn1tcVQvVeoG6qJxUdszgL6qrJBLtaT+/8teg9/jfn0iQwipEgYfTjalgHwXx3nop0dK2ZxzcOzdclfTY3uguM6HBNK6rK3hL2B/LnidAuWE0EWMp5/kqNunDJsSXsUM4+g6zg7ceVjlndZ0YLrQozxRkhPkgHHGtjFxn01PpeGSMMdoAVc8iOgpGKrjx/AAIp2jNY1fZZ1G4cYzW+gsJo5PrmPV7gGxA+M+HG5xdwMVfa9/cxS0qVU2eMw92OB7hdgaIW6cwONETgrd0Y2vF/oHiw7AbRiX/3GxkOJfJ9/5S0P6cxJF4JsvVHkrgit6gZPMCMXFQxDQsK8DlmQ9YxzDRIJCb9jxKE0=";

            var jsonStr = applePayInfo.Decrypt(dataFromJson4China);
            Assert.IsFalse(string.IsNullOrEmpty(jsonStr));

            path = @"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
sTDTowIBAQ==";
            //dataFromJson = "l6w4oBmvLF/f/6Gj7idhO5aIFlwZ5qZrqSLxR+mLqsjJqHLf2OUTObn+UOO/Iaupc+nc6Kuz1ZTQbBMG2w6/KC8F07lZTPnCOcHVxxBP03UQn6gNkNV8DLNNqJ9GoBDzbGy/dWgKBBPNEdgA59jKY+8H/XQzvNuZqiFDM4LTr+O3/FdqGy6PxfFHRwRNV15WoKtsfQ91xPf++MI4GoTZSdxpc63ewm/8l5Q81AqnZMH03PMRyu8POT92tl10tg8GRmQFCXMgBm7GM+6nz0tebOoLndspqHbe9xtAGmxzseuFCi4A2q8WaqzPgnQ917bvuUnxNHAkXoPTVIUCsXzxXA==";
            applePayInfo = new ApplePayInfo4ChinaRsa(CertKind.string64) { wrappedKey = wrappedKey, base64Cert = path, certPassWord = pwd }.Init();
            //var jsonStr = applePayInfo.Decrypt(dataFromJson);
            //dataFromJson = "juTLgE+0gQPg3RSe5DdJj1/w4amEvJOWSIka+nHNGFmFVd028omhkwMNNqG0exHgT39DAXnKqVNBh4ExGvIEgO7yi97JhDOmwq0nTyvPF/U393wgizrmoe8zX1FpUzI7e2co8PIYCJLkC6uTIuuumsE//503nDhvnz9frzmiMYVhdquf56couB028QBhJiQAupLM+NawVJ41i7e7WJIfyVhYEEn1Qw0TKZKy+Y65PkhAgdwlUGkKUI6r2IpHCc/l4EWvpn1tcVQvVeoG6qJxUdszgL6qrJBLtaT+/8teg9/jfn0iQwipEgYfTjalgHwXx3nop0dK2ZxzcOzdclfTY3uguM6HBNK6rK3hL2B/LnidAuWE0EWMp5/kqNunDJsSXsUM4+g6zg7ceVjlndZ0YLrQozxRkhPkgHHGtjFxn01PpeGSMMdoAVc8iOgpGKrjx/AAIp2jNY1fZZ1G4cYzW+gsJo5PrmPV7gGxA+M+HG5xdwMVfa9/cxS0qVU2eMw92OB7hdgaIW6cwONETgrd0Y2vF/oHiw7AbRiX/3GxkOJfJ9/5S0P6cxJF4JsvVHkrgit6gZPMCMXFQxDQsK8DlmQ9YxzDRIJCb9jxKE0=";

            jsonStr = applePayInfo.Decrypt(dataFromJson4China);
            Assert.IsFalse(string.IsNullOrEmpty(jsonStr));
        }
    }
}
