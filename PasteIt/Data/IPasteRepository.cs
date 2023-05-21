using System;
using Microsoft.AspNetCore.Identity;
using PasteIt.Entities;

namespace PasteIt.Data
{
	public interface IPasteRepository
	{
        public string CreatePaste(Paste data);
        public IEnumerable<Paste> RecentPastes();
        public IEnumerable<Paste> GetPastesByAuthor(string author, int pages);
        public Paste ReadPaste(string Id);

        public Task<IdentityUser> GetLoggedInUser();
    }
}

