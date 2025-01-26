using ContatoInteligenteAPI.Services.Authentication;
using ContatoInteligenteAPI.Services.GitHub;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace ContatoInteligenteAPI.Configurations
{
    public static class ServicesConfigurations
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGitHubClientService, GitHubClientService>()
                    .AddScoped<IGitHubApplicationService, GitHubApplicationService>();

            var gitHubSettings = configuration.GetSection("GitHub").Get<GitHubSettings>();

            if (gitHubSettings != null)
                services.AddSingleton(gitHubSettings);            

        }

        public static void AddBasicAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuth", policy =>
                    policy.RequireAuthenticatedUser());
            });
        }

        public static void AddSwager(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("BasicAuthentication", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Autenticação básica. Informe o usuário e senha",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BasicAuthentication"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}
