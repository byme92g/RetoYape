# 📱 Reto Yape - Microservicios de Transacciones 💸

Este proyecto simula el flujo de transacciones tipo _Yapeo_ con microservicios en .NET y Kafka.  
Incluye endpoints para **crear**, **buscar** y **listar** transacciones.

---

## 📌 Instrucciones

- Ejecute el proyecto con docker-compose para levantar todos los servicios automáticamente usando el archivo docker-compose.yml.
  - Si los estados de las transacciones no se actualizan, reinicia el proyecto. A veces Kafka no inicia en el orden correcto y los topics no están listos hasta reiniciar.
- Las migraciones de la base de datos se ejecutan automáticamente al iniciar el proyecto.

## 🚀 Endpoints Disponibles

Accede al Swagger UI:
http://localhost:8080/swagger/index.html

O prueba con Postman los siguientes endpoints:

| Método | Endpoint                                     | Descripción                               |
| ------ | -------------------------------------------- | ----------------------------------------- |
| POST   | `/Transaction/Yapear`                        | Crear una nueva transacción (Yapeo)       |
| GET    | `/Transaction/BuscarYapeo?externalId={GUID}` | Buscar un Yapeo por su ID externo         |
| GET    | `/Transaction/VerYapeos`                     | Listar todas las transacciones realizadas |

---

## 📄 Ejemplo de Respuesta

```json
[
  {
    "transactionExternalId": "9255d61e-eda3-45c3-b018-c90dc71ce6ec",
    "sourceAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "targetAccountId": "1fa85f64-5717-4562-b3fc-2c963f66afa6",
    "transferType": "TransaccionInterna",
    "value": 2001,
    "createdAt": "2025-03-21T03:54:38.5292462",
    "status": "Rejected"
  },
  {
    "transactionExternalId": "fee66104-644f-490f-a804-79504c6fc1b4",
    "sourceAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "targetAccountId": "1fa85f64-5717-4562-b3fc-2c963f66afa6",
    "transferType": "TransaccionInterna",
    "value": 11,
    "createdAt": "2025-03-21T04:00:13.2196485",
    "status": "Approved"
  }
]
```

## ⚠️ Notas Importantes

- **Para emergencias**, se deja un **script de respaldo** en la siguiente ruta: TransactionService\Transaction.Infraestructure\Scripts\script.sql

## 🏗️ Arquitectura - Microservicios

El proyecto está dividido en 5 microservicios que se comunican entre sí:

| Transaction Service | <------Kafka------> | Anti-Fraud Service |

| SQL Server |
| Zookeeper |
| Kafka |
