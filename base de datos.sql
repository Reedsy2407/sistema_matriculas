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
('Ingeniería de Software'),
('Matemáticas Avanzadas'),
('Derecho Corporativo'),
('Administración de Empresas'),
('Ciencias de la Computación')
GO


create table tb_carrera
(
    id_carrera int IDENTITY(1,1) primary key,
    nom_carrera varchar(50) not null
)
go

INSERT INTO tb_carrera (nom_carrera) VALUES
('Ingeniería de Sistemas'),
('Administración de Empresas'),
('Derecho'),
('Economía'),
('Ciencia de Datos')
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
('Roberto', 'González', 'Admin2025!', 'r.gonzalez@universidad.edu', NULL, 1, 1),
('Lucía', 'Fernández', 'Soporte$2025', 'l.fernandez@universidad.edu', NULL, 1, 1)
GO

-- Crear usuarios Docentes (asociados a especialidades)
INSERT INTO tb_usuario (nom_usuario, ape_usuario, contrasena, correo,cod_especialidad, id_rol, estado)
VALUES 
('Carlos', 'Mendoza', 'admin123', 'admin1@mail.com', 1, 2, 1),  
('Laura', 'García', 'L4ur4G@rc1a', 'l.garcia@universidad.edu', 2, 2, 1),    
('Pedro', 'Martínez', 'P3dr0M@rt', 'p.martinez@universidad.edu', 4, 2, 1),
('Ana', 'Silva', 'S1lv4A54', 'a.silva@universidad.edu', 3, 2, 1),
('Jorge', 'Ramírez', 'J0rg3R@m', 'j.ramirez@universidad.edu', 5, 2, 1)  
GO

-- Crear usuarios Alumnos (asociados a carreras)
INSERT INTO tb_usuario (nom_usuario, ape_usuario, contrasena, correo, cod_especialidad, id_rol, estado)
VALUES 
('Juan', 'Pérez', 'alumno123', 'alumno1@mail.com',NULL, 3, 1), 
('María', 'López', 'M@r1aL0p3z', 'm.lopez@universidad.edu', NULL, 3, 1), 
('Ana', 'Gómez', 'An4G0m3z', 'a.gomez@universidad.edu', NULL, 3, 1),
('Luis', 'Torres', 'L4u1sT0rr3s', 'l.torres@universidad.edu', NULL, 3, 1),
('Sofía', 'Vargas', 'S0f1@V4rg', 's.vargas@universidad.edu', NULL, 3, 1)    
GO

INSERT INTO tb_usuario_carrera (id_usuario, id_carrera)
VALUES
(6, 1), (6, 5), -- Juan Pérez en Sistemas y Ciencia de Datos
(7, 2), (7, 4), -- María López en Administración y Economía
(8, 1), -- Ana Gómez en Sistemas
(9, 3), -- Luis Torres en Derecho
(10, 5); -- Sofía Vargas en Ciencia de Datos
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
('Derecho Civil', 5, 3),
('Bases de Datos', 4, 1),
('Estadística Avanzada', 4, 4),
('Machine Learning', 5, 5),
('Derecho Laboral', 4, 3),
('Finanzas Corporativas', 4, 2),
('Algoritmos Avanzados', 5, 5)
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
('D404', 20, 0),
('E505', 35, 1),
('A102', 25, 1),
('B203', 30, 1),
('C304', 20, 1)
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

INSERT INTO tb_seccion (cod_seccion, cupos_disponible, cupos_maximos, id_usuario, id_aula, id_curso) VALUES
-- Programación I
('P1TA', 15, 30, 3, 1, 1),   -- Teoría A
('P1LB', 15, 30, 3, 2, 1),   -- Laboratorio B

-- Cálculo I
('C1TC', 20, 25, 4, 3, 2),   -- Teoría C
('C1PD', 20, 25, 4, 4, 2),   -- Práctica D

-- Derecho Civil
('D1TE', 12, 20, 5, 5, 4),   -- Teoría E
('D1CF', 12, 20, 5, 6, 4),   -- Casos F

-- Bases de Datos
('B1TG', 18, 30, 6, 7, 5),   -- Teoría G
('B1LH', 18, 30, 6, 1, 5),   -- Laboratorio H

-- Machine Learning
('M1TI', 10, 25, 7, 2, 7),   -- Teoría I
('M1PJ', 10, 25, 7, 3, 7),   -- Proyectos J

-- Algoritmos Avanzados
('A1TK', 8, 20, 3, 4, 10),   -- Teoría K
('A1LL', 8, 20, 3, 5, 10),   -- Laboratorio L

-- Estadística Avanzada
('E1TM', 15, 30, 4, 6, 6),   -- Teoría M
('E1WN', 15, 30, 4, 7, 6)    -- Taller N
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
-- Programación I
(1, 1, '08:00', '10:00', 'Teoría'),      -- PROG1-T
(2, 3, '08:00', '10:00', 'Laboratorio'),  -- PROG1-L

-- Cálculo I
(3, 2, '09:00', '11:00', 'Teoría'),      -- CALC1-T
(4, 4, '09:00', '11:00', 'Práctica'),    -- CALC1-P

-- Derecho Civil
(5, 1, '14:00', '16:00', 'Teoría'),      -- DER1-T
(6, 3, '14:00', '16:00', 'Casos'),       -- DER1-C

-- Bases de Datos
(7, 2, '14:00', '16:00', 'Teoría'),      -- BD1-T
(8, 4, '14:00', '16:00', 'Laboratorio'), -- BD1-L

-- Machine Learning
(9, 3, '16:00', '18:00', 'Teoría'),      -- ML1-T
(10, 5, '16:00', '18:00', 'Proyectos'),  -- ML1-P

-- Algoritmos Avanzados
(11, 4, '08:00', '10:00', 'Teoría'),     -- ALG1-T
(12, 5, '08:00', '10:00', 'Laboratorio'),-- ALG1-L

-- Estadística Avanzada
(13, 5, '10:00', '12:00', 'Teoría'),     -- EST1-T
(14, 2, '10:00', '12:00', 'Taller')      -- EST1-W
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
GO

create table tb_matricula
(
    id_matricula int IDENTITY(1000,1) primary key,
	id_usuario int,
	id_periodo int,
	foreign key (id_usuario) references tb_usuario(id_usuario),
	foreign key (id_periodo) references tb_periodo(id_periodo)
)
go

INSERT INTO tb_matricula (id_usuario, id_periodo) VALUES
(6, 1), -- Juan Pérez en periodo 2025-1
(7, 1), -- María López en periodo 2025-1
(8, 1), -- Ana Gómez en periodo 2025-1
(9, 1), -- Luis Torres en periodo 2025-1
(10, 1), -- Sofía Vargas en periodo 2025-1
(6, 2), -- Juan Pérez también en periodo 2025-2
(8, 2), -- Ana Gómez en periodo 2025-2
(10, 2) -- Sofía Vargas en periodo 2025-2
GO

create table tb_detalle_matricula
(
    id_matricula int,
	id_seccion int,
	id_curso int,
	foreign key (id_matricula) references tb_matricula(id_matricula),
	foreign key (id_curso) references tb_curso(id_curso),
	foreign key (id_seccion) references tb_seccion(id_seccion)
)
GO

INSERT INTO tb_detalle_matricula (id_matricula, id_seccion, id_curso) VALUES
-- Periodo 2025-1
(1000, 1, 1),  -- Juan Pérez en PROG1-T (Teoría)
(1000, 2, 1),  -- Juan Pérez en PROG1-L (Laboratorio)
(1000, 3, 2),  -- Juan Pérez en CALC1-T (Teoría)
(1001, 5, 4),  -- María López en DER1-T (Teoría)
(1001, 14, 6), -- María López en EST1-W (Taller)
(1002, 1, 1),  -- Ana Gómez en PROG1-T (Teoría)
(1002, 7, 5),  -- Ana Gómez en BD1-T (Teoría)
(1003, 5, 4),  -- Luis Torres en DER1-T (Teoría)
(1004, 9, 7),  -- Sofía Vargas en ML1-T (Teoría)
(1004, 11,10) -- Sofía Vargas en ALG1-T (Teoría)
go

CREATE TABLE tb_seccion_curso (
    id_seccion INT NOT NULL,
    id_curso INT NOT NULL,
    PRIMARY KEY (id_seccion, id_curso),
    FOREIGN KEY (id_seccion) REFERENCES tb_seccion(id_seccion),
    FOREIGN KEY (id_curso) REFERENCES tb_curso(id_curso)
);
GO

INSERT INTO tb_seccion_curso (id_seccion, id_curso) VALUES
(1,1), (2,1),   -- Programación I
(3,2), (4,2),   -- Cálculo I
(5,4), (6,4),   -- Derecho Civil
(7,5), (8,5),   -- Bases de Datos
(9,7), (10,7),  -- Machine Learning
(11,10),(12,10),-- Algoritmos Avanzados
(13,6),(14,6);  -- Estadística Avanzada
GO