using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackend.Models;

namespace MunchrBackend.Context
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DataContext(DbContextOptions options) : base (options){}
    }
}