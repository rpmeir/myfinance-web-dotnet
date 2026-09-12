using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Service.Interfaces;

public interface IPlanoContaService
{
    void Cadastrar(PlanoConta planoConta);
    void Excluir(int id);
    List<PlanoConta> ListarRegistros();
    PlanoConta? RetornarRegistro(int id);
}