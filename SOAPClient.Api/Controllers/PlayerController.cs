using SOAPClient.Core.Serializer;
using SOAPClient.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace SOAPClient.Api.Controllers
{
    [RoutePrefix("api")]
    public class PlayerController : ApiController
    {
        [HttpGet, Route("player")]
        public IEnumerable<Player> GetPlayers(string country = null)
            => PlayerSerializer.GetPlayers(country);
    }
}
