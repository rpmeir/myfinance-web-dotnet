
SQL Server

```sql

create database myfinance;

create table planoconta (
    id int identity(1,1) not null,
    descricao varchar(50) not null,
    tipo char(1) not null,
    primary key (id)
);

create table transacao (
    id int identity(1,1) not null,
    historico text null,
    data date not null,
    valor decimal(9,2),
    planocontaid int not null,
    primary key (id),
    foreign key (planocontaid) references planoconta (id)
);
```

Postgres

```sql
create database myfinance;


create table planoconta (
    id int generated always as identity primary key,
    descricao varchar(50) not null,
    tipo char(1) not null
);

create table transacao (
    id int generated always as identity primary key,
    historico text,
    data date not null,
    valor numeric(9,2),
    planocontaid int not null references planoconta (id)
);

```
