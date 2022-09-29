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

        if(assignedUser == null && workItem.AssignedToId != null) return (Response.BadRequest, 0);

        IReadOnlyCollection<Tag> tags = new List<Tag>();
        
        entity.Title = workItem.Title;
        entity.Description = workItem.Description;
        entity.State = State.New;
        entity.AssignedTo = assignedUser;
        entity.Tags = tags;

        var taskExists = _context.WorkItems.FirstOrDefault(t => t.Id == entity.Id) != null;
        
        if(taskExists)
        {
            return (Response.Conflict, 0);
        }

        entity.Created = DateTime.UtcNow;
        entity.StateUpdated = DateTime.UtcNow;

        _context.WorkItems.Add(entity);
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
        var tagQuery = from t in Read()
            where t.Tags.Contains(tag)
            select t ;

        return tagQuery.Any() ? tagQuery.ToList() : null!;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId)
    {
        var userQuery = from t in Read() where t.Id == userId select t;
        
        return userQuery.Any() ? userQuery.ToList() : null!;
    }

    public IReadOnlyCollection<WorkItemDTO> ReadByState(State state)
    {
        var stateQuery = from t in Read() where t.State == state select t;
        
        return stateQuery.Any() ? stateQuery.ToList() : null!;
    }
    
    public IReadOnlyCollection<WorkItemDTO> Read()
    {
        if(!_context.WorkItems.Any())
        {
            return null!;
        }


        var tasks = from t in _context.WorkItems
            select new WorkItemDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(x => x.Name).ToList(), t.State);

        return tasks.ToList();
    }

    public Response Update(WorkItemUpdateDTO workitem)
    {
        
        var entity = _context.WorkItems.Find(workitem.Id);
        
        if(entity == null) return Response.NotFound;
        
        var assignedUser = _context.Users.FirstOrDefault(u => u.Id == workitem.AssignedToId);

        if(assignedUser == null) return Response.BadRequest;

        entity.Title = workitem.Title;
        entity.Description = workitem.Description;
        entity.AssignedTo = assignedUser;
        entity.Tags = workitem.Tags.Select(x => new Tag {Name = x})
            .ToList();
       
        if(entity.State != workitem.State) entity.StateUpdated = DateTime.UtcNow;
        
        entity.State = workitem.State;
        
        _context.SaveChanges();
        
        return Response.Updated;
    }

    public Response Delete(int workItemId)
    {
        var workitem = _context.WorkItems.FirstOrDefault(u => u.Id == workItemId);
        
        if(workitem == null)
        {
            return Response.NotFound;
        }
        
        if(workitem.State == State.New) _context.WorkItems.Remove(workitem);
        else if (workitem.State == State.Active) {
            workitem.State = State.Removed;
            workitem.StateUpdated = DateTime.UtcNow;
        }
        else return Response.Conflict;
        
        return Response.Deleted;
    }

    public WorkItemDetailsDTO? Find(int workItemId)
    {
        var taskNotExists = _context.WorkItems.FirstOrDefault(t => t.Id == workItemId) == null;

        if (taskNotExists)
        {
            return null;
        }
        
        var workitem = from t in _context.WorkItems where t.Id == workItemId 
        select new WorkItemDetailsDTO(t.Id, t.Title, t.Description, t.Created, t.AssignedTo.Name, t.Tags.Select(x => x.Name).ToList(), t.State, t.StateUpdated);
        return workitem.First();
    }
}
