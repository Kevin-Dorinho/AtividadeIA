using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AtividadeIA;

public class Turma
{
    private int id;
    private string serieDaTurma;
    private string turno;
    private Curso curso;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string SerieDaTurma
    {
        get { return serieDaTurma; }
        set { serieDaTurma = value; }
    }

    public string Turno
    {
        get { return turno; }
        set { turno = value; }
    }

    public Curso Curso
    {
        get { return curso; }
        set { curso = value; }
    }

    public Turma(string serieDaTurma, string turno, Curso curso)
    {
        this.serieDaTurma = serieDaTurma;
        this.turno = turno;
        this.curso = curso;
    }

    public Turma(int id, string serieDaTurma, string turno, Curso curso)
    {
        this.id = id;
        this.serieDaTurma = serieDaTurma;
        this.turno = turno;
        this.curso = curso;
    }

    public void Mostrar()
    {
        Console.WriteLine($"Turma - ID: {id} | Série: {serieDaTurma} | Turno: {turno} | Curso: {curso.Nome}");
    }

    public void Salvar()
    {
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = "INSERT INTO TURMA (SERIE_DA_TURMA, TURNO, ID_CURSO) VALUES (@Serie, @Turno, @IdCurso); SELECT LAST_INSERT_ID();";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@Serie", serieDaTurma);
                comando.Parameters.AddWithValue("@Turno", turno);
                comando.Parameters.AddWithValue("@IdCurso", curso.Id);
                this.id = Convert.ToInt32(comando.ExecuteScalar());
            }
            conexao.Close();
        }
    }

    public static List<Turma> ObterTodas()
    {
        List<Turma> listaTurmas = new List<Turma>();
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = @"
                SELECT T.ID, T.SERIE_DA_TURMA, T.TURNO, T.ID_CURSO, C.NOME AS NOME_CURSO 
                FROM TURMA T 
                INNER JOIN CURSO C ON T.ID_CURSO = C.ID 
                ORDER BY T.SERIE_DA_TURMA;";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        int idTurma = leitor.GetInt32("ID");
                        string serie = leitor.GetString("SERIE_DA_TURMA");
                        string turnoTurma = leitor.GetString("TURNO");
                        int idCurso = leitor.GetInt32("ID_CURSO");
                        string nomeCurso = leitor.GetString("NOME_CURSO");

                        Curso cursoAssociado = new Curso(idCurso, nomeCurso);
                        listaTurmas.Add(new Turma(idTurma, serie, turnoTurma, cursoAssociado));
                    }
                }
            }
            conexao.Close();
        }
        return listaTurmas;
    }

    public static Turma? ObterPorId(int idTurma)
    {
        Turma? turmaBuscada = null;
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = @"
                SELECT T.ID, T.SERIE_DA_TURMA, T.TURNO, T.ID_CURSO, C.NOME AS NOME_CURSO 
                FROM TURMA T 
                INNER JOIN CURSO C ON T.ID_CURSO = C.ID 
                WHERE T.ID = @ID;";
            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@ID", idTurma);
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    if (leitor.Read())
                    {
                        int idRetornado = leitor.GetInt32("ID");
                        string serie = leitor.GetString("SERIE_DA_TURMA");
                        string turnoTurma = leitor.GetString("TURNO");
                        int idCurso = leitor.GetInt32("ID_CURSO");
                        string nomeCurso = leitor.GetString("NOME_CURSO");

                        Curso cursoAssociado = new Curso(idCurso, nomeCurso);
                        turmaBuscada = new Turma(idRetornado, serie, turnoTurma, cursoAssociado);
                    }
                }
            }
            conexao.Close();
        }
        return turmaBuscada;
    }
}
