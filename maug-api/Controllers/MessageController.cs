using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace maug_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        
        const string queueName = "maugq";
        IQueueClient queueClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string serviceBusConnectionString;

        public MessageController(ILogger<MessageController> logger, IConfiguration configuration) 
        {
            _configuration = configuration;
            _logger = logger;
            serviceBusConnectionString = configuration["Azure:ServiceBus:ConnectionString"];
        }
        // GET: api/Default
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Message
        [HttpPost]
        public void Post([FromBody] JObject body)
        {
            queueClient = new QueueClient(serviceBusConnectionString, queueName);
            string payload = body.ToString();

            var sbMssage = new Message(Encoding.UTF8.GetBytes(payload));
            queueClient.SendAsync(sbMssage);
        }

       
    }
}
