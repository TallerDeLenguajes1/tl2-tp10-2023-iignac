using System.Data.SQLite;
using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public class TableroRepository : ITableroRepository
{
    private readonly string connectionString;

    public TableroRepository(string CadenaDeConexion)
    {
        connectionString = CadenaDeConexion;
    }

    public void CreateTablero(Tablero nuevoTablero){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Tablero (id_usuario_propietario, nombre, descripcion) 
                VALUES (@idUsProp, @nombreTab, @descripcionTab);";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsProp", nuevoTablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombreTab", nuevoTablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcionTab", nuevoTablero.Descripcion));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception("Hubo un problema al crear un nuevo tablero");
        }
    }

    public void UpdateTablero(Tablero tablero){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Tablero SET id_usuario_propietario = @idUsProp, nombre = @nombreTab, descripcion = @descripcionTab WHERE id = @idTab;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTab", tablero.Id));
                command.Parameters.Add(new SQLiteParameter("@idUsProp", tablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombreTab", tablero.Nombre));
                command.Parameters.Add(new SQLiteParameter("@descripcionTab", tablero.Descripcion));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception("Hubo un problema al modificar el tablero");
        }
    }

    public List<Tablero> GetAllTableros(){
        try{
            List<Tablero> listaTableros = new List<Tablero>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Tablero;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()){
                        Tablero tablero = new Tablero
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString()
                        };
                        listaTableros.Add(tablero);
                    }
                }
                connection.Close();
            }
            return listaTableros;
        }catch(Exception){
            throw new Exception("Hubo un problema con la base de datos al momento de hacer la lectura de datos de tableros");
        }
    }

    public Tablero GetTablero(int idTablero){
        Tablero tableroEncontrado = null;
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Tablero WHERE id = @idTab;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tableroEncontrado = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                }
            }
            connection.Close();
        }
        if(tableroEncontrado == null) throw new Exception("Tablero inexistente");
        return tableroEncontrado;
    }

    public List<Tablero> GetTablerosDeUsuario(int idUsuario){
        try{
            List<Tablero> listaTableros = new List<Tablero>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idUs;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tablero tablero = new Tablero
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString()
                        };
                        listaTableros.Add(tablero);
                    }
                }
                connection.Close();
            }
            return listaTableros;
        }catch(Exception){
            throw new Exception("Hubo un problema con la base de datos al hacer la lectura de datos de tableros");
        }
    }

    public void RemoveTablero(int idTablero){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"DELETE FROM Tablero WHERE id = @idTab;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
                command.ExecuteNonQuery();
                connection.Close();
            }       
        }catch(Exception){
            throw new Exception("Hubo un problema con la base de datos al eliminar el tablero");
        }
    }
}