-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 04-09-2026 a las 03:44:02
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria_lab2`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria_lab2` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `inmobiliaria_lab2`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `imgPortada` varchar(200) DEFAULT NULL,
  `cupo` int(11) NOT NULL,
  `direccion` varchar(200) NOT NULL,
  `precio` float NOT NULL,
  `estado` tinyint(4) NOT NULL DEFAULT 1,
  `id_propietario` int(11) NOT NULL,
  `id_tipo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id_persona` int(11) NOT NULL,
  `profesion` varchar(100) DEFAULT NULL,
  `garante` varchar(100) DEFAULT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `personas`
--

CREATE TABLE `personas` (
  `id` int(11) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `dni` varchar(8) NOT NULL,
  `mail` varchar(100) NOT NULL,
  `telefono` varchar(50) NOT NULL,
  `direccion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `personas`
--

INSERT INTO `personas` (`id`, `apellido`, `nombre`, `dni`, `mail`, `telefono`, `direccion`) VALUES
(1, 'Malovini', 'Victor Jose David', '33552691', 'malovinivjd@hotmail.com', '02664705269', 'Juana Azurduy esq Sobremonte'),
(10, 'asdasd', 'asa', '22222222', 'mail2@mail.com', '111111111', 'calle valida 123'),
(23, 'Fernandez', 'Sofia', '11111111', 'mailsofi@mail.com', '2664111111', 'calle valida 123'),
(24, 'Malovini', 'Mile', '33333333', 'mailmile@mail.com', '33333333', 'calle valida 123'),
(25, 'Gonzalez', 'Laura', '44444444', 'laura.gonzalez@mail.com', '2664222222', 'Mitre 345, San Luis'),
(26, 'Rodriguez', 'Juan', '55555555', 'juan.rodriguez@mail.com', '2664333333', 'Colon 678, San Luis'),
(27, 'Pereyra', 'Carla', '66666666', 'carla.pereyra@mail.com', '2664444444', 'Pringles 901, San Luis'),
(28, 'Sanchez', 'Martin', '77777777', 'martin.sanchez@mail.com', '2664555555', 'Lafinur 234, San Luis'),
(29, 'Lopez', 'Julieta', '88888888', 'julieta.lopez@mail.com', '2664666666', 'Lujan 567, San Luis');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id_persona` int(11) NOT NULL,
  `cbu` varchar(50) NOT NULL,
  `cuit` varchar(20) NOT NULL,
  `estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id_persona`, `cbu`, `cuit`, `estado`) VALUES
(1, '2147483647111111111111', '20335526911', 1),
(23, '1111111111111111111111', '20111111112', 1),
(24, '3333333333333333333333', '20333333331', 1),
(25, '4444444444444444444444', '20222222222', 1),
(26, '5555555555555555555555', '20233333333', 1),
(27, '6666666666666666666666', '20244444444', 1);

-- Volcado de datos para la tabla `inquilinos`

INSERT INTO `inquilinos` (`id_persona`, `profesion`, `garante`, `estado`) VALUES
(10, 'Empleado administrativo', 'Roberto Fernandez', 1),
(28, 'Ingeniero', 'Miguel Sanchez', 1),
(29, 'Docente', 'Patricia Lopez', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `reservas`
--

CREATE TABLE `reservas` (
  `id` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `fecha_desde` datetime NOT NULL,
  `fecha_hasta` datetime NOT NULL,
  `monto` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcado de datos para la tabla `tipoinmueble`

INSERT INTO `tipoinmueble` (`id`, `nombre`) VALUES
(1, 'Casa'),
(2, 'Departamento'),
(3, 'Local comercial'),
(4, 'Oficina'),
(5, 'Terreno');

-- Volcado de datos para la tabla `inmuebles`

INSERT INTO `inmuebles` (`Id`, `imgPortada`, `cupo`, `direccion`, `precio`, `estado`, `id_propietario`, `id_tipo`) VALUES
(1, NULL, 5, 'Av. Illia 450, San Luis', 250000, 1, 1, 1),
(2, NULL, 3, 'Junin 820, San Luis', 180000, 1, 23, 2),
(3, NULL, 0, 'Rivadavia 1200, San Luis', 320000, 1, 24, 3),
(4, NULL, 8, 'Mitre 345, San Luis', 400000, 1, 25, 4),
(5, NULL, 0, 'Colon 678, San Luis', 150000, 1, 26, 5);

-- Volcado de datos para la tabla `reservas`

INSERT INTO `reservas` (`id`, `id_inquilino`, `id_inmueble`, `fecha_desde`, `fecha_hasta`, `monto`) VALUES
(1, 10, 1, '2026-09-10 00:00:00', '2027-09-10 00:00:00', 250000),
(2, 28, 2, '2026-10-01 00:00:00', '2027-10-01 00:00:00', 180000),
(3, 29, 4, '2026-11-15 00:00:00', '2027-11-15 00:00:00', 400000),
(4, 10, 3, '2027-12-01 00:00:00', '2028-12-01 00:00:00', 320000);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `id_propietario` (`id_propietario`),
  ADD KEY `id_tipo` (`id_tipo`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id_persona`);

--
-- Indices de la tabla `personas`
--
ALTER TABLE `personas`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD UNIQUE KEY `mail` (`mail`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id_persona`),
  ADD UNIQUE KEY `cuit` (`cuit`);

--
-- Indices de la tabla `reservas`
--
ALTER TABLE `reservas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `id_inquilino` (`id_inquilino`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `personas`
--
ALTER TABLE `personas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT de la tabla `reservas`
--
ALTER TABLE `reservas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`id_propietario`) REFERENCES `propietarios` (`id_persona`),
  ADD CONSTRAINT `inmuebles_ibfk_2` FOREIGN KEY (`id_tipo`) REFERENCES `tipoinmueble` (`id`);

--
-- Filtros para la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD CONSTRAINT `inquilinos_ibfk_1` FOREIGN KEY (`id_persona`) REFERENCES `personas` (`id`);

--
-- Filtros para la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD CONSTRAINT `propietarios_ibfk_1` FOREIGN KEY (`id_persona`) REFERENCES `personas` (`id`);

--
-- Filtros para la tabla `reservas`
--
ALTER TABLE `reservas`
  ADD CONSTRAINT `reservas_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmuebles` (`Id`),
  ADD CONSTRAINT `reservas_ibfk_2` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilinos` (`id_persona`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
