namespace NextekkManagmt.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NextekkManagmt.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NextekkManagmt.Models.ApplicationDbContext context)
        {
            var gender = new List<Gender>
            {
                new Gender {ID=Guid.NewGuid(), GenderType="Male" },
                new Gender {ID=Guid.NewGuid(), GenderType="Female" }

            };
            gender.ForEach(c => context.Genders.AddOrUpdate(f => f.GenderType, c));
            context.SaveChanges();
            var status = new List<MaritalStatus>
            {
                new MaritalStatus {ID=Guid.NewGuid(), Status="Single" , order=1 },
                new MaritalStatus {ID=Guid.NewGuid(), Status="Married", order=2 },
                new MaritalStatus {ID=Guid.NewGuid(), Status="Divorced", order=3 }

            };
            status.ForEach(c => context.MaritalStatuss.AddOrUpdate(f => f.Status, c));
            context.SaveChanges();
            var qualification = new List<EducationQualification>
            {
                new EducationQualification {ID=Guid.NewGuid(), Level="Ph.D" , order=1 },
                new EducationQualification {ID=Guid.NewGuid(), Level="M.Sc", order=2 },
                new EducationQualification {ID=Guid.NewGuid(), Level="B.Sc", order=3 },
                new EducationQualification {ID=Guid.NewGuid(), Level="HND", order=4 },
                new EducationQualification {ID=Guid.NewGuid(), Level="OND", order=5 },
                new EducationQualification {ID=Guid.NewGuid(), Level="Secondary School Leaving Certificate", order=6 }
            };
            qualification.ForEach(c => context.EducationQualifications.AddOrUpdate(f => f.Level, c));
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
