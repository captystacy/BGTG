using BGTG.POS.Data.Base;
using BGTG.POS.Entities;
using Microsoft.EntityFrameworkCore;

namespace BGTG.POS.Data
{
    /// <summary>
    /// Database for application
    /// </summary>
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>, IApplicationDbContext
    {
        /// <inheritdoc />
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region Calendar plan tool

        #endregion

        #region System

        public DbSet<Log> Logs { get; set; }

        #endregion
    }
}