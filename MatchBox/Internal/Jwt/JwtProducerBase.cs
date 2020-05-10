using MatchBox.Configuration;
using MatchBox.Data.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
    public abstract class JwtProducerBase : IJwtProducer
    {
        public JwtProducerBase(IOptions<MatchBoxConfiguration> settings)
           : base()
        {
            Configuration = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public MatchBoxConfiguration Configuration { get; }

        public abstract string Generate(DbUser user);
    }
}
