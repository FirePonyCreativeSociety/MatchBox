using MatchBox.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.API.Models
{
    public class Event : IIntId
    {
        public int EventId { get; set; }

        int IIntId.GetId() => this.EventId;
    }
}
