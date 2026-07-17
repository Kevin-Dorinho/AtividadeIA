using MySql.Data.MySqlClient;

namespace AtividadeIA;

public static class Conexao
{
    private static string stringConexao = "server=localhost;port=3307;uid=root;password=;database=atividade.ia";

    public static MySqlConnection ObterConexao()
    {
        return new MySqlConnection(stringConexao);
    }

    public static MySqlConnection ObterConexaoSemBanco()
    {
        return new MySqlConnection("server=localhost;port=3307;uid=root;password=;");
    }
}
