using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AtividadeIA;

public class Aluno
{
    private int id;
    private string nome;
    private string rg;
    private string cpf;
    private DateTime dataNascimento;
    private string endereco;
    private string telefone;
    private string email;
    private Curso curso;
    private Turma turma;

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

    public string Rg
    {
        get { return rg; }
        set { rg = value; }
    }

    public string Cpf
    {
        get { return cpf; }
        set { cpf = value; }
    }

    public DateTime DataNascimento
    {
        get { return dataNascimento; }
        set { dataNascimento = value; }
    }

    public string Endereco
    {
        get { return endereco; }
        set { endereco = value; }
    }

    public string Telefone
    {
        get { return telefone; }
        set { telefone = value; }
    }

    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    public Curso Curso
    {
        get { return curso; }
        set { curso = value; }
    }

    public Turma Turma
    {
        get { return turma; }
        set { turma = value; }
    }

    public Aluno(string nome, string rg, string cpf, DateTime dataNascimento, string endereco, string telefone, string email, Curso curso, Turma turma)
    {
        this.nome = nome;
        this.rg = rg;
        this.cpf = cpf;
        this.dataNascimento = dataNascimento;
        this.endereco = endereco;
        this.telefone = telefone;
        this.email = email;
        this.curso = curso;
        this.turma = turma;
    }

    public Aluno(int id, string nome, string rg, string cpf, DateTime dataNascimento, string endereco, string telefone, string email, Curso curso, Turma turma)
    {
        this.id = id;
        this.nome = nome;
        this.rg = rg;
        this.cpf = cpf;
        this.dataNascimento = dataNascimento;
        this.endereco = endereco;
        this.telefone = telefone;
        this.email = email;
        this.curso = curso;
        this.turma = turma;
    }

    public void Mostrar()
    {
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine($"Aluno ID: {id}");
        Console.WriteLine($"Nome: {nome}");
        Console.WriteLine($"RG: {rg} | CPF: {cpf}");
        Console.WriteLine($"Nascimento: {dataNascimento.ToString("dd/MM/yyyy")}");
        Console.WriteLine($"Endereço: {endereco}");
        Console.WriteLine($"Telefone: {telefone} | Email: {email}");
        Console.WriteLine($"Curso: {curso.Nome}");
        Console.WriteLine($"Turma: {turma.SerieDaTurma} ({turma.Turno})");
        Console.WriteLine("---------------------------------------------");
    }

    public void Salvar()
    {
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = @"
                INSERT INTO ALUNO (NOME, RG, CPF, DATA_NASCIMENTO, ENDERECO, TELEFONE, EMAIL, ID_CURSO, ID_TURMA) 
                VALUES (@Nome, @Rg, @Cpf, @DataNascimento, @Endereco, @Telefone, @Email, @IdCurso, @IdTurma);
                SELECT LAST_INSERT_ID();";

            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                comando.Parameters.AddWithValue("@Nome", nome);
                comando.Parameters.AddWithValue("@Rg", rg);
                comando.Parameters.AddWithValue("@Cpf", cpf);
                comando.Parameters.AddWithValue("@DataNascimento", dataNascimento.ToString("yyyy-MM-dd"));
                comando.Parameters.AddWithValue("@Endereco", endereco);
                comando.Parameters.AddWithValue("@Telefone", telefone);
                comando.Parameters.AddWithValue("@Email", email);
                comando.Parameters.AddWithValue("@IdCurso", curso.Id);
                comando.Parameters.AddWithValue("@IdTurma", turma.Id);

                this.id = Convert.ToInt32(comando.ExecuteScalar());
            }
            conexao.Close();
        }
    }

    public static List<Aluno> ObterTodos()
    {
        List<Aluno> listaAlunos = new List<Aluno>();
        using (MySqlConnection conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string query = @"
                SELECT A.ID, A.NOME, A.RG, A.CPF, A.DATA_NASCIMENTO, A.ENDERECO, A.TELEFONE, A.EMAIL, 
                       A.ID_CURSO, C.NOME AS NOME_CURSO, 
                       A.ID_TURMA, T.SERIE_DA_TURMA, T.TURNO 
                FROM ALUNO A 
                INNER JOIN CURSO C ON A.ID_CURSO = C.ID 
                INNER JOIN TURMA T ON A.ID_TURMA = T.ID 
                ORDER BY A.NOME;";

            using (MySqlCommand comando = new MySqlCommand(query, conexao))
            {
                using (MySqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        int idAluno = leitor.GetInt32("ID");
                        string nomeAluno = leitor.GetString("NOME");
                        string rgAluno = leitor.GetString("RG");
                        string cpfAluno = leitor.GetString("CPF");
                        DateTime dataNasc = leitor.GetDateTime("DATA_NASCIMENTO");
                        string end = leitor.GetString("ENDERECO");
                        string tel = leitor.GetString("TELEFONE");
                        string mail = leitor.GetString("EMAIL");

                        int idCurso = leitor.GetInt32("ID_CURSO");
                        string nomeCurso = leitor.GetString("NOME_CURSO");
                        Curso cursoAssoc = new Curso(idCurso, nomeCurso);

                        int idTurma = leitor.GetInt32("ID_TURMA");
                        string serie = leitor.GetString("SERIE_DA_TURMA");
                        string turno = leitor.GetString("TURNO");
                        Turma turmaAssoc = new Turma(idTurma, serie, turno, cursoAssoc);

                        listaAlunos.Add(new Aluno(idAluno, nomeAluno, rgAluno, cpfAluno, dataNasc, end, tel, mail, cursoAssoc, turmaAssoc));
                    }
                }
            }
            conexao.Close();
        }
        return listaAlunos;
    }
}
