# MinIO Storage Configuration

## Access Variables
- **Endpoint:** `minio:9000` (dentro do Docker) ou `localhost:9000` (host local)
- **Access Key:** `minio`
- **Secret Key:** `minio123`

Essas variáveis já estão definidas no `docker-compose.yml`:
```yaml
  MINIO_ROOT_USER: minio
  MINIO_ROOT_PASSWORD: minio123
```

## Buckets
- Recomenda-se criar um bucket chamado `vehicle-files` para armazenar os arquivos dos veículos.
- Os buckets podem ser criados manualmente via painel web do MinIO ou automaticamente pela aplicação.

## Permissões
- O usuário root (`minio`) tem permissão total para criar, ler, atualizar e deletar arquivos/buckets.
- Para ambientes de produção, recomenda-se criar usuários e políticas específicas no MinIO para limitar o acesso.

## Painel Web
- Acesse o painel web do MinIO em: [http://localhost:9000](http://localhost:9000)
- Faça login com as credenciais acima para gerenciar buckets e arquivos.

## Exemplo de conexão (C#)
```csharp
var minioClient = new Minio.MinioClient()
    .WithEndpoint("minio", 9000)
    .WithCredentials("minio", "minio123")
    .Build();
```

---

> **Observação:**
> - O acesso externo (fora do Docker) deve usar `localhost:9000`.
> - O acesso entre containers usa o nome do serviço (`minio:9000`).
> - As credenciais devem ser mantidas seguras em produção.
