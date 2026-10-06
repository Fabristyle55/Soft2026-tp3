# Trabajo Práctico Integrador
## Desarrollo de Software

### Integrantes
- Salas Vallejo Facundo Raúl - 56635 - Facundo.SalasVallejo@alu.frt.utn.edu.ar
- Santillan Giuliano Fabrizio - 56439 - Fabrizio.SantillanGiuliano@alu.frt.utn.edu.ar
- Recalde Tomás Andres - 56703 - Tomas.Recalde@alu.frt.utn.edu.ar

## Intrucciones para configurar y ejecutar el proyecto manualmente
### Pre-Requisitos
- NET 8 SDK
- SQL Server (o la versión de localdb que viene con Visual Studio)
- Visual Studio Community 2022 (con ASP.NET CORE)
- Git

### Pasos
1. Clonar el repositorio: Abre una terminal y clona el repositorio en tu máquina local:
git clone https://github.com/DylanSDev/Dsw2025Tpi
cd Dsw2025Tpi/
git checkout dev

2. Configurar la base de datos: Asegúrate de que la cadena de conexión en Dsw2025Tpi.Api/program.cs sea correcta para tu entorno de SQL Server. La configuración actual usa (localdb)\\mssqllocaldb.
builder.Services.AddDbContext<Dsw2025TpiContext>(options =>
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Dsw2025TpiDb;Integrated Security=True"));

3. Aplicar las migraciones y seedear los datos: El proyecto está configurado para aplicar las migraciones automáticamente y cargar los datos de los clientes desde un archivo JSON al iniciar la aplicación. Puedes verificarlo en el archivo Dsw2025Tpi.Api/Program.cs.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Dsw2025TpiContext>();
        context.Database.Migrate(); // Aplica las migraciones pendientes
        context.Seedwork<Customer>("Sources/customers.json"); // Usa el método de extensión para seedear los clientes
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

4. Ejecutar el proyecto: Puedes ejecutar el proyecto directamente desde Visual Studio Community 2022. Simplemente abre el archivo de solución (.sln) y presiona F5 o el botón "Run" para iniciar la API.
Visual Studio debería restaurar automáticamente los paquetes NuGet. Si no lo hace, haz clic derecho en la solución en el "Explorador de soluciones" y selecciona "Restaurar paquetes NuGet".
Alternativamente, puedes usar la línea de comandos:
dotnet run --project Dsw2025Tpi.Api/Dsw2025Tpi.Api.csproj

5. Acceder a Swagger: Una vez que la aplicación se esté ejecutando, Swagger se abrirá automáticamente en tu navegador. Si no es así, puedes acceder a la documentación de la API en 
https://localhost:7138/swagger/index.html o la URL que se muestre en tu terminal.


### EndPoints Aplicados hasta el punto 6 incluido
●	1. Crear un producto
○	Método HTTP: POST
○	Ruta: /api/products
○	Descripción: Crea un nuevo producto con los datos proporcionados en el cuerpo de la solicitud.
○	Cuerpo de la Solicitud (Ejemplo JSON):
JSON
{
 "sku": "ABC123",
 "internalCode": "INT-001",
 "name": "Producto de ejemplo",
 "description": "Descripción del producto de ejemplo.",
 "currentUnitPrice": 199.99,
 "stockQuantity": 50
}
○	Respuesta Exitosa: 201 Created con el objeto del producto creado.
○	Respuesta de Error: 400 Bad Request si los datos enviados no son válidos.
●	2. Obtener todos los productos:
○	Método HTTP: GET
○	Ruta: /api/products
○	Descripción: Retorna la lista completa de productos disponibles.
○	Respuesta Exitosa: 200 OK con una colección de objetos producto.
○	Respuesta de Error: 204 No Content si no hay productos registrados
●	3. Obtener un producto por ID:
○	Método HTTP: GET
○	Ruta: /api/products/{id}
○	Descripción: Retorna los detalles completos de un producto específico.
○	Respuesta Exitosa: 200 OK con el objeto del producto solicitado.
○	Respuesta de Error: 404 Not Found si no se encuentra un producto con el ID proporcionado.
●	4. Actualizar un producto:
○	Método HTTP: PUT
○	Ruta: /api/products/{id}
○	Descripción: Actualiza los datos de un producto existente con el ID proporcionado, usando los datos enviados en el cuerpo de la solicitud.
○	Cuerpo de la Solicitud (Ejemplo JSON):
 
JSON
{
 "sku": "ABC123",
 "internalCode": "INT-001",
 "name": "Producto de ejemplo",
 "description": "Descripción del producto de ejemplo.",
 "currentUnitPrice": 199.99,
 "stockQuantity": 50
}
○	Respuesta Exitosa: 200 OK con el objeto del producto actualizado.
○	Respuesta de Error:
○	•	400 Bad Request si los datos enviados no son válidos.
○	•	404	Not Found si no se encuentra un producto con el ID proporcionado.
●	5. Inhabilitar un producto:
○	Método HTTP: PATCH
○	Ruta: /api/products/{id}
○	Descripción: Modificar el atributo IsActive para inhabilitarlo.
○	Respuesta Exitosa: 204 No Content si la operación fue exitosa.
○	Respuesta de Error: 404 Not Found si no se encuentra un producto con el ID proporcionado.
●	6. Crear una nueva orden:
○	Método HTTP: POST
○	Ruta: /api/orders
○	Descripción: Permite registrar una nueva orden de compra en el sistema. La orden debe incluir un identificador de cliente (simulado), direcciones de envío y facturación, y una lista de ítems que componen la orden (productos con sus cantidades y precios unitarios al momento de la compra). Antes de crear la orden, se debe verificar que haya suficiente stock disponible para cada producto solicitado. Si el stock es insuficiente para algún producto, la orden no debe crearse y se debe retornar un error. Si la orden se crea exitosamente, el stock de los productos involucrados debe ser decrementado. No se debe implementar ninguna lógica de pago.
○	Cuerpo de la Solicitud (Ejemplo JSON):
JSON
{
 "customerId": "a1b2c3d4-e5f6-7890-1234-567890abcdef", // simulado para el cliente
 "shippingAddress": "Calle Falsa 123, Ciudad, País",
 "billingAddress": "Calle Falsa 123, Ciudad, País",
 "orderItems": [
	{
	"productId": 1234567890,
	"quantity": 1,
 
	"name": "Producto 1",
	"description": "Descripción del producto 1",
	"currentUnitPrice": 100.00
	}, …
  ]
 }

○	Respuesta Exitosa: 201 Created con los detalles completos de la orden creada, incluyendo su OrderId generado.
○	Respuesta de Error: 400 Bad Request si los datos de la solicitud son inválidos o incompletos, o si no hay stock suficiente para uno o más productos.

En todos los casos, ante un error del lado del servidor se debe retornar el código de estado 500.



### Backend

## Introducción
Se desea desarrollar una plataforma de comercio electrónico (E-commerce). 
En esta primera etapa el objetivo es construir el módulo de Órdenes, permitiendo la gestión completa de éstas.

## Visión General del Producto
Del relevamiento preliminar se identificaron los siguientes requisitos:
- Los visitantes pueden consultar los productos sin necesidad de estar registrados o iniciar sesión.
- Para realizar un pedido se requiere el inicio de sesión.
- Una orden, para ser aceptada, debe incluir la información básica del cliente, envío y facturación.
- Antes de registrar la orden se debe verificar la disponibilidad de stock (o existencias) de los productos.
- Si la orden es exitosa hay que actualizar el stock de cada producto.
- Se deben poder consultar órdenes individuales o listar varias con posibilidad de filtrado.
- Será necesario el cambio de estado de una orden a medida que avanza en su ciclo de vida.
- Los administradores solo pueden gestionar los productos (alta, modificación y baja) y actualizar el estado de la orden.
- Los clientes pueden crear y consultar órdenes.

[Documento completo](https://frtutneduar.sharepoint.com/:b:/s/DSW2025/ETueAd4rTe1Gilj_Yfi64RYB5oz9s2dOamxKSfMFPREbiA?e=azZcwg) 

## Alcance para el Primer Parcial
> [!IMPORTANT]
> Del apartado `IMPLEMENTACIÓN` (Pag. 7), completo hasta el punto `6` (inclusive)


### Características de la Solución

- Lenguaje: C# 12.0
- Plataforma: .NET 8
