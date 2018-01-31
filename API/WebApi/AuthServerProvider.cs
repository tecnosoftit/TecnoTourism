using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Services;
using ViewModel.General;

namespace WebApi
{
    public class AuthServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly Security _sec = new Security();

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = _sec.GetUser(context.UserName, context.Password);
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.ProfilePicture)) user.ProfilePicture = "http://coderthemes.com/minton_2.3/material/assets/images/users/avatar-2.jpg";
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                var roles = _sec.GetRoles(user.UserId);
                foreach (var item in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, item.Role));
                }
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName, user.UserLastname));
                identity.AddClaim(new Claim("UserInfo", JsonConvert.SerializeObject(user)));
                identity.AddClaim(new Claim("Roles", JsonConvert.SerializeObject(roles)));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid Grant", "invalid Credentials");
                return;
            }
        }
    }
}