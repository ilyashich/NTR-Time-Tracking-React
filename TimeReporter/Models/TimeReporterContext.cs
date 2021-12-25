using System;
using Microsoft.EntityFrameworkCore;

namespace TimeReporter.Models
{
    public class TimeReporterContext : DbContext
    {
        public TimeReporterContext(DbContextOptions<TimeReporterContext> options) : base(options) {
        }

        public DbSet<Accepted> Accepteds {get; set;}

        public DbSet<Activity> Activities { get; set;}

        public DbSet<Entry> Entries {get; set;}

        public DbSet<Report> Reports { get; set;}

        public DbSet<Subactivity> Subactivities { get; set;}

        public DbSet<Worker> Workers {get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitialMigration(modelBuilder);
        }
        
        private void InitialMigration(ModelBuilder mb)
        {
            string[] names =
            {
                "Clarkson", "Hammond", "May", "Plant", "Page", "Bonham", "Jones", "Hetfield", "Hammett",
                "Ulrich","Trujillo"
            };

            for(var i = 0; i < 11; ++i)
            {
                mb.Entity<Worker>().HasData(new Worker() { WorkerId = i+1, Name = names[i] });
            }
            
            string[] activityCodes =
            {
                "Mercury-1", "Jupiter-2", "Pluto-1", "Saturn-5", "Venus-3", 
                "Uranus-2", "OTHER", "Neptune-7", "Luna-1", "Europa-77"
            };
            string[] activityNames =
            {
                "Mercury", "Jupiter", "Pluto", "Saturn", "Venus", "Uranus", 
                "Other", "Neptune", "Luna", "Europa"
            };
            int[] activityBudgets =
            {
                115, 50, 100, 100, -10, 70, -1, 150, 150, 300
            };

            for (var i = 0; i < 10; ++i)
            {
                mb.Entity<Activity>().HasData(new Activity()
                {
                    ActivityId = i + 1, Code = activityCodes[i],
                    Name = activityNames[i], Budget = activityBudgets[i], Active = true, WorkerId = i + 1
                });

                var isFrozen = i is 0 or 3;

                mb.Entity<Report>().HasData(new Report()
                {
                    ReportId = i + 1, Frozen = isFrozen, Date = DateTime.Today, WorkerId = i + 1
                });
                
            }

            for (var i = 0; i < 10; ++i)
            {
                if (i != 6)
                {
                    mb.Entity<Subactivity>().HasData(new Subactivity()
                        {SubactivityId = i + 1, Code = "database", ActivityId = i + 1});
                }
                else
                {
                    mb.Entity<Subactivity>().HasData(new Subactivity()
                        {SubactivityId = i + 1, Code = "", ActivityId = i + 1});
                }
                
            }

            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 1, Time = 50, ReportId = 1, ActivityId = 2, WorkerId = 1
            });
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 2, Time = 110, ReportId = 1, ActivityId = 7, WorkerId = 1
            });
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 3, Time = 25, ReportId = 1, ActivityId = 3, WorkerId = 1
            });
            
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 4, Time = 100, ReportId = 4, ActivityId = 2, WorkerId = 4
            });
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 5, Time = 150, ReportId = 4, ActivityId = 8, WorkerId = 4
            });
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 6, Time = 110, ReportId = 4, ActivityId = 7, WorkerId = 4
            });
            mb.Entity<Accepted>().HasData(new Accepted()
            {
                AcceptedId = 7, Time = 160, ReportId = 4, ActivityId = 5, WorkerId = 4
            });


            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 1, Date = DateTime.Today, WorkerId = 1, Time = 70, Description = "created table",
                ActivityId = 2, ReportId = 1, SubactivityId = 2
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 2, Date = DateTime.Today, WorkerId = 1, Time = 150, Description = "tea time",
                ActivityId = 7, ReportId = 1, SubactivityId = 7
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 3, Date = DateTime.Today, WorkerId = 1, Time = 30, Description = "added row",
                ActivityId = 3, ReportId = 1, SubactivityId = 3
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 4, Date = DateTime.Today, WorkerId = 4, Time = 70, Description = "edited row",
                ActivityId = 2, ReportId = 4, SubactivityId = 2
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 5, Date = DateTime.Today, WorkerId = 4, Time = 150, Description = "added column",
                ActivityId = 8, ReportId = 4, SubactivityId = 8
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 6, Date = DateTime.Today, WorkerId = 4, Time = 30, Description = "resting",
                ActivityId = 7, ReportId = 4, SubactivityId = 7
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 7, Date = DateTime.Today, WorkerId = 4, Time = 30, Description = "created table",
                ActivityId = 5, ReportId = 4, SubactivityId = 5
            });
            
            mb.Entity<Entry>().HasData(new Entry()
            {
                EntryId = 8, Date = DateTime.Today, WorkerId = 8, Time = 70, Description = "created table",
                ActivityId = 3, ReportId = 8, SubactivityId = 3
            });
            
        }

    }
}