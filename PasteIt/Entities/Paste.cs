using System;
namespace PasteIt.Entities
{
	public class Paste
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string? Syntax { get; set; }
		public string? Status { get; set; }
		public string? Description { get; set; }
		public Folder? Folder { get; set; }
		public string? Expire { get; set; }
		public string? Password { get; set; }
		public List<Tag>? Tags { get; set; }
		public string? Author { get; set; }
		public DateTime CreatedTime { get; set; }
		public int ViewCount { get; set; }
		public string Notes { get; set; }
    }
}

