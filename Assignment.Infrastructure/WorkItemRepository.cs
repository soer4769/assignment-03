namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
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
    
    public (Response Response, int WorkItemId) Create(WorkItemCreateDTO workItem)
    {
        Response response;
        var entity = new WorkItem();
        var assignedUser = _context.Users.FirstOrDefault(u => u.Id == workItem.AssignedToId);
        IReadOnlyCollection<Tag> tags = new List<Tag>();
        
        entity.Title = workItem.Title;
        entity.Description = workItem.Description;
        entity.State = State.New;
        entity.AssignedTo = assignedUser;
        entity.Tags = tags;

        _context.Tasks.Add(entity);
        _context.SaveChanges();

        response = Response.Created;


        return (response, entity.Id);
    }

    public IReadOnlyCollection<WorkItemDTO> ReadRemoved()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(Core.State state)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        var tasks = from t in _context.Tasks
            select new WorkItemDTO(t.Id, t.Title, t.AssignedTo.Name, TagToString(t.Tags).ToList(), t.State);

        return tasks.ToList().AsReadOnly();
    }

    public Response Update(WorkItemUpdateDTO task)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }

    public WorkItemDetailsDTO Find(int workItemId)
    {
        throw new NotFiniteNumberException();
    }
}
