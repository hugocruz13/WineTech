[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/38tL-Jy9)

# Trabalho Prático II

Integração de Sistemas de Informação

Licenciatura em Engenharia de Sistemas Informáticos (regime _laboral_) 2025-26

## Constituição do grupo

| número | nome      | email                 |
| ------ | --------- | --------------------- |
| a23010 | Hugo Cruz | a23010@alunos.ipca.pt |
| a23016 | Dani Cruz | a23016@alunos.ipca.pt |

## Descrição do problema a resolver

Tema: Sistema inteligente de monitorização de adegas para preservação e comercialização de vinhos.

Breve descrição: Este projeto tem como objetivo desenvolver um sistema integrado capaz de monitorizar as condições ambientais de adegas de vinho, nomeadamente temperatura, humidade e luz, através de sensores IoT.
A solução permite centralizar esses dados, apresentá-los de forma clara aos utilizadores finais e garantir transparência sobre as condições de armazenamento dos vinhos disponíveis para compra.
Além da monitorização, o sistema inclui funcionalidades de e-commerce, permitindo aos utilizadores consultar vinhos, gerir um carrinho de compras e finalizar aquisições de forma segura.

Mais informação:

- A solução segue uma arquitetura orientada a serviços (SOA).

para uma descrição do problema e arquitetura prevista para a solução consultar: [doc_23010_23016-descricao.pdf](./doc/doc_23010_23016-descricao.pdf)

## Organização do repositório

[doc/](./doc/) documentação com o relatório

[src/](./src/) código da solução desenvolvida

## Instruções de execução

Pré-requisitos:

- .NET 10
- Node.js (LTS)
- npm
- Docker

Clonar o repositório:

```bash
git clone git@github.com:IPCALESI2526/tp02-23010_23016.git
```

Aceder à pasta do projeto:

```bash
cd tp02-23010_23016/src
```

Mudar para a branch de desenvolvimento:

```bash
git checkout dev
```

Iniciar os serviços com Docker:

```bash
docker compose up -d
```

## Base de Dados

[src/database](./src/database) – scripts da base de dados

#### Credenciais (ambiente de desenvolvimento)

- **Servidor**: `localhost,1433`
- **Utilizador**: `sa`
- **Password**: `Passw0rd_dev!`

> ⚠️ Estas credenciais são usadas **exclusivamente em ambiente de desenvolvimento**.

#### Scripts

O ficheiro `DataBase.sql` é responsável pela criação da base de dados.  
Os restantes ficheiros SQL correspondem a _stored procedures_ utilizadas pelo sistema, desenvolvidas em **modo de desenvolvimento**.

Para facilitar a criação e atualização destas _stored procedures_, existe o ficheiro `run_all.sql`, que agrega todos os scripts e permite a sua execução numa única operação.

## Azure Storage (Modo Desenvolvimento)

Em ambiente de desenvolvimento é utilizado o **Azurite**, que é equivalente a um **Storage Account do Azure**.

Por defeito, os containers são criados como **privados**. Para permitir o acesso público aos blobs, é necessário utilizar a ferramenta **Microsoft Azure Storage Explorer**.

Nos containers de blobs (`adega-images`, `user-images` e `vinho-images`), deve-se:

- Selecionar cada container
- Aceder às opções de acesso
- Definir o nível de acesso como **Public**

Esta configuração é necessária para permitir o acesso às imagens durante o desenvolvimento.

## Backend

O backend encontra-se pronto a executar. Apenas é necessário iniciar simultaneamente os projetos **API** e **SOAP**.

No Visual Studio:

- Abrir a solução `backend`
- Clicar com o botão direito na solução e selecionar **Properties**
- Em **Startup Project**, escolher **Multiple startup projects**
- Definir os projetos **API** e **SOAP** com a ação **Start**
- Aplicar as alterações e executar a solução

## Frontend

Para executar o frontend:

```bash
cd frontend
npm install
```

Criar um ficheiro .env com o seguinte conteúdo:

```bash
VITE_API_URL=https://localhost:7148
```

Iniciar a aplicação:

```bash
npm run dev
```
