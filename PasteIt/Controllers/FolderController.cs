using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasteIt.Data;
using PasteIt.Models;
using PasteIt.Entities;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace PasteIt.Controllers
{
    public class FolderController : Controller
    {
        private readonly IPasteRepository _pasteRepository;

        private readonly string idRegex = @"[^a-z0-9]";

        public FolderController(IPasteRepository pasteRepository)
        {
            _pasteRepository = pasteRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult BrowseFolder(string folderId)
        {
            var model = _pasteRepository.ReadFolderPastes(folderId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> CreateFolder(IFormCollection form)
        {
            IdentityUser user = await _pasteRepository.GetLoggedInUser();
            if (user == null) return "401";
            var newFolder = new Folder()
            {
                Id = Regex.Replace(form["name"].ToString().ToLower(), idRegex, string.Empty),
                Name = form["name"],
                Owner = user.NormalizedUserName,
                Description = form["description"],
                Password = form["password"]
            };

            _pasteRepository.CreateFolder(newFolder);

            return "200";
        }

        [HttpGet]
        public async Task<JsonResult> GetFolders()
        {
            IdentityUser user = await _pasteRepository.GetLoggedInUser();
            JsonResponseViewModel model = new JsonResponseViewModel();
            if (user == null)
            {
                model.ResponseCode = 401;
                model.ResponseMessage = "Unauthorized access!";
                return Json(model);
            }

            IEnumerable<Folder> folders = _pasteRepository.ReadFolder(user.NormalizedUserName.ToString());
            if (folders != null)
            {
                model.ResponseCode = 200;
                var jsonList = new List<string>();
                model.ResponseMessage = JsonSerializer.Serialize(folders);
            }
            else
            {
                model.ResponseCode = 500;
                model.ResponseMessage = "No folder(s) available";
            }
            return Json(model);
        }
    }
}
