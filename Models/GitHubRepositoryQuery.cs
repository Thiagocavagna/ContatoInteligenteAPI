using Octokit;

namespace ContatoInteligenteAPI.Models
{
    public class GitHubRepositoryQuery
    {        
        public Language? Language { get; set; }
        public SortDirection Order { get; set; }
        public RepoSearchSort? Sort { get; set; }
        public int PerPage { get; set; } = 5;
    }
}
