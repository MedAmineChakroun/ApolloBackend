using ApolloBackend.Functions;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Objets100cLib;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SynchronisationSageController : Controller
    {
        private readonly ISynchronisationSage _synchronisationSage;

        public SynchronisationSageController(ISynchronisationSage synchronisationSage)
        {
            _synchronisationSage = synchronisationSage;
        }
        

        [HttpPost("sync-commande")]
        public async Task<List<DocumentVente>> SyncCommande()
        {
            return await _synchronisationSage.SynchroniseCommandes();
        }

        [HttpPost("sync-Article")]
        public async Task<bool> SyncArticle(string CodeArt)
        {
            return await _synchronisationSage.SynchroniseArticles(CodeArt);
        }


        [HttpPost("sync-Client")]
        public async Task<bool> SyncClient(string CodeClient)
        {
            return await _synchronisationSage.SynchroniseClients(CodeClient);
        }
        [HttpPost("Delete-Client")]
        public async Task<bool> DeleteClient(string CodeClient)
        {
            return await _synchronisationSage.DeleteClient(CodeClient);
        }
        [HttpPost("Delete-Article")]
        public async Task<bool> DeleteArticle(string CodeArticle)
        {
            return await _synchronisationSage.DeleteArticle(CodeArticle);
        }
        [HttpPost("Delete-Commande")]
        public async Task<bool> DeleteCommande(string NumCommande)
        {
            return await _synchronisationSage.DeleteCommande(NumCommande);
        }
    }
}
