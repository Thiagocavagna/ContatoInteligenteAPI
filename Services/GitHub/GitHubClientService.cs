using ContatoInteligenteAPI.Common;
using ContatoInteligenteAPI.Common.Helpers;
using ContatoInteligenteAPI.Configurations;
using ContatoInteligenteAPI.Models;
using Octokit;

namespace ContatoInteligenteAPI.Services.GitHub
{
    public class GitHubClientService : ServiceBase, IGitHubClientService
    {
        private readonly GitHubClient _client;
        public GitHubClientService(GitHubSettings gitHubSettings)
        {
            var token = gitHubSettings.Token?.DecryptAES();

            _client = new GitHubClient(new ProductHeaderValue("ContatoInteligenteAPI"))
            {
                Credentials = new Credentials(token)
            };
        }

        public async Task<(Result result, string data)> GetUserProfileAsync(string userName)
        {
            var user = await _client.User.Get(userName);

            if (user == null)
                return Unsuccessful<string>("Falha ao obter os dados do usuário.");

            return Successful(user.AvatarUrl);
        }

        public async Task<(Result result, List<GitHubRepositoryView> data)> SearchAsync(SearchRepositoriesRequest searchRequest)
        {
            var searchResult = await _client.Search.SearchRepo(searchRequest);
            var repositories = searchResult?.Items;

            if (repositories == null)
                return Unsuccessful<List<GitHubRepositoryView>>("Falha ao obter os dados da pesquisa.");

            var avatarLinkResponse = await GetUserProfileAsync(searchRequest.User);

            if (!avatarLinkResponse.result.Success)
                return Unsuccessful<List<GitHubRepositoryView>>(avatarLinkResponse.result);

            var repositoriesView = repositories.Select(r => new GitHubRepositoryView
            {
                Name = r.Name,
                Description = r.Description,
                UpdatedAt = r.UpdatedAt,
                UserAvatarLink = avatarLinkResponse.data
            }).OrderBy(x => x.UpdatedAt).ToList();

            return Successful(repositoriesView);
        }
    }
}
