﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedoa.core
{
    public class Likes : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}