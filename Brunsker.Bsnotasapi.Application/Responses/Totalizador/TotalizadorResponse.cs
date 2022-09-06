namespace Brunsker.Bsnotas.Application.Responses.Totalizador
{
    public class TotalizadorResponse
    {
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public int Canceled { get; set; }
        public decimal IssValueRetained { get; set; }
    }
}
