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
		services.AddModularityApplication<HostServiceModule>(Configuration);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="app"></param>
	/// <param name="env"></param>
	/// <param name="lifetime"></param>
	/// <remarks>
	/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	/// </remarks>
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			
		}

		lifetime.ApplicationStarted.Register(() => OnStarted(app));
		lifetime.ApplicationStopping.Register(() => OnStopping(app));
		lifetime.ApplicationStopped.Register(() => OnStarted(app));

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

	private void OnStarted(IApplicationBuilder app)
	{
		//"On-started" logic
	}

	private void OnStopping(IApplicationBuilder app)
	{
		//"On-stopping" logic
	}

	private void OnStopped(IApplicationBuilder app)
	{
		//"On-stopped" logic
	}
}