using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ClearSkiesAirlines.DAL
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string Brukernavn { get; set; }
        public byte[] Passord { get; set; }
        public byte[] Salt { get; set; }
    }
    public class Kunde
    {
        [Key]
        public int KundeId { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Epost{ get; set; }
        public string Adresse { get; set; }
        public string Telefon { get; set; }

        public virtual PostSted PostSted { get; set; }

        public virtual List<Handel> Handler { get; set; }

    }

    public class PostSted
    {
        [Key]
        public string PostNr { get; set; }
        public string Sted { get; set; }

        public virtual List<Kunde> Kunder { get; set; }
    }

    public class Reise
    {
        [Key]
        public int ReiseId { get; set; }
        public string Fra { get; set; }
        public string Til { get; set; }
        public DateTime Avreise { get; set; }
        public DateTime Ankomst { get; set; }
        public String Varighet { get; set; }
        public int Kapasitet { get; set; }
        public int Pris { get; set; }

        public virtual List<Billett> Billetter { get; set; }
    }

    public class Billett
    {
        [Key]
        public int BillettId { get; set; }
        public virtual Handel Handel { get; set; }
        public virtual Reise Reise { get; set; }
        public string PassasjerFornavn { get; set; }
        public string PassasjerEtternavn { get; set; }
    }

    public class Handel
    {
        [Key]
        public int HandelId { get; set; }
        public int TotalPris { get; set; }
        public string Kontonummer { get; set; }
        public virtual List<Billett> Billetter { get; set; }
        public virtual Kunde Kunde { get; set; }
    }

    public class ChangeLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime DateChanged { get; set; }
    }

    public class AirlineDbContext : DbContext
    {
        public AirlineDbContext() : base("name=AirlineDb")
        {
                Database.CreateIfNotExists();
                //NB: Må kjøres to ganger når databasen ikke eksisterer
                // OG den dropper alltid nytt innhold og derfor kommenteres ut om nødvendig.
                Database.SetInitializer(new DBInit());
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Kunde> Kunder { get; set; }
        public DbSet<PostSted> PostSted { get; set; }
        public DbSet<Reise> Reiser { get; set; }
        public DbSet<Billett> Billetter { get; set; }
        public DbSet<Handel> Handler { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostSted>()
                        .HasKey(p => p.PostNr);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.UtcNow;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop].ToString();
                    var currentValue = change.CurrentValues[prop].ToString();
                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        ChangeLogs.Add(log);
                    }
                }
            }
            return base.SaveChanges();
        }

        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
    }
}