using MyFinanceWeb.Domain.Entities.Base;

namespace MyFinanceWeb.Domain.Entities;

public class Transacao : EntityBase
{
    public string Historico { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public int PlanoContaId { get; set; }
    public PlanoConta PlanoConta { get; set; } = null!;

}