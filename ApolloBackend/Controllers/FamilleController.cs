﻿using ApolloBackend.Entities;
using ApolloBackend.Functions;
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
    }
}
