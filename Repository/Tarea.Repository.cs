using System.Data.SQLite;

namespace tl2_tp10_2023_iignac.Models;

public class TareaRepository : ITareaRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void Create(Tarea nuevaTarea){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
    }

    public void Update(Tarea tareaModificar){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
    }

    public List<Tarea> GetAllByIdTablero(int idTablero){
        List<Tarea> listaTareas = new List<Tarea>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
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

    public Tarea GetByIdTarea(int idTarea){
        Tarea tarea = new Tarea();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Tarea WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea) Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null;
                }
            }
            connection.Close();
        }
        return tarea;
    }

    public void Remove(int idTarea){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"DELETE FROM Tarea WHERE id = @idTarea;";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.ExecuteNonQuery();
            connection.Close();
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