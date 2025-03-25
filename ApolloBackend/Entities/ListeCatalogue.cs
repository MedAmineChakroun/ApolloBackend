using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeCatalogue
{
    public int? CatalogueId { get; set; }

    public string? CatalogueCode { get; set; }

    public string CatalogueIntitule { get; set; } = null!;

    public short? CatalogueNiveau { get; set; }

    public int? CatalogueParentId { get; set; }

    public string? CatalogueParentCode { get; set; }

    public string? CatalogueParentIntitule { get; set; }
}
