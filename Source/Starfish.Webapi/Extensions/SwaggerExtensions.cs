using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// Swagger扩展方法
/// </summary>
internal static class SwaggerExtensions
{
	private static readonly string _name;
	private static readonly string _description;

	static SwaggerExtensions()

	{
		var assembly = Assembly.GetEntryAssembly();

		if (assembly == null)
		{
			throw new NullReferenceException();
		}

		if (assembly.HasAttribute(out AssemblyTitleAttribute titleAttribute) && !string.IsNullOrEmpty(titleAttribute.Title))
		{
			_name = titleAttribute.Title;
		}
		else
		{
			_name = assembly.GetName().Name!.Split(".")[1];
		}

		_description = assembly.HasAttribute(out AssemblyDescriptionAttribute descriptionAttribute) ? descriptionAttribute.Description : $"Starfish {_name} Service";
	}

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

			        gen.SwaggerDoc(_name, new OpenApiInfo
			        {
				        Title = _name,
				        Version = "v1",
				        Description = _description,
				        License = new OpenApiLicense
				        {
					        Name = "© 2023 Nerosoft. All Rights Reserved."
				        }
			        });

			        foreach (var file in Directory.GetFiles(AppContext.BaseDirectory, "*.Summary.xml"))
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
			option.SwaggerEndpoint($"/swagger/{_name}/swagger.json", _name);
			//foreach (var (key, _) in ApiGroups)
			//{
			//    option.SwaggerEndpoint($"/swagger/{key}/swagger.json", key);
			//}
		});
	}
}