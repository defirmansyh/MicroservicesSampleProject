using System;
using System.Collections.Generic;

namespace TiVi.UserCatalogService.DataAccess;

public partial class MovieGenreMap
{
    public int MovieGenreMapId { get; set; }

    public int MovieId { get; set; }

    public int GenreId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual CatalogMovie Movie { get; set; } = null!;
}
