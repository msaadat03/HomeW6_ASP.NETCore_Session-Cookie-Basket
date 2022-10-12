﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneTwoMany_Fiorello.Models
{
    public class Blog : BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
