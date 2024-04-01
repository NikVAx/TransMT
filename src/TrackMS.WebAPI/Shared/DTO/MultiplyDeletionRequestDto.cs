namespace TrackMS.WebAPI.Shared.DTO;

public class MultiplyDeletionRequestDto<TKey>
{
    public IEnumerable<TKey> Keys { get; set; }
}
