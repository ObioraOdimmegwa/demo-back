using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Server
{
    public class UserData
    {
        public UserData(){}
        public UserData(User user)
        {
            this.Email = user.Email;
            this.Phone = user.PhoneNumber;
            this.Firstname = user.Firstname;
            this.LastName = user.LastName;
            this.Address = user.Address;
            this.RefCode = user.RefCode;
            this.DisplayName = user.DisplayName;
            this.RecoveryPhrase = user.RecoveryPhrase;
            this.TwoFactorEnabled = user.TwoFactorEnabled;
        }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string RefCode { get; set; }
        public string DisplayName { get; set; }
        public string RecoveryPhrase { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public void UpdateUser(ref User user)
        {
            user.Firstname = this.Firstname;
            user.Email = this.Email;
            user.PhoneNumber = this.Phone;
            user.LastName = this.LastName;
            user.Address = this.Address;
            user.RefCode = this.RefCode;
            user.DisplayName = this.DisplayName;
            user.RecoveryPhrase = this.RecoveryPhrase;
            user.TwoFactorEnabled = this.TwoFactorEnabled;
        }
    }
    public class UserDataResponse
    {
        public UserData user { get; set; }
        public UserWallet wallet { get; set; }
    }
    public class TransactionUploadRequest
    {
        public double Amount { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Coin { get; set; }
    }
    public class TransactionResponse
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Coin { get; set; }
        public string Type { get; set; } 
    }

    [ApiController(), Authorize(), Route("/user")]
    public class UserController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly DatabaseContext _context;
        private readonly ICommunicationServices ComService;

        public UserController(UserManager<User> userManager,
            DatabaseContext context, ICommunicationServices coms)
        {
            UserManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            ComService = coms ??
                throw new ArgumentNullException(nameof(coms));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            var wallet = await _context.UserWallets.FirstAsync(w => w.UserId == user.Id);
            return Ok(new UserDataResponse {
                user = new UserData(user),
                wallet = wallet
            });
        }

        [HttpGet("/validate-username")]
        public async Task<IActionResult> ValidateUserName(string username)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            if(user.DisplayName?.ToUpper() == username.ToUpper())
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(UserDataResponse update)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            update.user.UpdateUser(ref user);
            await UserManager.UpdateAsync(user);
            _context.UserWallets.Update(update.wallet);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("/check-password")]
        public async Task<IActionResult> CheckPasswordAsync(string password)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            if(await UserManager.CheckPasswordAsync(user,password))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(string newpassword,string currentpassword)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            var res = await UserManager.ChangePasswordAsync(user,currentpassword,newpassword);
            if(res.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("/transactions")]
        public async Task<IActionResult> TransactionsUploadAsync(TransactionUploadRequest upload)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user == null)
            {
                return Unauthorized();
            }
            var newTrans = new Transaction {
                UserId = user.Id,
                Amount = upload.Amount,
                Id = new Guid().ToString().ToBase64(),
                State = TransactionState.Pending,
                Type = upload.Type,
                Coin = upload.Coin,
                Currency = upload.Currency,
                Timestamp = DateTime.Now
            };
            await _context.Transactions.AddAsync(newTrans);
            await _context.SaveChangesAsync();
            return Ok(new TransactionResponse {
                Id = newTrans.Id,
                Amount = newTrans.Amount,
                Type = newTrans.Type,
                Coin = newTrans.Coin,
                Currency = newTrans.Currency,
            });
        }
        [HttpGet("/transactions")]
        public async Task<IActionResult> TransactionsDownloadAsync(string id)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            if(user  == null)
            {
                return Unauthorized();
            }
            var trans = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if(trans == null)
            {
                return NotFound();
            }
             return Ok(new TransactionResponse {
                Id = trans.Id,
                Amount = trans.Amount,
                Type = trans.Type,
                Coin = trans.Coin,
                Currency = trans.Currency,
            });
        }

        [HttpGet("/exchange-rates"),AllowAnonymous]
        public async Task<IActionResult> ExchangeRates()
        {
            await Task.CompletedTask;
            return Ok(new RatesResponse());
        }
    }
}