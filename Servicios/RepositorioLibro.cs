using Dapper;
using Microsoft.Data.SqlClient;
using crudCuenta.Models;


namespace crudCuenta.Servicios
{
    public interface IRepositorioLibro
    {
        Task actualizar(LibroViewModel libro);
        Task borrar(int id);
        Task crear(LibroViewModel libro);
        Task<IEnumerable<LibroViewModel>> obtenerLibros();
        Task<LibroViewModel> obtenerPorId(int id);
    }

    public class RepositorioLibro: IRepositorioLibro
    {
        private readonly string connectionString;
        public RepositorioLibro(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }   

        public async Task<IEnumerable<LibroViewModel>>obtenerLibros()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<LibroViewModel>(
                    @"SELECT id_libro AS GsIdLibro, nombre AS GsNombreLibro, autor as GsNombreAutor, isbn as GsIsbn FROM tbl_libro"
                );
        }
        
        public async Task<LibroViewModel>obtenerPorId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<LibroViewModel>(
                    @"SELECT id_libro AS GsIdLibro, nombre AS GsNombreLibro, autor as GsNombreAutor, isbn as GsIsbn 
                    FROM tbl_libro 
                    WHERE id_libro = @id", new {id}
                );
        }

        public async Task crear(LibroViewModel libro)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.QueryAsync("usp_guardar_libro", new
            {
                nombre = libro.GsNombreLibro,
                autor = libro.GsNombreAutor,
                isbn = libro.GsIsbn
            }, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task actualizar(LibroViewModel libro)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("usp_actualizar_libro", new
            {
                id_libro = libro.GsIdLibro,
                nombre = libro.GsNombreLibro,
                autor = libro.GsNombreAutor,
                isbn = libro.GsIsbn
            }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync("usp_eliminar_libro",
                new { id_libro = id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
