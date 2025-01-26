using ContatoInteligenteAPI.Models;
using Octokit;

namespace ContatoInteligenteAPI.Services.GitHub
{
    public interface IGitHubClientService
    {
        Task<(Result result, List<GitHubRepositoryView> data)> SearchAsync(SearchRepositoriesRequest searchRequest);
        Task<(Result result, string data)> GetUserProfileAsync(string userName);
    }
}
