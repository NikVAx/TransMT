namespace TrackMS.WebAPI.DTO.API;

public class MultiplyDeletionRequestDto<TKey>
{
    public IEnumerable<TKey> Keys { get; set; }
}
