using MatchBox.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
    public interface IJwtProducer
    {
        string Generate(DbUser user);
    }
}
