namespace Assignment.Core;

public interface IWorkItemRepository
{
    (Response Response, int WorkItemId) Create(WorkItemCreateDTO workItem);
    WorkItemDetailsDTO Find(int workItemId);
    IReadOnlyCollection<WorkItemDTO> Read();
    IReadOnlyCollection<WorkItemDTO> ReadRemoved();
    IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag);
    IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId);
    IReadOnlyCollection<WorkItemDTO> ReadByState(State state);
    Response Update(WorkItemUpdateDTO workItem);
    Response Delete(int workItemId);
}