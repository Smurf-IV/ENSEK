using Ensek.Service.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ensek.Service.Controllers;

//[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
}