using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Maxima.Net.SDK.Domain.Mappings
{
    public partial class MapAll : Profile
    {
        public MapAll()
        {
            CidadeMapping();
        }
    }
}