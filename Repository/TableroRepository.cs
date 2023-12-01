using System.Data.SQLite;

namespace tl2_tp10_2023_iignac.Models;

public class TableroRepository : ITableroRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void Create(Tablero nuevoTablero){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
    }

    public void Update(Tablero tableroModificar){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"UPDATE Tablero SET id_usuario_propietario = @idUsProp, nombre = @nombreTab, descripcion = @descripcionTab WHERE id = @idTab;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTab", tableroModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@idUsProp", tableroModificar.IdUsuarioPropietario));
            command.Parameters.Add(new SQLiteParameter("@nombreTab", tableroModificar.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcionTab", tableroModificar.Descripcion));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tablero> GetAll(){
        List<Tablero> listaTableros = new List<Tablero>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tablero;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    listaTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaTableros;
    }

    public Tablero GetByIdTablero(int idTablero){
        Tablero tablero = new Tablero();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tablero WHERE id = @idTab;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }
            }
            connection.Close();
        }
        return tablero;
    }

    public List<Tablero> GetAllByIdUsuario(int idUsuario){
        List<Tablero> listaTableros = new List<Tablero>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tablero WHERE id_usuario_propietario = @idUs;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tablero tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    listaTableros.Add(tablero);
                }
            }
            connection.Close();
        }
        return listaTableros;
    }

    public void Remove(int idTablero){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"DELETE FROM Tablero WHERE id = @idTab;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTab", idTablero));
            command.ExecuteNonQuery();
            connection.Close();
        }
    } 
}