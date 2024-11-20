using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using EcosferaDigital.Models;

namespace EcosferaDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly string _connectionString;

        public UsuarioController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDbConnection") 
                                ?? throw new InvalidOperationException("A string de conexão 'OracleDbConnection' não foi encontrada nas configurações.");
        }

        // GET: /usuario
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync(); // Usar método assíncrono para abrir a conexão
                var command = new OracleCommand("SELECT * FROM Usuario", connection);
                var reader = await command.ExecuteReaderAsync(); // Usar ExecuteReaderAsync

                while (await reader.ReadAsync()) // Usar ReadAsync
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Endereco = reader.GetString(2),
                        Email = reader.GetString(3),
                        Telefone = reader.GetString(4)
                    });
                }
            }

            return Ok(usuarios);
        }

        // GET: /usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(); // Usar método assíncrono
            using var command = new OracleCommand("SELECT usuario_id, nome, endereco, email, telefone FROM Usuario WHERE usuario_id = :id", connection);
            command.Parameters.Add(new OracleParameter("id", id));

            using var reader = await command.ExecuteReaderAsync(); // Usar ExecuteReaderAsync
            if (await reader.ReadAsync()) // Usar ReadAsync
            {
                return Ok(new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Endereco = reader.GetString(2),
                    Email = reader.GetString(3),
                    Telefone = reader.GetString(4)
                });
            }

            return NotFound();
        }

        // POST: /usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario([FromBody] UsuarioPost usuarioPost)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(); // Usar método assíncrono

            // 1. Buscar o maior ID já presente no banco
            using var command = new OracleCommand("SELECT MAX(usuario_id) FROM Usuario", connection);
            var maxId = await command.ExecuteScalarAsync(); // Usar ExecuteScalarAsync
            var novoId = (maxId == DBNull.Value) ? 1 : Convert.ToInt32(maxId) + 1; // Incrementar o maior ID

            // 2. Inserir o novo usuário com o ID gerado manualmente
            using var insertCommand = new OracleCommand("INSERT INTO Usuario (usuario_id, nome, endereco, email, telefone) VALUES (:usuario_id, :nome, :endereco, :email, :telefone)", connection);
            insertCommand.Parameters.Add(new OracleParameter("usuario_id", novoId));
            insertCommand.Parameters.Add(new OracleParameter("nome", usuarioPost.Nome));
            insertCommand.Parameters.Add(new OracleParameter("endereco", usuarioPost.Endereco));
            insertCommand.Parameters.Add(new OracleParameter("email", usuarioPost.Email));
            insertCommand.Parameters.Add(new OracleParameter("telefone", usuarioPost.Telefone));

            await insertCommand.ExecuteNonQueryAsync(); // Usar ExecuteNonQueryAsync

            // 3. Criar o usuário com o ID manualmente gerado
            var usuario = new Usuario
            {
                Id = novoId,
                Nome = usuarioPost.Nome,
                Endereco = usuarioPost.Endereco,
                Email = usuarioPost.Email,
                Telefone = usuarioPost.Telefone
            };

            // 4. Retornar o usuário criado com o ID gerado manualmente
            return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.Id }, usuario);
        }

        // PUT: /usuario/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Usuario>> UpdateUsuario(int id, [FromBody] UsuarioPost usuarioPost)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(); // Usar método assíncrono

            // Atualiza o usuário com base no ID
            using var command = new OracleCommand("UPDATE Usuario SET nome = :nome, endereco = :endereco, email = :email, telefone = :telefone WHERE usuario_id = :id", connection);
            command.Parameters.Add(new OracleParameter("nome", usuarioPost.Nome));
            command.Parameters.Add(new OracleParameter("endereco", usuarioPost.Endereco));
            command.Parameters.Add(new OracleParameter("email", usuarioPost.Email));
            command.Parameters.Add(new OracleParameter("telefone", usuarioPost.Telefone));
            command.Parameters.Add(new OracleParameter("id", id));

            var rowsAffected = await command.ExecuteNonQueryAsync(); // Usar ExecuteNonQueryAsync
            if (rowsAffected == 0)
            {
                return NotFound();
            }

            // Retorna o usuário atualizado
            return Ok(new Usuario
            {
                Id = id,
                Nome = usuarioPost.Nome,
                Endereco = usuarioPost.Endereco,
                Email = usuarioPost.Email,
                Telefone = usuarioPost.Telefone
            });
        }

        // DELETE: /usuario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(); // Usar método assíncrono

            // Comando de deleção
            using var command = new OracleCommand("DELETE FROM Usuario WHERE usuario_id = :id", connection);
            command.Parameters.Add(new OracleParameter("id", id));

            var rowsAffected = await command.ExecuteNonQueryAsync(); // Usar ExecuteNonQueryAsync
            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
