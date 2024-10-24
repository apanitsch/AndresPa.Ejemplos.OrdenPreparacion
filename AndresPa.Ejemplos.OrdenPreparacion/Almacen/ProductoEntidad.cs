namespace AndresPa.Ejemplos.OrdenPreparacion.Almacen;
internal class ProductoEntidad
{
    public int ProductoId { get; set; }
    public int ClienteId { get; set; }
    public string Nombre { get; set; }
    public List<ProductoStock> Stock { get; } = [];


    //Es importante que un valor calculado sea un METODO.
    public int CalcularTotalStock(int depositoId) => Stock.Where(s => s.DepositoId == depositoId).Sum(s => s.Cantidad); //LINQ
}
