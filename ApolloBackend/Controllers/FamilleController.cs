using ApolloBackend.Entities;
using ApolloBackend.Functions;
using ApolloBackend.Models;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilleController : ControllerBase
    {
        private readonly FamilleFunctions _familleFunctions;
        public FamilleController(FamilleFunctions famille)
        {
            _familleFunctions = famille;
        }
        [HttpGet]
        public async Task<ActionResult<List<ListeFamille>>> GetFamilles()
        {
            return await _familleFunctions.GetFamilles();
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _familleFunctions.GetNbFamilles();
            return Ok(count);
        }
    }
}
