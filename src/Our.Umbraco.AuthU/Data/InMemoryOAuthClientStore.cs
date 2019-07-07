using System.Collections.Generic;
using System.Linq;
using Our.Umbraco.AuthU.Extensions;
using Our.Umbraco.AuthU.Interfaces;
using Our.Umbraco.AuthU.Models;

namespace Our.Umbraco.AuthU.Data
{
    public class InMemoryOAuthClientStore : IOAuthClientStore
    {
        private IEnumerable<OAuthClient> _clients; 

        public InMemoryOAuthClientStore(IEnumerable<OAuthClient> clients)
        {
            foreach (var client in clients)
            {
                client.Secret = client.Secret.GenerateOAuthHash();
            }

            this._clients = clients;
        }

        public OAuthClient FindClient(string clientId)
        {
            return this._clients.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
