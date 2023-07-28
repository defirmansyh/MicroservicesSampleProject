using System;
using System.Collections.Generic;

namespace TiVi.UserCatalogService.DataAccess;

public partial class CatalogMovie
{
    public int MovieId { get; set; }

    public string MovieName { get; set; } = null!;

    public short ReleaseYear { get; set; }

    public decimal? MoviePrice { get; set; }

    public bool IsAcitve { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<MovieGenreMap> MovieGenreMaps { get; set; } = new List<MovieGenreMap>();

    public virtual ICollection<MovieTrending> MovieTrendings { get; set; } = new List<MovieTrending>();

    public virtual ICollection<UserProfileMovie> UserProfileMovies { get; set; } = new List<UserProfileMovie>();
}
