using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeClient
{
    public int TiersId { get; set; }

    public string TiersCode { get; set; } = null!;

    public string? TiersIntitule { get; set; }

    public string? TiersContact { get; set; }

    public string? TiersAdresse1 { get; set; }

    public string? TiersAdresse2 { get; set; }

    public string? TiersCodePostal { get; set; }

    public string? TiersVille { get; set; }

    public string? TiersPays { get; set; }

    public string? TiersMf { get; set; }

    public string? TiersColnom { get; set; }

    public string? TiersColprenom { get; set; }

    public string? TiersTel1 { get; set; }

    public string? TiersTel2 { get; set; }

    public string? TiersMails { get; set; }
}
