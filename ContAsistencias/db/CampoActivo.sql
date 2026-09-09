-- Añadir columna activo (por defecto 1 = activo)
ALTER TABLE usuario
ADD activo BIT NOT NULL CONSTRAINT DF_usuario_activo DEFAULT(1);