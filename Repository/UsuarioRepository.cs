using System.Data.SQLite;

namespace tl2_tp10_2023_iignac.Models;

public class UsuarioRepository : IUsuarioRepository
{
    private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared"; //@"Data...

    public void Create(Usuario nuevoUsuario){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombreUs, @contraseniaUs, @rolUs);";  //$INSERT..."; 
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUs", nuevoUsuario.NombreUsuario));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUs", nuevoUsuario.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rolUs", Convert.ToInt32(nuevoUsuario.RolUsuario)));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(Usuario usuarioModificar){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query;
            if(!string.IsNullOrEmpty(usuarioModificar.Contrasenia)){
                query = @"UPDATE Usuario SET nombre_de_usuario = @nombreUs, contrasenia = @contraseniaUs, rol = @rolUs WHERE id = @idUs;"; //$UPDATE...
            }else{
                query = @"UPDATE Usuario SET nombre_de_usuario = @nombreUs, rol = @rolUs WHERE id = @idUs;"; //$UPDATE...
            }
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUs", usuarioModificar.Id));
            command.Parameters.Add(new SQLiteParameter("@nombreUs", usuarioModificar.NombreUsuario));
            if(!string.IsNullOrEmpty(usuarioModificar.Contrasenia)) command.Parameters.Add(new SQLiteParameter("@contraseniaUs", usuarioModificar.Contrasenia));
            command.Parameters.Add(new SQLiteParameter("@rolUs", Convert.ToInt32(usuarioModificar.RolUsuario)));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Usuario> GetAll(){
        List<Usuario> listaUsuarios = new List<Usuario>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Usuario;"; //$SELECT...
            SQLiteCommand command = new SQLiteCommand(query, connection);
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.RolUsuario = (Rol) Convert.ToInt32(reader["rol"]);
                    listaUsuarios.Add(usuario);
                }
            }
            connection.Close();
        }
        return listaUsuarios;
    }

    public Usuario GetUsuario(int idUsuario){
        Usuario usuario = new Usuario();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Usuario WHERE id = @idUs;"; 
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.RolUsuario = (Rol) Convert.ToInt32(reader["rol"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }

    public Usuario GetUsuario(string nombre, string contrasenia){
        Usuario usuario = new Usuario();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contraseniaUsuario;"; 
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombre));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUsuario", contrasenia));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                    usuario.RolUsuario = (Rol) Convert.ToInt32(reader["rol"]);
                }
            }
            connection.Close();
        }
        return usuario;
    }

    public void Remove(int idUsuario){
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            string query = @"DELETE FROM Usuario WHERE id = @idUs;"; //$DELETE...
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
            command.ExecuteNonQuery();
            connection.Close();
        }
    } 
}