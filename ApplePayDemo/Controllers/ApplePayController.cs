using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplePayDemo.Controllers
{
    [Route("api/[controller]")]
    public class ApplePayController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]PaymentData value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class PaymentData
    {
        public string data { get; set; }
        public Header head { get; set; }
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
