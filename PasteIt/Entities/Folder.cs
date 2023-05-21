using System;
namespace PasteIt.Entities
{
	public class Folder
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Owner { get; set; }
		public List<Paste>? Pastes { get; set; }
		public string? Password { get; set; }
		public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
