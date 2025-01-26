namespace ContatoInteligenteAPI.Models
{
    public class GitHubRepositoryView
    {
        public string UserAvatarLink { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }        
    }
}
