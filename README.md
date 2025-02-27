# API de Vendas

Esta API REST permite registrar, buscar e atualizar vendas. Foi desenvolvida em .NET Core e C# seguindo boas pr�ticas de desenvolvimento.

## Tecnologias Utilizadas

- .NET Core 9.0
- C#
- Swagger para documenta��o
- Testes unit�rios com xUnit e Moq

## Instala��o e Execu��o

1. Clone este reposit�rio:
   ```sh
   git clone https://github.com/thiagokleberr/iatec.git
   ```
2. Navegue at� o diret�rio do projeto:
   ```sh
   cd sales-api
   ```
3. Restaure as depend�ncias:
   ```sh
   dotnet restore
   ```
4. Execute a aplica��o:
   ```sh
   dotnet run
   ```
5. Acesse a documenta��o Swagger no navegador:
   ```sh
   http://localhost:5000/api-docs
   ```

## Endpoints

### Registrar Venda

**POST** `/v1/sales`

**Request:**
```json
{
  "seller": {
    "id": 1,
    "cpf": "123.456.789-00",
    "name": "Jo�o Vendedor"
  },
  "items": [
    {
      "productId": 101,
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
      "name": "Jo�o Vendedor"
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

## Testes Unit�rios

Os testes unit�rios foram implementados com xUnit e Moq.
Para executar os testes, utilize o comando:
```sh
  dotnet test
```

## Estrutura do Projeto

- `Controllers/` - Cont�m os controllers da API
- `Services/` - Cont�m a l�gica de neg�cios
- `Models/` - Cont�m as classes de dom�nio
- `Tests/` - Cont�m os testes unit�rios

## Considera��es Finais

Este projeto segue os princ�pios SOLID e boas pr�ticas de desenvolvimento, garantindo um c�digo limpo e bem estruturado.

