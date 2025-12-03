create database ISI;

use ISI;

create table Vinhos (
  Id         int identity not null, 
  Nome       nvarchar(255) not null, 
  Produtor   nvarchar(255) not null, 
  Ano        int not null, 
  Tipo       nvarchar(255) not null, 
  Regiao     nvarchar(255) not null, 
  Descricao  nvarchar(255) not null, 
  Quantidade int not null, 
  ImagemUrl  nvarchar(255) null, 
  primary key (Id));
create table Sensores (
  Id                    int identity not null, 
  IdentificadorHardware nvarchar(255) null, 
  Tipo                  nvarchar(255) not null, 
  Estado                bit not null, 
  primary key (Id));
create table Leituras (
  Id       int identity not null, 
  SensorId int not null, 
  Valor    float(10) not null, 
  DataHora datetime2(0) default GETDATE() not null, 
  primary key (Id));
create table Alertas (
  Id         int identity not null, 
  TipoAlerta nvarchar(255) not null, 
  Mensagem   nvarchar(255) not null, 
  DataHora   datetime2(0) default GETDATE() not null, 
  Resolvido  bit not null, 
  SensoresId int not null, 
  primary key (Id));
create table Utilizadores (
  Id          int identity not null, 
  Auth0UserId nvarchar(100) not null unique, 
  primary key (Id));
create table Compras (
  Id             int identity not null, 
  DataCompra     datetime2(7) default GETDATE() not null, 
  ValorTotal     decimal(19, 0) null, 
  UtilizadoresId int not null, 
  primary key (Id));
create table Carrinho (
  Id             int identity not null, 
  VinhosId       int not null, 
  UtilizadoresId int not null, 
  Quantidade     int not null, 
  primary key (Id));
create table LinhasCompra (
  Id            int identity not null, 
  ComprasId     int not null, 
  VinhosId      int not null, 
  Quantidade    int not null, 
  PrecoUnitario float(10) not null, 
  primary key (Id));
alter table Leituras add constraint FKLeituras498639 foreign key (SensorId) references Sensores (Id);
alter table Carrinho add constraint FKCarrinho434971 foreign key (VinhosId) references Vinhos (Id);
alter table Carrinho add constraint FKCarrinho190422 foreign key (UtilizadoresId) references Utilizadores (Id);
alter table Compras add constraint FKCompras967168 foreign key (UtilizadoresId) references Utilizadores (Id);
alter table LinhasCompra add constraint FKLinhasComp46120 foreign key (ComprasId) references Compras (Id);
alter table LinhasCompra add constraint FKLinhasComp914077 foreign key (VinhosId) references Vinhos (Id);
alter table Alertas add constraint FKAlertas224704 foreign key (SensoresId) references Sensores (Id);
