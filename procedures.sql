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

CREATE PROCEDURE sp_LoginUsuario 
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

CREATE PROCEDURE sp_ObtenerMenusPorRol
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
    @estado       BIT,
	@new_id_usuario INT OUTPUT

AS
BEGIN
    INSERT INTO tb_usuario
        (nom_usuario, ape_usuario, correo, contrasena, cod_especialidad, id_rol, estado)
    VALUES
        (@nom_usuario, @ape_usuario, @correo, @contrasena, NULL, 3, @estado);
	SET @new_id_usuario = SCOPE_IDENTITY();
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

CREATE OR ALTER PROCEDURE usp_listarCarreras
AS
BEGIN
    SELECT id_carrera, nom_carrera
    FROM tb_carrera
    ORDER BY nom_carrera;
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

CREATE OR ALTER PROCEDURE usp_asignarCarreraUsuario
    @id_usuario  INT,
    @id_carrera  INT
AS
BEGIN
    INSERT INTO tb_usuario_carrera (id_usuario, id_carrera)
    VALUES (@id_usuario, @id_carrera);
END
GO

CREATE OR ALTER PROCEDURE usp_eliminarCarrerasUsuario
    @id_usuario INT
AS
BEGIN
    DELETE FROM tb_usuario_carrera
    WHERE id_usuario = @id_usuario;
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

CREATE OR ALTER PROCEDURE uspBuscarCarreraPorId
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
        s.id_curso = 1
end
GO

select * from tb_matricula
INSERT INTO tb_matricula (id_usuario, id_periodo)
VALUES (6, 1);


INSERT INTO tb_detalle_matricula (id_matricula, id_seccion, id_curso)
VALUES 
  (1001, 1, 1),  -- Programación I
  (1001, 2, 2);  -- Cálculo I
  GO

CREATE OR ALTER PROCEDURE usp_listarCursosYHorariosMatriculados
    @id_usuario INT,
    @id_periodo INT = NULL
AS
BEGIN
    SELECT
        C.nom_curso,
        CASE SH.dia_semana
            WHEN 1 THEN 'Lunes'    WHEN 2 THEN 'Martes'
            WHEN 3 THEN 'Miércoles' WHEN 4 THEN 'Jueves'
            WHEN 5 THEN 'Viernes'   WHEN 6 THEN 'Sábado'
            ELSE 'Domingo'
        END AS dia_semana,
        CONVERT(VARCHAR(5), SH.hora_inicio, 108) AS hora_inicio,
        CONVERT(VARCHAR(5), SH.hora_fin,    108) AS hora_fin
    FROM tb_matricula M
    INNER JOIN tb_detalle_matricula DM 
        ON M.id_matricula = DM.id_matricula
    INNER JOIN tb_seccion         S  
        ON DM.id_seccion = S.id_seccion
    INNER JOIN tb_seccion_horario SH 
        ON S.id_seccion  = SH.id_seccion
    INNER JOIN tb_curso           C  
        ON DM.id_curso   = C.id_curso
    WHERE M.id_usuario = @id_usuario
      AND (
           @id_periodo IS NULL 
           OR M.id_periodo = @id_periodo
      )
    ORDER BY C.nom_curso, SH.dia_semana, SH.hora_inicio;
END
GO

EXEC usp_listarCursosYHorariosMatriculados 
     @id_usuario = 7, 
     @id_periodo = 1;

----------------------------------------------LISTADO MATRICULA
INSERT INTO tb_matricula (id_usuario, id_periodo)
VALUES (7, 1); 

INSERT INTO tb_seccion_curso (id_seccion, id_curso)
VALUES (1, 1); 

INSERT INTO tb_detalle_matricula (id_matricula, id_seccion, id_curso)
VALUES (1000, 1, 1);
GO

CREATE OR ALTER PROCEDURE usp_listarMatricula
    @Id_matricula INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.id_matricula,
        U.id_usuario,
        U.nom_usuario + ' ' + U.ape_usuario AS nombre_completo,
        P.codigo_periodo,
        C.id_carrera,
        C.nom_carrera,
        CU.id_curso,
        CU.nom_curso,
        CU.creditos_curso,
        S.id_seccion,
        S.cod_seccion,
        A.id_aula,
        A.cod_aula
    FROM tb_matricula M
    INNER JOIN tb_usuario            U   ON M.id_usuario = U.id_usuario
    INNER JOIN tb_periodo            P   ON M.id_periodo = P.id_periodo
    INNER JOIN tb_detalle_matricula  DM  ON M.id_matricula = DM.id_matricula
    INNER JOIN tb_seccion            S   ON DM.id_seccion = S.id_seccion
    INNER JOIN tb_aula               A   ON S.id_aula = A.id_aula
    INNER JOIN tb_curso              CU  ON DM.id_curso = CU.id_curso
    INNER JOIN tb_carrera            C   ON CU.id_carrera = C.id_carrera
    WHERE M.id_matricula = @Id_matricula
    ORDER BY CU.nom_curso;
END
GO

EXEC usp_listarMatricula 1000

SELECT * FROM tb_usuario
SELECT * FROM tb_seccion
SELECT * FROM tb_seccion_curso
SELECT * FROM tb_detalle_matricula	
select * from tb_periodo