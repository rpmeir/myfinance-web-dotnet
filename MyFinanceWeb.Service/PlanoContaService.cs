namespace MyFinanceWeb.Service;

using MyFinanceWeb.Domain.Entities;
using MyFinanceWeb.Infra.Interfaces;
using MyFinanceWeb.Service.Interfaces;

public class PlanoContaService : IPlanoContaService
{
    private readonly IPlanoContaRepository _planoContaRepository;

    public PlanoContaService(IPlanoContaRepository planoContaRepository)
    {
        _planoContaRepository = planoContaRepository;
    }

    public int Cadastrar(PlanoConta entity)
    {
        return _planoContaRepository.Cadastrar(entity);
    }

    public int Excluir(int id)
    {
        return _planoContaRepository.Excluir(id);
    }

    public List<PlanoConta> ListarRegistros()
    {
        return _planoContaRepository.ListarRegistros();
    }

    public PlanoConta? RetornarRegistro(int id)
    {
        return _planoContaRepository.RetornarRegistro(id);
    }
}