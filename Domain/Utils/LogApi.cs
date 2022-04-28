using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maxima.Net.SDK.Domain.Utils
{
    public class LogApi
    {
        public string LocalLog { get; set; }

        public LogApi(string localLog)
        {
            LocalLog = localLog;
            Console.WriteLine("\nIniciando integracao " + LocalLog + "...");
        }

        public void InserirOk(long pagina = 0, long totalPaginas = 0, long qtdRegistros = 0)
        {
            Console.WriteLine("[OK] Incluir " + LocalLog + ": pagina " + pagina + "/" + totalPaginas
                + " processada com sucesso" + " (" + qtdRegistros + " registros incluidos)");
        }

        public void NenhumRegistroAlterado(long pagina = 0, long totalPaginas = 0)
        {
            Console.WriteLine("[OK] " + LocalLog + ": pagina " + pagina + "/" + totalPaginas
                + " nenhum registro precisou ser inserido ou alterado");
        }

        public void InserirPedidoOk(string numPedido)
        {
            Console.WriteLine("[OK] Incluir " + LocalLog + " NumPedido: " + numPedido + " inserido com sucesso");
        }
        public void InserirPedidoErro(string numPedido, object erros)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERRO] Incluir " + LocalLog + " NumPedido: " + numPedido + " NAO inserido ");
            Console.WriteLine(erros);
            Console.ResetColor();
        }
        public void InserirErro(long pagina, long totalPaginas, long qtdRegistros, object error)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERRO] Incluir " + LocalLog + ": pagina " + pagina + "/" + totalPaginas
                + " NAO processada" + " (" + qtdRegistros + " registros NAO incluidos)");
            Console.WriteLine(error);
            Console.ResetColor();
        }

        public void AlterarOk(long pagina, long totalPaginas, long qtdRegistros)
        {
            Console.WriteLine("[OK] Alterar " + LocalLog + ": pagina " + pagina + "/" + totalPaginas
                + " processada com sucesso" + " (" + qtdRegistros + " registros incluidos)");
        }

        public void AlterarErro(long pagina, long totalPaginas, long qtdRegistros, object error)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERRO] Alterar " + LocalLog + ": pagina " + pagina + "/" + totalPaginas
                + " NAO processada" + " (" + qtdRegistros + " registros NAO incluidos)");
            Console.WriteLine(error);
            Console.ResetColor();
        }

        public void ExcluirOk(long qtdRegistros)
        {
            Console.WriteLine("[OK] Excluir " + LocalLog + ": " + qtdRegistros + " registros excluidos com sucesso");
        }

        public void ExcluirErro(long qtdRegistros, object error)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERRO] Excluir " + LocalLog + ": " + qtdRegistros + " registros NAO excluidos");
            Console.WriteLine(error);
            Console.ResetColor();
        }

        public void GlobalError(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[ERRO] " + LocalLog + ":\n" + mensagem);
            Console.ResetColor();
        }
    }
}