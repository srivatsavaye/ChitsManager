USE [master]
GO
/****** Object:  Database [ChitsManager]    Script Date: 4/18/2014 12:51:07 AM ******/
CREATE DATABASE [ChitsManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ChitsManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SriSuryaChits.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ChitsManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\SriSuryaChits_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ChitsManager] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ChitsManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ChitsManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ChitsManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ChitsManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ChitsManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ChitsManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [ChitsManager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ChitsManager] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ChitsManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ChitsManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ChitsManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ChitsManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ChitsManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ChitsManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ChitsManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ChitsManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ChitsManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ChitsManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ChitsManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ChitsManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ChitsManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ChitsManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ChitsManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ChitsManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ChitsManager] SET RECOVERY FULL 
GO
ALTER DATABASE [ChitsManager] SET  MULTI_USER 
GO
ALTER DATABASE [ChitsManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ChitsManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ChitsManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ChitsManager] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ChitsManager', N'ON'
GO
USE [ChitsManager]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAuctionsByChitId]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_GetAuctionsByChitId]
@ChitId int = NULL
as 

SELECT  Auctionid
      ,ch.ChitId
	  ,ch.ChitName
      ,c.CustomerId
	  ,c.CustomerName
      ,[Month]
      ,AuctionAmount
  FROM ChitsManager.dbo.tblAuctions a
  inner join ChitsManager.dbo.tblCustomers c
  on c.CustomerId = a.CustomerId
  inner join ChitsManager.dbo.tblChits ch
  on ch.ChitId = a.ChitId
  where ((ch.ChitId = @Chitid) or (@ChitId is null))
  and ch.ActiveFlag = 1 
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomers]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_GetCustomers]
@CustomerId int = null
as
select 
CustomerId
,CustomerName
,Address
,City
,HomePhone
,CellPhone
 from tblCustomers
where CustomerId = ISNULL(@CustomerId, CustomerId) 



GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCustomer]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_InsertCustomer]
@CustomerName nvarchar(200)
,@Address nvarchar(200)
,@City nvarchar(50)
,@HomePhone varchar(50)
,@CellPhone varchar(50)
,@CustomerId int Output
as
insert into tblCustomers (CustomerName, Address, City, HomePhone, CellPhone)
select @CustomerName, @Address, @City, @HomePhone, @CellPhone

return @@Identity

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCustomer]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[sp_UpdateCustomer]
@CustomerName nvarchar(200)
,@Address nvarchar(200) = null
,@City nvarchar(50) = null
,@HomePhone varchar(50) = null
,@CellPhone varchar(50) = null
,@CustomerId int 
as

update tblCustomers
set CustomerName = @CustomerName
,Address = ISNULL(@Address, Address)
,City = ISNULL(@City, City)
,HomePhone = ISNULL(@HomePhone, HomePhone)
,CellPhone = ISNULL(@CellPhone, CellPhone)
where CustomerId = @CustomerId


GO
/****** Object:  Table [dbo].[tblAuctions]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAuctions](
	[Auctionid] [int] IDENTITY(1,1) NOT NULL,
	[ChitId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Month] [int] NULL,
	[AuctionAmount] [int] NULL,
 CONSTRAINT [PK_tblAuctions] PRIMARY KEY CLUSTERED 
(
	[Auctionid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblChits]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblChits](
	[ChitId] [int] IDENTITY(1,1) NOT NULL,
	[ChitName] [nvarchar](50) NOT NULL,
	[NumberofMonths] [smallint] NOT NULL,
	[NumberofCustomers] [smallint] NOT NULL,
	[MonthStarted] [nvarchar](50) NULL,
	[Multiplier] [smallint] NOT NULL,
	[ActiveFlag] [bit] NOT NULL,
 CONSTRAINT [PK_tblChits] PRIMARY KEY CLUSTERED 
(
	[ChitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCustomers]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblCustomers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](200) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[City] [nvarchar](50) NULL,
	[HomePhone] [varchar](50) NULL,
	[CellPhone] [varchar](50) NULL,
	[ActiveFlag] [bit] NOT NULL,
 CONSTRAINT [PK_tblCustomers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblPayments]    Script Date: 4/18/2014 12:51:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPayments](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[AuctionId] [int] NOT NULL,
	[Month] [smallint] NOT NULL,
	[AmountDue] [int] NULL,
	[DueDate] [date] NULL,
	[PaidDate] [date] NULL,
	[Paid] [bit] NOT NULL,
 CONSTRAINT [PK_tblPayments] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[tblChits] ADD  CONSTRAINT [DF_tblChits_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
GO
ALTER TABLE [dbo].[tblCustomers] ADD  CONSTRAINT [DF_tblCustomers_ActiveFlag]  DEFAULT ((1)) FOR [ActiveFlag]
GO
ALTER TABLE [dbo].[tblPayments] ADD  CONSTRAINT [DF_tblPayments_Paid]  DEFAULT ((0)) FOR [Paid]
GO
ALTER TABLE [dbo].[tblAuctions]  WITH CHECK ADD  CONSTRAINT [FK_tblAuctions_tblChits] FOREIGN KEY([ChitId])
REFERENCES [dbo].[tblChits] ([ChitId])
GO
ALTER TABLE [dbo].[tblAuctions] CHECK CONSTRAINT [FK_tblAuctions_tblChits]
GO
ALTER TABLE [dbo].[tblAuctions]  WITH CHECK ADD  CONSTRAINT [FK_tblAuctions_tblCustomers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tblCustomers] ([CustomerId])
GO
ALTER TABLE [dbo].[tblAuctions] CHECK CONSTRAINT [FK_tblAuctions_tblCustomers]
GO
ALTER TABLE [dbo].[tblPayments]  WITH CHECK ADD  CONSTRAINT [FK_tblPayments_tblAuctions] FOREIGN KEY([AuctionId])
REFERENCES [dbo].[tblAuctions] ([Auctionid])
GO
ALTER TABLE [dbo].[tblPayments] CHECK CONSTRAINT [FK_tblPayments_tblAuctions]
GO
USE [master]
GO
ALTER DATABASE [ChitsManager] SET  READ_WRITE 
GO
