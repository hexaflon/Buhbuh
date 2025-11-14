using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace TestTest.Pages
{
    [Authorize(Roles = "Nauczyciel, Admin")]
    public class IndexTeacherModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private IAppLogger _logger;
        private readonly UserManager<Person> _userManager;
        public IndexTeacherModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _userManager = userManager;
            _logger = Logger.getInstance();
            _context = context;
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        public IList<Person> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_userManager.Users != null)
            {
                User = await _userManager.Users.ToListAsync();
                _logger.ShowLogs();
            }
        }
    }
}
