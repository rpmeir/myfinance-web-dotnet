namespace MyFinanceWeb.Infra.Interfaces.Base;

public interface IRepository<TEntity> where TEntity : class
{
    void Cadastrar(TEntity entity);
    bool Excluir(int id);
    List<TEntity> ListarRegistros();
    TEntity? RetornarRegistro(int id);
}
