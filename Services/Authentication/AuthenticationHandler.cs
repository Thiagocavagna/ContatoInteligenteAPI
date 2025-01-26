using ContatoInteligenteAPI.Common.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Encodings.Web;

namespace ContatoInteligenteAPI.Services.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly string _validUsername;
        private readonly string _validPassword;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _validUsername = configuration["Authentication:User"]!;
            _validPassword = configuration["Authentication:Password"]!;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Cabeçalho de autenticação faltando.");

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (!authorizationHeader.StartsWith("Basic ", StringComparison.InvariantCultureIgnoreCase))
                return AuthenticateResult.Fail("Cabeçalho de autorização inválido.");

            var base64Credentials = authorizationHeader.Substring("Basic ".Length).Trim();
            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(':');

            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Formato de autorização inválido.");

            var username = credentials[0];
            var password = credentials[1];

            if (username.VerifyPassword(_validUsername) && password.VerifyPassword(_validPassword))
            {
                var claims = new[] { new System.Security.Claims.Claim("name", username) };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, "Basic");
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "BasicAuthentication");

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Usuário ou senha inválido.");
        }
    }

}
