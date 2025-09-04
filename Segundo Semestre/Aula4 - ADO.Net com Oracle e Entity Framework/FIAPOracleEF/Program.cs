using System.Globalization;
using System.Text;
using FIAPOracleEF.Database;   // AppDbContext, GenericRepository
using FIAPOracleEF.Models;     // Aluno, Materia
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

const string DataSource = "oracle.fiap.com.br:1521/ORCL";

AppDbContext? _db = null;
GenericRepository<Aluno>? _alunosRepo = null;
GenericRepository<Materia>? _materiasRepo = null;

bool IsLogged()
{
    return _db != null;
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        string? s = Console.ReadLine();
        int n;
        if (int.TryParse(s, out n))
        {
            return n;
        }
        Console.WriteLine("Please enter a valid integer.");
    }
}

string ReadString(string prompt, bool allowEmpty = false, string? @default = null)
{
    while (true)
    {
        Console.Write(prompt);
        string? s = Console.ReadLine();
        if (!string.IsNullOrEmpty(s))
        {
            return s;
        }
        if (allowEmpty)
        {
            return @default ?? "";
        }
        Console.WriteLine("Value cannot be empty.");
    }
}

bool Confirm(string prompt = "Confirm (y/n)? ")
{
    Console.Write(prompt);
    string? ans = Console.ReadLine();
    if (ans == null)
    {
        return false;
    }
    ans = ans.Trim().ToLowerInvariant();
    return ans == "y" || ans == "yes" || ans == "s" || ans == "sim";
}

string ReadPasswordMasked(string prompt = "Senha: ")
{
    Console.Write(prompt);
    StringBuilder sb = new StringBuilder();
    ConsoleKeyInfo key;
    while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
    {
        if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
        {
            sb.Length--;
        }
        else if (!char.IsControl(key.KeyChar))
        {
            sb.Append(key.KeyChar);
        }
    }
    Console.WriteLine();
    return sb.ToString();
}

void Login()
{
    if (IsLogged())
    {
        Console.WriteLine("Already logged in.");
        return;
    }

    Console.Write("Usuário: ");
    string? user = Console.ReadLine();
    string pwd = ReadPasswordMasked();

    string connString = "User Id=" + user + ";Password=" + pwd + ";Data Source=" + DataSource;
    _db = new AppDbContext(connString);

    try
    {
        bool ok = _db.Database.CanConnect();
        if (!ok)
        {
            throw new Exception("Falha ao conectar. Verifique usuário/senha/ORCL.");
        }

        _alunosRepo = new GenericRepository<Aluno>(_db);
        _materiasRepo = new GenericRepository<Materia>(_db);

        Console.WriteLine("Connected (EF).");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
        _db.Dispose();
        _db = null;
    }
}

void Logout()
{
    if (_db != null)
    {
        _db.Dispose();
    }
    _db = null;
    _alunosRepo = null;
    _materiasRepo = null;
    Console.WriteLine("Logged out.");
}

void MenuAlunos()
{
    if (!IsLogged() || _alunosRepo == null)
    {
        Console.WriteLine("You must login first.");
        return;
    }

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== Alunos (EF) ====");
        Console.WriteLine("1) Listar todos");
        Console.WriteLine("2) Buscar por Id");
        Console.WriteLine("3) Inserir");
        Console.WriteLine("4) Atualizar");
        Console.WriteLine("5) Excluir");
        Console.WriteLine("6) Procurar por Nome");
        Console.WriteLine("7) Excluir por Id");
        Console.WriteLine("9) Voltar");
        Console.Write("Escolha: ");
        string? choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    {
                        List<Aluno> list = _alunosRepo.GetAll();
                        if (list.Count == 0)
                        {
                            Console.WriteLine("Tabela Alunos está vazia.");
                        }
                        else
                        {
                            Console.WriteLine("Id | Nome | Matricula");
                            foreach (Aluno a in list)
                            {
                                Console.WriteLine(a.Id + " | " + a.Nome + " | " + a.Matricula);
                            }
                        }
                        break;
                    }
                case "2":
                    {
                        int id = ReadInt("Id: ");
                        Aluno? a = _alunosRepo.GetById(id);
                        if (a == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine(a.Id + " | " + a.Nome + " | " + a.Matricula);
                        }
                        break;
                    }
                case "3":
                    {
                        string nome = ReadString("Nome: ");
                        string mat = ReadString("Matricula: ");
                        Aluno novo = new Aluno();
                        novo.Nome = nome;
                        novo.Matricula = mat;

                        int rows = _alunosRepo.Insert(novo);
                        Console.WriteLine(rows > 0 ? "Inserido. Id=" + novo.Id : "Nada inserido.");
                        break;
                    }
                case "4":
                    {
                        int id = ReadInt("Id: ");
                        Aluno? a = _alunosRepo.GetById(id);
                        if (a == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine("Atual atual: " + a.Id + " | " + a.Nome + " | " + a.Matricula);
                            Console.Write("Novo Nome (enter para manter): ");
                            string? nNome = Console.ReadLine();
                            Console.Write("Nova Matricula (enter para manter): ");
                            string? nMat = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(nNome))
                            {
                                a.Nome = nNome;
                            }
                            if (!string.IsNullOrWhiteSpace(nMat))
                            {
                                a.Matricula = nMat;
                            }

                            bool ok = _alunosRepo.Update(a);
                            Console.WriteLine(ok ? "Atualizado." : "Nada atualizado.");
                        }
                        break;
                    }
                case "5":
                    {
                        int id = ReadInt("Id: ");
                        Aluno? a = _alunosRepo.GetById(id);
                        if (a == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine(a.Id + " | " + a.Nome + " | " + a.Matricula);
                            if (Confirm("Excluir este registro (y/n)? "))
                            {
                                bool ok = _alunosRepo.Delete(a);
                                Console.WriteLine(ok ? "Excluído." : "Nada excluído.");
                            }
                        }
                        break;
                    }
                case "6":
                    {
                        string term = ReadString("Procurar por nome: ");
                        try
                        {
                            List<Aluno> list = _alunosRepo.SearchBy(a => a.Nome != null && a.Nome.ToUpper().Contains(term.ToUpper()));
                            if (list.Count == 0)
                            {
                                Console.WriteLine("Nada encontrado.");
                            }
                            else
                            {
                                Console.WriteLine("Id | Nome | Matricula");
                                foreach (Aluno a in list)
                                {
                                    Console.WriteLine(a.Id + " | " + a.Nome + " | " + a.Matricula);
                                }
                            }
                        }
                        catch (NotImplementedException)
                        {
                            Console.WriteLine("SearchBy ainda não implementado (faça ao vivo).");
                        }
                        break;
                    }
                case "7":
                    {
                        int id = ReadInt("Id: ");
                        try
                        {
                            bool ok = _alunosRepo.DeleteById(id);
                            Console.WriteLine(ok ? "Excluído." : "Nada excluído (ou não encontrado).");
                        }
                        catch (NotImplementedException)
                        {
                            Console.WriteLine("DeleteById ainda não implementado (faça ao vivo).");
                        }
                        break;
                    }
                case "9":
                    {
                        return;
                    }
                default:
                    {
                        Console.WriteLine("Opção inválida.");
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro: " + ex.Message);
        }
    }
}

void MenuMaterias()
{
    if (!IsLogged() || _materiasRepo == null)
    {
        Console.WriteLine("You must login first.");
        return;
    }

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== Materias (EF) ====");
        Console.WriteLine("1) Listar todos");
        Console.WriteLine("2) Buscar por Id");
        Console.WriteLine("3) Inserir");
        Console.WriteLine("4) Atualizar");
        Console.WriteLine("5) Excluir");
        Console.WriteLine("6) Procurar por Nome (vai lançar NotImplemented até implementar)");
        Console.WriteLine("7) Excluir por Id (vai lançar NotImplemented até implementar)");
        Console.WriteLine("9) Voltar");
        Console.Write("Escolha: ");
        string? choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    {
                        List<Materia> list = _materiasRepo.GetAll();
                        if (list.Count == 0)
                        {
                            Console.WriteLine("Tabela Materias está vazia.");
                        }
                        else
                        {
                            Console.WriteLine("Id | Nome | Codigo");
                            foreach (Materia m in list)
                            {
                                Console.WriteLine(m.Id + " | " + m.Nome + " | " + m.Codigo);
                            }
                        }
                        break;
                    }
                case "2":
                    {
                        int id = ReadInt("Id: ");
                        Materia? m = _materiasRepo.GetById(id);
                        if (m == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine(m.Id + " | " + m.Nome + " | " + m.Codigo);
                        }
                        break;
                    }
                case "3":
                    {
                        string nome = ReadString("Nome: ");
                        string cod = ReadString("Codigo: ");
                        Materia novo = new Materia();
                        novo.Nome = nome;
                        novo.Codigo = cod;

                        int rows = _materiasRepo.Insert(novo);
                        Console.WriteLine(rows > 0 ? "Inserido. Id=" + novo.Id : "Nada inserido.");
                        break;
                    }
                case "4":
                    {
                        int id = ReadInt("Id: ");
                        Materia? m = _materiasRepo.GetById(id);
                        if (m == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine("Atual atual: " + m.Id + " | " + m.Nome + " | " + m.Codigo);
                            Console.Write("Novo Nome (enter para manter): ");
                            string? nNome = Console.ReadLine();
                            Console.Write("Novo Codigo (enter para manter): ");
                            string? nCod = Console.ReadLine();

                            if (!string.IsNullOrWhiteSpace(nNome))
                            {
                                m.Nome = nNome;
                            }
                            if (!string.IsNullOrWhiteSpace(nCod))
                            {
                                m.Codigo = nCod;
                            }

                            bool ok = _materiasRepo.Update(m);
                            Console.WriteLine(ok ? "Atualizado." : "Nada atualizado.");
                        }
                        break;
                    }
                case "5":
                    {
                        int id = ReadInt("Id: ");
                        Materia? m = _materiasRepo.GetById(id);
                        if (m == null)
                        {
                            Console.WriteLine("Não encontrado.");
                        }
                        else
                        {
                            Console.WriteLine(m.Id + " | " + m.Nome + " | " + m.Codigo);
                            if (Confirm("Excluir este registro (y/n)? "))
                            {
                                bool ok = _materiasRepo.Delete(m);
                                Console.WriteLine(ok ? "Excluído." : "Nada excluído.");
                            }
                        }
                        break;
                    }
                case "6":
                    {
                        string term = ReadString("Procurar por nome: ");
                        try
                        {
                            List<Materia> list = _materiasRepo.SearchBy(m => m.Nome != null && m.Nome.ToUpper().Contains(term.ToUpper()));
                            if (list.Count == 0)
                            {
                                Console.WriteLine("Nada encontrado.");
                            }
                            else
                            {
                                Console.WriteLine("Id | Nome | Codigo");
                                foreach (Materia m in list)
                                {
                                    Console.WriteLine(m.Id + " | " + m.Nome + " | " + m.Codigo);
                                }
                            }
                        }
                        catch (NotImplementedException)
                        {
                            Console.WriteLine("SearchBy ainda não implementado (faça ao vivo).");
                        }
                        break;
                    }
                case "7":
                    {
                        int id = ReadInt("Id: ");
                        try
                        {
                            bool ok = _materiasRepo.DeleteById(id);
                            Console.WriteLine(ok ? "Excluído." : "Nada excluído (ou não encontrado).");
                        }
                        catch (NotImplementedException)
                        {
                            Console.WriteLine("DeleteById ainda não implementado (faça ao vivo).");
                        }
                        break;
                    }
                case "9":
                    {
                        return;
                    }
                default:
                    {
                        Console.WriteLine("Opção inválida.");
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro: " + ex.Message);
        }
    }
}

void MainMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== FIAP Oracle CRUD (EF) ====");
        if (!IsLogged())
        {
            Console.WriteLine("1) Login");
            Console.WriteLine("0) Sair");
            Console.Write("Escolha: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        Login();
                        break;
                    }
                case "0":
                    {
                        return;
                    }
                default:
                    {
                        Console.WriteLine("Opção inválida.");
                        break;
                    }
            }
        }
        else
        {
            Console.WriteLine("1) Alunos");
            Console.WriteLine("2) Materias");
            Console.WriteLine("3) Logout");
            Console.WriteLine("0) Sair");
            Console.Write("Escolha: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    {
                        MenuAlunos();
                        break;
                    }
                case "2":
                    {
                        MenuMaterias();
                        break;
                    }
                case "3":
                    {
                        Logout();
                        break;
                    }
                case "0":
                    {
                        return;
                    }
                default:
                    {
                        Console.WriteLine("Opção inválida.");
                        break;
                    }
            }
        }
    }
}

// ---- run ----
MainMenu();
