using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.utils;
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
        private readonly UserManager<Osoba> _userManager;
        public IndexTeacherModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _userManager = userManager;
            //Getting an Instance of Singleton
            _logger = Logger.getInstance();
            _context = context;
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            //Declaring a Decorator
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        public IList<Osoba> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_userManager.Users != null)
            {
                User = await _userManager.Users.ToListAsync();
                //Usage of Decorator
                _logger.ShowLogs();
            }
        }
    }
}
