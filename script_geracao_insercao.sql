-- Tabela Clientes
CREATE TABLE Clientes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    telefone VARCHAR(20),
    email VARCHAR(255),
    endereco VARCHAR(255)
);
GO

-- Tabela Produtos
CREATE TABLE Produtos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    preco DECIMAL(18, 2) NOT NULL,
    quantidadeEmEstoque INT NOT NULL
);
GO

-- Tabela Carrinhos
CREATE TABLE Carrinhos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cliente_id INT,
    FOREIGN KEY (cliente_id) REFERENCES Clientes(id)
);
GO

-- Tabela ItensCarrinho
CREATE TABLE ItensCarrinho (
    id INT IDENTITY(1,1) PRIMARY KEY,
    carrinho_id INT,
    produto_id INT,
    quantidade INT,
    FOREIGN KEY (carrinho_id) REFERENCES Carrinhos(id),
    FOREIGN KEY (produto_id) REFERENCES Produtos(id)
);
GO

-- Tabela Pedidos
CREATE TABLE Pedidos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    numero VARCHAR(50),
    data DATE,
    cliente_id INT,
    status VARCHAR(50),
    FOREIGN KEY (cliente_id) REFERENCES Clientes(id)
);
GO

-- Tabela ItensPedido
CREATE TABLE ItensPedido (
    id INT IDENTITY(1,1) PRIMARY KEY,
    pedido_id INT,
    produto_id INT,
    quantidade INT,
    preco FLOAT,
    FOREIGN KEY (pedido_id) REFERENCES Pedidos(id),
    FOREIGN KEY (produto_id) REFERENCES Produtos(id)
);
GO

-- Tabela Estoque
CREATE TABLE Estoque (
    id INT IDENTITY(1,1) PRIMARY KEY,
    produto_id INT,
    quantidade INT,
    dataValidade DATE,
    FOREIGN KEY (produto_id) REFERENCES Produtos(id)
);
GO

-- Tabela MovimentacoesEstoque
CREATE TABLE MovimentacoesEstoque (
    id INT IDENTITY(1,1) PRIMARY KEY,
    produto_id INT,
    quantidade INT,
    tipo VARCHAR(50),
    dataMovimentacao DATE,
    FOREIGN KEY (produto_id) REFERENCES Produtos(id)
);
GO

-- Tabela Fornecedores
CREATE TABLE Fornecedores (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    contato VARCHAR(255),
    endereco VARCHAR(255)
);
GO

-- Tabela Insumos
CREATE TABLE Insumos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    tipo VARCHAR(255),
    quantidade INT,
    dataValidade DATE
);
GO

-- Tabela Plantacoes
CREATE TABLE Plantacoes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    alimento VARCHAR(255),
    dataCadastro DATE,
    dataPlantio DATE,
    dataColheita DATE,
    status VARCHAR(50),
    producao_id INT,
    FOREIGN KEY (producao_id) REFERENCES Producoes(id)
);
GO

-- Tabela Producoes
CREATE TABLE Producoes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    localizacao VARCHAR(255),
    dataCadastro DATE,
    status VARCHAR(50)
);
GO

-- Tabela PlantacaoInsumos
CREATE TABLE PlantacaoInsumos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    plantacao_id INT NOT NULL,
    insumo_id INT NOT NULL,
    quantidade INT NOT NULL,
    FOREIGN KEY (plantacao_id) REFERENCES Plantacoes(id),
    FOREIGN KEY (insumo_id) REFERENCES Insumos(id)
);
GO

-- Tabela Usuarios
CREATE TABLE Usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    tipo VARCHAR(50) NOT NULL
);
GO

-- Tabela GoogleMapsAPI
CREATE TABLE GoogleMapsAPI (
    id INT IDENTITY(1,1) PRIMARY KEY,
    apiKey VARCHAR(255),
    pedido_id INT,
    FOREIGN KEY (pedido_id) REFERENCES Pedidos(id)
);
GO

-- Tabela Rastreamentos
CREATE TABLE Rastreamentos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(50),
    status VARCHAR(50),
    localizacaoAtual VARCHAR(255),
    pedido_id INT,
    FOREIGN KEY (pedido_id) REFERENCES Pedidos(id)
);
GO
