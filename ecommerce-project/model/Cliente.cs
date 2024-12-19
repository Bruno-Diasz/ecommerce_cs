using System.Text.Json;

static class ClientList
{
    private static List<Cliente> Clients = new List<Cliente>();

    // Insere um novo cliente
    public static void AddCliente(Cliente client)
    {
        Open();

        // Validação: Verificar se o email já existe
        if (Clients.Exists(c => c.email == client.email))
            throw new Exception("Cliente com este email já existe!");

        // Gerar ID automaticamente
        client.id = Clients.Any() ? Clients.Max(c => c.id) + 1 : 1;

        Clients.Add(client);
        Close();
    }

    // Deleta um cliente pelo ID
    public static void DelCliente(int id)
    {
        Open();

        Cliente client = null;
        foreach (var c in Clients)
        {
            if (c.id == id)
            {
                client = c;
                break;
            }
        }

        if (client == null)
            throw new Exception("Cliente não encontrado!");

        Clients.Remove(client);
        Close();
    }

    // Lista todos os clientes
    public static List<Cliente> ListarClientes()
    {
        Open();
        return Clients;
    }

    // Atualiza os dados de um cliente
    public static void AttCliente(Cliente client)
    {
        Open();

        Cliente existingClient = null;
        foreach (var c in Clients)
        {
            if (c.id == client.id)
            {
                existingClient = c;
                break;
            }
        }

        if (existingClient == null)
            throw new Exception("Cliente não encontrado!");

        // Atualizar dados
        existingClient.nome = client.nome;
        existingClient.email = client.email;
        existingClient.telefone = client.telefone;
        existingClient.senha = client.senha;
        Close();
    }

    // Carrega os dados do arquivo JSON
    private static void Open()
    {
        if (File.Exists("ClientList.json"))
        {
            try
            {
                string data = File.ReadAllText("ClientList.json");
                Clients = JsonSerializer.Deserialize<List<Cliente>>(data) ?? new List<Cliente>();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao abrir o arquivo JSON.", ex);
            }
        }
    }

    // Salva os dados no arquivo JSON
    private static void Close()
    {
        try
        {
            string data = JsonSerializer.Serialize(Clients);
            File.WriteAllText("ClientList.json", data);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao salvar o arquivo JSON.", ex);
        }
    }
}

class Cliente
{
    public int id { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string telefone { get; set; }
    public string senha{get;set; } 
    public Cliente() { }

    public Cliente(int id, string nome, string email, string telefone, string senha)
    {
        this.id = id;
        this.nome = nome;
        this.email = email;
        this.telefone = telefone;
        this.senha = senha;
    }

    public override string ToString()
    {
        return $"ID: {id}\nNome: {nome}\nEmail: {email}\nTelefone: {telefone}";
    }
}
