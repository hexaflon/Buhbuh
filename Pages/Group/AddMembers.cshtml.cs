using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class AddMembersModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;

        public AddMembersModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet([FromQuery]int? id)
        {
            if (!id.HasValue) return RedirectToPage("./List");
            

            var users = _userManager.GetUsersInRoleAsync("Uczen");
            var students = users.Result.ToList();
            students = students.Where(o => !_context.Participant.Any(u => u.StudentId == o.PersonId && u.GroupId == id)).ToList();


            var foundUsers = _userManager.GetUsersInRoleAsync("Uczen");
            foundStudents = foundUsers.Result.ToList();
            foundStudents = foundStudents
                .Where(o => _context.Participant.Any(u => u.StudentId == o.PersonId && u.GroupId == id))
                .ToList();

            var studentList = students.Select(person => new SelectListItem
            {
                Value = person.PersonId.ToString(),
                Text = $"{person.Surname} {person.Name} {person.Email}"
            }).ToList();
            ViewData["studentId"] = new SelectList(studentList, "Value","Text");
            ViewData["groupId"] = id; 
            return Page();
        }

        [BindProperty]
        public Participant Participants { get; set; } = default!;
        public List<Person> foundStudents { get; set; } = default!;


        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync([FromQuery]int? id)
        {
            if (!id.HasValue) return RedirectToPage("./List");

            if (!ModelState.IsValid || _context.Participant == null || Participants == null)
            {
                return Page();
            }

            int? groupList = _context.Group.Where(gr => gr.Id == id).Select(gr => gr.Id).FirstOrDefault();
            if (groupList == null) return NotFound();
            Participants.GroupId = id;

            var participantList = _context.Participant.ToList();
            if (participantList == null) Participants.Id = 0;
            else
            {
                Participants.Id = participantList.OrderByDescending(u => u.Id)
                                                        .Select(u => u.Id).FirstOrDefault() + 1;
            }

            
            //dodać dla użytkownika
            

            _context.Participant.Add(Participants);
            await _context.SaveChangesAsync();

            return RedirectToPage("", new {id=id});
        }


        

        public async Task<IActionResult> OnGetDelete([FromQuery]int? personId, [FromQuery]int? groupId)
        {
            
            if(personId == null || groupId == null) return NotFound();
            var userToRemove = await _context.Participant.FirstOrDefaultAsync(u => u.StudentId == personId && u.GroupId == groupId);
            
            if (userToRemove != null)
            {
                _context.Participant.Remove(userToRemove);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("", new { id = groupId });
        }
    }
}
