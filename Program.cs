using System;
using System.Collections.Generic;

namespace AtividadeIA;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Inicializando banco de dados...");
        try
        {
            BancoDeDados.Inicializar();
            Console.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inicializar o banco de dados: {ex.Message}");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        bool executando = true;
        while (executando)
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("        SISTEMA DE GERENCIAMENTO ESCOLAR     ");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Cadastrar Curso");
            Console.WriteLine("2. Cadastrar Turma");
            Console.WriteLine("3. Cadastrar Aluno");
            Console.WriteLine("4. Listar Cursos");
            Console.WriteLine("5. Listar Turmas");
            Console.WriteLine("6. Listar Alunos");
            Console.WriteLine("0. Sair");
            Console.WriteLine("=============================================");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine() ?? "";

            switch (opcao)
            {
                case "1":
                    CadastrarCurso();
                    break;
                case "2":
                    CadastrarTurma();
                    break;
                case "3":
                    CadastrarAluno();
                    break;
                case "4":
                    ListarCursos();
                    break;
                case "5":
                    ListarTurmas();
                    break;
                case "6":
                    ListarAlunos();
                    break;
                case "0":
                    executando = false;
                    Console.WriteLine("Saindo do sistema. Até mais!");
                    break;
                default:
                    Console.WriteLine("Opção inválida! Tente novamente.");
                    AguardarTecla();
                    break;
            }
            Console.Clear();
        }
    }

    private static void CadastrarCurso()
    {
        Console.Clear();
        Console.WriteLine("--- CADASTRAR CURSO ---");
        Console.Write("Nome do Curso: ");
        string nome = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("O nome do curso não pode ser vazio.");
            AguardarTecla();
            return;
        }

        Curso novoCurso = new Curso(nome);
        try
        {
            novoCurso.Salvar();
            Console.WriteLine("\nCurso cadastrado com sucesso!");
            novoCurso.Mostrar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar curso: {ex.Message}");
        }
        AguardarTecla();
    }

    private static void CadastrarTurma()
    {
        Console.Clear();
        Console.WriteLine("--- CADASTRAR TURMA ---");
        
        List<Curso> cursos = Curso.ObterTodos();
        if (cursos.Count == 0)
        {
            Console.WriteLine("Não existem cursos cadastrados. Cadastre um curso primeiro.");
            AguardarTecla();
            return;
        }

        Console.WriteLine("Cursos disponíveis:");
        foreach (Curso c in cursos)
        {
            c.Mostrar();
        }

        Console.Write("\nDigite o ID do Curso para esta turma: ");
        if (!int.TryParse(Console.ReadLine(), out int idCurso))
        {
            Console.WriteLine("ID inválido.");
            AguardarTecla();
            return;
        }

        Curso? cursoSelecionado = Curso.ObterPorId(idCurso);
        if (cursoSelecionado == null)
        {
            Console.WriteLine("Curso não encontrado.");
            AguardarTecla();
            return;
        }

        Console.Write("Série da Turma (Ex: 1º Ano): ");
        string serie = Console.ReadLine() ?? "";
        Console.Write("Turno (Ex: Matutino, Vespertino, Noturno): ");
        string turno = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(serie) || string.IsNullOrWhiteSpace(turno))
        {
            Console.WriteLine("Série e Turno são obrigatórios.");
            AguardarTecla();
            return;
        }

        Turma novaTurma = new Turma(serie, turno, cursoSelecionado);
        try
        {
            novaTurma.Salvar();
            Console.WriteLine("\nTurma cadastrada com sucesso!");
            novaTurma.Mostrar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar turma: {ex.Message}");
        }
        AguardarTecla();
    }

    private static void CadastrarAluno()
    {
        Console.Clear();
        Console.WriteLine("--- CADASTRAR ALUNO ---");

        List<Curso> cursos = Curso.ObterTodos();
        List<Turma> turmas = Turma.ObterTodas();

        if (cursos.Count == 0 || turmas.Count == 0)
        {
            Console.WriteLine("É necessário ter cursos e turmas cadastrados antes de adicionar um aluno.");
            AguardarTecla();
            return;
        }

        Console.WriteLine("Cursos disponíveis:");
        foreach (Curso c in cursos)
        {
            c.Mostrar();
        }
        Console.Write("\nDigite o ID do Curso do aluno: ");
        if (!int.TryParse(Console.ReadLine(), out int idCurso))
        {
            Console.WriteLine("ID de curso inválido.");
            AguardarTecla();
            return;
        }
        Curso? cursoSelecionado = Curso.ObterPorId(idCurso);
        if (cursoSelecionado == null)
        {
            Console.WriteLine("Curso não encontrado.");
            AguardarTecla();
            return;
        }

        Console.WriteLine("\nTurmas disponíveis para este curso:");
        bool encontrouTurma = false;
        foreach (Turma t in turmas)
        {
            if (t.Curso.Id == cursoSelecionado.Id)
            {
                t.Mostrar();
                encontrouTurma = true;
            }
        }

        if (!encontrouTurma)
        {
            Console.WriteLine("Não existem turmas associadas a este curso. Crie uma turma para este curso primeiro.");
            AguardarTecla();
            return;
        }

        Console.Write("\nDigite o ID da Turma do aluno: ");
        if (!int.TryParse(Console.ReadLine(), out int idTurma))
        {
            Console.WriteLine("ID de turma inválido.");
            AguardarTecla();
            return;
        }
        Turma? turmaSelecionada = Turma.ObterPorId(idTurma);
        if (turmaSelecionada == null || turmaSelecionada.Curso.Id != cursoSelecionado.Id)
        {
            Console.WriteLine("Turma inválida ou não pertence ao curso selecionado.");
            AguardarTecla();
            return;
        }

        Console.Write("Nome do Aluno: ");
        string nome = Console.ReadLine() ?? "";
        Console.Write("RG: ");
        string rg = Console.ReadLine() ?? "";
        Console.Write("CPF: ");
        string cpf = Console.ReadLine() ?? "";
        Console.Write("Data de Nascimento (dd/mm/aaaa): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataNascimento))
        {
            Console.WriteLine("Data de nascimento inválida.");
            AguardarTecla();
            return;
        }
        Console.Write("Endereço: ");
        string endereco = Console.ReadLine() ?? "";
        Console.Write("Telefone: ");
        string telefone = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(rg) || string.IsNullOrWhiteSpace(cpf))
        {
            Console.WriteLine("Nome, RG e CPF são campos obrigatórios.");
            AguardarTecla();
            return;
        }

        Aluno novoAluno = new Aluno(nome, rg, cpf, dataNascimento, endereco, telefone, email, cursoSelecionado, turmaSelecionada);
        try
        {
            novoAluno.Salvar();
            Console.WriteLine("\nAluno cadastrado com sucesso!");
            novoAluno.Mostrar();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar aluno: {ex.Message}");
        }
        AguardarTecla();
    }

    private static void ListarCursos()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE CURSOS ---");
        List<Curso> cursos = Curso.ObterTodos();
        if (cursos.Count == 0)
        {
            Console.WriteLine("Nenhum curso cadastrado.");
        }
        else
        {
            foreach (Curso c in cursos)
            {
                c.Mostrar();
            }
        }
        AguardarTecla();
    }

    private static void ListarTurmas()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE TURMAS ---");
        List<Turma> turmas = Turma.ObterTodas();
        if (turmas.Count == 0)
        {
            Console.WriteLine("Nenhuma turma cadastrada.");
        }
        else
        {
            foreach (Turma t in turmas)
            {
                t.Mostrar();
            }
        }
        AguardarTecla();
    }

    private static void ListarAlunos()
    {
        Console.Clear();
        Console.WriteLine("--- LISTA DE ALUNOS ---");
        List<Aluno> alunos = Aluno.ObterTodos();
        if (alunos.Count == 0)
        {
            Console.WriteLine("Nenhum aluno cadastrado.");
        }
        else
        {
            foreach (Aluno a in alunos)
            {
                a.Mostrar();
            }
        }
        AguardarTecla();
    }

    private static void AguardarTecla()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}