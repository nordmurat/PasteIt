using System;
using PasteIt.Entities;

namespace PasteIt.Models.ViewModels
{
	public class ProfilePastesViewModel
	{
        public IEnumerable<Paste> RecentPastes { get; set; }
        public IEnumerable<Paste> Pastes { get; set; }
    }
}

