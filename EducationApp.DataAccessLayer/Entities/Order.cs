﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.DataAccessLayer.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ApplicationUser UsersId { get; set; }
        public DateTime Date { get; set; }
        public Payment PaymentsId { get; set; }

    }
}
