using System;
using PasteIt.Entities;

namespace PasteIt.Models.ViewModels
{
	public class PasteReadViewModel
	{
		public IEnumerable<Paste> RecentPastes { get; set; }
		public Paste Paste { get; set; }
	}
}

