using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maxima.Net.SDK.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxima.Net.SDK.Data
{
    public class ErpContext : DbContext
    {

        public string DbPath { get; }
        public ErpContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "erp_database.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        public virtual DbSet<ControleDadosModel> ControleDadosModels { get; set; }
        public virtual DbSet<ParametroModel> ParametroModels { get; set; }

    }
}