using Api.Tests.Fixtures;
using App.Data.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using SystemTextJsonPatch;

namespace Api.Tests.EndpointBased.Projects;

public static class ReadonlyTests
{
    public static async Task GetByIdAndCompare<T>(EndpointsGroup<T> server, T compareTo)
        where T : BaseEntity
    {
        var retrieved = await server.GetById(compareTo.Id);

        retrieved.Should().BeEquivalentTo(compareTo);
    }
}

public static class CustomAsserts
{
    public static async Task AddNew_ReturnsSameObjWithNewId<T, TId>(T fromClient, EndpointsGroup<T> server)
        where T : BaseEntity
    {
        var created = await server.Create(fromClient);

        created.AddNewAsserts<T, TId>(fromClient);
    }

    public static async Task AddNew_CanBeRetrievedById<T, TId>(T newElement, EndpointsGroup<T> server)
        where T : BaseEntity
    {
        var created = await server.Create(newElement);
        await ReadonlyTests.GetByIdAndCompare(server, created);
    }

    public static async Task AddNew_CanBeRetrievedInPaged<T>(T newElement,
        EndpointsGroup<T> server,
        int pageSize
    )
        where T : BaseEntity
    {
        var created = await server.Create(newElement);
        //TODO: revisit.
        // What if an aggregate has default filtering criteria
        // (like active or "belongs" to a specific user)
        var page = (created.Id / pageSize) + 1;
        var retrieved = await server.GetPaged(page, pageSize);
        retrieved.Should().NotBeEmpty();
        //TODO: recheck that it does comparison
        retrieved.Should().ContainEquivalentOf(created);
    }

    public static async Task AddNew_CanBeRetrievedInList<T>(T newElement, EndpointsGroup<T> server)
        where T : BaseEntity
    {
        {
            var created = await server.Create(newElement);
            var retrieved = await server.GetAll();

            retrieved.Should().ContainEquivalentOf(created);
        }
    }

    // Maybe should be a separate class with multiple select ?!? 
    // Creates a new value to make sure it is independent test suite
    public static async Task Update_FullUpdated<T>(T newElement, 
        Action<T> updateAction,
        EndpointsGroup<T> server)
        where T : BaseEntity
    {
        var toUpdate = await server.Create(newElement);
        updateAction(toUpdate);

        var updatedResponse = await server.Update(toUpdate);
        var loaded = await server.GetById(toUpdate.Id);

        updatedResponse.Should().BeEquivalentTo(toUpdate);
        loaded.Should().BeEquivalentTo(updatedResponse);
    }

    public static async Task Patch<T>(T newElement,
        EndpointsGroup<T> server,
        JsonPatchDocument<T> patchDoc,
        Action<T> checkPatchedProperties,
        Action<T,T> comparePatchedResponseAndCreated)
        where T : BaseEntity
    {
        var created = await server.Create(newElement);

        var patchedResponse = await server.Patch(created.Id, patchDoc);
        var loaded = await server.GetById(created.Id);

        checkPatchedProperties(patchedResponse);
        comparePatchedResponseAndCreated(patchedResponse, created);
        loaded.Should().BeEquivalentTo(patchedResponse);
    }

    private static async Task PatchId_Fail()
    {
        /*
        var toCreate = ValidNewAggRequest();
        var created = await _server.Create(toCreate);
        const int newId = int.MaxValue - 1;

        // Make sure that shared db doesn't have this entity
        var withNewId = await _server.GetById(newId);
        withNewId.Should().BeNull();

        var patch = new JsonPatchDocument<Agg>();
        patch.Replace(a => a.Id, newId);

        var patchedResponse = await _server.OnlyPatch(created.Id, patch);
        patchedResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        */
    }
}