namespace Nerosoft.Starfish.Webapi;

/// <summary>
/// 
/// </summary>
public class Startup
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="configuration"></param>
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	/// <summary>
	/// Gets the configuration instance.
	/// </summary>
	public IConfiguration Configuration { get; }

	/// <summary>
	/// Configure application services.
	/// </summary>
	/// <param name="services"></param>
	/// <remarks>
	/// This method gets called by the runtime. Use this method to add services to the container.
	/// </remarks>
	public void ConfigureServices(IServiceCollection services)
	{
		/*
        services.AddEntityFrameworkRepository<DataContext>(options =>
        {
            //options.UseNpgsql("Host=localhost;Database=euonia_sample;Username=postgres;Password=nerosoft.8888");
            //options.UseNpgsql("postgres://postgres:nerosoft.8888@localhost:5432/euonia_sample");
            options.UseInMemoryDatabase("Euonia.Sample");
        }); //PageActionEndpointConventionBuilder{ })

        */

		//services.AddModularityApplication<HostModuleContext>(Configuration);
		services.AddControllers();
		services.AddEndpointsApiExplorer()
				.AddSwaggerGen();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="app"></param>
	/// <param name="env"></param>
	/// <remarks>
	/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	/// </remarks>
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Starfish Webapi v1"));
		}

		app.UseWebSockets();

		app.InitializeApplication();

		app.UseHttpsRedirection();

		app.UseAuthentication();

		app.UseRouting();

		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
			//endpoints.MapGrpcServices();
			endpoints.MapHealthChecks("health");
			if (env.IsDevelopment())
			{
				//endpoints.MapGrpcReflectionService();
			}
		});
	}
}
