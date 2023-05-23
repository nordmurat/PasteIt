using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasteIt.Data;
using PasteIt.Entities;
using PasteIt.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PasteIt.Controllers
{
    public class PasteController : Controller
    {
        private readonly IPasteRepository _repository;

        public PasteController(IPasteRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Create()
        {
            IEnumerable<Paste> recentPastes = _repository.RecentPastes();
            return View("Create", recentPastes);
        }

        public IActionResult Read(string id, string? password)
        {
            PasteReadViewModel model = new PasteReadViewModel
            {
                RecentPastes = _repository.RecentPastes(),
                Paste = _repository.ReadPaste(id)
            };

            if (model.Paste.Password != null)
            {
                if (model.Paste.Password != password)
                {
                    ViewData["password"] = password;
                    return View("ReadRequiredPassword", model);
                }
                else
                {
                    _repository.increaseViewCount(model.Paste.Id);
                }
            }

            TimeSpan span = DateTime.UtcNow - model.Paste.CreatedTime;
            switch (model.Paste.Expire)
            {
                case "SD":
                    if (model.Paste.ViewCount > 1) model.Paste = null;
                    break;
                case "10M":
                    if (span.Minutes >= 10) model.Paste = null;
                    break;
                case "1H":
                    if (span.Hours >= 1) model.Paste = null;
                    break;
                case "1D":
                    if (span.Days >= 1) model.Paste = null;
                    break;
                case "1W":
                    if (span.Days >= 7) model.Paste = null;
                    break;
                case "2W":
                    if (span.Days >= 14) model.Paste = null;
                    break;
                case "1M":
                    if (span.Days >= 30) model.Paste = null;
                    break;
                case "6M":
                    if (span.Days >= 180) model.Paste = null;
                    break;
                case "1Y":
                    if (span.Days >= 365) model.Paste = null;
                    break;
            }

            if (model.Paste == null) return StatusCode(403);

            return View(model);
        }

        [ValidateAntiForgeryToken()]
        [HttpPost]
        public async Task<IActionResult> Create(Paste model)
        {
            var user = await _repository.GetLoggedInUser();
            if (user != null)
                model.Author = user.NormalizedUserName;

            var newId = _repository.CreatePaste(model);
            return RedirectToAction("Index", "Home");
        }
    }
}