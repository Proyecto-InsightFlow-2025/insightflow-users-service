

# InsightFlow — Users Service (Microservicio)

Servicio REST independiente que forma parte de la plataforma **InsightFlow**.
Este servicio gestiona el ciclo de vida de los usuarios:
**creación, autenticación, consulta, listado, actualización y desactivación (Soft Delete)**.

Implementado en **ASP.NET Core 8.0** siguiendo un enfoque de microservicios.
Para el Taller 3, la persistencia se maneja con un **almacenamiento In-Memory registrado como Singleton**, simulando una base de datos temporal.

---

## Arquitectura y Patrones

### Arquitectura General

* Microservicio **RESTful** totalmente desacoplado.

### Persistencia In-Memory

* `ApplicationDbContext` como **Singleton**.
* La información se mantiene mientras la app esté activa.
* Simula una base de datos mediante listas en memoria.

### Organización del Código (Clean-ish Architecture)

* **Controllers** → manejan HTTP, validan modelos y delegan.
* **Repositories** → acceso a datos in-memory (`IUserRepository`).
* **Data** → contexto Singleton + Seeder.
* **Models** → entidad `User`.
* **DTOs** → `CreateUserRequest`, `LoginRequestDto`, `ViewUserResponse`.
* **Mappers** → conversión entre entidades y DTOs.

### Seguridad

* **Login retorna ID del usuario**.
* No se usa JWT.
* La validación para actualización es **sin autorización adicional** (según el controller actual).
* Soft Delete tiene comentarios para JWT, pero **no está activo**.

---

##  Requisitos Previos

* **.NET 8.0 SDK**
* **Docker** (opcional)
* Puerto **5214** libre (o configurado por la app)

---

## Ejecución del Proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/Proyecto-InsightFlow-2025/insightflow-users-service.git
cd insightflow-users-service
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Ejecutar la aplicación

El sistema cargará automáticamente los usuarios del Seeder: **Admin**, **Ignacio**, **David**.

```bash
dotnet run
```

### 4. Documentación y URLs

* **Swagger**: [http://localhost:5214/swagger](http://localhost:5214/swagger)
* **Base URL**: [http://localhost:5214/user](http://localhost:5214/user)

---


# **Endpoints Disponibles**

---

# Crear Usuario

### **POST /user**

Crea un nuevo usuario.
Valida colisiones por **email** y **username**.

### Body (CreateUserRequest)

```json
{
  "firstName": "Nuevo",
  "lastName": "Usuario",
  "email": "nuevo@insightflow.cl",
  "username": "nusuario",
  "password": "Password123!",
  "birthdate": "1995-05-20",
  "address": "Av. Brasil 123",
  "phoneNumber": "+56912345678"
}
```

### Respuestas

* **201 Created** → Retorna objeto `ViewUserResponse`
* **400 Bad Request** → ModelState inválido
* **409 Conflict** → Email o Username ya existen

---

# Listar Usuarios (requiere autenticación)

### **GET /user?requestUserId={guid}**

Devuelve lista paginada y filtrable.

> **Este endpoint exige el parámetro `requestUserId`**.
> Si falta o es un GUID vacío → **401 Unauthorized**.

### Query params (UserQuery)

| Param         | Tipo   | Descripción          |
| ------------- | ------ | -------------------- |
| pageNumber    | int    | Página actual        |
| pageSize      | int    | Tamaño de página     |
| username      | string | Filtrar por username |
| email         | string | Filtrar por email    |
| firstName     | string | Filtrar por nombre   |
| lastName      | string | Filtrar por apellido |
| requestUserId | Guid   | **Obligatorio**      |

### Respuesta 200 OK

```json
{
  "items": [
    { /* Usuario */ }
  ],
  "totalCount": 3,
  "totalPages": 1
}
```

### Errores

* **401 Unauthorized** → Falta `requestUserId`
* **400 Bad Request** → Query inválida

---

# Obtener Usuario por ID

### **GET /user/{id}**

Retorna un usuario según su GUID.

### Respuestas

* **200 OK** → Usuario encontrado
* **404 Not Found** → No existe

---

# Actualizar Usuario (requiere autenticación)

### **PATCH /user/{id}?requestUserId={guid}**

Actualiza un usuario usando **CreateUserRequest**.

> **Solo puedes editar tu propio perfil.**
> Si `id != requestUserId` → **401 Unauthorized**

### Body

Mismo formato que crear usuario.

### Respuestas

* **200 OK** → Retorna usuario actualizado (`ViewUserResponse`)
* **401 Unauthorized** → Intento de editar a otro usuario
* **404 Not Found** → Usuario no existe

---

# Eliminar Usuario (Soft Delete)

### **DELETE /user/{id}**

Marca el usuario como inactivo (`IsActive = false`).

> El controller **no exige JWT actualmente**.

### Respuestas

* **204 No Content** → Eliminado
* **404 Not Found** → No existe

---

# Login

### **POST /user/login**

Autentica y retorna datos básicos del usuario.

### Body

```json
{
  "email": "ignacio@insightflow.cl",
  "password": "Password123!"
}
```

### Flujo del controlador

1. Busca usuario por email
2. Verifica contraseña con **BCrypt**
3. Verifica `IsActive == true`
4. Devuelve datos básicos

### Respuesta 200 OK

```json
{
  "id": "GUID",
  "username": "iavendano",
  "firstName": "Ignacio",
  "lastName": "Avendaño",
  "email": "ignacio@insightflow.cl"
}
```

### Errores

* **401 Unauthorized** → Credenciales incorrectas
* **401 Unauthorized** → Usuario inactivo

---

#  Pipeline CI/CD (GitHub Actions)

* **CI**: build de imagen Docker al hacer push a `main`.
* **CD**: Webhook de Render despliega la última versión.
* **Docker Hub**: imagen publicada automáticamente.

---

#  Datos de Prueba (Seeder)

| Usuario | Email                                                   | Password           | Rol   |
| ------- | ------------------------------------------------------- | ------------------ | ----- |
| Admin   | [admin@insightflow.cl](mailto:admin@insightflow.cl)     | SecurePassword123! | Admin |
| Ignacio | [ignacio@insightflow.cl](mailto:ignacio@insightflow.cl) | Password123!       | User  |
| David   | [david@insightflow.cl](mailto:david@insightflow.cl)     | Password123!       | User  |

