CREATE TABLE usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    rol VARCHAR(20) NOT NULL,
    CONSTRAINT chk_rol CHECK (rol IN ('admin', 'empleado')),
    CONSTRAINT UQ_correo UNIQUE (correo)
);
GO

CREATE TABLE asistencias (
    id_asistencia INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    fecha_asistencia DATE NOT NULL,
    hora_asistencia TIME NOT NULL,
    tipo_asistencia VARCHAR(10) NOT NULL,
    CONSTRAINT chk_tipo_asistencia CHECK (tipo_asistencia IN ('entrada', 'salida')),
    FOREIGN KEY (id_usuario) REFERENCES usuario(id_usuario)
);
GO