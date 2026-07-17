using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AtividadeIA;

public class Curso
{
    private int id;
    private string nome;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Nome
    {
        get { return nome; }
        set { nome = value; }
    }

    public Curso(string nome)
    {
        this.nome = nome;
    }

    public Curso(int id, string nome)
    {
        this.id = id;
        this.nome = nome;
    }

    public void Mostrar()
    {
        Console.WriteLine($"Curso - ID: {id} | Nome: {nome}");
    }

    public void Salvar()
    {
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = "INSERT INTO CURSO (NOME) VALUES (@Nome); SELECT LAST_INSERT_ID();";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@Nome", nome);
                this.id = Convert.ToInt32(comando.ExecuteScalar());
            }
            conexao.Close();
        }
    }

    public static List<Curso> ObterTodos()
    {
        List<Curso> listaCursos = new List<Curso>();
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = "SELECT ID, NOME FROM CURSO ORDER BY NOME;";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        int idCurso = leitor.GetInt32("ID");
                        string nomeCurso = leitor.GetString("NOME");
                        listaCursos.Add(new Curso(idCurso, nomeCurso));
                    }
                }
            }
            conexao.Close();
        }
        return listaCursos;
    }

    public static Curso? ObterPorId(int idCurso)
    {
        Curso? cursoBuscado = null;
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = "SELECT ID, NOME FROM CURSO WHERE ID = @ID;";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@ID", idCurso);
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        int idRetornado = leitor.GetInt32("ID");
                        string nomeCurso = leitor.GetString("NOME");
                        cursoBuscado = new Curso(idRetornado, nomeCurso);
                    }
                }
            }
            conexao.Close();
        }
        return cursoBuscado;
    }
}
