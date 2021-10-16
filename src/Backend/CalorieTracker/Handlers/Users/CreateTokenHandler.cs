using CalorieTracker.Models.Users;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Handlers.Users {
    public class CreateTokenHandler : IRequestHandler<CreateTokenRequest, string> {
        private readonly string jwtSecretKey;

        public CreateTokenHandler(IConfiguration configuration) {
            jwtSecretKey = configuration.GetValue<string>("ScretKeyForJWT");
        }

        public Task<string> Handle(CreateTokenRequest request, CancellationToken cancellationToken) {
            var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, request.User.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, request.User.Email),
                    new Claim(JwtRegisteredClaimNames.Name, request.User.Name)
                });
            claimsIdentity.AddClaims(request.User.Roles.Select(r => new Claim("roles", r.Name)));

            var key = Encoding.ASCII.GetBytes(jwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}
