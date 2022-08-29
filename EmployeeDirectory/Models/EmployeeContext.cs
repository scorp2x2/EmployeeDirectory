using Microsoft.EntityFrameworkCore;
using System;

namespace EmployeeDirectory.Models
{
    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=EmployeeDirectory.db");
        }
    }
}
