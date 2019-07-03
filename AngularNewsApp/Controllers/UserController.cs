using AngularNewsApp.Models;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AngularNewsApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NewsAppContext _context;
        private IConfiguration _config;

        public UserController(IConfiguration config, NewsAppContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            int userId = Convert.ToInt32(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);

            var user = _context.User.Where(x=>x.id == userId).SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            bool loginOk = !_context.User.Select(u => u.login).Contains(user.login);
                
            //Si l'utilisateur n'est pas déjà inscrit
            if (loginOk)
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = user.id }, user);
            }

            return BadRequest();

        }

        // DELETE: User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }

        // Génération d'un token de réinitialisation et envoi d'un email
        [HttpGet("reset_password/{email}")]
        public async Task<IActionResult> GetResetPassword(string email)
        {

            User user = _context.User.Where(x => x.email == email).SingleOrDefault();
            if(user == null)
            {
                return NotFound();
            }

            var token = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret("tokenReset33-password&!")
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Build();

            var tokens = _context.UserValidationToken.Where(x => x.User.email == email);

            _context.UserValidationToken.RemoveRange(tokens);

            _context.UserValidationToken.Add(new UserValidationToken()
            {
                id_user = user.id,
                date = DateTime.Now,
                token = token
            });

            _context.SaveChanges();

            new MailUtility().SendMail("Réinitialisation du mot de passe", "http://localhost:4200/reset_password/"+ token);

            return Ok();

        }

        // Génération d'un token de réinitialisation et envoi d'un email
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            string token = model.token;

            string newPassword = model.newPassword;

            UserValidationToken v = _context.UserValidationToken.Where(x => x.token == token).SingleOrDefault();

            if (v == null)
                return BadRequest();

            User user = await _context.User.FindAsync(v.id_user);

            user.password = newPassword;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(v.id_user))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _context.UserValidationToken.Remove(v);

            await _context.SaveChangesAsync();

            return NoContent();

        }

        //Validation d'un token (date et presence en bdd)
        [HttpGet("reset_password/validate_token/{token}")]
        public async Task<IActionResult> ValidateTokenResetPassword(string token)
        {
            UserValidationToken v = _context.UserValidationToken.Where(x => x.token == token).Single();

            token = v.token;

            const string secret = "tokenReset33-password&!";

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
                Console.WriteLine(json);
            }
            catch (TokenExpiredException)
            {
                _context.UserValidationToken.Remove(v);
            }
            catch (SignatureVerificationException)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}


public class ResetPasswordModel
{
    public string token { get; set; }
    public string newPassword { get; set; }
}