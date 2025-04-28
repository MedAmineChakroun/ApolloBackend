using System;
using System.Collections.Generic;

namespace ApolloBackend.Entities;

public partial class ListeStock
{
    public string ArRef { get; set; } = null!;

    public decimal? AsQteSto { get; set; }
}
