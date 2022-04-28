using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maxima.Net.SDK.Domain.Entidade;
using Maxima.Net.SDK.Integracao.Entidades;

namespace Maxima.Net.SDK.Domain.Mappings
{
    public partial class MapAll
    {
        public void CidadeMapping()
        {

            CreateMap<CidadeErp, CidadeMaxima>()
               .ForMember(dc => dc.NomeCidade, map => map.MapFrom(c => c.cNome))
               .ForMember(dc => dc.UF, map => map.MapFrom(c => c.cUF))
               .ForMember(dc => dc.CodigoCidadeIBGE, map => map.MapFrom(c => c.nCodIBGE))
               .ForMember(dc => dc.CodigoCidade, map => map.MapFrom(c => c.cCod))
               .AfterMap((omie, maxima) =>
               {
                   maxima.Hash = Utils.UtilsApi.GerarHashMD5(maxima);
               });
        }
    }
}