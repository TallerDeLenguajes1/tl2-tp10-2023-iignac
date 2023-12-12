using System.Data.SQLite;
using System.Linq.Expressions;
using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public class TareaRepository : ITareaRepository
{
    private readonly string connectionString;

    public TareaRepository(string CadenaDeConexion)
    {
        connectionString = CadenaDeConexion;
    }

    public void CreateTarea(Tarea nuevaTarea){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) 
                VALUES (@idTablero, @nombreTarea, @estadoTarea, @descripcionTarea, @colorTarea, @idUsuarioAsignado);";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTablero", nuevaTarea.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@nombreTarea", nuevaTarea.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estadoTarea", Convert.ToInt32(nuevaTarea.Estado))); //(int)nuevaTarea.Estado
                command.Parameters.Add(new SQLiteParameter("@descripcionTarea", nuevaTarea.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@colorTarea", nuevaTarea.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", nuevaTarea.IdUsuarioAsignado));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception("Hubo un problema al registrar una nueva tarea");
        }
    }

    public void UpdateTarea(Tarea tareaModificar){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Tarea SET id_tablero = @idTablero, nombre = @nombreTarea, estado = @estadoTarea, 
                descripcion = @descripcionTarea, color = @colorTarea, id_usuario_asignado = @idUsuarioAsignado 
                WHERE id = @idTarea;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTarea", tareaModificar.Id));
                command.Parameters.Add(new SQLiteParameter("@idTablero", tareaModificar.IdTablero));
                command.Parameters.Add(new SQLiteParameter("@nombreTarea", tareaModificar.Nombre));
                command.Parameters.Add(new SQLiteParameter("@estadoTarea", tareaModificar.Estado)); //Convert.ToInt32(tareaModificar.Estado)
                command.Parameters.Add(new SQLiteParameter("@descripcionTarea", tareaModificar.Descripcion));
                command.Parameters.Add(new SQLiteParameter("@colorTarea", tareaModificar.Color));
                command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tareaModificar.IdUsuarioAsignado));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception("Hubo un problema al actualizar la tarea existente");
        }
    }

    public List<Tarea> GetTareasDeTablero(int idTablero){
        try{
            List<Tarea> listaTareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IdTablero = Convert.ToInt32(reader["id_tablero"]),
                            Nombre = reader["nombre"].ToString(),
                            Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                            Descripcion = reader["descripcion"].ToString(),
                            Color = reader["color"].ToString(),
                            IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null
                        };
                        listaTareas.Add(tarea);
                    }
                }
                connection.Close();
            }
            return listaTareas;
        }catch(Exception){
            throw new Exception("Hubo un problema en la base de datos para realizar la lectura de datos de las tareas");
        }
    }

    public Tarea GetTarea(int idTarea){
        Tarea tareaEncontrada = null;
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Tarea WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tareaEncontrada = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null
                    };
                }
            }
            connection.Close();
        }
        if(tareaEncontrada == null) throw new Exception("Tarea inexistente");
        return tareaEncontrada;
    }

    public void RemoveTarea(int idTarea){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"DELETE FROM Tarea WHERE id = @idTarea;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception($"Hubo un problema al eliminar la tarea de id: '{idTarea}'");
        }
    }

    /*
    public bool AsignarUsuario(int idUsuario, int idTarea){
        bool tareaAsignada = false;
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            if(command.ExecuteNonQuery() > 0) tareaAsignada=true;
            connection.Close();
        }
        return tareaAsignada;
    }
    
    public bool UpdateNombre(int idTarea, string nuevoNombre){
        bool tareaModificada = false;
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"UPDATE Tarea SET nombre = @nuevoNombre WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@nuevoNombre", nuevoNombre));
            if(command.ExecuteNonQuery() > 0) tareaModificada=true;
            connection.Close();
        }
        return tareaModificada;
    }

    public bool UpdateEstado(int idTarea, EstadoTarea nuevoEstado){
        bool tareaModificada = false;
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"UPDATE Tarea SET estado = @nuevoEstado WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@nuevoEstado", nuevoEstado));
            if(command.ExecuteNonQuery() > 0) tareaModificada=true;
            connection.Close();
        }
        return tareaModificada;
    }

    public List<Tarea> GetAllByIdUsuario(int idUsuario){
        List<Tarea> listaTareas = new List<Tarea>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @idUsuarioAsignado;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", idUsuario));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea) Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null;
                    listaTareas.Add(tarea);
                }
            }
            connection.Close();
        }
        return listaTareas;
    }

    public int GetCantTareasSegunEstado(EstadoTarea estado){
        int cant;
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT count(id) FROM Tarea WHERE estado = @estadoTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@estadoTarea", estado));
            cant = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
        }
        return cant;
    }
    */
}