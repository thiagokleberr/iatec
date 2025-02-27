# API de Vendas

Esta API REST permite registrar, buscar e atualizar vendas. Foi desenvolvida em .NET Core e C# seguindo boas práticas de desenvolvimento.

## Tecnologias Utilizadas

- .NET Core 9.0
- C#
- Swagger para documentação
- Testes unitários com xUnit e Moq

## Instalação e Execução

1. Clone este repositório:
   ```sh
   git clone https://github.com/thiagokleberr/iatec.git
   ```
2. Navegue até o diretório do projeto:
   ```sh
   cd sales-api
   ```
3. Restaure as dependências:
   ```sh
   dotnet restore
   ```
4. Execute a aplicação:
   ```sh
   dotnet run
   ```
5. Acesse a documentação Swagger no navegador:
   ```sh
   http://localhost:7236/api-docs
   ```
⚠️ Importante: A porta do servidor pode mudar dependendo do ambiente. 
    Caso a aplicação não esteja rodando na porta 7236, verifique a saída do terminal para saber a porta correta.


## Endpoints

### Registrar Venda

**POST** `/v1/sales`

**Request:**
```json
{
  "seller": {
    "cpf": "123.456.789-00",
    "name": "João Vendedor"
  },
  "items": [
    {
      "quantity": 2,
      "price": 50.0
    }
  ]
}
```

**Response:**
```json
{
  "data": "Venda registrada com sucesso! Id: 1",
  "errors": null
}
```

### Buscar Venda

**GET** `/v1/sales/{id}`

**Response:**
```json
{
  "data": {
    "id": 1,
    "seller": {
      "id": 1,
      "cpf": "123.456.789-00",
      "name": "João Vendedor"
    },
    "status": "Aguardando pagamento",
    "items": [
      {
        "productId": 101,
        "quantity": 2,
        "price": 50.0
      }
    ]
  },
  "errors": null
}
```

### Atualizar Status da Venda

**PUT** `/v1/sales/{id}`

**Request:**
```json
{
  "status": "Pagamento Aprovado"
}
```

**Response:**
```json
{
  "data": "Venda atualizada com sucesso!",
  "errors": null
}
```

## Testes Unitários

Os testes unitários foram implementados com xUnit e Moq.
Para executar os testes, utilize o comando:
```sh
  dotnet test
```

## Estrutura do Projeto

- `Controllers/` - Contém os controllers da API
- `Services/` - Contém a lógica de negócios
- `Models/` - Contém as classes de domínio
- `Tests/` - Contém os testes unitários

## Considerações Finais

Este projeto segue os princípios SOLID e boas práticas de desenvolvimento, garantindo um código limpo e bem estruturado.

