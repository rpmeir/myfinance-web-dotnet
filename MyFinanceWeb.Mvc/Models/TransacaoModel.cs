namespace MyFinanceWeb.Mvc.Models;

public class TransacaoModel
{
    public int Id { get; set; }
    public string Historico { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int PlanoContaId { get; set; }
}