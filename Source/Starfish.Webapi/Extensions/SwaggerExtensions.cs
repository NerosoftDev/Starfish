using Microsoft.OpenApi.Models;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// Swagger扩展方法
/// </summary>
internal static class SwaggerExtensions
{
	private static readonly Dictionary<string, string> _apiGroups = new()
	{
		["setting"] = "setting",
		["identity"] = "identity",
		["system"] = "system"
	};

	/// <summary>
	/// Adds the Swagger services.
	/// </summary>
	/// <param name="services"></param>
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer()
		        .AddSwaggerGen(gen =>
		        {
			        gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			        {
				        Name = "Authorization",
				        Type = SecuritySchemeType.Http,
				        Scheme = "bearer"
			        });

			        gen.AddSecurityRequirement(new OpenApiSecurityRequirement
			        {
				        {
					        new OpenApiSecurityScheme
					        {
						        Reference = new OpenApiReference
						        {
							        Type = ReferenceType.SecurityScheme,
							        Id = "Bearer"
						        }
					        },
					        new List<string>()
				        }
			        });

			        foreach (var (key, value) in _apiGroups)
			        {
				        gen.SwaggerDoc(key, new OpenApiInfo
				        {
					        Title = key,
					        Version = "v1",
					        Description = value,
					        License = new OpenApiLicense
					        {
						        Name = "© 2023 Nerosoft. All Rights Reserved."
					        }
				        });
			        }
			        
			        // gen.SwaggerDoc("v1", new OpenApiInfo
			        // {
				       //  Title = "Starfish Webapi",
				       //  Version = "v1",
				       //  Description = "Starfish Webapi",
				       //  License = new OpenApiLicense
				       //  {
					      //   Name = "© 2023 Nerosoft. All Rights Reserved."
				       //  }
			        // });

			        foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
			        {
				        gen.IncludeXmlComments(file);
			        }

			        gen.DocInclusionPredicate((doc, description) => description.GroupName == null || description.GroupName.Equals(doc, StringComparison.OrdinalIgnoreCase));
		        });
	}

	/// <summary>
	/// Adds the Swagger middleware.
	/// </summary>
	/// <param name="app"></param>
	public static void UseSwagger(this IApplicationBuilder app)
	{
		// ReSharper disable once UnusedLambdaParameter
		app.UseSwagger(_ =>
		{
			//option.SerializeAsV2 = true;
		});
		app.UseSwaggerUI(option =>
		{
			foreach (var (key, name) in _apiGroups)
			{
				option.SwaggerEndpoint($"/swagger/{key}/swagger.json", name);
			}
		});
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Starfish Webapi"));
	}
}