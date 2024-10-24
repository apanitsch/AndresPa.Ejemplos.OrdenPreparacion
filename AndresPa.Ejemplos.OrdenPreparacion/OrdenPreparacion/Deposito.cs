namespace AndresPa.Ejemplos.OrdenPreparacion
{
    partial class OrdenDePreparacionModel
    {
        public class Deposito
        {
            public int Id { get; set; }
            public string Direccion { get; set; }
            public string DisplayText
            {
                get { return $"Deposito ID {Id}. {Direccion}"; }
            }
        }
    }
}
