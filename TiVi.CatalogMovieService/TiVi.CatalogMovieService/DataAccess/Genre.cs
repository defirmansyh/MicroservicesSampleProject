using System;
using System.Collections.Generic;

namespace TiVi.UserCatalogService.DataAccess;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreDescription { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<MovieGenreMap> MovieGenreMaps { get; set; } = new List<MovieGenreMap>();
}
