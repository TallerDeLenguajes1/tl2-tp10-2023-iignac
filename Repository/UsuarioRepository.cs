using System.CodeDom;
using System.Data.SQLite;
using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string connectionString;

    public UsuarioRepository(string CadenaDeConexion){
        connectionString = CadenaDeConexion;
    }

    public void CreateUsuario(Usuario usuario){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombreUs, @contraseniaUs, @rolUs);";  //$INSERT..."; 
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombreUs", usuario.NombreUsuario));
                command.Parameters.Add(new SQLiteParameter("@contraseniaUs", usuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rolUs", Convert.ToInt32(usuario.RolUsuario)));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception("Hubo un problema al crear un nuevo usuario");
        }
    }

    public void UpdateUsuario(Usuario usuario){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query;
                if(!string.IsNullOrEmpty(usuario.Contrasenia)){
                    query = @"UPDATE Usuario SET nombre_de_usuario = @nombreUs, contrasenia = @contraseniaUs, rol = @rolUs WHERE id = @idUs;"; //$UPDATE...
                }else{
                    query = @"UPDATE Usuario SET nombre_de_usuario = @nombreUs, rol = @rolUs WHERE id = @idUs;"; //$UPDATE...
                }
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUs", usuario.Id));
                command.Parameters.Add(new SQLiteParameter("@nombreUs", usuario.NombreUsuario));
                if(!string.IsNullOrEmpty(usuario.Contrasenia)) command.Parameters.Add(new SQLiteParameter("@contraseniaUs", usuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rolUs", Convert.ToInt32(usuario.RolUsuario)));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception($"Hubo un problema al modificar al usuario de nombre '{usuario.NombreUsuario}'");
        }
    }

    public List<Usuario> GetAllUsuarios(){
        try{
            List<Usuario> listaUsuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT * FROM Usuario;"; //$SELECT...
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            NombreUsuario = reader["nombre_de_usuario"].ToString(),
                            Contrasenia = reader["contrasenia"].ToString(),
                            RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                        };
                        listaUsuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
            return listaUsuarios;
        }catch(Exception){
            throw new Exception("Hubo un problema en la base de datos para realizar la lectura de datos de los usuarios");
        }
    }

    public Usuario GetUsuario(int idUsuario){
        Usuario usuarioEncontrado = null;
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Usuario WHERE id = @idUs;"; 
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        Contrasenia = reader["contrasenia"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
                }
            }
            connection.Close();
        }
        if(usuarioEncontrado == null) throw new Exception("Usuario inexistente");
        return usuarioEncontrado;
    }

    public Usuario GetUsuario(string nombre, string contrasenia){
        Usuario usuarioEncontrado = null;
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUs AND contrasenia = @contraseniaUs;"; 
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@nombreUs", nombre));
            command.Parameters.Add(new SQLiteParameter("@contraseniaUs", contrasenia));
            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read()){
                    usuarioEncontrado = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        NombreUsuario = reader["nombre_de_usuario"].ToString(),
                        Contrasenia = reader["contrasenia"].ToString(),
                        RolUsuario = (Rol)Convert.ToInt32(reader["rol"])
                    };
                }
            }
            connection.Close();
        }
        if(usuarioEncontrado == null) throw new Exception("Usuario inexistente");
        return usuarioEncontrado;
    }

    public void RemoveUsuario(int idUsuario){
        try{
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"DELETE FROM Usuario WHERE id = @idUs;"; //$DELETE...
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUs", idUsuario));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }catch(Exception){
            throw new Exception($"Hubo un problema al eliminar al usuario de id '{idUsuario}'");
        }
    } 
}