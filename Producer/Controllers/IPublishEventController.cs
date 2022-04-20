using Microsoft.AspNetCore.Mvc;

namespace Producer.Controllers;

public interface IPublishEventController
{
    IEnumerable<string> Get();
    string Get(int id);
    void Post([FromBody] string value);
    void Put(int id, [FromBody] string value);
    void Delete(int id);
}