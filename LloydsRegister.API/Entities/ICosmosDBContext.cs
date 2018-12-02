using System.Linq;
using System.Threading.Tasks;
using LloydsRegister.API.Models;

namespace LloydsRegister.API.Entities
{
    public interface ICosmosDbContext
    {
        IQueryable<ManagingAgentDto> ManagingAgents();
        Task CreateDocumentAsync(ManagingAgentDto managingAgent);
        Task ReplaceDocumentAsync(ManagingAgentDto managingAgent);
        Task DeleteDocumentAsync(ManagingAgentDto managingAgent);
    }
}
