namespace MyFinanceWeb.Mvc.Models;

public class PlanoContaModel
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public char Tipo { get; set; }
}