using System.Text.Json;

namespace AndresPa.Ejemplos.Deposito.Almacen;

internal static class DepositosAlmacen
{
    private static List<DepositoEntidad> depositos = [];

    public static IReadOnlyCollection<DepositoEntidad> Depositos => depositos.AsReadOnly();

    public static void Grabar()
    {
        var datos = JsonSerializer.Serialize(depositos);
        File.WriteAllText("Depositos.json", datos);
    }

    public static void Leer()
    {
        if (!File.Exists(@"Depositos.json"))
        {
            return;
        }

        var datos = File.ReadAllText("Depositos.json");

        depositos = JsonSerializer.Deserialize<List<DepositoEntidad>>(datos)!;
    }
}
