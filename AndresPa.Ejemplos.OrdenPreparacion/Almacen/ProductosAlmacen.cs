using System.Text.Json;

namespace AndresPa.Ejemplos.OrdenPreparacion.Almacen;

internal static class ProductosAlmacen
{
    private static List<ProductoEntidad> productos = [];

    public static IReadOnlyCollection<ProductoEntidad> Productos => productos.AsReadOnly();

    public static void Grabar()
    {
        var datos = JsonSerializer.Serialize(productos);
        File.WriteAllText("Productos.json", datos);
    }

    public static void Leer()
    {
        if (!File.Exists(@"Productos.json"))
        {
            return;
        }

        var datos = File.ReadAllText("Productos.json");

        productos = JsonSerializer.Deserialize<List<ProductoEntidad>>(datos)!;
    }
}
