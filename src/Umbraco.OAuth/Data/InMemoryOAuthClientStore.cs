using System.Collections.Generic;
using System.Linq;
using Umbraco.OAuth.Extensions;
using Umbraco.OAuth.Interfaces;
using Umbraco.OAuth.Models;

namespace Umbraco.OAuth.Data
{
    public class InMemoryOAuthClientStore : IOAuthClientStore
    {
        private IEnumerable<OAuthClient> _clients; 

        public InMemoryOAuthClientStore(IEnumerable<OAuthClient> clients)
        {
            foreach (var client in clients)
            {
                client.Secret = client.Secret.GenerateHash();
            }

            this._clients = clients;
        }

        public OAuthClient FindClient(string clientId)
        {
            return this._clients.FirstOrDefault(x => x.ClientId == clientId);
        }
    }
}
