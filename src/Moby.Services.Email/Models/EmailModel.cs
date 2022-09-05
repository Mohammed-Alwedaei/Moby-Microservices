namespace Moby.Services.Email.API.Models;

public class EmailModel
{
    public int Id { get; set; }

    public string? Email { get; set; }
    public string? Log { get; set; }

    public DateTime SendDate { get; set; } = DateTime.Now;
}
