# Estrutura dos Documentos/Collections no MongoDB

## Users
```json
{
  "_id": "ObjectId",
  "email": "string",
  "password": "string",
  "roles": ["string"]
}
```

## Vehicles
```json
{
  "_id": "ObjectId",
  "make": "string",      // Opcional
  "model": "string",     // Obrigatório
  "year": "int",         // Obrigatório, >= 2020
  "plate": "string"      // Obrigatório, min 7 chars, alfanumérico
}
```

## VehicleFiles
```json
{
  "_id": "ObjectId",
  "vehicleId": "ObjectId",
  "createdAt": "ISODate",
  "fileName": "string",
  "fileMimetype": "string",
  "fileUrl": "string"
}
```

- Todos os campos obrigatórios e validações devem ser aplicados na lógica da API.
- `vehicleId` em `VehicleFiles` referencia o `_id` de `Vehicles`.
- `roles` em `Users` define permissões para autenticação.
