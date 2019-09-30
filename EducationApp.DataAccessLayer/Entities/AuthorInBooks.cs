﻿using EducationApp.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class AuthorInBooks
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int PrintingEditionId { get; set; }
    }
}