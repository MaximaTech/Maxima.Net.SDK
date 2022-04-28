using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;

namespace Maxima.Net.SDK.Domain.Interfaces
{
    public interface ICidadeApiErp
    {
        [JobDisplayName("Atualizar Cidades")]
        Task EnviarCidades(CancellationToken token);
    }
}