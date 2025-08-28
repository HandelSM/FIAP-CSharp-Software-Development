using System.Globalization;
using FIAPOracleCRUD.Database;
using FIAPOracleDB.Database;
using FIAPOracleDB.Models;
using Oracle.ManagedDataAccess.Client;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

OracleConnection? _con = null;
AlunoRepository? _alunos = null;
MateriaRepository? _materias = null;

bool IsLogged() => _con is not null && _con.State == System.Data.ConnectionState.Open;

void LoginAsync()
{
    if (IsLogged())
    {
        Console.WriteLine("Already logged in.");
        return;
    }

    Console.Write("Usuário: ");
    var user = Console.ReadLine();
    var pwd = Connection.ReadPasswordMasked();

    try
    {
        _con = Connection.Create(user!, pwd);
        _con.Open();
        _alunos = new AlunoRepository(_con);
        _materias = new MateriaRepository(_con);
        Console.WriteLine($"Connected. Server version: {_con.ServerVersion}");
    }
    catch (OracleException ex)
    {
        Console.WriteLine($"OracleException (ORA-{ex.Number}): {ex.Message}");
        _con?.Dispose();
        _con = null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        _con?.Dispose();
        _con = null;
    }
}

void Logout()
{
    if (_con is not null)
    {
        try { _con.Close(); } catch { /* ignore */ }
        _con.Dispose();
    }
    _con = null;
    _alunos = null;
    _materias = null;
    Console.WriteLine("Logged out.");
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var s = Console.ReadLine();
        if (int.TryParse(s, out var n)) return n;
        Console.WriteLine("Please enter a valid integer.");
    }
}

string ReadString(string prompt, bool allowEmpty = false, string? @default = null)
{
    while (true)
    {
        Console.Write(prompt);
        var s = Console.ReadLine();
        if (!string.IsNullOrEmpty(s)) return s!;
        if (allowEmpty) return @default ?? "";
        Console.WriteLine("Value cannot be empty.");
    }
}

bool Confirm(string prompt = "Confirm (y/n)? ")
{
    Console.Write(prompt);
    var ans = Console.ReadLine()?.Trim().ToLowerInvariant();
    return ans is "y" or "yes" or "s" or "sim";
}

void MenuAlunosAsync()
{
    if (!IsLogged() || _alunos is null)
    {
        Console.WriteLine("You must login first.");
        return;
    }

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== Alunos ====");
        Console.WriteLine("1) Listar todos");
        Console.WriteLine("2) Buscar por Id");
        Console.WriteLine("3) Inserir");
        Console.WriteLine("4) Atualizar");
        Console.WriteLine("5) Excluir");
        Console.WriteLine("6) Procurar por Nome");
        Console.WriteLine("9) Voltar");
        Console.Write("Escolha: ");
        var choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    {
                        var list = _alunos.GetAll();
                        if (list.Count == 0) { Console.WriteLine("Tabela Alunos está vazia."); break; }
                        Console.WriteLine("Id | Nome | Matricula");
                        foreach (var a in list)
                            Console.WriteLine($"{a.Id} | {a.Nome} | {a.Matricula}");
                        break;
                    }
                case "2":
                    {
                        var id = ReadInt("Id: ");
                        var a = _alunos.GetById(id);
                        if (a is null) Console.WriteLine("Não encontrado.");
                        else Console.WriteLine($"{a.Id} | {a.Nome} | {a.Matricula}");
                        break;
                    }
                case "3":
                    {
                        var nome = ReadString("Nome: ");
                        var mat = ReadString("Matricula: ");
                        var novo = new Aluno { Nome = nome, Matricula = mat };
                        var newId = _alunos.Insert(novo);
                        Console.WriteLine($"Inserido Id={newId}");
                        break;
                    }
                case "4":
                    {
                        var id = ReadInt("Id: ");
                        var a = _alunos.GetById(id);
                        if (a is null) { Console.WriteLine("Não encontrado."); break; }

                        Console.WriteLine($"Atual atual: {a.Id} | {a.Nome} | {a.Matricula}");
                        Console.Write("Novo Nome (enter para manter): ");
                        var nNome = Console.ReadLine();
                        Console.Write("Nova Matricula (enter para manter): ");
                        var nMat = Console.ReadLine();

                        a.Nome = string.IsNullOrWhiteSpace(nNome) ? a.Nome : nNome!;
                        a.Matricula = string.IsNullOrWhiteSpace(nMat) ? a.Matricula : nMat!;
                        var ok = _alunos.Update(a);
                        Console.WriteLine(ok ? "Atualizado." : "Nada atualizado.");
                        break;
                    }
                case "5":
                    {
                        var id = ReadInt("Id: ");
                        var a = _alunos.GetById(id);
                        if (a is null) { Console.WriteLine("Não encontrado."); break; }
                        Console.WriteLine($"{a.Id} | {a.Nome} | {a.Matricula}");
                        if (!Confirm("Excluir este registro (y/n)? ")) break;

                        var ok = _alunos.Delete(id);
                        Console.WriteLine(ok ? "Excluído." : "Nada excluído.");
                        break;
                    }
                case "6":
                    {
                        var term = ReadString("Procurar por nome: ");
                        var list = _alunos.SearchByName(term);
                        if (list.Count == 0) { Console.WriteLine("Nada encontrado."); break; }
                        Console.WriteLine("Id | Nome | Matricula");
                        foreach (var a in list) Console.WriteLine($"{a.Id} | {a.Nome} | {a.Matricula}");
                        break;
                    }
                case "9":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
        catch (OracleException ex)
        {
            Console.WriteLine($"OracleException (ORA-{ex.Number}): {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}

void MenuMateriasAsync()
{
    if (!IsLogged() || _materias is null)
    {
        Console.WriteLine("You must login first.");
        return;
    }

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== Materias ====");
        Console.WriteLine("1) Listar todos");
        Console.WriteLine("2) Buscar por Id");
        Console.WriteLine("3) Inserir");
        Console.WriteLine("4) Atualizar");
        Console.WriteLine("5) Excluir");
        Console.WriteLine("9) Voltar");
        Console.Write("Escolha: ");
        var choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    {
                        var list = _materias.GetAll();
                        if (list.Count == 0) { Console.WriteLine("Tabela Materias está vazia."); break; }
                        Console.WriteLine("Id | Nome | Codigo");
                        foreach (var m in list)
                            Console.WriteLine($"{m.Id} | {m.Nome} | {m.Codigo}");
                        break;
                    }
                case "2":
                    {
                        var id = ReadInt("Id: ");
                        var m = _materias.GetById(id);
                        if (m is null) Console.WriteLine("Não encontrado.");
                        else Console.WriteLine($"{m.Id} | {m.Nome} | {m.Codigo}");
                        break;
                    }
                case "3":
                    {
                        var nome = ReadString("Nome: ");
                        var cod = ReadString("Codigo: ");
                        var novo = new Materia { Nome = nome, Codigo = cod };
                        var newId = _materias.Insert(novo);
                        Console.WriteLine($"Inserido Id={newId}");
                        break;
                    }
                case "4":
                    {
                        var id = ReadInt("Id: ");
                        var m = _materias.GetById(id);
                        if (m is null) { Console.WriteLine("Não encontrado."); break; }

                        Console.WriteLine($"Atual atual: {m.Id} | {m.Nome} | {m.Codigo}");
                        Console.Write("Novo Nome (enter para manter): ");
                        var nNome = Console.ReadLine();
                        Console.Write("Novo Codigo (enter para manter): ");
                        var nCod = Console.ReadLine();

                        m.Nome = string.IsNullOrWhiteSpace(nNome) ? m.Nome : nNome!;
                        m.Codigo = string.IsNullOrWhiteSpace(nCod) ? m.Codigo : nCod!;
                        var ok = _materias.Update(m);
                        Console.WriteLine(ok ? "Atualizado." : "Nada atualizado.");
                        break;
                    }
                case "5":
                    {
                        var id = ReadInt("Id: ");
                        var m = _materias.GetById(id);
                        if (m is null) { Console.WriteLine("Não encontrado."); break; }
                        Console.WriteLine($"{m.Id} | {m.Nome} | {m.Codigo}");
                        if (!Confirm("Excluir este registro (y/n)? ")) break;

                        var ok = _materias.Delete(id);
                        Console.WriteLine(ok ? "Excluído." : "Nada excluído.");
                        break;
                    }
                case "9":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
        catch (OracleException ex)
        {
            Console.WriteLine($"OracleException (ORA-{ex.Number}): {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}

void MainMenuAsync()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("==== FIAP Oracle CRUD ====");
        Console.WriteLine(IsLogged()
            ? "1) Alunos\n2) Materias\n3) Logout\n0) Sair"
            : "1) Login\n0) Sair");
        Console.Write("Escolha: ");
        var choice = Console.ReadLine();

        if (!IsLogged())
        {
            switch (choice)
            {
                case "1": LoginAsync(); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
        else
        {
            switch (choice)
            {
                case "1": MenuAlunosAsync(); break;
                case "2": MenuMateriasAsync(); break;
                case "3": Logout(); break;
                case "0": return;
                default: Console.WriteLine("Opção inválida."); break;
            }
        }
    }
}

MainMenuAsync();
