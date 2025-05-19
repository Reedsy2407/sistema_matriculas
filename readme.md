<!-- ============================================== -->
<!--      B E A U T I F U L   R E A D M E          -->
<!-- ============================================== -->

<!-- Badges -->
[![.NET](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-blue.svg)](https://www.microsoft.com/en-us/sql-server)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Last commit](https://img.shields.io/github/last-commit/tu-usuario/tu-repositorio.svg)](https://github.com/tu-usuario/tu-repositorio/commits/main)

---

# 📌 Sistema de Matrículas Universitarias

**Un proyecto de ejemplo completo** que abarca:
- Diseño y población de **Base de Datos SQL Server** (tablas, relaciones, constraints, datos de prueba).
- **Procedimientos almacenados** (`usp_*`, `sp_LoginUsuario`, etc.) para operaciones CRUD y validaciones de negocio.
- **Capa DAO** (Data Access Object) en C# para invocar SP y mapear resultados a modelos de dominio.
- **API REST** construida con **ASP.NET Core Web API**, exponiendo endpoints para:
  - Autenticación (Login)
  - Gestión de usuarios, carreras, cursos, secciones, aulas, docentes, etc.
  - Registro y anulación de matrículas.
  - Consulta de matrículas y generación de reportes en PDF.
- **Proyecto MVC (Frontend)** en **ASP.NET Core MVC**:
  - **Razor Views** para: registro/edición/listado de Cursos, Docentes, Aulas, Alumnos, Inscripción de Clases y Consulta de Horarios.
  - **Autenticación basada en cookies** y control de menús según rol (Administrador / Docente / Alumno).
  - **Consumo de la API** a través de `HttpClient`.
  - **Generación de PDF** con Rotativa.AspNetCore.

Este repositorio incluye **todo** lo conversado y desarrollado durante el proyecto: scripts de SQL, procedimientos, código C# (DAO, Interfaces, Controladores API, Controladores Front, Modelos), vistas, validaciones y documentación.

---

## 📂 Estructura del proyecto

```text
/
├── .gitignore
├── README.md
├── LICENSE
├── scripts/                       ← Todos los scripts SQL
│   ├── 01_base de datos.sql
│   ├── 02_procedures.sql
│
├── src/
│   ├── MatriculasAPI/             ← Proyecto ASP.NET Core Web API
│   │   ├── Controllers/           ← Controladores expuestos (/Login, /Menu, /Clase, /Curso, /Matricula, etc.)
│   │   ├── Repository/
│   │   │   ├── Interfaces/         ← Interfaces de cada DAO (ILogin, IMenu, IClase, ICurso, IMatricula, etc.)
│   │   │   └── DAO/                ← Implementaciones de los DAOs, invocan SP y retornan modelos
│   │   ├── Models/                 ← Todos los modelos de dominio (LoginRequest/Response, Menu, Carrera, Curso, Seccion, HorarioPorCurso, Matricula, etc.)
│   │   ├── appsettings.json        ← Cadena de conexión `cn` a SQL Server
│   │   └── Program.cs / Startup.cs ← Configuración de servicios, CORS, DI de DAOs, etc.
│   │
│   └── Matriculas/                 ← Proyecto ASP.NET Core MVC (Frontend)
│       ├── Controllers/            ← Controladores de MVC (LoginController, HomeController, CursoController, ClaseController, MatriculaController, etc.)
│       ├── Views/
│       │   ├── Shared/             ← Layout, PartialViews (Navbar, _ValidationScripts, etc.)
│       │   ├── Login/              ← Vista de login
│       │   ├── Curso/              ← Vistas de listado, registrar, editar, asignar horario
│       │   ├── Clase/              ← Vistas de selección de carrera, selección de curso, seleccionar horarios
│       │   ├── Matricula/          ← Vistas de listado de matrículas, exportar PDF
│       │   └── ...                 ← Otras vistas (Docente, Aula, Alumno) si se agregaron
│       ├── Models/                 ← Modelos compartidos (se reutilizan de MatriculasMODELS)
│       ├── wwwroot/                ← Archivos estáticos: CSS, JS, imágenes, librerías (bootstrap, sweetalert2, etc.)
│       └── Program.cs / Startup.cs ← Configuración de MVC, rutas, inyección de HttpClient (si aplica), uso de IHttpContextAccessor, etc.
│
└── MatriculasMODELS/               ← Biblioteca de modelos y DTOs compartida
    ├── Login/                      ← LoginRequest, LoginResponse
    ├── Menu/                       ← Menu, MenuPorRol
    ├── Carrera/                    ← Carrera, etc.
    ├── Curso/                      ← Curso, CursoO (para operaciones de registrar/editar)
    ├── Aula/                       ← Aula
    ├── Docente/                    ← Docente
    ├── Alumno/                     ← Alumno
    ├── Matricula/                  ← MatriculaRequest, MatriculaDeleteRequest, MatriculaResponse, Matriculas (para consulta de horaria)
    └── Periodo/                    ← Periodo




## 3. Requisitos Previos

1. **Instalar .NET 7.0 SDK**

   - Descarga e instalación desde:
     ```bash
     https://dotnet.microsoft.com/download/dotnet/7.0
     ```

2. **SQL Server 2019+ y SQL Server Management Studio (SSMS)**

   - Necesario para ejecutar los scripts de creación de base de datos y procedimientos almacenados.

3. **Visual Studio 2022**  
   (o **VS Code** con la extensión C#)

4. **Rotativa.AspNetCore**  
   - Se instalará automáticamente vía NuGet al restaurar paquetes del proyecto MVC.

5. **Git**  
   - Para clonar este repositorio:
     ```bash
     git clone https://github.com/tu-usuario/tu-repositorio.git
     ```
