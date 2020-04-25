using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Model
{
    public abstract class DbEntityBase
    {
        public long Id { get; set; }
    }
}
