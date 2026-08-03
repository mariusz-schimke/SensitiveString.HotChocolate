using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Execution;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using TextPrivacy.SensitiveString;
using TextPrivacy.SensitiveString.HotChocolate;
using Xunit;

namespace TextPrivacy.SensitiveString.HotChocolate.Tests;

public class PagingIntegrationTests
{
    private static async Task<IRequestExecutor> BuildExecutorAsync() =>
        await new ServiceCollection()
            .AddGraphQL()
            .AddSensitiveStringSupport()
            .AddFiltering(c => c.AddDefaults().AddSensitiveStringSupport())
            .AddSorting(c => c.AddDefaults().AddSensitiveStringSupport())
            .AddQueryType<Query>()
            .BuildRequestExecutorAsync();

    [Fact]
    public async Task Schema_BuildsWithSensitiveStringSupport()
    {
        var executor = await BuildExecutorAsync();

        Assert.NotNull(executor.Schema);
    }

    // Cursor paging while sorting by a SensitiveString field forces HotChocolate to encode
    // the sort key into the cursor via the registered ICursorKeySerializer.
    [Fact]
    public async Task Paging_SortedBySensitiveStringKey_ProducesCursorsWithoutErrors()
    {
        var executor = await BuildExecutorAsync();

        var result = await executor.ExecuteAsync(
            """
            {
              people(first: 1, order: { name: ASC }) {
                nodes { id }
                pageInfo { endCursor hasNextPage }
              }
            }
            """);

        var operationResult = result.ExpectOperationResult();

        AssertNoErrors(operationResult.Errors);
        AssertNonNullEndCursor(result);
    }

    [Fact]
    public async Task Paging_SortedBySensitiveEmailKey_ProducesCursorsWithoutErrors()
    {
        var executor = await BuildExecutorAsync();

        var result = await executor.ExecuteAsync(
            """
            {
              people(first: 1, order: { email: ASC }) {
                nodes { id }
                pageInfo { endCursor hasNextPage }
              }
            }
            """);

        var operationResult = result.ExpectOperationResult();

        AssertNoErrors(operationResult.Errors);
        AssertNonNullEndCursor(result);
    }

    private static void AssertNoErrors(IReadOnlyList<IError>? errors) =>
        Assert.True(
            errors is null || errors.Count == 0,
            $"Expected no GraphQL errors but got: {(errors is null ? "<null>" : string.Join("; ", errors.Select(e => e.Message)))}");

    // A non-null endCursor proves HotChocolate encoded the SensitiveString sort key into a
    // cursor via the registered ICursorKeySerializer — the exact path that failed on HC15.
    private static void AssertNonNullEndCursor(IExecutionResult result)
    {
        var json = result.ToJson();
        Assert.Contains("\"endCursor\"", json);
        Assert.DoesNotMatch("\"endCursor\"\\s*:\\s*null", json);
    }

    public class Query
    {
        [UsePaging]
        [UseSorting]
        public IQueryable<Person> GetPeople() =>
            new[]
            {
                new Person { Id = 1, Name = "Charlie".AsSensitive(), Email = "charlie@example.com".AsSensitiveEmail() },
                new Person { Id = 2, Name = "Ada".AsSensitive(), Email = "ada@example.com".AsSensitiveEmail() },
                new Person { Id = 3, Name = "Bob".AsSensitive(), Email = "bob@example.com".AsSensitiveEmail() },
            }.AsQueryable();
    }

    public class Person
    {
        public int Id { get; set; }
        public SensitiveString Name { get; set; } = string.Empty.AsSensitive();
        public SensitiveEmail Email { get; set; } = "x@example.com".AsSensitiveEmail();
    }
}
