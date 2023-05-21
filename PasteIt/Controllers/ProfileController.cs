using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasteIt.Data;
using PasteIt.Entities;
using PasteIt.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PasteIt.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPasteRepository _pasteRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProfileController(IPasteRepository pasteRepository)
        {
            _pasteRepository = pasteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PastesAsync(int pages = 0)
        {
            IdentityUser user = await _pasteRepository.GetLoggedInUser();
            var model = new ProfilePastesViewModel()
            {
                Pastes = _pasteRepository.GetPastesByAuthor(user.NormalizedUserName, pages),
                RecentPastes = _pasteRepository.RecentPastes()
            };
            return View(model);
        }
    }
}

