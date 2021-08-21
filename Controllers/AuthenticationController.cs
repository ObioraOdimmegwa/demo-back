using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Server
{
    public class LoginForm
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class SignUpForm
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class TwoAuthForm
    {
        public string email { get; set; }
        public string token { get; set; }
    }
    public class ResetPasswordForm
    {
        public string token { get; set; }
        public string newpassword { get; set; } 
    }

    [ApiController, AllowAnonymous, Route("/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;
        private readonly IConfiguration Configuration;
        private readonly DatabaseContext _context;
        private readonly ICommunicationServices CommService;

        public AuthenticationController(UserManager<User> userManager,
        IConfiguration configuration, SignInManager<User> signInManager,
        DatabaseContext context, ICommunicationServices coms)
        {
            UserManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
            Configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            SignInManager = signInManager ??
                throw new ArgumentNullException(nameof(SignInManager));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            CommService = coms ??
                throw new ArgumentNullException(nameof(coms));
        }
        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAsync(SignUpForm form)
        {
            if (await UserManager.Users.AnyAsync(u => u.NormalizedEmail == form.email.ToUpper()))
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = $"{form.email} has already been registered"
                });
            }
            User newUser = new User
            {
                Email = form.email,
                UserName = form.email,
            };

            // set user's recovery phrase
            newUser.SetPassPhrase();
            IdentityResult res = await UserManager.CreateAsync(newUser, form.password);
            if (res.Succeeded)
            {
                User user = await UserManager.FindByEmailAsync(newUser.Email);
                await _context.UserWallets.AddAsync(new UserWallet
                {
                    UserId = user.Id,
                    Currency = Currencies.USD,
                });
                await _context.SaveChangesAsync();
                Console.WriteLine($"{user.Email} Logged in");
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginAsync(LoginForm form)
        {
            var user = await UserManager.FindByEmailAsync(form.email);
            if (user == null)
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Your email is incorrect" });
            }
            if (!await UserManager.CheckPasswordAsync(user, form.password))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Your password is incorrect" });
            }
            if (user.TwoFactorEnabled)
            {
                Random rand = new Random();
                var valToken = new ValidationToken
                {
                    UserId = user.Id,
                    Token = rand.Next(11111, 99999).ToString(),
                    Purpose = TokenPurpose.TwoFactorAuth,
                    ExpireAt = DateTime.Now.AddMinutes(30),
                };
                if(await _context.ValidationTokens.AnyAsync(t => t.UserId == user.Id))
                {
                    _context.ValidationTokens.Update(valToken);
                }
                else
                {
                    await _context.ValidationTokens.AddAsync(valToken);
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"New Login 2 factor code {valToken.Token}");
                await CommService.Send2FactorCode(user,valToken.Token,HttpContext.Request.Host.ToUriComponent());
                return Ok(new LoginResponse
                {
                    Token = string.Empty,
                    TwoFactorCode = valToken.Token
                });
            }
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new LoginResponse
            {
                Token = token,
                TwoFactorCode = string.Empty
            });
        }

        [HttpPost("/login-2auth")]
        public async Task<IActionResult> TwoAuthConfirm(TwoAuthForm form)
        {
            var user = await UserManager.FindByEmailAsync(form.email);
            if (user == null)
            {
                Console.WriteLine($"UnAuthorized Token Check");
                return Unauthorized(new ErrorResponse
                {
                    ErrorMessage = "Authorization code is incorrect"
                });
            }
            var valToken = await _context.ValidationTokens.FirstOrDefaultAsync(t => t.UserId == user.Id &&
                t.Purpose == TokenPurpose.TwoFactorAuth);
            if (valToken == null ||
                DateTime.Now >= valToken.ExpireAt || 
                form.token != valToken.Token)
            {
                Console.WriteLine($"{form.token} Incorrect");
                return Unauthorized(new ErrorResponse
                {
                    ErrorMessage = "Authorization code is incorrect"
                });
            }
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new LoginResponse
            {
                Token = token,
                TwoFactorCode = string.Empty
            });
        }

        [HttpGet("/reset-start")]
        public async Task<IActionResult> ResetPassowrdStart(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if(user == null)
            {
                return BadRequest(new ErrorResponse {
                    ErrorMessage = "We can't find a user with that e-mail address"
                });
            }
            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            string resetUrl =$"https://192.168.43.68:5001/reset-password/{HttpUtility.UrlEncode(resetToken)}";
            Console.WriteLine($"Reset password with {resetUrl}");
            await CommService.SendPasswordReset(user,resetUrl,HttpContext.Request.Host.ToUriComponent());            
            var valToken = new ValidationToken
            {
                Token = resetToken,
                UserId = user.Id,
                Purpose = TokenPurpose.PasswordReset,
                ExpireAt = DateTime.Now.AddMinutes(15),
            };
            if(await _context.ValidationTokens.AnyAsync(t => t.UserId == user.Id))
            {
                _context.ValidationTokens.Update(valToken);
            }
            else
            {
                await _context.ValidationTokens.AddAsync(valToken);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/reset-finish")]
        public async Task<IActionResult> ResetPasswordFinish(ResetPasswordForm form)
        {
            var resetToken = await _context.ValidationTokens.FirstOrDefaultAsync(t => t.Token == form.token && t.Purpose == TokenPurpose.PasswordReset);
            if(resetToken == null ||
                DateTime.Now >= resetToken.ExpireAt ||
                form.token != resetToken.Token)
            {
                return BadRequest(new ErrorResponse {
                    ErrorMessage = "An error occured, please try again"
                });
            }
            var user = await UserManager.FindByIdAsync(resetToken.UserId);
            var res = await UserManager.ResetPasswordAsync(user,resetToken.Token,form.newpassword);
            if(!res.Succeeded)
            {
                return BadRequest(new ErrorResponse {
                    ErrorMessage = "An error occured, please try again"
                });
            }
            return Ok();
        }

        private SigningCredentials GetSigningCredentials()
        {
            var _jwtSettings = Configuration.GetSection("JWTSettings");
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var principal = await SignInManager.CreateUserPrincipalAsync(user);
            return principal.Claims.ToList();
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var _jwtSettings = Configuration.GetSection("JWTSettings");
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}