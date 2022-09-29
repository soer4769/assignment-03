namespace Assignment.Core;

public interface ITagRepository
{
    (Response Response, int TagId) Create(TagCreateDTO tag);
    TagDTO Find(int tagId);
    IReadOnlyCollection<TagDTO> Read();
    Response Update(TagUpdateDTO tag);
    Response Delete(int tagId, bool force = false);
}