using System.Text.Json;

namespace AndresPa.Ejemplos.Cliente.Almacen;

internal static class ClientesAlmacen
{
    private static List<ClienteEntidad> clientes = [];

    public static IReadOnlyCollection<ClienteEntidad> Clientes => clientes.AsReadOnly();

    public static void Grabar()
    {
        var datos = JsonSerializer.Serialize(clientes);
        File.WriteAllText("Clientes.json", datos);
    }

    public static void Leer()
    {
        if (!File.Exists(@"Clientes.json"))
        {
            return;
        }

        var datos = File.ReadAllText("Clientes.json");

        clientes = JsonSerializer.Deserialize<List<ClienteEntidad>>(datos)!;
    }
}
