using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maxima.Net.SDK.Data.Models
{
    /// <summary>
    /// Classe utilizada para armazenar as entidades enviadas para Máxima.
    /// </summary>
    public class ControleDadosModel : IEntityTypeConfiguration<ControleDadosModel>
    {
        public int Id { get; set; }
        public string Tabela { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public void Configure(EntityTypeBuilder<ControleDadosModel> builder)
        {
            builder.HasKey(p => new { p.Id });
        }

        public override string ToString()
        {
            return $"{Tabela} - {Chave} - {Valor}";
        }
    }
}
