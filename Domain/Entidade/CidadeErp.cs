using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxima.Net.SDK.Domain.Entidade
{
    /// <summary>
    /// Exemplo de uma entidade cidade de um ERP.
    /// </summary>
    public class CidadeErp
    {
        public string cCod { get; set; }
        public string cNome { get; set; }
        public string cUF { get; set; }
        public string nCodIBGE { get; set; }
        public int nCodSIAFI { get; set; }
    }
}