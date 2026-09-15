namespace MyFinanceWeb.Infra.Interfaces.Base;

public interface IRepository<TEntity> where TEntity : class
{
    int Cadastrar(TEntity entity);
    int Excluir(int id);
    List<TEntity> ListarRegistros();
    TEntity? RetornarRegistro(int id);
}
