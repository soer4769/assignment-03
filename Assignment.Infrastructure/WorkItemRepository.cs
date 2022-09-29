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
        return ReadByState(State.Removed);
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag)
    {
        return (from t in Read() where t.Tags.Contains(tag) select t).ToList();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        return (from t in Read() where t.Id == userId select t).ToList();
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(State state)
    {
        return (from t in Read() where t.State == state select t).ToList();
    }
    
    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        var tasks = from t in _context.Tasks
            select new WorkItemDTO(t.Id, t.Title, t.AssignedTo.Name,t.Tags.Select(x => x.Name).ToList(), t.State);

        return tasks.ToList();
    }

    public Response Update(WorkItemUpdateDTO workitem)
    {
        var updateQuery = _context.Tasks.FirstOrDefault(u => u.Id == workitem.Id);
        
        updateQuery!.State = State.Active;
        updateQuery.Title = "what";

        _context.Tasks.Update(updateQuery);
        
        return Response.Updated;
    }

    public Response Delete(int workItemId)
    {
        var workitem = _context.Users.FirstOrDefault(u => u.Id == workItemId);
        
        if(workitem == null)
        {
            return Response.NotFound;
        }
        
        _context.Users.Remove(workitem);
        
        return Response.Deleted;
    }

    public WorkItemDetailsDTO Find(int workItemId)
    {
        var workitem = from t in _context.Tasks where t.Id == workItemId 
        select new WorkItemDetailsDTO(t.Id, t.Title, t.Description, t.created, t.AssignedTo.Name, t.Tags.Select(x => x.Name).ToList(), t.State, t.StateUpdated);
        return workitem.First();
    }
}
