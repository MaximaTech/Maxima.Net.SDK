using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Maxima.Net.SDK.Data;
using Maxima.Net.SDK.Data.Models;
using Maxima.Net.SDK.Domain.Entidade;
using Maxima.Net.SDK.Domain.Enums;
using Maxima.Net.SDK.Domain.Interfaces;
using Maxima.Net.SDK.Domain.Utils;
using Maxima.Net.SDK.Integracao.Api;
using Maxima.Net.SDK.Integracao.Dto;
using Maxima.Net.SDK.Integracao.Entidades;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Maxima.Net.SDK.Domain.Service
{
    public class CidadeApiErp : ICidadeApiErp
    {

        private readonly ErpContext dbContext;
        private readonly IMapper mapper;
        private readonly MaximaIntegracao apiMaxima;

        public CidadeApiErp(MaximaIntegracao apiMaxima, IMapper mapper, ErpContext dbContext)
        {
            this.apiMaxima = apiMaxima;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task EnviarCidades(CancellationToken token)
        {
            LogApi log = new("Cidades");

            try
            {
                var cidadesBd = await dbContext.ControleDadosModels.Where(c => c.Tabela == ControleDadosEnum.CIDADES).AsNoTracking().ToListAsync();
                var processados = new List<string>();
                var listaIncluir = new List<CidadeMaxima>();
                var listaAlterar = new List<CidadeMaxima>();
                var cidadesErp = JsonConvert.DeserializeObject<List<CidadeErp>>(File.ReadAllText("./Data/Payloads/cidades_erp.json"));

                foreach (var cidades in cidadesErp)
                {
                    var mapCidades = mapper.Map<CidadeMaxima>(cidades);

                    if (cidadesBd.Any(x => x.Chave == mapCidades.CodigoCidade && x.Valor != mapCidades.Hash))
                    {
                        listaAlterar.Add(mapCidades);
                    }
                    else if (!cidadesBd.Any(x => x.Chave == mapCidades.CodigoCidade))
                    {
                        listaIncluir.Add(mapCidades);
                    }
                }

                if (listaIncluir.Any())
                {
                    ResponseApiMaxima<CidadeMaxima> retornoApiMaxima = await apiMaxima.IncluirCidade(listaIncluir);
                    if (retornoApiMaxima.Sucesso)
                    {
                        processados.AddRange(retornoApiMaxima.ItensInserido.Select(x => x.CodigoCidade).ToList());

                        foreach (var item in retornoApiMaxima.ItensInserido)
                        {
                            ControleDadosModel cidadeModel = new()
                            {
                                Tabela = ControleDadosEnum.CIDADES,
                                Chave = item.CodigoCidade,
                                Valor = item.Hash
                            };
                            dbContext.ControleDadosModels.Add(cidadeModel);
                        }
                        await dbContext.SaveChangesAsync();

                        log.InserirOk(1, 1, retornoApiMaxima.ItensInserido.Count());

                        if (retornoApiMaxima.ErrosValidacao.Any())
                            log.InserirErro(1, 1, retornoApiMaxima.TotalItensNaoInserido, retornoApiMaxima.ErrosValidacaoFormatado);
                    }
                    else
                    {
                        log.InserirErro(1, 1, listaIncluir.Count, retornoApiMaxima.ErrosValidacaoFormatado);
                    }
                }

                if (listaAlterar.Any())
                {
                    ResponseApiMaxima<CidadeMaxima> retornoApiMaxima = await apiMaxima.AlterarCidade(listaIncluir);
                    if (retornoApiMaxima.Sucesso)
                    {
                        processados.AddRange(retornoApiMaxima.ItensInserido.Select(x => x.CodigoCidade).ToList());

                        foreach (var item in retornoApiMaxima.ItensInserido)
                        {
                            var cidadeBd = cidadesBd.Where(c => c.Chave == item.CodigoCidade).FirstOrDefault();
                            dbContext.Update(cidadeBd);
                        }
                        await dbContext.SaveChangesAsync();

                        log.InserirOk(1, 1, retornoApiMaxima.ItensInserido.Count());

                        if (retornoApiMaxima.ErrosValidacao.Any())
                            log.InserirErro(1, 1, retornoApiMaxima.TotalItensNaoInserido, retornoApiMaxima.ErrosValidacaoFormatado);
                    }
                    else
                    {
                        log.InserirErro(1, 1, listaIncluir.Count, retornoApiMaxima.ErrosValidacaoFormatado);
                    }
                }

                if (!listaAlterar.Any() && !listaIncluir.Any())
                    log.NenhumRegistroAlterado(1, 1);



                var cidadeList = await dbContext.ControleDadosModels.Where(c => c.Tabela == ControleDadosEnum.CIDADES)
                    .AsNoTracking()
                    .Select(f => f.Chave)
                    .ToListAsync();

                var excluidos = processados
                    .Except(cidadeList)
                    .ToList();

                var cidadeRemove = await dbContext.ControleDadosModels.Where(c => c.Tabela == ControleDadosEnum.CIDADES)
                    .AsNoTracking()
                    .Where(f => excluidos.Contains(f.Chave))
                    .ToListAsync();

                if (excluidos.Any())
                {
                    var listaExclusao = cidadeRemove.Select(x => x.Valor);
                    RetornoApiMaxima retornoApiMaxima = await apiMaxima.DeletarCidades(listaExclusao.ToArray());
                    if (retornoApiMaxima.Sucesso)
                    {
                        dbContext.ControleDadosModels.RemoveRange(cidadeRemove);
                        await dbContext.SaveChangesAsync();

                        log.ExcluirOk(listaExclusao.Count());
                    }
                    else
                    {
                        dbContext.ChangeTracker.Clear();
                        log.ExcluirErro(listaExclusao.Count(), retornoApiMaxima.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                log.GlobalError(ex.Message);
                throw;
            }
        }

    }
}