using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() : base("MovieDbContext")
        {
        }

        public DbSet<Studio> Studios { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<FilmActor> FilmActors { get; set; }
        public DbSet<AwardRecipient> AwardRecipients { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Employee>()
                .HasOptional(e => e.Director)
                .WithMany()
                .HasForeignKey(e => e.DirectorAssistantID);
        }
        
        [DbFunction("dbo", "GetDirectorFullName")]
        public static string GetDirectorFullName(int directorId)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [DbFunction("dbo", "GetFilmCountForGenre")]
        public static int GetFilmCountForGenre(int genreId)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        public virtual IEnumerable<Actor> GetActorsByFilm(int filmId)
        {
            return this.Database.SqlQuery<Actor>("dbo.GetActorsByFilm @filmId", new SqlParameter("@filmId", filmId));
        }
    }
}