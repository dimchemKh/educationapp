using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class DbBaseInitializing
    {
        private ApplicationContext _context;
        public DbBaseInitializing(ApplicationContext context)
        {
            _context = context;
        }

    }
}
