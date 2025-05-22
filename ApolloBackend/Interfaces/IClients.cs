using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface IClients
    {
        public Task<List<Client>> GetClientsAsync();
        public Task<Client> GetClientByCodeAsync(string id);

        public Task<bool> UpdateFlag(int id, int newFlag);
    }
}
