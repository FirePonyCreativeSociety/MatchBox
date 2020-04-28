using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
	internal class AuthenticationRequirementsOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Security == null)
				operation.Security = new List<OpenApiSecurityRequirement>();

			//Branch only?
			var scheme = new OpenApiSecurityScheme 
			{ 
				Reference = new OpenApiReference 
				{ 
					Type = ReferenceType.SecurityScheme, 
					Id = JwtBearerDefaults.AuthenticationScheme
				} 
			};

			operation.Security.Add(new OpenApiSecurityRequirement
			{
				[scheme] = new List<string>()
			});
		}
	}
}
