﻿//HintName: Immediate.Handlers.ServiceCollectionExtensions.g.cs
namespace Microsoft.Extensions.DependencyInjection;

public static class HandlerServiceCollectionExtensions
{
	public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddHandlers(
		this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)
	{
		services.AddScoped(typeof(global::Dummy.LoggingBehavior<,>));
		global::Dummy.GetUsersQuery.AddHandlers(services);
		
		return services;
	}
}