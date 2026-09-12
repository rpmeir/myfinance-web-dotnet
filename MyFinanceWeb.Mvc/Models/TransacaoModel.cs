namespace MyFinanceWeb.Mvc.Models;

public class TransacaoModel
{
    public int Id { get; set; }
    public string Historico { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public char? Tipo { get; set; }
    public int PlanoContaId { get; set; }
}