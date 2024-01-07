﻿namespace Immediate.Handlers.Tests.Tests;

[UsesVerify]
public class SingleBehaviorTest
{
	private readonly string _input = """
using System.Threading.Tasks;
using Dummy;
using Immediate.Handlers.Shared;

[assembly: RenderMode(renderMode: RenderMode.Normal)]

[assembly: Behaviors(
	typeof(LoggingBehavior<,>)
)]

namespace Dummy;

public class GetUsersEndpoint(GetUsersQuery.Handler handler)
{
	public async Task<IEnumerable<User>> GetUsers() =>
		handler.HandleAsync(new GetUsersQuery.Query());
}

[Handler]
public static class GetUsersQuery
{
	public record Query;

	private static Task<IEnumerable<User>> HandleAsync(
		Query _,
		UsersService usersService,
		CancellationToken token)
	{
		return usersService.GetUsers();
	}
}

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
	: Behavior<TRequest, TResponse>
{
	public override async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
	{
		var response = await InnerHandler.HandleAsync(request, cancellationToken);

		return response;
	}
}

public class User { }
public class UsersService
{
	public Task<IEnumerable<User>> GetUsers() =>
		Task.FromResult(Enumerable.Empty<User>());
}

public interface ILogger<T>;
""";

	[Theory]
	[InlineData(DriverReferenceAssemblies.Normal)]
	[InlineData(DriverReferenceAssemblies.Msdi)]
	public async Task SingleBehavior(DriverReferenceAssemblies assemblies)
	{
		var driver = TestHelper.GetDriver(_input, assemblies);

		var runResult = driver.GetRunResult();
		_ = await Verify(runResult)
			.UseParameters(assemblies.ToString());
	}
}
