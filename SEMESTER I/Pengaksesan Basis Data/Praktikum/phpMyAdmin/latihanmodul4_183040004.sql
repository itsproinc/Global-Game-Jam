-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 29 Okt 2018 pada 11.45
-- Versi Server: 10.1.30-MariaDB
-- PHP Version: 7.2.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `latihanmodul4_183040004`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `mahasiswa`
--

CREATE TABLE `mahasiswa` (
  `nrp` char(10) NOT NULL,
  `nama` char(30) DEFAULT NULL,
  `alamat` char(30) DEFAULT NULL,
  `umur` int(11) DEFAULT NULL,
  `no_hp` char(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data untuk tabel `mahasiswa`
--

INSERT INTO `mahasiswa` (`nrp`, `nama`, `alamat`, `umur`, `no_hp`) VALUES
('133040001', 'Asep Kasep', 'Setiabudi 223 Bandung', 19, '02123334534'),
('133040002', 'Budi', 'Buah Batu 23 Bandung', 18, '08123335555'),
('133040003', 'Cantika', 'A Yani 4 Sumedang', 18, '08123335566'),
('133040004', 'Dendy', 'M Toha 78 Bandung', 19, '08123337789'),
('133040005', 'Eman', 'Baros 11 Cimahi', 20, '08123367543');

-- --------------------------------------------------------

--
-- Struktur dari tabel `pegawai`
--

CREATE TABLE `pegawai` (
  `nip` varchar(5) NOT NULL,
  `nama` varchar(25) NOT NULL,
  `alamat` varchar(30) NOT NULL,
  `tgl_lahir` date DEFAULT NULL,
  `no_telp` varchar(15) DEFAULT NULL,
  `thn_masuk` year(4) DEFAULT NULL,
  `golongan` char(10) DEFAULT NULL,
  `gaji` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data untuk tabel `pegawai`
--

INSERT INTO `pegawai` (`nip`, `nama`, `alamat`, `tgl_lahir`, `no_telp`, `thn_masuk`, `golongan`, `gaji`) VALUES
('001', 'Ahmad Burhanuddin', 'Jln Buah Batu 15b', '1990-12-12', '08123334543', 2010, '3', 1250000),
('003', 'Budhy Bungaox', 'Jln Cisoka 112', '1989-01-20', '0812367654', 2011, '4', 1050000),
('004', 'Zulkarnaen', 'Jln Alhambra 2', '1991-02-20', '0812367655', 2009, '1', 1450000),
('005', 'Dewi Sudewa', 'Jln Iman 34', '1990-12-02', '08123337766', 2009, '1', 1450000),
('006', 'Ina Nurilian', 'Jln Cisatu 1', '1993-08-09', '0812345676', 2011, '4', 1050000),
('007', 'Chappy Chardut', 'Jln Cilama 13', '1992-07-09', '0812345688', 2011, '4', 1050000),
('008', 'Dodong M', 'Jln Sutami 16', '1990-07-10', '0812345555', 2010, '3', 1250000),
('009', 'Bakhtiar', 'Jln Batam', '2000-01-27', '0800000', 2018, '1', 12500000);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `mahasiswa`
--
ALTER TABLE `mahasiswa`
  ADD PRIMARY KEY (`nrp`);

--
-- Indexes for table `pegawai`
--
ALTER TABLE `pegawai`
  ADD PRIMARY KEY (`nip`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
