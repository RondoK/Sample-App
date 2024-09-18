namespace App.Data.Models;

public interface IHaveCreateInfo
{
    Guid CreatedBy { get; set; }
    DateTimeOffset CreatedAt { get; set; }
}

public interface IHaveUpdateInfo
{
    Guid UpdatedBy { get; set; }
    DateTimeOffset LastUpdatedAt { get; set; }
}

public interface IHaveId<T>
{
    T Id { get; set; }
}

public abstract class BaseEntity: IHaveId<int>, IHaveCreateInfo, IHaveUpdateInfo
{
    public int Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid UpdatedBy { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
}



public class Project : BaseEntity
{
    public string Title { get; set; }
    public ICollection<DoTask> DoTasks { get; set; }
    public bool Active { get; set; }
    
    public Guid RowId { get; set; }
}

public class DoTask : BaseEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public ICollection<DoTaskCheck> Checks { get; set; }
    
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}

/// <summary>
/// Like todo list inside of a task
/// </summary>
public class DoTaskCheck : BaseEntity
{
    public string Title { get; set; }
    public int CheckStatusId { get; set; }
    
    public int DoTaskId { get; set; }
    public DoTask DoTask { get; set; }
}

/// <summary>
/// Not an enum because I want to leave this as a deploy/run-time, not a build-time
/// </summary>
public class CheckStatus : BaseEntity
{
    public string Title { get; set; }
    
    public ICollection<DoTaskCheck> Checks { get; set; }
}

/// <summary>
/// Not an enum because I want to leave this as a deploy/run-time, not a build-time
/// </summary>
public class DoTaskStatus : IHaveId<int>
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public ICollection<DoTask> Tasks { get; set; }
}