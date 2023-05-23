using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using PasteIt.Entities;

namespace PasteIt.Data
{
	public class PasteRepository : IPasteRepository
	{
		private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PasteRepository(AppDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

		public string CreatePaste(Paste data)
		{
			data.Id = data.Title;
			data.CreatedTime = DateTime.UtcNow;
            data.Notes = "";
			_context.Pastes.Add(data);
			_context.SaveChanges();
			return data.Id;
		}

		public IEnumerable<Paste> RecentPastes()
        {
            IEnumerable<Paste> pastes = _context.Pastes.OrderByDescending(p => p.CreatedTime).Where(p => p.Status == "1").Take(9).ToList();
            foreach (var paste in pastes)
			{
                paste.Notes = TimeAgo(paste.CreatedTime);
			}
            return pastes;
        }

        public IEnumerable<Paste> GetPastesByAuthor(string author, int page = 0)
        {
            return _context.Pastes.OrderByDescending(p => p.CreatedTime).Where(p => p.Author == author).Skip(25 * page).Take(25).ToList();
        }

        public Paste ReadPaste(string Id)
        {
            var paste = _context.Pastes.FirstOrDefault(c => c.Id == Id);

            return paste;
        }

        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.UtcNow - dt;

            if (span.TotalDays > 365)
            {
                int years = (int)(Math.Floor(span.TotalDays / 365));
                return $"{years} year{(years == 1 ? "" : "s")} ago";
            }
            if (span.TotalDays > 30)
            {
                int months = (int)(Math.Floor(span.TotalDays / 30));
                return $"{months} month{(months == 1 ? "" : "s")} ago";
            }
            if (span.TotalDays > 7)
            {
                int weeks = (int)(Math.Floor(span.TotalDays / 7));
                return $"{weeks} week{(weeks == 1 ? "" : "s")} ago";
            }
            if (span.TotalDays > 1)
            {
                int days = (int)(Math.Floor(span.TotalDays));
                return $"{days} day{(days == 1 ? "" : "s")} ago";
            }
            if (span.TotalHours > 1)
            {
                int hours = (int)(Math.Floor(span.TotalHours));
                return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
            }
            if (span.TotalMinutes > 1)
            {
                int minutes = (int)(Math.Floor(span.TotalMinutes));
                return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
            }
            if (span.TotalSeconds >= 1)
            {
                int seconds = (int)(Math.Floor(span.TotalSeconds));
                return $"{seconds} second{(seconds == 1 ? "" : "s")} ago";
            }

            return "just now";
        }

        public async Task<IdentityUser> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                IdentityUser user = await _userManager.FindByIdAsync(userId);
                return user;
            }
            return null;
        }

        public void increaseViewCount(string Id)
        {
            Paste paste = _context.Pastes.FirstOrDefault(x => x.Id == Id);
            if (paste != null)
            {
                paste.ViewCount++;
                _context.SaveChanges();
            }
        }
    }
}

