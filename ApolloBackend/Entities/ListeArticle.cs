using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeArticle
{
    public int ArtId { get; set; }

    public string ArtCode { get; set; } = null!;

    public string? ArtIntitule { get; set; }

    public string? ArtFamille { get; set; }

    public decimal? ArtPrixVente { get; set; }

    public decimal? ArtPrixAchat { get; set; }

    public string? ArtBarcode { get; set; }

    public short? ArtEtat { get; set; }

    public string? ArtUnite { get; set; }

    public string? ArtStat1 { get; set; }

    public string? ArtStat2 { get; set; }

    public string? ArtStat3 { get; set; }

    public string? ArtStat4 { get; set; }

    public string? ArtStat5 { get; set; }

    public short? ArtSuiviStock { get; set; }

    public string? ArtCatalogue1 { get; set; }

    public string? ArtCatalogue2 { get; set; }

    public string? ArtCatalogue3 { get; set; }

    public string? ArtCatalogue4 { get; set; }
}
