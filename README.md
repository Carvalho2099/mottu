# Vehicle Management API

API para gerenciamento de veículos, usuários e arquivos, com integração a RabbitMQ, Minio e MongoDB.

## Sumário
- [Endpoints](#endpoints)
- [Exemplos de Payload](#exemplos-de-payload)
- [Exemplos de Uso](#exemplos-de-uso)
- [Setup do Projeto](#setup-do-projeto)

---

## Arquivos de Documentação Complementar

O projeto inclui arquivos de documentação auxiliar para configuração e referência rápida dos serviços integrados:

- **minio-storage.md**: Guia de configuração do MinIO, incluindo variáveis de acesso, permissões e dicas para buckets. Útil para quem precisa entender ou ajustar o armazenamento de arquivos.
- **mongo-collections-structure.md**: Estrutura dos documentos e collections no MongoDB usados pela aplicação, com exemplos de payloads. Consulte para criar queries ou ajustar dados manualmente.
- **rabbitmq-config.md**: Parâmetros de acesso, filas sugeridas e variáveis de ambiente para o RabbitMQ. Ideal para integração de mensageria.

Esses arquivos estão na raiz do projeto e podem ser abertos diretamente em qualquer editor de texto. Para mais detalhes, acesse-os pelo caminho relativo:

- `./minio-storage.md`
- `./mongo-collections-structure.md`
- `./rabbitmq-config.md`

---

## Endpoints

### Saúde
- **GET** `/health`
  - _Retorna status da API._
  - **Exemplo de resposta:**
    ```json
    { "status": "Healthy" }
    ```
    ![Exemplo visual da resposta de saúde](docs/img/response-health.png)


### Autenticação
- **POST** `/auth/login`
  - _Autentica usuário._
  - **Payload:**
    ```json
    { "username": "user", "password": "pass" }
    ```
  - **Resposta (200):**
    ```json
    { "token": "jwt-token", "user": { "id": "...", "username": "user" } }
    ```
    ![Exemplo visual da resposta de login](docs/img/response-login.png)

  - **Resposta (401):**
    ```json
    { "errors": ["Invalid credentials"] }
    ```

### Veículos
- **GET** `/vehicles`
  - _Lista veículos._
  - **Resposta:**
    ```json
    [ { "id": "1", "model": "Model S", "plate": "XYZ9876", "year": 2022 } ]
    ```
    ![Exemplo visual da resposta de listagem de veículos](docs/img/response-vehicles-list.png)

- **POST** `/vehicles`
  - _Cadastra veículo._
  - **Payload:**
    ```json
    { "model": "Model S", "plate": "XYZ9876", "year": 2022 }
    ```
  - **Resposta (201):**
    ```json
    { "id": "1", "model": "Model S", "plate": "XYZ9876", "year": 2022 }
    ```
    ![Exemplo visual da resposta de cadastro de veículo](docs/img/response-vehicles-create.png)

  - **Resposta (400):**
    ```json
    { "errors": ["Model is required."] }
    ```

### Arquivos de Veículo
- **GET** `/vehicles/{vehicleId}/files`
  - _Lista arquivos de um veículo._
  - **Resposta:**
    ```json
    [ { "id": "file1", "vehicleId": "1", "fileName": "doc.pdf" } ]
    ```
    ![Exemplo visual da resposta de listagem de arquivos](docs/img/response-files-list.png)


---

## Exemplos de Uso

### Autenticação
```bash
curl -X POST http://localhost:5000/auth/login \
     -H 'Content-Type: application/json' \
     -d '{"username": "user", "password": "pass"}'
```

### Listar Veículos
```bash
curl http://localhost:5000/vehicles
```

### Cadastrar Veículo
```bash
curl -X POST http://localhost:5000/vehicles \
     -H 'Content-Type: application/json' \
     -d '{"model": "Model S", "plate": "XYZ9876", "year": 2022}'
```

---

## Setup do Projeto

### Pré-requisitos
- .NET 8 ou superior
- Docker (para MongoDB, RabbitMQ, Minio)

### Serviços Externos via Docker Compose

O projeto utiliza um arquivo `docker-compose.yml` para orquestrar os serviços externos necessários:

- **MongoDB**: Banco de dados principal
  - Porta: `27017`
  - Usuário: `root`, Senha: `example`
- **Minio**: Armazenamento de arquivos S3 compatível
  - Porta: `9000` (API), `9001` (Console)
  - Usuário: `minio`, Senha: `minio123`
- **RabbitMQ**: Fila de mensagens
  - Porta: `5672` (broker), `15672` (console)
  - Usuário: `guest`, Senha: `guest`
- **API**: Sua aplicação
  - Porta: `5001` (externa, mapeada para 80 interna)

### Variáveis de Ambiente Importantes

A aplicação lê variáveis de ambiente para conectar aos serviços:

```env
Mongo__ConnectionString=mongodb://root:example@mongodb:27017
Minio__Endpoint=minio:9000
Minio__AccessKey=minio
Minio__SecretKey=minio123
RabbitMQ__HostName=rabbitmq
RabbitMQ__UserName=guest
RabbitMQ__Password=guest
```
Essas variáveis já estão configuradas no serviço `api` do `docker-compose.yml`.

### Subindo todos os serviços

Para rodar tudo de forma integrada, basta executar:

```bash
docker-compose up -d
```

Isso irá subir MongoDB, Minio, RabbitMQ e a própria API.

- Para ver os logs da API:
  ```bash
  docker logs -f api
  ```
- Para acessar o console do Minio: http://localhost:9001
- Para acessar o console do RabbitMQ: http://localhost:15672 (user: guest, senha: guest)

### Rodando a aplicação localmente (fora do container)

Se quiser rodar a API em sua máquina (com os serviços externos via docker):

```bash
dotnet run --project VehicleManagement.Api/VehicleManagement.Api.csproj
```

Certifique-se de que as variáveis de ambiente estejam setadas ou configuradas no `appsettings.Development.json`.

### Rodando os testes

```bash
dotnet test VehicleManagement.Tests/VehicleManagement.Tests.csproj
```

---

## Observações
- Configure variáveis de ambiente ou `appsettings.json` para conexões com MongoDB, RabbitMQ e Minio.
- Para exemplos detalhados de payloads de erro, consulte os testes ou o código dos controllers.
- Para dúvidas, consulte a documentação dos endpoints ou abra uma issue.
