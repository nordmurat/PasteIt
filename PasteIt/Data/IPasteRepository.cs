using System;
using Microsoft.AspNetCore.Identity;
using PasteIt.Entities;

namespace PasteIt.Data
{
	public interface IPasteRepository
	{
        public string CreatePaste(Paste data);
        public IEnumerable<Paste> RecentPastes();
        public void increaseViewCount(string Id);
        public IEnumerable<Paste> GetPastesByAuthor(string author, int pages);
        public Paste ReadPaste(string Id);

        public Folder ReadFolderPastes(string Id);
        public void CreateFolder(Folder data);
        public IEnumerable<Folder> ReadFolder(string owner);

        public Task<IdentityUser> GetLoggedInUser();
    }
}

