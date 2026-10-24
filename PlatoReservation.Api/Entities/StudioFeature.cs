namespace PlatoReservation.Api.Entities;

public class StudioFeature
{
    public int Id { get; set; }

    public int StudioId { get; set; }
    public string Title { get; set; } = string.Empty;

    // پلاتویی که این امکان به آن تعلق دارد
    public Studio Studio { get; set; } = null!;
}
