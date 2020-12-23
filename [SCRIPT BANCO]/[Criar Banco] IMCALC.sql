/* ...::: Criação do banco de dados oficial do projeto :::... */

USE master;

GO

IF EXISTS (SELECT name from master.sys.databases WHERE name = N'BMISOLUTION')
DROP DATABASE BMISOLUTION;

go

--1º Crie o Banco ( Selecione a linha abaixo e pressione F5 ou clique em executar)
CREATE DATABASE BMISOLUTION;
--2º Para criar as tabelas precisa estar com o banco em uso, Selecione a linha abaixo e pressione F5 ou clique em executar)
GO
USE BMISOLUTION;


CREATE TABLE TL_CLIENT (
		CLI_INT_ID INT NOT NULL IDENTITY (1,1),
		CLIENT_NAME VARCHAR(50) NOT NULL,
		CLIENT_AGE VARCHAR(50)  NOT NULL,
		CLIENT_WEIGH FLOAT  NOT NULL,
		CLIENT_HEIGHT FLOAT  NOT NULL,
		CLIENT_RESLT_COD CHAR(1)   NOT NULL,


		CONSTRAINT PK_CLI_INT_ID PRIMARY KEY (CLI_INT_ID),
);
CREATE TABLE TL_IMC (
		IMC_ID INT NOT NULL IDENTITY (1,1),
		IMC_DESC VARCHAR(50) NOT NULL,

		CONSTRAINT PK_IMC_ID PRIMARY KEY (IMC_ID)

);

/* Popular Informações para operação */

INSERT INTO TL_IMC (IMC_DESC) VALUES ('Abaixo do peso');
INSERT INTO TL_IMC (IMC_DESC) VALUES ('Peso normal');
INSERT INTO TL_IMC (IMC_DESC) VALUES ('Sobrepeso');
INSERT INTO TL_IMC (IMC_DESC) VALUES ('Obesidade grau 1');
INSERT INTO TL_IMC (IMC_DESC) VALUES ('Obesidade grau 2');
INSERT INTO TL_IMC (IMC_DESC) VALUES ('Obesidade grau 3');

/* Verificar no Banco o resultado de um cliente 
SELECT CLIENT_NAME, CLIENT_AGE, CLIENT_WEIGH, CLIENT_HEIGHT, IMC_DESC 
FROM TL_CLIENT 
INNER JOIN TL_IMC ON CLIENT_RESLT_COD = IMC_ID 
WHERE CLI_INT_ID= 1
*/