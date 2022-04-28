using Maxima.Net.SDK.Integracao.Api;
using Maxima.Net.SDK.Integracao.Dto.Pedido;

namespace Maxima.Net.SDK.Domain.Utils
{
    public class WorkPubSubMaxima
    {
        private readonly MaximaIntegracao apiMaxima;

        public WorkPubSubMaxima(MaximaIntegracao apiMaxima)
        {
            this.apiMaxima = apiMaxima;
        }

        public void IniciarOuvintes()
        {
            apiMaxima.OnIncluirPedido = IncluirPedidoMaxima;
            apiMaxima.OnStatusPedido = StatusPedidoErp;
        }

        public void IncluirPedidoMaxima(PedidoMaxima pedidoMaxima)
        {
            System.Console.WriteLine("Pedido Recebido");
            //Aqui vou receber o pedido que veio da maxima.
            //Converto o pedido máxima para o formato do ERP e envio.
        }

        public void StatusPedidoErp(string statusPedidoErp)
        {
            System.Console.WriteLine("Status do Pedido Recebido");
            //Aqui vou receber o status do pedido vindo do ERP.
        }
    }
}