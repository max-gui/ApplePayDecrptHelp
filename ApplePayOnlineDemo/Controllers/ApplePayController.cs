using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplePayDecryptHelp;
using System.Collections.Concurrent;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplePayDemo.Controllers
{
    [Route("api/[controller]")]
    public class ApplePayController : Controller
    {
        public static ConcurrentDictionary<string, string> ResultTable { get; set; } = new ConcurrentDictionary<string, string>();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var ret = ResultTable.Select(e => e.Value);
            ResultTable.Clear();
            return ret;
        }

        // GET api/values/5
        [HttpGet("{guid}")]
        public string Get(string guid)
        {
            var value = string.Empty;
            ResultTable.TryRemove(guid, out value);
            return value;
        }

        // POST api/values
        [HttpPost]
        public object Post([FromBody]PaymentData value)
        {
            //var path = "private.p12";
            var pwd = "123456";
            var cert = @"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
AgEB";
            //dataFromJson = "l6w4oBmvLF/f/6Gj7idhO5aIFlwZ5qZrqSLxR+mLqsjJqHLf2OUTObn+UOO/Iaupc+nc6Kuz1ZTQbBMG2w6/KC8F07lZTPnCOcHVxxBP03UQn6gNkNV8DLNNqJ9GoBDzbGy/dWgKBBPNEdgA59jKY+8H/XQzvNuZqiFDM4LTr+O3/FdqGy6PxfFHRwRNV15WoKtsfQ91xPf++MI4GoTZSdxpc63ewm/8l5Q81AqnZMH03PMRyu8POT92tl10tg8GRmQFCXMgBm7GM+6nz0tebOoLndspqHbe9xtAGmxzseuFCi4A2q8WaqzPgnQ917bvuUnxNHAkXoPTVIUCsXzxXA==";
            var applePayInfo = new ApplePayInfo4ChinaRsa(CertKind.string64) { wrappedKey = value.header.wrappedKey, base64Cert = cert, certPassWord = pwd }.Init();
            //var jsonStr = applePayInfo.Decrypt(dataFromJson);
            //dataFromJson = "juTLgE+0gQPg3RSe5DdJj1/w4amEvJOWSIka+nHNGFmFVd028omhkwMNNqG0exHgT39DAXnKqVNBh4ExGvIEgO7yi97JhDOmwq0nTyvPF/U393wgizrmoe8zX1FpUzI7e2co8PIYCJLkC6uTIuuumsE//503nDhvnz9frzmiMYVhdquf56couB028QBhJiQAupLM+NawVJ41i7e7WJIfyVhYEEn1Qw0TKZKy+Y65PkhAgdwlUGkKUI6r2IpHCc/l4EWvpn1tcVQvVeoG6qJxUdszgL6qrJBLtaT+/8teg9/jfn0iQwipEgYfTjalgHwXx3nop0dK2ZxzcOzdclfTY3uguM6HBNK6rK3hL2B/LnidAuWE0EWMp5/kqNunDJsSXsUM4+g6zg7ceVjlndZ0YLrQozxRkhPkgHHGtjFxn01PpeGSMMdoAVc8iOgpGKrjx/AAIp2jNY1fZZ1G4cYzW+gsJo5PrmPV7gGxA+M+HG5xdwMVfa9/cxS0qVU2eMw92OB7hdgaIW6cwONETgrd0Y2vF/oHiw7AbRiX/3GxkOJfJ9/5S0P6cxJF4JsvVHkrgit6gZPMCMXFQxDQsK8DlmQ9YxzDRIJCb9jxKE0=";

            var jsonStr = (applePayInfo as ApplePayInfo4ChinaRsa).Decrypt(value.data);
            var id = Guid.NewGuid().ToString();
            ResultTable.TryAdd(id, jsonStr);

            return new { id = id, json = jsonStr};
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
    

    public class PaymentData
    {
        public string data { get; set; }
        public Header header { get; set; }
        public string signature { get; set; }
        public string version { get; set; }

        public enum Version
        {
            EC_v1,
            RSA_v1
        }

        public Version GetVersion()
        {
            var ret = default(Version);
            Enum.TryParse(version, true, out ret);

            return ret;
        }

        public class Header
        {
            public string applicationData { get; set; }
            public string ephemeralPublicKey { get; set; }
            public string wrappedKey { get; set; }
            public string publicKeyHash { get; set; }
            public string transactionId { get; set; }
        }
    }
}
