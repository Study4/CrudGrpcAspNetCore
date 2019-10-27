using CrudGrpcAspNetCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudGrpcAspNetCore.Infrastructures
{
    public class SkyHRContext : DbContext
    {
        public SkyHRContext(DbContextOptions<SkyHRContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
