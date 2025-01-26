using ContatoInteligenteAPI.Models;

namespace ContatoInteligenteAPI.Services.GitHub
{
    public interface IGitHubApplicationService
    {
        Task<(Result result, List<GitHubRepositoryView> data)> GetRepositoriesByUserAsync(string userName, GitHubRepositoryQuery query);
    }
}
