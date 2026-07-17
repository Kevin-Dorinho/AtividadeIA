using System;
using MySql.Data.MySqlClient;

namespace AtividadeIA;

public static class BancoDeDados
{
    public static void Inicializar()
    {
        using (MySqlConnection conexaoSemBanco = Conexao.ObterConexaoSemBanco())
        {
            conexaoSemBanco.Open();
            string comandoCriarBanco = "CREATE DATABASE IF NOT EXISTS `atividade.ia`;";
            using (MySqlCommand comando = new MySqlCommand(comandoCriarBanco, conexaoSemBanco))
            {
                comando.ExecuteNonQuery();
            }
            conexaoSemBanco.Close();
        }

        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();

            string criarTabelaCurso = @"
                CREATE TABLE IF NOT EXISTS CURSO (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    NOME VARCHAR(150) NOT NULL
                );";

            string criarTabelaTurma = @"
                CREATE TABLE IF NOT EXISTS TURMA (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    SERIE_DA_TURMA VARCHAR(100) NOT NULL,
                    TURNO VARCHAR(50) NOT NULL,
                    ID_CURSO INT NOT NULL,
                    CONSTRAINT FK_TURMA_CURSO FOREIGN KEY (ID_CURSO) REFERENCES CURSO(ID) ON DELETE CASCADE
                );";

            string criarTabelaAluno = @"
                CREATE TABLE IF NOT EXISTS ALUNO (
                    ID INT AUTO_INCREMENT PRIMARY KEY,
                    NOME VARCHAR(150) NOT NULL,
                    RG VARCHAR(20) NOT NULL,
                    CPF VARCHAR(14) NOT NULL,
                    DATA_NASCIMENTO DATE NOT NULL,
                    ENDERECO VARCHAR(250) NOT NULL,
                    TELEFONE VARCHAR(20) NOT NULL,
                    EMAIL VARCHAR(150) NOT NULL,
                    ID_CURSO INT NOT NULL,
                    ID_TURMA INT NOT NULL,
                    CONSTRAINT FK_ALUNO_CURSO FOREIGN KEY (ID_CURSO) REFERENCES CURSO(ID) ON DELETE CASCADE,
                    CONSTRAINT FK_ALUNO_TURMA FOREIGN KEY (ID_TURMA) REFERENCES TURMA(ID) ON DELETE CASCADE
                );";

            using (MySqlCommand comando = new MySqlCommand(criarTabelaCurso, conexao))
            {
                comando.ExecuteNonQuery();
            }

            using (MySqlCommand comando = new MySqlCommand(criarTabelaTurma, conexao))
            {
                comando.ExecuteNonQuery();
            }

            using (MySqlCommand comando = new MySqlCommand(criarTabelaAluno, conexao))
            {
                comando.ExecuteNonQuery();
            }

            conexao.Close();
        }
    }
}
