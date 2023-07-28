using System;
using System.Collections.Generic;

namespace TiVi.UserCatalogService.DataAccess;

public partial class UserProfileMovie
{
    public int UserProfileMovieId { get; set; }

    public int UserAccountId { get; set; }

    public int MovieId { get; set; }

    public short MovieRating { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual CatalogMovie Movie { get; set; } = null!;

    public virtual UserAccount UserAccount { get; set; } = null!;
}
