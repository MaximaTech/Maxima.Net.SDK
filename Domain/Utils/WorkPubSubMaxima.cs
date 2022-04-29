using Maxima.Net.SDK.Integracao.Api;
using Maxima.Net.SDK.Integracao.Dto.Pedido;

namespace Maxima.Net.SDK.Domain.Utils
{
    /// <summary>
    /// Essa clase tem como objetivo fazer a ponte entre o SDK Máxima e o seu projeto.
    /// Quando ocorrer um evento enviado pela Máxima a função que foi anexada sera chamada imediatamente.
    /// </summary>
    public class WorkPubSubMaxima
    {
        private readonly MaximaIntegracao apiMaxima;

        public WorkPubSubMaxima(MaximaIntegracao apiMaxima)
        {
            this.apiMaxima = apiMaxima;
        }

        /// <summary>
        /// Metodo responsável por ligar os ouvintes dessa classe com a api da Máxima.
        /// Ativando os ouvintes você sera notificando quando acontecer um evento, ex: quando chegar um pedido novo no banco da Máxima. 
        /// </summary>
        public void IniciarOuvintes()
        {
            apiMaxima.OnIncluirPedido = IncluirPedidoMaxima;
            apiMaxima.OnStatusPedido = StatusPedidoErp;
        }

        public void IncluirPedidoMaxima(PedidoMaxima pedidoMaxima)
        {
            System.Console.WriteLine("Pedido Recebido");
            //Aqui vou receber o pedido que veio da Máxima.
            //Converto o pedido Máxima para o formato do ERP e envio.
        }

        public void StatusPedidoErp(string statusPedidoErp)
        {
            System.Console.WriteLine("Status do Pedido Recebido");
            //Aqui vou receber o status do pedido vindo do ERP.
        }
    }
}