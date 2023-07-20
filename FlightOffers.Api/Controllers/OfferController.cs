using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace FlightOffers.Api.Controllers;

[Produces("application/json")]
[Route("/api/public/[controller]/[action]")]
public class OfferController : Controller
{
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Fetch()
    {
        if (!ModelState.IsValid)
            return BadRequest($"Invalid data supplied. Required: {string.Join(',', ModelState.Keys)}");
        return Ok();
    }
}