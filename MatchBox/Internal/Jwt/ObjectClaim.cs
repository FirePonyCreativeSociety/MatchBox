using MatchBox.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System.Security.Claims
{
    public class ObjectClaim : Claim
    {
        public ObjectClaim(string type, object value)
            : base(type, ConvertObjectToValue(value))
        {
        }

        static string ConvertObjectToValue(object value)
        {
            return value?.ToString() ?? "{}";
        }

        protected override byte[] CustomSerializationData
        {
            get
            { 
                var tmp = base.CustomSerializationData;
                return tmp;
            }
        }

        protected override void WriteTo(BinaryWriter writer, byte[] userData)
        {
            base.WriteTo(writer, userData);
        }
    }
}
