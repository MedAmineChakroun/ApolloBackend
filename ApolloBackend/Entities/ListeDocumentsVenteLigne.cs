using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeDocumentsVenteLigne
{
    public short LigneDocType { get; set; }

    public string LigneDocPiece { get; set; } = null!;

    public DateTime? LigneDocDate { get; set; }

    public string? LigneDocReg { get; set; }

    public string? LigneArtCode { get; set; }

    public string? LigneArtDesi { get; set; }

    public decimal? LigneQte { get; set; }

    public decimal? LignePu { get; set; }

    public decimal? LigneHt { get; set; }

    public decimal? LigneTtc { get; set; }

    public int LigneId { get; set; }
}
