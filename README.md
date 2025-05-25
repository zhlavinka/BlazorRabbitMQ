# Blazor / RabbitMQ Demo

This is a simple proof-of-concept app demonstrating real-time messaging with **Blazor WebAssembly**, **SignalR**, and **RabbitMQ**.

---

## Setup

1. **Run RabbitMQ with Docker Compose**

Make sure Docker is installed and running. Then from the root folder where `docker-compose.yml` lives, run:

```docker-compose up -d```

This starts RabbitMQ on `localhost:5672` and the management UI at `http://localhost:15672` (default user/pass: `guest`/`guest`).

---

## Project Structure

| Project                   | Description                                                     |
|---------------------------|-----------------------------------------------------------------|
| **BlazorRabbitMQ.Server** | ASP.NET Core hosted backend<br>Runs the SignalR Hub, API controller, and a background worker that listens to RabbitMQ messages and forwards them to clients. |
| **BlazorRabbitMQ.Client** | Blazor WebAssembly frontend<br>Connects to the SignalR hub and displays incoming messages in real time. |
| **BlazorRabbitMQ.Shared** | Shared DTOs and models used by both client and server.         |

---

##  How to Use

### Publish a Message to RabbitMQ via API

Send a POST request to the API to publish a message:

POST http://localhost:5092/api/messages
Content-Type: application/json

{
  "message": "Hello from RabbitMQ!"
}

Example using curl:

curl -X POST http://localhost:5092/api/messages -H "Content-Type: application/json" -d "{\"message\":\"Hello from RabbitMQ!\"}"

- The server publishes the message to RabbitMQ.
- The background service consumes the message from RabbitMQ.
- It forwards the message to connected Blazor clients via SignalR.
- Your Blazor app UI updates live with the new message.

---

## Notes

- The RabbitMQ connection settings are configured for localhost; adjust if running RabbitMQ elsewhere.
- The SignalR hub is mapped at `/chathub`.
- This is intended as a simple demo for learning purposes.
