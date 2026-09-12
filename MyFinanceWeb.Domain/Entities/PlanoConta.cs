using MyFinanceWeb.Domain.Entities.Base;

namespace MyFinanceWeb.Domain.Entities;

public class PlanoConta : EntityBase
{
    public string Descricao { get; set; } = string.Empty;
    public char Tipo { get; set; } = ' ';
}