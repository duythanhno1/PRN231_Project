using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Responsitory.IService;
using Responsitory.DTO;
using AutoMapper.Execution;
using Microsoft.IdentityModel.Tokens;
using ShopProjectAPI.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ShopProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService _AccountService;
        private readonly IEmailService _EmailService;

        public AccountController(IAccountService AccountService, IEmailService emailService)
        {
            _AccountService = AccountService;
            _EmailService = emailService;
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            try
            {
                return Ok(await _AccountService.GetAccountAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Account = await _AccountService.GetByIdAsync(id);
                return Account == null ? NotFound() : Ok(Account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount(CustomerResponse CustomerResponse)
        {
            try
            {
                if (CustomerResponse.FullName == "" ||
                   CustomerResponse.Address == "" ||
                   CustomerResponse.Phone == "" ||
                   CustomerResponse.Email == "" ||
                   CustomerResponse.PassWord == "" ||
                   CustomerResponse.ReEnterPassword == ""

                )
                {
                    return BadRequest(new { Message = "Account chua xac thuc! Thu lai" });
                }
                var newAccountId = await _AccountService.CreateAccountAsync(CustomerResponse);
                var Account = await _AccountService.GetByIdAsync(newAccountId);

                // Tạo nội dung email với newAccountId
                var mailContent = new MailContent()
                {
                    Subject = "Register !! Verified email!!",
                    To = CustomerResponse.Email,
                    Body = $@"<!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                    <meta charset=""UTF-8"" />
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                                    <title>Browser</title>
                                </head>
                                <body style=""margin: 0; padding: 0; background-color: #ffffff; display: flex; justify-content: center; align-items: center; height: 100vh;"">
                                    <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" height=""50%"" style=""max-width: 500px;max-height: 500px; "" align=""center"">
                                        <tr>
                                            <td bgcolor=""aliceblue"" style=""padding: 25px; text-align: center;border-radius: 10px;"">
                                                <img src=""https://scontent.fsgn5-5.fna.fbcdn.net/v/t1.15752-9/363532788_1004684137406306_8242624363090733781_n.png?_nc_cat=100&ccb=1-7&_nc_sid=8cd0a2&_nc_eui2=AeFLAwJv7fxX6ItfFAURLUJ3ezHbICoCNlB7MdsgKgI2UD1ruyCGsY8WbtcYKVe_iYyw7mP1aQZ_qb0gBxZyIZS7&_nc_ohc=faCIDP0V3kIAX-ALVnm&_nc_ht=scontent.fsgn5-5.fna&oh=03_AdT4nr-fzmhn-g7b6wqoAVL7W0GlXu7roDwDMAYtSRvUYw&oe=6565DB73"" alt=""Logo"" width=""200"" height=""120"" style=""display: block; margin: 0 auto;"">

                                                <p style=""font-size: 18px; color: black;margin:0px !important;"">Hi you,</p>
                                                <p style=""font-size: 16px;color: black;"">We're happy you signed up for Phone Love. To start exploring the Phone Shop Website, please confirm your email address.</p>
                                                <a href=""https://localhost:44312/AccountDocker/Verified?{newAccountId}"" style=""background: #004aad; color: #fff; text-decoration: none; display: inline-block; width: 7rem; height: 2rem;padding-top: 0.3rem; line-height: 1.8rem; text-align: center; border-radius: 2rem; font-size: 14px;"">Verify now</a>
                                                <p style=""font-size: 18px;color: black;"">Welcome to Phone Love!</p>
                                            </td>
                                        </tr>
                                    </table>
                                </body>
                                </html>"
                };
                _EmailService.SendMail(mailContent);
                return Account == null ? NotFound() : Ok(Account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, ChangePasswordResponse AccountResponse)
        {
            try
            {
                await _AccountService.UpdateAccountAsync(id, AccountResponse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _AccountService.DeleteAccountAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> CheckLogin(string email, string pass)
        {
            bool check = await _AccountService.CheckLog(email, pass);
            Account acc = _AccountService.GetMemberByMail(email);
            if (acc == null || !check)
            {
                return BadRequest(new { Message = "UnAcc" });
            }
            TokenReponse tokenReponse = new TokenReponse();
            if (acc.AccountStatus == "1" && acc.isVerified == true)
            {
                if (check)
                {
                    var token = GenerateToken(acc);
                    tokenReponse.Account = acc;
                    tokenReponse.AccessToken = token;
                    return Ok(tokenReponse);
                }
            }
            else if (acc.isVerified == false)
            {
                return BadRequest(new { Message = "Verified" });
            }
            else
            {
                return BadRequest(new { Message = "Ban" });
            }

            return BadRequest(new { Message = "Invalid login credentials" });
        }
        [AllowAnonymous]
        private string GenerateToken(Account acc)
        {
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet."
));
            var credentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim (JwtRegisteredClaimNames.Email, acc.Email),
                new Claim ("UserID", acc.UserID.ToString()),
                new Claim ("admin", acc.Admin.ToString()),
                new Claim ("AccountId", acc.AccountID.ToString())
            };
            var token = new JwtSecurityToken
                (
                issuer: "issuer",
                audience: "audience",
                claim,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials
                );
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
        List<AccountResponse> accounts { get; set; }
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductAsync(string search)
        {

            try
            {
                accounts = await _AccountService.SearchAccountAsync(search);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        Account acc { get; set; }
        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                acc = _AccountService.GetMemberByMail(email);
                string newpass = await _AccountService.ForgotPassword(email);
                MailContent mailContent = new MailContent()
                {
                    Subject = "ForgotPassword! Please sign in again!",
                    To = email,
                    Body = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n  " +
                    "  <title>Change Password Success</title>\r\n    <style>\r\n  " +
                    "      /* Define styles for the email */\r\n        body {\r\n         " +
                    "   font-family: Arial, sans-serif;\r\n            background-color: #f2f2f2;\r\n      " +
                    "      margin: 0;\r\n            padding: 0;\r\n        }\r\n\r\n        .container {\r\n      " +
                    "      max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n          " +
                    "  background-color: #ffffff;\r\n            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);\r\n          " +
                    "  border-radius: 5px;\r\n        }\r\n\r\n        .header {\r\n            background-color: #4285f4;\r\n       " +
                    "     color: #ffffff;\r\n            text-align: center;\r\n            padding: 20px;\r\n            border-radius: 5px 5px 0 0;\r\n  " +
                    "      }\r\n\r\n        h1 {\r\n            font-size: 24px;\r\n            margin: 0;\r\n        }\r\n\r\n        .content {\r\n         " +
                    "   padding: 20px;\r\n            color: #333;\r\n        }\r\n\r\n        p {\r\n            font-size: 16px;\r\n            margin: 10px 0;\r\n   " +
                    "     }\r\n\r\n        .footer {\r\n            text-align: center;\r\n            background-color: #f2f2f2;\r\n            padding: 10px;\r\n        " +
                    "    border-radius: 0 0 5px 5px;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n   " +
                    "         <h1>Change Password Success</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n                  <p>Your password has been successfully changed. You can now use your new password:<strong>{{newpass}}</strong> to access your account.</p>\r\n  " +
                    "      </div>\r\n        <div class=\"footer\">\r\n            <p>&copy; 2023 Your Company. All rights reserved.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n\r\n</html>\r\n"
                };

                mailContent.Body = mailContent.Body.Replace("{{newpass}}", newpass);
                _EmailService.SendMail(mailContent);
                return Ok(acc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet("CountAccount")]
        public async Task<IActionResult> Count()
        {
            try
            {
                CountAccount co = await _AccountService.CountAcc();
                return Ok(co);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }
        [Authorize(Policy = IdentifyRole.AdminRole)]
        [HttpGet("AdminAccount")]
        public async Task<IActionResult> AdminAccountAsync(int id)
        {
            try
            {
                await _AccountService.AdminAccountAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("Verified")]
        public async Task<IActionResult> VerifiedAccountAsync(int id)
        {
            try
            {
                await _AccountService.VerifiedAccountAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
