namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
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
            select new WorkItemDTO(t.Id, t.Title, t.AssignedTo.Name,t.Tags.Select(x => x.Name).ToList(), t.State);

        return tasks.ToList();
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
        var workitem = from t in _context.Tasks where t.Id == workItemId 
        select new WorkItemDetailsDTO(t.Id, t.Title, t.Description, t.created, t.AssignedTo.Name, t.Tags.Select(x => x.Name).ToList(), t.State, t.StateUpdated);
        return workitem.First();
    }
}
