using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.Controllers;

[AllowAnonymous]
public class AboutUsController : Controller
{
    public IActionResult Index() => View();
}
