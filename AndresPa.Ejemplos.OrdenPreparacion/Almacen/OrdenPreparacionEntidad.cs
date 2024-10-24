namespace AndresPa.Ejemplos.OrdenPreparacion.Almacen;


public class OrdenPreparacionEntidad
{
    public int OrdenPreparacionId { get; set; }
    public int ClienteId { get; set; }
    public EstadosOrdenPreparacion Estado { get; set; }
    public int DniTransportista { get; set; }
    public Prioridades Prioridad { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaEntrega { get; set; }

    public List<OrdenPreparacionDetalle> Detalle { get; } = new();
}
