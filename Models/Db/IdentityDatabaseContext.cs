using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace TestTest.Models.Db
{
    public class IdentityDatabaseContext : IdentityDbContext<Osoba>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
            : base(options)
        {
        }
    }
}