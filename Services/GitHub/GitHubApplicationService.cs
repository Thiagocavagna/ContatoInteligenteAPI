using ContatoInteligenteAPI.Common;
using ContatoInteligenteAPI.Models;
using Octokit;

namespace ContatoInteligenteAPI.Services.GitHub
{
    public class GitHubApplicationService : ServiceBase, IGitHubApplicationService
    {
        private readonly IGitHubClientService _gitHubClientService;
        public GitHubApplicationService(IGitHubClientService gitHubClientService)
        {
            _gitHubClientService = gitHubClientService;
        }

        public async Task<(Result result, List<GitHubRepositoryView> data)> GetRepositoriesByUserAsync(string userName, GitHubRepositoryQuery query)
        {
            var searchRequest = new SearchRepositoriesRequest
            {
                Language = query.Language,
                Order = query.Order,
                User = userName,
                SortField = query.Sort,
                PerPage = query.PerPage
            };

            var repositoriesResponse = await _gitHubClientService.SearchAsync(searchRequest);

            if (!repositoriesResponse.result.Success)
                return Unsuccessful<List<GitHubRepositoryView>>(repositoriesResponse.result);

            return Successful(repositoriesResponse.data);
        }
    }
}
