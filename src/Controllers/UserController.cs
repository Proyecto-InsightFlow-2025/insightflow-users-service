
using insightflow_users_service.src.DTOs;
using insightflow_users_service.src.Helpers.Requests;
using insightflow_users_service.src.Mappers;
using insightflow_users_service.src.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace insightflow_users_service.src.Controllers
{
    /// <summary>
    /// Controller for managing User operations in the Censudex system.
    /// </summary>
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository; 

        /// <summary>
        /// Initializes a new instance of the UserController class.
        /// </summary>
        /// <param name="repository">The User repository for data access operations.</param>
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new User in the system.
        /// </summary>
        /// <param name="dto">The User creation data transfer object.</param>
        /// <returns>
        /// Returns 201 Created with the newly created User on success.
        /// Returns 400 Bad Request if the model state is invalid.
        /// Returns 409 Conflict if a User with the same email or username already exists.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repository.ExistsByEmailAsync(dto.Email))
                return Conflict("Un usuario con el mismo correo electrónico ya existe.");

            if (await _repository.ExistsByUsernameAsync(dto.Username))
                return Conflict("Un usuario con el mismo nombre de usuario ya existe.");

            var user = dto.ToUser();
            var createdUser = await _repository.CreateAsync(user);
            var responseDto = createdUser.ToViewUserResponse();

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        /// <summary>
        /// Retrieves a paginated list of users with optional filtering.
        /// </summary>
        /// <param name="query">The query parameters for filtering, pagination, and sorting.</param>
        /// <returns>
        /// Returns 200 OK with a paginated list of Users.
        /// Returns 400 Bad Request if the model state is invalid.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (users, totalCount) = await _repository.GetAllAsync(query);
            var userDtos = users.Select(UserMapper.ToViewUserResponse).ToList();
            var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

            var response = new
            {
                Items = userDtos,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific user by their unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the User to retrieve.</param>
        /// <returns>
        /// Returns 200 OK with the User data if found.
        /// Returns 404 Not Found if the User does not exist.
        /// </returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ViewUserResponse>> GetById(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(UserMapper.ToViewUserResponse(user));
        }

        /// <summary>
        /// Updates an existing User's information.
        /// </summary>
        /// <param name="id">The GUID of the User to update.</param>
        /// <param name="dto">The updated User data.</param>
        /// <returns>
        /// Returns 204 No Content on successful update.
        /// Returns 404 Not Found if the User does not exist.
        /// </returns>
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CreateUserRequest dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            UserMapper.UpdateUserFromDto(user, dto);
            await _repository.UpdateAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Performs a soft delete of a User by marking them as inactive.
        /// Requires a valid JWT token and role 1 authorization.
        /// </summary>
        /// <param name="id">The GUID of the User to soft delete.</param>
        /// <param name="authHeader">The Authorization header containing the Bearer token.</param>
        /// <returns>
        /// Returns 204 No Content on successful soft delete.
        /// Returns 401 Unauthorized if the token is invalid or missing.
        /// Returns 403 Forbidden if the token does not belong to an authorized role.
        /// Returns 404 Not Found if the User does not exist.
        /// </returns>
        [HttpPatch("delete/{id:guid}")]
        public async Task<ActionResult> SoftDelete(Guid id, [FromHeader(Name = "Authorization")] string authHeader)
        {
            // if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            //     return Unauthorized("Debe proporcionar un token válido.");

            // var token = authHeader.Substring("Bearer ".Length).Trim();
            // var validated = await _tokenVerifier.VerifyTokenAsync(token);

            // if (validated == null)
            //     return Unauthorized("Token inválido o expirado.");

            // if (validated.Role != "1")
            //     return Forbid("No tiene permisos para realizar esta acción.");

            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _repository.SoftDeleteAsync(id);
            return NoContent();
        }


        /// <summary>
        /// Verifies existence of credentials.
        /// </summary>
        /// <param name="request">The login request from an auth service.</param>
        /// <returns>
        /// Returns 400 If neither email nor username is provided.
        /// Returns 401 If credentials are invalid or account is disabled.
        /// Returns 200 OK with basic User info on successful verification.
        /// </returns>
        // [HttpPost("credentials")]
        // public async Task<IActionResult> VerifyCredentials([FromBody] VerifyCredentialsRequest request)
        // {
        //     if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Username))
        //         return BadRequest("Debe proporcionar un correo electrónico o un nombre de usuario.");

        //     if (string.IsNullOrWhiteSpace(request.Password))
        //         return BadRequest("La contraseña es obligatoria.");

        //     // Buscar User por email o username
        //     var user = !string.IsNullOrWhiteSpace(request.Email)
        //         ? await _repository.GetByEmailAsync(request.Email)
        //         : await _repository.GetByUsernameAsync(request.Username!);

        //     if (user == null)
        //         return Unauthorized("Credenciales inválidas.");

        //     // Validar que la cuenta no ha sido eliminada
        //     bool disabled = user.IsActive == false;
        //     if (disabled)
        //         return Unauthorized("La cuenta que está intentando acceder ha sido desactivada.");

        //     // Validar contraseña (comparación segura con hash)
        //     bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        //     if (!isPasswordValid)
        //         return Unauthorized("Credenciales inválidas.");

        //     // Opcional: devolver datos básicos del User
        //     var response = new
        //     {
        //         user.Id,
        //         user.Role,
        //         user.Username,
        //         user.Email,
        //         user.IsActive
        //     };

        //     return Ok(response);
        // }
    }
}