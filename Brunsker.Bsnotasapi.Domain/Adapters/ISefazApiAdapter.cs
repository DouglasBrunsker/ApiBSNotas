﻿using System.Threading.Tasks;
using Brunsker.Bsnotasapi.Domain.Models;

namespace Brunsker.Bsnotas.Domain.Adapters
{
    public interface ISefazApiAdapter
    {
        Task ManifestaNotas(Manifestacao manifestacao, string webRootPath);
    }
}
