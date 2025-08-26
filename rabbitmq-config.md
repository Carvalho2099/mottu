# RabbitMQ Messaging Configuration

## Access Variables
- **Host:** `rabbitmq` (dentro do Docker) ou `localhost` (host local)
- **Port:** `5672` (AMQP), `15672` (Painel Web)
- **User:** `guest`
- **Password:** `guest`

Essas variáveis já estão definidas no `docker-compose.yml`:
```yaml
  RABBITMQ_DEFAULT_USER: guest
  RABBITMQ_DEFAULT_PASS: guest
```

## Filas/Tópicos Sugeridos
- **vehicle.events**: para eventos de criação, atualização e deleção de veículos
- **file.uploads**: para eventos de upload de arquivos

As filas podem ser criadas automaticamente pela aplicação ao iniciar.

## Painel Web
- Acesse o painel web do RabbitMQ em: [http://localhost:15672](http://localhost:15672)
- Login: `guest` / `guest`

## Exemplo de conexão (C#)
```csharp
var factory = new ConnectionFactory() {
    HostName = "rabbitmq",
    UserName = "guest",
    Password = "guest"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
```

---

> **Observação:**
> - O acesso externo (fora do Docker) deve usar `localhost`.
> - O acesso entre containers usa o nome do serviço (`rabbitmq`).
> - As credenciais padrão são para desenvolvimento. Em produção, defina usuários e permissões específicas.
