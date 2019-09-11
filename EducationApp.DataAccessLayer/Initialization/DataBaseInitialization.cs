using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using EducationApp.DataAccessLayer.AppContext;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class DataBaseInitialization : CreateDatabaseIfNotExists<ApplicationContext>
    {
        public override void InitializeDatabase(ApplicationContext context)
        {
            
        }
        protected override void Seed(ApplicationContext context)
        {
            base.Seed(context);
        }
    }
}
