using Domain.Endpoint.DTOs;
using Domain.Endpoint.Entities;
using Domain.Endpoint.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastucture.Endpoint.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISingletonSqlConnection _dbConexion;
        public UserRepository(ISingletonSqlConnection dbConexion)
        {
            _dbConexion = dbConexion;
        }

        public void Create(User user)
        {
            string insertQuery = "INSERT INTO Usuarios(UserId,FirstName,LastName,SchoolName,GradeLevel,Gender,Email,Password) " +
                "VALUES(@Id,@FirstName,@LastName,@SchoolName,@GradeLevel,@Gender,@Email,@Password)";
           
            SqlCommand sqlCommand = _dbConexion.GetCommand(insertQuery);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = user.Id
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.Email
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.Password
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@FirstName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.FirstName
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@LastName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.LastName
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@SchoolName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.SchoolName
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@GradeLevel",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.GradeLevel
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Gender",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = user.Gender
                }
            };
            sqlCommand.Parameters.AddRange(parameters);
            sqlCommand.ExecuteNonQuery();
        }

        public async Task<List<User>> GetAll()
        {
            string query = "SELECT * FROM Usuarios";
            DataTable dataTable =await _dbConexion.ExecuteQueryCommandAsync(query);
            List<User> usuario = dataTable.AsEnumerable()
                .Select(MapEntityFromDataRow)
                .ToList();

            return usuario;
        }

        private User MapEntityFromDataRow(DataRow row)
        {
            return new User()
            {
                Id = _dbConexion.GetDataRowValue<Guid>(row, "UserId"),
                FirstName = _dbConexion.GetDataRowValue<string>(row, "FirstName"),
                LastName = _dbConexion.GetDataRowValue<string>(row, "LastName"),
                Email = _dbConexion.GetDataRowValue<string>(row, "Email"),
                Password = _dbConexion.GetDataRowValue<string>(row, "Password"),
                SchoolName = _dbConexion.GetDataRowValue<string>(row, "SchoolName"),
                GradeLevel = _dbConexion.GetDataRowValue<string>(row, "GradeLevel"),
                Gender = _dbConexion.GetDataRowValue<string>(row, "Gender"),
             
            };
        }

        public User GetById(Guid id)
        {
            User usuario = null;
            string getQuery = "SELECT * FROM Usuarios WHERE UserId = @UsuarioId;";
            SqlCommand sqlCommand = _dbConexion.GetCommand(getQuery);
            SqlParameter parameter = new SqlParameter()
            {
                Direction = ParameterDirection.Input,
                ParameterName = "@UsuarioId",
                SqlDbType = SqlDbType.UniqueIdentifier,
                Value = id
            };
            sqlCommand.Parameters.Add(parameter);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                usuario = new User
                {
                    Id = reader.GetGuid(reader.GetOrdinal("UserId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    SchoolName = reader.GetString(reader.GetOrdinal("SchoolName")),
                    GradeLevel = reader.GetString(reader.GetOrdinal("GradeLevel")),
                    Gender = reader.GetString(reader.GetOrdinal("Gender")),
                };
            }
            reader.Close();
            return usuario;
        }


        public void ModificarUsuario(Guid Id, UserDTO modificarUsuario)
        {
            string updateQuery = "UPDATE  Usuarios SET  SchoolName= @SchoolName, Email = @Email,  GradeLevel= @GradeLevel WHERE  Id = @Id;";
            SqlCommand sqlCommand = _dbConexion.GetCommand(updateQuery);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = Id
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@SchoolName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = modificarUsuario.SchoolName
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = modificarUsuario.Email
                },
                new SqlParameter() {
                    Direction = ParameterDirection.Input,
                    ParameterName = "@GradeLevel",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = modificarUsuario.GradeLevel
                },
            };
            sqlCommand.Parameters.AddRange(parameters);
            sqlCommand.ExecuteNonQuery();
        }


        public void Eliminar(Guid Id)
        {
            string deleteQuery = "DELETE FROM  Usuarios WHERE Id = @Id;";
            SqlCommand sqlCommand = _dbConexion.GetCommand(deleteQuery);
            SqlParameter parameter = new SqlParameter()
            {
                Direction = ParameterDirection.Input,
                ParameterName = "@Id",
                SqlDbType = SqlDbType.UniqueIdentifier,
                Value = Id
            };
            sqlCommand.Parameters.Add(parameter);
            sqlCommand.ExecuteNonQuery();
        }

        public void Update(Guid Id, UserDTO modificarUsuario)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
