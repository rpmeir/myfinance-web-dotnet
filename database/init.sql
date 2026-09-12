CREATE TABLE IF NOT EXISTS planoconta (
    id integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    descricao varchar(50) NOT NULL,
    tipo char(1) NOT NULL
);

CREATE TABLE IF NOT EXISTS transacao (
    id integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    historico text,
    data date NOT NULL,
    valor numeric(9, 2),
    planocontaid integer NOT NULL REFERENCES planoconta (id)
);
