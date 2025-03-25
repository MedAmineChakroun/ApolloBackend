using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeFamille
{
    public string FamCode { get; set; } = null!;

    public string? FamIntitule { get; set; }

    public int FamId { get; set; }
}
