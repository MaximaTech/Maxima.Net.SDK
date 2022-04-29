using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxima.Net.SDK.Data.Models
{
    /// <summary>
    /// Classe utilizada para guardar alguns parâmetros de configurações locais.
    /// ex: login e senha Máxima.
    /// </summary>
    public class ParametroModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}