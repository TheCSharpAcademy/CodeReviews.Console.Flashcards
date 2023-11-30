USE [master]
GO

/****** Object:  Database [Flashcards]    Script Date: 30/11/2023 9:24:19 ******/
DROP DATABASE [Flashcards]
GO

/****** Object:  Database [Flashcards]    Script Date: 30/11/2023 9:24:19 ******/
CREATE DATABASE [Flashcards]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Flashcards', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Flashcards.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Flashcards_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Flashcards_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Flashcards].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Flashcards] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Flashcards] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Flashcards] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Flashcards] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Flashcards] SET ARITHABORT OFF 
GO

ALTER DATABASE [Flashcards] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Flashcards] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Flashcards] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Flashcards] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Flashcards] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Flashcards] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Flashcards] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Flashcards] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Flashcards] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Flashcards] SET  ENABLE_BROKER 
GO

ALTER DATABASE [Flashcards] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Flashcards] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Flashcards] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Flashcards] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Flashcards] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Flashcards] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Flashcards] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Flashcards] SET RECOVERY FULL 
GO

ALTER DATABASE [Flashcards] SET  MULTI_USER 
GO

ALTER DATABASE [Flashcards] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Flashcards] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Flashcards] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Flashcards] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Flashcards] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Flashcards] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [Flashcards] SET QUERY_STORE = ON
GO

ALTER DATABASE [Flashcards] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [Flashcards] SET  READ_WRITE 
GO

