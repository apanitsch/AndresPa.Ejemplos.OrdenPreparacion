using System.Text.Json;

namespace AndresPa.Ejemplos.OrdenPreparacion.Almacen;

internal static class OrdenesPreparacionAlmacen
{
    private static List<OrdenPreparacionEntidad> ordenesPreparacion = [];

    public static IReadOnlyCollection<OrdenPreparacionEntidad> OrdenesPreparacion => ordenesPreparacion.AsReadOnly();

    public static void Grabar()
    {
        var datos = JsonSerializer.Serialize(ordenesPreparacion);
        File.WriteAllText("OrdenesPreparacion.json", datos);
    }

    public static void Leer()
    {
        if (!File.Exists(@"OrdenesPreparacion.json"))
        {
            return;
        }

        var datos = File.ReadAllText("OrdenesPreparacion.json");

        ordenesPreparacion = JsonSerializer.Deserialize<List<OrdenPreparacionEntidad>>(datos)!;
    }

    internal static string Nueva(OrdenPreparacionEntidad nuevaOrden)
    {
        if (OrdenesPreparacionAlmacen.OrdenesPreparacion.Count == 0)
        {
            nuevaOrden.OrdenPreparacionId = 1;
        }
        else
        {
            nuevaOrden.OrdenPreparacionId = OrdenesPreparacionAlmacen.OrdenesPreparacion.Max(o => o.OrdenPreparacionId);
        }

        //Tenemos la posibilidad de realizar más validaciones.


        ordenesPreparacion.Add(nuevaOrden);
        return null; //sin errores.
    }
}
