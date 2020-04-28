using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MatchBox.Data.Models
{
    public abstract class DbEntityBase : ILongId
    {
        public long Id { get; set; }
    }
}
