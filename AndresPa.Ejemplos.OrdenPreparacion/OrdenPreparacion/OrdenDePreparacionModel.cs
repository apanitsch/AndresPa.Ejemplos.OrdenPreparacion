using AndresPa.Ejemplos.Cliente.Almacen;
using AndresPa.Ejemplos.Deposito.Almacen;
using AndresPa.Ejemplos.OrdenPreparacion.Almacen;

namespace AndresPa.Ejemplos.OrdenPreparacion;

internal partial class OrdenDePreparacionModel
{
    //Agregamos esta propiedad y hacemos que la pantalla, al seleccionar un cliente, le 
    //pase este dato al modelo.
    public string ClienteSeleccionado { get; set; }
    public int DepositoSeleccionado { get; set; }

    public List<Producto> ProductosDisponibles
    {
        get
        {
            var clienteEntidad = ClientesAlmacen.Clientes.FirstOrDefault(c => c.CUIT == ClienteSeleccionado);
            if (clienteEntidad == null)
            {
                return new List<Producto>();
            }

            var depositoEntidad = DepositosAlmacen.Depositos.FirstOrDefault(d => d.DepositoId == DepositoSeleccionado);
            if (depositoEntidad == null)
            {
                return new List<Producto>();
            }

            var productosCliente = new List<Producto>();

            foreach (var productoEntidad in ProductosAlmacen.Productos)
            {
                if (productoEntidad.ClienteId == clienteEntidad.ClienteId)
                {
                    var productoModelo = new Producto();
                    productoModelo.Id = productoEntidad.ProductoId.ToString();
                    productoModelo.Cantidad = productoEntidad.CalcularTotalStock(depositoEntidad.DepositoId);
                    productoModelo.Descripcion = productoEntidad.Nombre;

                    if (productoModelo.Cantidad == 0)
                    {
                        continue;
                    }

                    var productoAgregado = ProductosAgregados.FirstOrDefault(pa => pa.Id == productoEntidad.ProductoId.ToString());
                    if (productoAgregado != null) //significa que ya está este producto en la orden
                    {
                        continue;
                    }

                    productosCliente.Add(productoModelo);
                }
            }

            return productosCliente;
        }
    }

    public int DniTransportista { get; set; }
    public List<Producto> ProductosAgregados { get; private set; }
    public List<Cliente> Clientes { get; private set; }
    public List<Deposito> Depositos { get; private set; }

    public OrdenDePreparacionModel()
    {
        Depositos = DepositosAlmacen.Depositos.Select(d => new Deposito
        {
            Id = d.DepositoId,
            Direccion = d.Nombre
        }).ToList();

        Clientes = new List<Cliente>();
        foreach (var clienteEntidad in ClientesAlmacen.Clientes)
        {
            var clienteModelo = new Cliente();
            clienteModelo.Documento = clienteEntidad.CUIT;
            clienteModelo.Nombre = clienteEntidad.Nombre;
        }

        ProductosAgregados = [];
    }

    public string? ValidarDniTransportista(string dniText)
    {
        var isDniCompleto = !string.IsNullOrWhiteSpace(dniText);

        if (!isDniCompleto)
        {
            return "Por favor complete el campo DNI Transportista.";
        }

        if (!int.TryParse(dniText, out _))
        {
            return "El DNI de Transportista debe ser un número válido. Por favor ingrese un valor correcto.";
        }

        if (dniText.Length < 7)
        {
            return "El DNI de Transportista debe tener como mínimo 7 dígitos. Por favor ingrese un valor correcto.";
        }

        if (dniText.Length > 8)
        {
            return "El DNI de Transportista debe tener como máximo 8 dígitos. Por favor ingrese un valor correcto.";
        }

        return null;
    }

    public string? ValidarPrioridad(bool isPrioridadSeleccionada)
    {
        if (!isPrioridadSeleccionada)
        {
            return "Por favor complete el campo Prioridad.";
        }

        return null;

    }

    public string? ValidarCliente(string documentoCliente)
    {
        if (documentoCliente is null or "")
        {
            return "Por favor seleccione un cliente valido.";
        }

        return null;
    }

    public string? ValidarDeposito(string idDepositoOpcion)
    {
        if (idDepositoOpcion is null or "")
        {
            return "Por favor seleccione un deposito valido.";
        }

        return null;
    }

    public string? ValidarCantidades(int cantidad1, int cantidad2, string descripcion, string cantidadItem)
    {
        if (cantidad1 < 1)
        {
            return "No pueden agregarse a la órden de preparación " + cantidad1 + " unidades del producto " + descripcion + " ya que debe ser igual o superior a 1. Por favor intente con un valor igual o menor a " + cantidadItem + " pero mayor que 0.";
        }

        if (cantidad1 > cantidad2)
        {
            return "No pueden agregarse a la órden de preparación " + cantidad1 + " unidades del producto " + descripcion + " ya que solo se cuentan con " + cantidadItem + " unidades. Por favor intente con un valor igual o menor a " + cantidadItem;
        }

        return null;
    }

    public string? ValidarProductosAgregados(int cantidadProductosAgregados)
    {
        if (cantidadProductosAgregados < 1)
        {
            return "Debe agregar al menos 1 producto a la orden.";
        }

        return null;
    }

    private bool IsDateValid(DateTime selectedDate) =>
        // Comparar con la fecha actual
        selectedDate >= DateTime.Today;

    public string? ValidarFechaEntrega(DateTime fechaEntrega)
    {
        if (!IsDateValid(fechaEntrega))
        {
            return "Elige una fecha valida. No pueden elegirse fechas pasadas";
        }

        return null;
    }

    public string? AgregarProducto(string id, string descripcion, int cantidad1, int cantidad2)
    {
        // Validar la cantidad
        var errorCantidad = ValidarCantidades(cantidad1, cantidad2, descripcion, cantidad2.ToString());
        if (errorCantidad != null)
        {
            return errorCantidad; // Retornar el error si hay uno
        }

        // Agregar el producto a la lista de ProductosAgregados
        var producto = new Producto
        {
            Id = id,
            Descripcion = descripcion,
            Cantidad = cantidad1
        };

        ProductosAgregados.Add(producto);
        return null; // Sin errores
    }

    public void EliminarProducto(string id)
    {
        var producto = ProductosAgregados.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _ = ProductosAgregados.Remove(producto);
        }
    }

    public void EliminarTodosLosProductos()
    {
        ProductosAgregados = [];
    }

    public string CrearOrden(string documentoCliente, string nombreCliente, int dniTransportista, PrioridadEnum prioridad, DateTime fechaEntrega)
    {
        //Crear una orden de preparacion (entidad)
        //pasarsela al archivo.

        var nuevaOrden = new OrdenPreparacionEntidad();

        nuevaOrden.Prioridad = prioridad switch
        {
            PrioridadEnum.Alta => Prioridades.Alta,
            PrioridadEnum.Media => Prioridades.Media,
            PrioridadEnum.Baja => Prioridades.Baja,
            _ => throw new Exception($"Prioridad no contemplada: {prioridad}")
        };

        //depende como funciona la pantalla, acá puede caer un documento invalido o no.
        var cliente = ClientesAlmacen.Clientes.Where(c => c.CUIT == documentoCliente).FirstOrDefault();
        if (cliente == null)
        {
            return "No hay un cliente seleccionado.";
        }

        nuevaOrden.ClienteId = cliente.ClienteId;
        nuevaOrden.DniTransportista = dniTransportista;
        nuevaOrden.FechaEntrega = fechaEntrega;
        nuevaOrden.FechaEmision = DateTime.Now;

        foreach (var producto in ProductosAgregados)
        {
            var nuevoProductoOrden = new OrdenPreparacionDetalle();
            nuevoProductoOrden.ProductoId = int.Parse(producto.Id);
            nuevoProductoOrden.Cantidad = producto.Cantidad;
        }

        string error = OrdenesPreparacionAlmacen.Nueva(nuevaOrden);
        if (error != null)
        {
            return null;
        }

        // Retornar un mensaje de éxito
        return $"Orden Creada Satisfactoriamente. ID de Orden: {nuevaOrden.OrdenPreparacionId}. Fecha de emisión: {nuevaOrden.FechaEmision:dd/MM/yyyy HH:mm}";
    }
}
