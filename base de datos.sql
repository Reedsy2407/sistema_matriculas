use master
go

create database matriculas_bd
go

use matriculas_bd
go

create table tb_rol
(
    id_rol int IDENTITY(1,1) primary key,
    nom_rol varchar(50) not null
)
go

INSERT INTO tb_rol (nom_rol) VALUES 
('Administrador'),
('Docente'),
('Alumno')
GO

create table tb_menu(
	id_menu int identity(1,1) primary key,
	titulo_menu varchar(50) not null,
	url_menu varchar(100),
	controlador varchar(100),
	orden smallint not null,
	es_activo bit default 1
)
go

INSERT INTO tb_menu (titulo_menu, url_menu, controlador, orden, es_activo) VALUES
('Listado de Cursos',     'listadoCursos',   'Curso',  1, 1),
('Registrar Curso',       'registrarCurso',  'Curso',  2, 1),
('Listado de Docentes',   'listadoDocentes', 'Docente',3, 1),
('Registrar Docente',     'registrarDocente','Docente',4, 1),
('Listado Aulas',         'listadoAulas',    'Aula',   5, 1),
('Registrar Aulas',       'registrarAula',   'Aula',   6, 1),
('Listado de Alumnos',    'listadoAlumnos',  'Alumno', 7, 1),
('Registrar Alumno',      'registrarAlumno', 'Alumno', 8, 1),
('Inscribirse a Clases',  'seleccionCarrera',     'Clase',  9, 1);
GO



create table tb_menu_rol(
	id_menu int not null,
	id_rol int not null,
	primary key (id_menu, id_rol),
	foreign key (id_menu) references tb_menu(id_menu),
	foreign key (id_rol) references tb_rol(id_rol)
 )
go
INSERT INTO tb_menu_rol (id_menu, id_rol)
SELECT id_menu, 1 FROM tb_menu
GO

INSERT INTO tb_menu_rol (id_menu, id_rol)
VALUES (9, 3);
GO

create table tb_especialidad
(
	cod_especialidad int IDENTITY(1,1) primary key,
	nom_especialidad varchar(50) not null
)
go

INSERT INTO tb_especialidad (nom_especialidad) VALUES
('Especialidad 1'),
('Especialidad 2'),
('Especialidad 3'),
('Especialidad 4'),
('Especialidad 5')
GO


create table tb_carrera
(
    id_carrera int IDENTITY(1,1) primary key,
    nom_carrera varchar(50) not null
)
go

INSERT INTO tb_carrera (nom_carrera) VALUES
('Carrera 1'),
('Carrera 2'),
('Carrera 3'),
('Carrera 4'),
('Carrera 5')
GO

create table tb_usuario
(
    id_usuario int IDENTITY(1,1) primary key,
	nom_usuario varchar(50) not null,
	ape_usuario varchar(50) not null,
	correo varchar(100) not null,
	contrasena varchar(30) null,
	cod_especialidad int null, --Docente
	id_rol int not null, 
	estado bit not null,
	foreign key (id_rol) references tb_rol(id_rol),
	foreign key (cod_especialidad) references tb_especialidad(cod_especialidad),
)
go

CREATE TABLE tb_usuario_carrera
(
    id_usuario INT NOT NULL,
    id_carrera INT NOT NULL,
    PRIMARY KEY (id_usuario, id_carrera),
    FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id_usuario),
    FOREIGN KEY (id_carrera) REFERENCES tb_carrera(id_carrera)
);
GO

-- Crear usuarios Administradores
INSERT INTO tb_usuario (nom_usuario, ape_usuario, contrasena, correo, cod_especialidad, id_rol, estado)
VALUES 
('Admin', 'Principal', 'admin123', 'admin1@mail.com', NULL, 1, 1),
('Soporte', 'Técnico', 'soporte456', 'admin2@mail.com', NULL, 1, 1)
GO

-- Crear usuarios Docentes (asociados a especialidades)
INSERT INTO tb_usuario (nom_usuario, ape_usuario, contrasena, correo,cod_especialidad, id_rol, estado)
VALUES 
('Carlos', 'Mendoza', 'docente123', 'docente1@mail.com', 1, 2, 1),  
('Laura', 'García', 'docente456', 'docente2@mail.com', 2, 2, 1),    
('Pedro', 'Martínez', 'docente789', 'docente3@mail.com', 4, 2, 1)  
GO

-- Crear usuarios Alumnos (asociados a carreras)
INSERT INTO tb_usuario (nom_usuario, ape_usuario, contrasena, correo, cod_especialidad, id_rol, estado)
VALUES 
('Juan', 'Pérez', 'alumno123', 'alumno1@mail.com',NULL, 3, 1), 
('María', 'López', 'alumno456','alumno2@mail.com', NULL, 3, 1), 
('Ana', 'Gómez', 'alumno789','alumno3@mail.com', NULL, 3, 1)    
GO

INSERT INTO tb_usuario_carrera (id_usuario, id_carrera)
VALUES
(6, 1), (6, 2),
(7, 2),
(8, 1);
GO

create table tb_curso
(
    id_curso int IDENTITY(1,1) primary key,
    nom_curso varchar(50) not null,
	creditos_curso smallint,
	id_carrera int,
	foreign key (id_carrera) references tb_carrera(id_carrera)
)
go

INSERT INTO tb_curso (nom_curso, creditos_curso, id_carrera) VALUES
('Programación I', 4, 1),
('Cálculo I', 4, 1),
('Introducción a la Administración', 3, 2),
('Derecho Civil', 5, 3)
GO

create table tb_aula
(
	id_aula int identity(1,1) primary key,
    cod_aula char(4) CHECK (cod_aula LIKE '[A-E][0-9][0-9][0-9]'),
    capacidad_aula smallint,
    es_disponible bit
)
go

INSERT INTO tb_aula (cod_aula, capacidad_aula, es_disponible) VALUES
('A101', 30, 1),
('B202', 25, 1),
('C303', 40, 1),
('D404', 20, 0)
GO


create table tb_seccion
(
	id_seccion int identity(1,1) primary key,
    cod_seccion char(4) CHECK (cod_seccion LIKE '[A-Z][0-9][A-Z][A-Z]'),
    cupos_disponible smallint,
    cupos_maximos smallint,
    id_usuario int, --docente
    id_aula int,
	id_curso int,
    foreign key (id_usuario) references tb_usuario(id_usuario),
	foreign key (id_curso) references tb_curso(id_curso),
	foreign key (id_aula) references tb_aula(id_aula)
)
go


INSERT INTO tb_seccion (cod_seccion, cupos_disponible, cupos_maximos, horario_seccion, id_usuario, id_aula, id_curso) VALUES
('A1MA', 25, 30, 3, 1, 1),  -- Docente Carlos Mendoza enseña Programación I
('B1TG', 20, 25, 4, 2, 2)   -- Docente Laura García enseña Cálculo I
GO

CREATE TABLE tb_seccion_horario
(
    id_seccion     INT         NOT NULL,
    dia_semana     TINYINT     NOT NULL,
    hora_inicio    TIME        NOT NULL,
    hora_fin       TIME        NOT NULL,
    tipo_horario   VARCHAR(50) NOT NULL, 
    PRIMARY KEY    (id_seccion, dia_semana, hora_inicio),
    FOREIGN KEY    (id_seccion) REFERENCES tb_seccion(id_seccion)
);
GO

INSERT INTO tb_seccion_horario (id_seccion, dia_semana, hora_inicio, hora_fin, tipo_horario)
VALUES
    (1, 1, '08:00', '10:00', 'Teoria'),
    (1, 3, '14:00', '16:00', 'Laboratorio'),

    (2, 2, '10:00', '12:00', 'Teoria'),
    (2, 4, '16:00', '18:00', 'Laboratorio');
GO

create table tb_periodo
(
	id_periodo int identity(1,1)primary key,
    codigo_periodo char(6)  CHECK (codigo_periodo LIKE '[2][0][2][0-9][-][1-2]'),
	fcha_inicio DateTime not null,
	fcha_fin DateTime not null
)
go

INSERT INTO tb_periodo VALUES ('2025-1', '2025-03-01', '2025-07-15')

INSERT INTO tb_periodo VALUES ('2025-2', '2025-08-01', '2025-12-15')



create table tb_matricula
(
    id_matricula int IDENTITY(1000,1) primary key,
	id_usuario int,
	id_periodo int,
	foreign key (id_usuario) references tb_usuario(id_usuario),
	foreign key (id_periodo) references tb_periodo(id_periodo)
)
go

create table tb_detalle_matricula
(
    id_matricula int,
	id_seccion int,
	id_curso int,
	foreign key (id_matricula) references tb_matricula(id_matricula),
	foreign key (id_curso) references tb_curso(id_curso),
	foreign key (id_seccion) references tb_seccion(id_seccion)
)

CREATE TABLE tb_seccion_curso (
    id_seccion INT NOT NULL,
    id_curso INT NOT NULL,
    PRIMARY KEY (id_seccion, id_curso),
    FOREIGN KEY (id_seccion) REFERENCES tb_seccion(id_seccion),
    FOREIGN KEY (id_curso) REFERENCES tb_curso(id_curso)
);

SELECT 
        s.id_seccion,
        s.cod_seccion,
        sh.dia_semana,
        CASE sh.dia_semana
            WHEN 1 THEN 'Lunes'
            WHEN 2 THEN 'Martes'
            WHEN 3 THEN 'Miércoles'
            WHEN 4 THEN 'Jueves'
            WHEN 5 THEN 'Viernes'
            WHEN 6 THEN 'Sábado'
            ELSE 'Domingo'
        END AS nombre_dia,
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
        s.id_curso = 2