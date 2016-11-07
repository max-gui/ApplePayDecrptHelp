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
        public string Post([FromBody]PaymentData value)
        {
            var path = "private.p12";
            var pwd = "123456";
            //dataFromJson = "l6w4oBmvLF/f/6Gj7idhO5aIFlwZ5qZrqSLxR+mLqsjJqHLf2OUTObn+UOO/Iaupc+nc6Kuz1ZTQbBMG2w6/KC8F07lZTPnCOcHVxxBP03UQn6gNkNV8DLNNqJ9GoBDzbGy/dWgKBBPNEdgA59jKY+8H/XQzvNuZqiFDM4LTr+O3/FdqGy6PxfFHRwRNV15WoKtsfQ91xPf++MI4GoTZSdxpc63ewm/8l5Q81AqnZMH03PMRyu8POT92tl10tg8GRmQFCXMgBm7GM+6nz0tebOoLndspqHbe9xtAGmxzseuFCi4A2q8WaqzPgnQ917bvuUnxNHAkXoPTVIUCsXzxXA==";
            var applePayInfo = new ApplePayInfo4ChinaRsa { wrappedKey = value.header.wrappedKey, certPath = path, certPassWord = pwd }.Init();
            //var jsonStr = applePayInfo.Decrypt(dataFromJson);
            //dataFromJson = "juTLgE+0gQPg3RSe5DdJj1/w4amEvJOWSIka+nHNGFmFVd028omhkwMNNqG0exHgT39DAXnKqVNBh4ExGvIEgO7yi97JhDOmwq0nTyvPF/U393wgizrmoe8zX1FpUzI7e2co8PIYCJLkC6uTIuuumsE//503nDhvnz9frzmiMYVhdquf56couB028QBhJiQAupLM+NawVJ41i7e7WJIfyVhYEEn1Qw0TKZKy+Y65PkhAgdwlUGkKUI6r2IpHCc/l4EWvpn1tcVQvVeoG6qJxUdszgL6qrJBLtaT+/8teg9/jfn0iQwipEgYfTjalgHwXx3nop0dK2ZxzcOzdclfTY3uguM6HBNK6rK3hL2B/LnidAuWE0EWMp5/kqNunDJsSXsUM4+g6zg7ceVjlndZ0YLrQozxRkhPkgHHGtjFxn01PpeGSMMdoAVc8iOgpGKrjx/AAIp2jNY1fZZ1G4cYzW+gsJo5PrmPV7gGxA+M+HG5xdwMVfa9/cxS0qVU2eMw92OB7hdgaIW6cwONETgrd0Y2vF/oHiw7AbRiX/3GxkOJfJ9/5S0P6cxJF4JsvVHkrgit6gZPMCMXFQxDQsK8DlmQ9YxzDRIJCb9jxKE0=";

            var jsonStr = (applePayInfo as ApplePayInfo4ChinaRsa).Decrypt(value.data);
            var id = Guid.NewGuid().ToString();
            ResultTable.TryAdd(id, jsonStr);

            return id;
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
