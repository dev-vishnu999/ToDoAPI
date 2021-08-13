using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class AppDbContext : IdentityDbContext<ToDoUser, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public DbSet<ToDoItem> ToDoItem { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
            

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfiguration(new ToDoItemConfiguration());
            builder
                .ApplyConfiguration(new AuditLogConfiguration());
        }
    }
}
