using System.Text.Json;

static class ClientList
{
    private static List<Client> Clients = new List<Client>();

    // Insere um novo cliente
    public static void Insert(Client client)
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
    public static void Delete(int id)
    {
        Open();

        Client client = null;
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
    public static List<Client> ListAll()
    {
        Open();
        return Clients;
    }

    // Atualiza os dados de um cliente
    public static void Update(Client client)
    {
        Open();

        Client existingClient = null;
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
        existingClient.name = client.name;
        existingClient.email = client.email;
        existingClient.phone = client.phone;
        existingClient.password = client.password;
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
                Clients = JsonSerializer.Deserialize<List<Client>>(data) ?? new List<Client>();
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

class Client
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string password;

    public string Password
    {
        get => "******"; // Expor apenas asteriscos
        set => password = value; 
    }

    public Client() { }

    public Client(int id, string name, string email, string phone, string password)
    {
        this.id = id;
        this.name = name;
        this.email = email;
        this.phone = phone;
        this.password = password;
    }

    public override string ToString()
    {
        return $"ID: {id}\nNome: {name}\nEmail: {email}\nTelefone: {phone}";
    }
}
