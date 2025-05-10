use matriculas_bd
select * from tb_usuario
create or alter procedure usp_listarEspecialidad
as
	Select * from tb_especialidad
go

--CURSOS
	CREATE OR ALTER PROCEDURE usp_listarCursos
	AS
	BEGIN
		SELECT 
			c.id_curso,
			c.nom_curso,
			c.creditos_curso,
			c.id_carrera,
			ca.nom_carrera
		FROM 
			tb_curso c
		INNER JOIN 
			tb_carrera ca ON c.id_carrera = ca.id_carrera
	END
	GO

CREATE OR ALTER PROCEDURE usp_registrarCurso
    @nom_curso       VARCHAR(50),
    @creditos_curso  SMALLINT,
    @id_carrera      INT
AS
BEGIN

    INSERT INTO tb_curso (nom_curso, creditos_curso, id_carrera)
    VALUES (@nom_curso, @creditos_curso, @id_carrera);
END
GO

CREATE OR ALTER PROCEDURE usp_buscarCurso
    @id_curso INT
AS
BEGIN
    SELECT
        c.id_curso,
        c.nom_curso,
        c.creditos_curso,
        c.id_carrera,
        ca.nom_carrera
    FROM tb_curso AS c
    INNER JOIN tb_carrera AS ca
        ON c.id_carrera = ca.id_carrera
    WHERE c.id_curso = @id_curso;
END
GO

CREATE OR ALTER PROCEDURE usp_actualizarCurso
    @id_curso        INT,
    @nom_curso       VARCHAR(50),
    @creditos_curso  SMALLINT,
    @id_carrera      INT
AS
BEGIN

    UPDATE tb_curso
    SET
        nom_curso      = @nom_curso,
        creditos_curso = @creditos_curso,
        id_carrera     = @id_carrera
    WHERE id_curso = @id_curso;
END
GO



--LOGIN

CREATE OR ALTER PROCEDURE sp_LoginUsuario 
    @correo VARCHAR(100),
    @contrasena VARCHAR(30),
    @login_exitoso BIT OUTPUT,
    @mensaje VARCHAR(100) OUTPUT,
    @id_usuario INT OUTPUT,
    @id_rol INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SET @login_exitoso = 0;
    SET @mensaje = '';
    SET @id_usuario = 0;
    SET @id_rol = 0;
    
    IF EXISTS (
        SELECT 1 FROM tb_usuario 
        WHERE correo = @correo 
        AND contrasena = @contrasena
        AND estado = 1
    )
    BEGIN
        SELECT 
            @id_usuario = id_usuario,
            @id_rol = id_rol
        FROM tb_usuario
        WHERE correo = @correo
        AND contrasena = @contrasena
        AND estado = 1;
        
        IF @id_rol = 2
        BEGIN
            SET @mensaje = 'Acceso denegado para este tipo de usuario';
            SET @login_exitoso = 0;
            SET @id_usuario = 0;
            SET @id_rol = 0;
        END
        ELSE
        BEGIN
            SET @mensaje = 'Login exitoso';
            SET @login_exitoso = 1;
        END
    END
    ELSE
    BEGIN
        SET @mensaje = 'Usuario o contraseña incorrectos, o usuario inactivo';
    END
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerMenusPorRol
    @id_rol INT
AS
BEGIN
    SELECT 
        m.id_menu,
        m.titulo_menu,
        m.url_menu,
        m.orden,
		m.controlador
    FROM 
        tb_menu m
    INNER JOIN 
        tb_menu_rol mr ON m.id_menu = mr.id_menu
    WHERE 
        mr.id_rol = @id_rol
        AND m.es_activo = 1
    ORDER BY 
        m.orden
END
GO




--DOCENTES
create or alter procedure usp_listarDocentes
as
    select 
        U.id_usuario as id_docente, 
        U.nom_usuario + ' ' + U.ape_usuario as nom_docente, 
        E.nom_especialidad,
        U.estado
    from tb_usuario as U
    join tb_especialidad as E on U.cod_especialidad = E.cod_especialidad
    where U.id_rol = 2  -- Rol Docente
go


create or alter procedure usp_registrarDocentes(
    @nom_docente varchar(50),
    @ape_docente varchar(50),
    @correo varchar(100),
    @cod_especialidad int,
    @estado bit
)
as
begin
    insert into tb_usuario (
        nom_usuario,
        ape_usuario,
        correo,
        cod_especialidad,
        id_rol,
        estado
    ) values (
        @nom_docente,
        @ape_docente,
        @correo,
        @cod_especialidad,
        2,  -- ID 2 para rol docente
        @estado
    )
end
go

CREATE OR ALTER PROCEDURE usp_actualizarDocentes
    @id_docente INT,
    @nom_docente VARCHAR(50),
    @ape_docente VARCHAR(50),
    @correo VARCHAR(100),
    @cod_especialidad INT,
    @estado BIT
AS
BEGIN
    UPDATE tb_usuario
    SET 
        nom_usuario = @nom_docente,
        ape_usuario = @ape_docente,
        correo = @correo,
        cod_especialidad = @cod_especialidad,
        estado = @estado
    WHERE 
        id_usuario = @id_docente
        AND id_rol = 2; 
END
GO

CREATE OR ALTER PROCEDURE usp_buscarDocente
    @id_docente INT
AS
BEGIN
    SELECT 
        U.id_usuario AS id_docente,
        U.nom_usuario AS nom_docente,
        U.ape_usuario AS ape_docente,
        U.correo,
        U.cod_especialidad,
        E.nom_especialidad,
        U.estado
    FROM 
        tb_usuario U
    LEFT JOIN
        tb_especialidad E ON U.cod_especialidad = E.cod_especialidad
    WHERE 
        U.id_usuario = @id_docente
        AND U.id_rol = 2;  
END
GO

--AULAS

CREATE OR ALTER PROCEDURE usp_listarAulas
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
		id_aula,
        cod_aula,
        capacidad_aula,
        es_disponible
    FROM tb_aula;
END
GO

CREATE OR ALTER PROCEDURE usp_buscarAula
    @id_aula int
AS
BEGIN    
    SELECT 
		id_aula,
        cod_aula,
        capacidad_aula,
        es_disponible
    FROM tb_aula
    WHERE id_aula = @id_aula;
END
GO

CREATE OR ALTER PROCEDURE usp_registrarAulas
    @cod_aula       CHAR(4),
    @capacidad_aula SMALLINT,
    @es_disponible  BIT
AS
BEGIN    
    INSERT INTO tb_aula (cod_aula, capacidad_aula, es_disponible)
    VALUES (@cod_aula, @capacidad_aula, @es_disponible);
END
GO

CREATE OR ALTER PROCEDURE usp_actualizarAulas
	@id_aula		INT,
    @cod_aula       CHAR(4),
    @capacidad_aula SMALLINT,
    @es_disponible  BIT
AS
BEGIN
    UPDATE tb_aula
    SET 
		cod_aula       = @cod_aula,
        capacidad_aula = @capacidad_aula,
        es_disponible  = @es_disponible
    WHERE id_aula = @id_aula;
END
GO

--ALUMNOS

CREATE OR ALTER PROCEDURE usp_listarAlumnos
AS
BEGIN
    SELECT 
        id_usuario,
        nom_usuario + ' ' + ape_usuario as nom_usuario, 
        correo,
        contrasena,
        estado
    FROM tb_usuario
    WHERE id_rol = 3;  -- Alumno
END
GO

CREATE OR ALTER PROCEDURE usp_buscarAlumno
    @id_usuario INT
AS
BEGIN
    SELECT 
        id_usuario,
        nom_usuario,
        ape_usuario,
        correo,
        contrasena,
        estado
    FROM tb_usuario
    WHERE id_usuario = @id_usuario
      AND id_rol = 3;
END
GO

CREATE OR ALTER PROCEDURE usp_registrarAlumno
    @nom_usuario  VARCHAR(50),
    @ape_usuario  VARCHAR(50),
    @correo       VARCHAR(100),
    @contrasena   VARCHAR(30),
    @estado       BIT
AS
BEGIN
    INSERT INTO tb_usuario
        (nom_usuario, ape_usuario, correo, contrasena, cod_especialidad, id_rol, estado)
    VALUES
        (@nom_usuario, @ape_usuario, @correo, @contrasena, NULL, 3, @estado);
END
GO

CREATE OR ALTER PROCEDURE usp_actualizarAlumno
    @id_usuario   INT,
    @nom_usuario  VARCHAR(50),
    @ape_usuario  VARCHAR(50),
    @correo       VARCHAR(100),
    @contrasena   VARCHAR(30),
    @estado       BIT
AS
BEGIN
    UPDATE tb_usuario
    SET 
        nom_usuario = @nom_usuario,
        ape_usuario = @ape_usuario,
        correo      = @correo,
        contrasena  = @contrasena,
        estado      = @estado
    WHERE id_usuario = @id_usuario
      AND id_rol = 3;
END
GO

CREATE OR ALTER PROCEDURE usp_BuscarPeriodoActual
AS
BEGIN
    SELECT 
        id_periodo,
        codigo_periodo,
        fcha_inicio,
        fcha_fin
    FROM 
        tb_periodo
    WHERE 
        CONVERT(DATE, GETDATE()) BETWEEN fcha_inicio AND fcha_fin;
END
GO

CREATE OR ALTER PROCEDURE uspListarCarrerasPorUsuario
    @id_usuario INT
AS
BEGIN
    SELECT 
        c.id_carrera,
        c.nom_carrera
    FROM 
        tb_usuario_carrera uc
    INNER JOIN 
        tb_carrera c ON uc.id_carrera = c.id_carrera
    WHERE 
        uc.id_usuario = @id_usuario
    ORDER BY 
        c.nom_carrera
END
GO

CREATE OR ALTER PROCEDURE uspListarCarrerasPorUsuario
    @id_usuario INT
AS
BEGIN
    SELECT 
        c.id_carrera,
        c.nom_carrera
    FROM 
        tb_carrera c
    INNER JOIN 
        tb_usuario_carrera uc ON c.id_carrera = uc.id_carrera
    WHERE 
        uc.id_usuario = @id_usuario
    ORDER BY 
        c.nom_carrera;
END
GO

CREATE OR ALTER PROCEDURE uspListarCursosPorCarrera
    @id_carrera INT
AS
BEGIN
    SELECT 
        c.id_curso,
        c.nom_curso,
        c.creditos_curso,
        ca.nom_carrera,
        p.fcha_inicio AS fecha_inicio_periodo,
        p.fcha_fin AS fecha_fin_periodo
    FROM 
        tb_curso c
    INNER JOIN 
        tb_carrera ca ON c.id_carrera = ca.id_carrera
    CROSS APPLY (
        SELECT TOP 1 fcha_inicio, fcha_fin
        FROM tb_periodo
        WHERE GETDATE() BETWEEN fcha_inicio AND fcha_fin
        ORDER BY fcha_inicio DESC
    ) p
    WHERE 
        c.id_carrera = @id_carrera;
END
GO

CREATE or ALTER PROCEDURE uspBuscarCarreraPorId
    @id_carrera INT
AS
BEGIN
    SELECT 
        id_carrera,
        nom_carrera
    FROM 
        tb_carrera
    WHERE 
        id_carrera = @id_carrera;
END
GO


create or alter procedure uspListarHorariosPorCurso
	@id_curso int
as begin
SELECT 
        s.id_seccion,
        s.cod_seccion,
        CASE sh.dia_semana
            WHEN 1 THEN 'Lunes'
            WHEN 2 THEN 'Martes'
            WHEN 3 THEN 'Miércoles'
            WHEN 4 THEN 'Jueves'
            WHEN 5 THEN 'Viernes'
            WHEN 6 THEN 'Sábado'
            ELSE 'Domingo'
        END AS dia_semana,
        CONVERT(VARCHAR(5), sh.hora_inicio, 108) AS hora_inicio,
        CONVERT(VARCHAR(5), sh.hora_fin, 108) AS hora_fin,
        sh.tipo_horario,
        a.cod_aula,
        u.nom_usuario + ' ' + u.ape_usuario AS nombre_docente,
        s.cupos_disponible,
        s.cupos_maximos,
        c.nom_curso
    FROM 
        tb_seccion s
    INNER JOIN 
        tb_seccion_horario sh ON s.id_seccion = sh.id_seccion
    INNER JOIN 
        tb_aula a ON s.id_aula = a.id_aula
    INNER JOIN 
        tb_usuario u ON s.id_usuario = u.id_usuario
    INNER JOIN 
        tb_curso c ON s.id_curso = c.id_curso
    WHERE 
        s.id_curso = @id_curso
end
GO

CREATE OR ALTER PROCEDURE uspInsertarMatriculaAlumno
    @id_alumno      INT,
    @id_carrera     INT,
    @id_curso       INT,
    @id_seccion     INT,
    @id_periodo     INT,
    @resultado      BIT OUTPUT,
    @mensaje        VARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE 
        @existe_alumno           BIT = 0,
        @existe_carrera          BIT = 0,
        @existe_curso            BIT = 0,
        @existe_seccion          BIT = 0,
        @existe_periodo          BIT = 0,
        @alumno_en_carrera       BIT = 0,
        @cupos_disponibles       BIT = 0,
        @conflicto_horario       BIT = 0,
        @id_matricula_existente  INT = NULL;
    
    -- Verificar si el alumno existe y es alumno activo
    SELECT @existe_alumno = 1 
    FROM tb_usuario 
    WHERE id_usuario = @id_alumno AND id_rol = 3 AND estado = 1;
    
    -- Verificar si la carrera existe
    SELECT @existe_carrera = 1 
    FROM tb_carrera 
    WHERE id_carrera = @id_carrera;
    
    -- Verificar si el curso existe y pertenece a la carrera
    SELECT @existe_curso = 1 
    FROM tb_curso 
    WHERE id_curso = @id_curso AND id_carrera = @id_carrera;
    
    -- Verificar si la sección existe y pertenece al curso
    SELECT @existe_seccion = 1 
    FROM tb_seccion 
    WHERE id_seccion = @id_seccion AND id_curso = @id_curso;
    
    -- Verificar si el período existe
    SELECT @existe_periodo = 1 
    FROM tb_periodo 
    WHERE id_periodo = @id_periodo;
    
    -- Verificar si el alumno pertenece a la carrera
    SELECT @alumno_en_carrera = 1 
    FROM tb_usuario_carrera 
    WHERE id_usuario = @id_alumno AND id_carrera = @id_carrera;
    
    -- Verificar cupos disponibles en la sección
    SELECT @cupos_disponibles = 1 
    FROM tb_seccion 
    WHERE id_seccion = @id_seccion AND cupos_disponible > 0;
    
    -- Verificar si el alumno ya está matriculado en esta misma sección
    SELECT @id_matricula_existente = m.id_matricula
    FROM tb_matricula m
    INNER JOIN tb_detalle_matricula dm ON m.id_matricula = dm.id_matricula
    WHERE m.id_usuario  = @id_alumno 
      AND m.id_periodo  = @id_periodo
      AND dm.id_seccion = @id_seccion;
    
    -- Verificar conflictos de horario
    IF EXISTS (
        SELECT sh1.dia_semana, sh1.hora_inicio, sh1.hora_fin
        FROM tb_seccion_horario sh1
        WHERE sh1.id_seccion = @id_seccion
        INTERSECT
        SELECT sh2.dia_semana, sh2.hora_inicio, sh2.hora_fin
        FROM tb_seccion_horario sh2
        INNER JOIN tb_detalle_matricula dm ON sh2.id_seccion = dm.id_seccion
        INNER JOIN tb_matricula m ON dm.id_matricula = m.id_matricula
        WHERE m.id_usuario = @id_alumno
          AND m.id_periodo = @id_periodo
    )
    BEGIN
        SET @conflicto_horario = 1;
    END
    
    -- ► NUEVA VALIDACIÓN: un solo horario por curso en el período
    IF EXISTS (
        SELECT 1
        FROM tb_matricula m
        JOIN tb_detalle_matricula dm ON m.id_matricula = dm.id_matricula
        WHERE m.id_usuario  = @id_alumno
          AND m.id_periodo  = @id_periodo
          AND dm.id_curso   = @id_curso
    )
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'Ya estás matriculado en otro horario de este mismo curso para el período actual.';
        RETURN;
    END
    
    -- Validaciones de error anteriores
    IF @existe_alumno = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'El alumno no existe o no tiene permisos para matricularse.';
        RETURN;
    END

    IF @existe_carrera = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'La carrera especificada no existe.';
        RETURN;
    END

    IF @existe_curso = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'El curso especificado no existe o no pertenece a la carrera.';
        RETURN;
    END

    IF @existe_seccion = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'La sección especificada no existe o no pertenece al curso.';
        RETURN;
    END

    IF @existe_periodo = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'El período académico especificado no existe.';
        RETURN;
    END

    IF @alumno_en_carrera = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'El alumno no pertenece a la carrera especificada.';
        RETURN;
    END

    IF @cupos_disponibles = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'No hay cupos disponibles en esta sección.';
        RETURN;
    END

    IF @id_matricula_existente IS NOT NULL
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'El alumno ya está matriculado en esta sección para el período actual.';
        RETURN;
    END

    IF @conflicto_horario = 1
    BEGIN
        SET @resultado = 0;
        SET @mensaje   = 'Existe un conflicto de horario con otra sección matriculada.';
        RETURN;
    END

    -- Inserción de matrícula y detalle
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @id_matricula INT;
        SELECT @id_matricula = id_matricula 
        FROM tb_matricula 
        WHERE id_usuario = @id_alumno 
          AND id_periodo = @id_periodo;

        IF @id_matricula IS NULL
        BEGIN
            INSERT INTO tb_matricula (id_usuario, id_periodo)
            VALUES (@id_alumno, @id_periodo);

            SET @id_matricula = SCOPE_IDENTITY();
        END
        
        INSERT INTO tb_detalle_matricula (id_matricula, id_seccion, id_curso)
        VALUES (@id_matricula, @id_seccion, @id_curso);
        
        UPDATE tb_seccion
        SET cupos_disponible = cupos_disponible - 1
        WHERE id_seccion = @id_seccion;
        
        COMMIT TRANSACTION;

        SET @resultado = 1;
        SET @mensaje   = 'Matrícula registrada exitosamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @resultado = 0;
        SET @mensaje   = 'Error al registrar la matrícula: ' + ERROR_MESSAGE();
    END CATCH
END
GO


		CREATE OR ALTER PROCEDURE uspEliminarMatriculaAlumno
    @id_alumno INT,
    @id_seccion INT,
    @id_periodo INT,
    @resultado BIT OUTPUT,
    @mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @existe_alumno BIT = 0;
    DECLARE @existe_seccion BIT = 0;
    DECLARE @existe_periodo BIT = 0;
    DECLARE @id_matricula INT = NULL;
    DECLARE @id_curso INT = NULL;
    
    -- Verificar si el alumno existe y es realmente un alumno
    SELECT @existe_alumno = 1 
    FROM tb_usuario 
    WHERE id_usuario = @id_alumno AND id_rol = 3 AND estado = 1;
    
    -- Verificar si la sección existe
    SELECT @existe_seccion = 1 
    FROM tb_seccion 
    WHERE id_seccion = @id_seccion;
    
    -- Verificar si el periodo existe
    SELECT @existe_periodo = 1 
    FROM tb_periodo 
    WHERE id_periodo = @id_periodo;
    
    -- Verificar si existe la matrícula y obtener el id_curso
    SELECT 
        @id_matricula = m.id_matricula,
        @id_curso = dm.id_curso
    FROM tb_matricula m
    INNER JOIN tb_detalle_matricula dm ON m.id_matricula = dm.id_matricula
    WHERE m.id_usuario = @id_alumno 
    AND m.id_periodo = @id_periodo
    AND dm.id_seccion = @id_seccion;
    
    -- Validar todas las condiciones
    IF @existe_alumno = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje = 'El alumno no existe o no tiene permisos para esta acción.';
        RETURN;
    END
    
    IF @existe_seccion = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje = 'La sección especificada no existe.';
        RETURN;
    END
    
    IF @existe_periodo = 0
    BEGIN
        SET @resultado = 0;
        SET @mensaje = 'El período académico especificado no existe.';
        RETURN;
    END
    
    IF @id_matricula IS NULL OR @id_curso IS NULL
    BEGIN
        SET @resultado = 0;
        SET @mensaje = 'No se encontró la matrícula del alumno en esta sección para el período actual.';
        RETURN;
    END
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Eliminar el detalle de matrícula (usando la clave compuesta)
        DELETE FROM tb_detalle_matricula
        WHERE id_matricula = @id_matricula 
        AND id_seccion = @id_seccion
        AND id_curso = @id_curso;
        
        -- Aumentar los cupos disponibles en la sección
        UPDATE tb_seccion
        SET cupos_disponible = cupos_disponible + 1
        WHERE id_seccion = @id_seccion;
        
        -- Verificar si quedan más detalles en la matrícula
        IF NOT EXISTS (SELECT 1 FROM tb_detalle_matricula WHERE id_matricula = @id_matricula)
        BEGIN
            -- Si no quedan más detalles, eliminar la matrícula principal
            DELETE FROM tb_matricula
            WHERE id_matricula = @id_matricula;
        END
        
        COMMIT TRANSACTION;
        
        SET @resultado = 1;
        SET @mensaje = 'Matrícula eliminada exitosamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @resultado = 0;
        SET @mensaje = 'Error al eliminar la matrícula: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE uspVerificarMatriculaAlumno
    @id_alumno INT,
    @id_seccion INT,
    @id_periodo INT,
    @existe BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1 
        FROM tb_matricula m
        INNER JOIN tb_detalle_matricula dm ON m.id_matricula = dm.id_matricula
        WHERE m.id_usuario = @id_alumno 
        AND m.id_periodo = @id_periodo
        AND dm.id_seccion = @id_seccion
    )
    BEGIN
        SET @existe = 1;
    END
    ELSE
    BEGIN
        SET @existe = 0;
    END
END;
GO
/*
SELECT * FROM tb_usuario WHERE id_rol = 3; -- Alumnos
SELECT * FROM tb_carrera;
SELECT * FROM tb_curso;
SELECT * FROM tb_seccion;
SELECT * FROM tb_periodo;


SELECT *
FROM tb_usuario 
WHERE id_usuario = 8 AND id_rol = 3 AND estado = 1;


-- Matricular al alumno Juan Pérez (id 6) en Programación I (id_curso 1), sección A1MA (id_seccion 1)
-- para el período 2025-1 (id_periodo 1) en la carrera 1
-- Ejecución del procedimiento almacenado
DECLARE @resultado BIT;
DECLARE @mensaje VARCHAR(200);
EXEC uspInsertarMatriculaAlumno 
    @id_alumno = 13, 
    @id_carrera = 2, 
    @id_curso = 1, 
    @id_seccion = 1,
    @id_periodo = 1,
    @resultado = @resultado OUTPUT,
    @mensaje = @mensaje OUTPUT;

SELECT @resultado AS Resultado, @mensaje AS Mensaje;
GO

-- Verificación de la matrícula creada
SELECT * FROM tb_matricula WHERE id_usuario = 13;
GO

-- Verificación del detalle de matrícula
SELECT * FROM tb_detalle_matricula WHERE id_matricula = 
    (SELECT id_matricula FROM tb_matricula WHERE id_usuario = 13);
GO

-- Verificación de cupos disponibles en la sección
SELECT cupos_disponible FROM tb_seccion WHERE id_seccion = 1;
GO

DECLARE @resultado BIT;
DECLARE @mensaje VARCHAR(200);

EXEC uspInsertarMatriculaAlumno
    @id_alumno = 10,    -- Ana Gómez
    @id_carrera = 5,   -- Ingeniería de Sistemas
    @id_curso = 10,     -- Cálculo I
    @id_seccion = 12,   -- Teoría Cálculo I
    @id_periodo = 1,   -- 2025-1
    @resultado = @resultado OUTPUT,
    @mensaje = @mensaje OUTPUT;

SELECT @resultado AS Resultado, @mensaje AS Mensaje;
-- Consulta de horarios de sección
SELECT * FROM tb_seccion_horario;
select * from tb_matricula
select * From tb_Detalle_matricula




SELECT * FROM tb_usuario_carrera WHERE id_usuario = 8
GO*/

CREATE OR ALTER PROCEDURE usp_listarMatricula
    @Id_usuario INT
AS
BEGIN
    SELECT 
        M.id_matricula as id_matricula,
        U.id_usuario as id_usuario, 
        U.nom_usuario + ' ' + U.ape_usuario AS nombre_completo,
        P.codigo_periodo as codigo_periodo,
        C.id_carrera as id_carrera,
        C.nom_carrera as nom_carrera,
        CU.id_curso as id_curso,
        CU.nom_curso as nom_curso,
        CU.creditos_curso as creditos_curso,
        S.id_seccion as id_seccion,
        S.cod_seccion as cod_seccion,
        A.id_aula as id_aula,
        A.cod_aula as cod_aula,
        H.hora_inicio as hora_inicio,
        H.hora_fin as hora_fin,
        H.tipo_horario as tipo_horario,
		CASE H.dia_semana
            WHEN 1 THEN 'Lunes'
            WHEN 2 THEN 'Martes'
            WHEN 3 THEN 'Miércoles'
            WHEN 4 THEN 'Jueves'
            WHEN 5 THEN 'Viernes'
            WHEN 6 THEN 'Sábado'
            ELSE 'Domingo'
        END AS nomDiaSemana
    FROM tb_matricula M
    INNER JOIN tb_usuario             U   ON M.id_usuario = U.id_usuario
    INNER JOIN tb_periodo            P   ON M.id_periodo = P.id_periodo
    INNER JOIN tb_detalle_matricula  DM  ON M.id_matricula = DM.id_matricula
    INNER JOIN tb_seccion            S   ON DM.id_seccion = S.id_seccion
    INNER JOIN tb_aula               A   ON S.id_aula = A.id_aula
    INNER JOIN tb_curso              CU  ON DM.id_curso = CU.id_curso
    INNER JOIN tb_carrera            C   ON CU.id_carrera = C.id_carrera
    INNER JOIN tb_seccion_horario    H   ON S.id_seccion = H.id_seccion
    WHERE U.id_usuario = @Id_usuario
    ORDER BY CU.nom_curso, H.dia_semana, H.hora_inicio;
END
GO


SELECT * FROM tb_matricula
SELECT * FROM tb_usuario
select * from tb_menu
select * from tb_menu_rol
SELECT * FROM tb_detalle_matricula
SELECT * FROM tb_seccion_curso
SELECT * FROM tb_seccion_horario
exec usp_listarMatricula @Id_usuario = 8
insert into tb_menu values('Listado de Matricula','listadoMatricula','Matricula',10,1)
insert into tb_menu_rol values (10,3)