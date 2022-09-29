namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    private static IEnumerable<string> TagToString(IReadOnlyCollection<Tag> tags)
    {
        foreach (var tag in tags)
        {
            yield return tag.Name;
        }
    }
    
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        Response response;
        var entity = new Task();
        var assignedUser = _context.Users.FirstOrDefault(u => u.Id == task.AssignedToId);
        IReadOnlyCollection<Tag> tags = new List<Tag>();
        
        entity.Title = task.Title;
        entity.Description = task.Description;
        entity.State = State.New;
        entity.AssignedTo = assignedUser;
        entity.Tags = tags;

        _context.Tasks.Add(entity);
        _context.SaveChanges();

        response = Response.Created;


        return (response, entity.Id);
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tasks = from t in _context.Tasks
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, TagToString(t.Tags).ToList(), t.State);

        return tasks.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        throw new NotImplementedException();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }
}
