using System.ComponentModel.DataAnnotations;

namespace MicroEndpoints.EndpointApp.Endpoints.Authors;

public class UpdateAuthorCommand
{
  [Required]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = null!;
}
