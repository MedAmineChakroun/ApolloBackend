using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeDocumentsVente
{
    public short? DocType { get; set; }

    public string? DocPiece { get; set; }

    public DateTime? DocDate { get; set; }

    public string? DocRef { get; set; }

    public decimal? DocTht { get; set; }

    public decimal? DocTtc { get; set; }

    public string? DocDepd { get; set; }

    public string? DocTiersCode { get; set; }

    public string? DocTiersIntitule { get; set; }

    public int DocId { get; set; }
}
