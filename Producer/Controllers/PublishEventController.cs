using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.EventBus.Abstractions;
using EventBus.EventBus.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishEventController : ControllerBase, IPublishEventController
    {
        private readonly IEventBus _eventBus;

        public PublishEventController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        // GET: api/PublishEvent
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PublishEvent/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PublishEvent
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _eventBus.Publish(new IntegrationEvent(
                Guid.NewGuid(), DateTime.Now
                ));
        } 

        // PUT: api/PublishEvent/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/PublishEvent/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
