using Immediate.Handlers.Analyzers;
using Immediate.Handlers.Tests.Helpers;

namespace Immediate.Handlers.Tests.AnalyzerTests.BehaviorAnalyzerTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "Not being consumed by other code")]
public partial class Tests
{
	[Fact]
	public async Task ConcreteBehaviorDoesNotInheritFromBoundedBehavior_Alerts() =>
		await AnalyzerTestHelpers.CreateAnalyzerTest<BehaviorsAnalyzer>(
			"""
			using System;
			using System.Collections.Generic;
			using System.IO;
			using System.Linq;
			using System.Net.Http;
			using System.Threading;
			using System.Threading.Tasks;
			using Immediate.Handlers.Shared;
			using Normal;

			[assembly: Behaviors(
				{|IHR0008:typeof(B1)|}
			)]

			namespace Normal;

			public class User { };
			public interface ILogger<T>;

			public class B1 : Behavior<int, int>
			{
				public override Task<int> HandleAsync(int request, CancellationToken cancellationToken)
				{
					throw new NotImplementedException();
				}
			}

			public class UsersService(ILogger<UsersService> logger)
			{
				public Task<IEnumerable<User>> GetUsers()
				{
					_ = logger.ToString();
					return Task.FromResult(Enumerable.Empty<User>());
				}
			}

			[Handler]
			[Behaviors(
				{|IHR0008:typeof(B1)|}
			)]
			public static partial class GetUsersQuery
			{
				public record Query;

				private static Task<IEnumerable<User>> HandleAsync(
					Query _,
					UsersService usersService,
					CancellationToken token)
				{
					token.ThrowIfCancellationRequested();
					return usersService.GetUsers();
				}
			}
			""",
			DriverReferenceAssemblies.Normal,
			[]
		).RunAsync();
}
