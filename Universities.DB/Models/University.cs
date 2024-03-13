using System.ComponentModel.DataAnnotations;

namespace Universities.DB.Models;

public sealed class University
{
    [Key]
    public int Id { get; set; }
    public string Country { get; set; }
    public string Name { get; set; }
    public string WebSites { get; set; }
}