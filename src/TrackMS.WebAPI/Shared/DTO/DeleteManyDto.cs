namespace TrackMS.WebAPI.Shared.DTO;

public class DeleteManyDto<TKey>
{
    public IEnumerable<TKey> Keys { get; set; }
}
