using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        
        public Client? Get(string email)
        {
            return _clientRepository.Get(email);
        }

        public Client? Get(int id)
        {
            return _clientRepository.Get(id);
        }

        public List<Client> GetAll()
        {
            List<Client> clients = _clientRepository.GetAll();
            return clients;
        }

        public void Add(Client client)
        {
            var clientList = _clientRepository.GetAll();
            var newId = clientList.Count != 0 ? clientList.Max(c => c.Id) + 1 : 1;
            client.Id = newId;

            clientList.Add(client);
            _clientRepository.Add(client);
        }
    }
}
