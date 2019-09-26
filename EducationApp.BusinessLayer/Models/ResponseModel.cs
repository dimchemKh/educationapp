using System;
using System.Collections.Generic;
using System.Text;

namespace EducationApp.BusinessLayer.Models
{
    public class ResponseModel<T>
    {
        public bool IsSucceeded { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
