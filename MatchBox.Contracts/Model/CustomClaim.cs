﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Contracts.Model
{
    public class CustomClaim : EntityBase
    {
        public User User { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
