using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LloydsRegister.API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace LloydsRegister.API.Entities
{
    public class CosmosDbContext : ICosmosDbContext
    {
        private const string EndpointUri = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseName = "LloydsRegister";
        private const string CollectionName = "LloydsRegister";
        private DocumentClient _client;

        public CosmosDbContext()
        {
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            CreateDatabaseAsync().Wait();
            CreateCollectionAsync().Wait();
        }

        private async Task CreateDatabaseAsync()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
        }

        private async Task CreateCollectionAsync()
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName });
        }

        public IQueryable<ManagingAgentDto> ManagingAgents()
        {
            var queryOptions = new FeedOptions { MaxItemCount = 1 };

            return _client.CreateDocumentQuery<ManagingAgent>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Select(s => new ManagingAgentDto()
                {
                    AgentCode = s.AgentCode,
                    AgentName = s.AgentName
                });
        }

        public async Task CreateDocumentAsync(ManagingAgentDto managingAgent)
        {
            var agentEntity = new ManagingAgent()
            {
                AgentCode = managingAgent.AgentCode,
                AgentName = managingAgent.AgentName
            };

            try
            {

                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, agentEntity.Id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), agentEntity);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task ReplaceDocumentAsync(ManagingAgentDto managingAgent)
        {
            var agentEntity = new ManagingAgent()
            {
                AgentCode = managingAgent.AgentCode,
                AgentName = managingAgent.AgentName
            };
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, agentEntity.Id), agentEntity);
        }

        public async Task DeleteDocumentAsync(ManagingAgentDto managingAgent)
        {
            var agentEntity = new ManagingAgent()
            {
                AgentCode = managingAgent.AgentCode,
                AgentName = managingAgent.AgentName
            };
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, agentEntity.Id));
        }
    }
}
