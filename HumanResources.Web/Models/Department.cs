﻿using Microsoft.JSInterop.Infrastructure;

namespace HumanResources.Web.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Established { get; set; }


        
    }
}
