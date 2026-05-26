# 🐰 Exercise — Async Messaging with RabbitMQ in a Store Microservice

> **Lesson:** 06 · Message Brokers
> **Level:** Beginner
> **Estimated Time:** 45–60 minutes
> **Tech:** Docker, Docker Compose, RabbitMQ, .NET 8

---

## 🎯 Goal

Connect three microservices — **OrderService**, **InventoryService**, and **NotificationService** — through RabbitMQ.

When a customer places an order, `OrderService` publishes a message.  
Both `InventoryService` and `NotificationService` receive it independently — one updates stock, the other sends a confirmation email — without either knowing about each other.

---

## 🗂️ Folder Structure

```
exercise-rabbitmq-store/
├── OrderService/
│   ├── Program.cs
│   ├── OrderService.csproj
│   └── Dockerfile
├── InventoryService/
│   ├── Program.cs
│   ├── InventoryService.csproj
│   └── Dockerfile
├── NotificationService/
│   ├── Program.cs
│   ├── NotificationService.csproj
│   └── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## 📝 Tasks

---

### Task 1 — Spin Up RabbitMQ

- [ ] Add RabbitMQ to `docker-compose.yml` using the `rabbitmq:3-management` image
- [ ] Expose port `5672` (AMQP) and `15672` (Management UI)
- [ ] Run `docker-compose up rabbitmq` and open `http://localhost:15672`
- [ ] Log in with `guest` / `guest` and confirm the dashboard loads

> 💡 Keep this tab open — you'll use the management UI to watch messages flow in real time throughout this exercise.

---

### Task 2 — OrderService Publishes an Order

- [ ] Create a `POST /orders` endpoint that accepts `productId` and `quantity`
- [ ] When an order comes in, publish it as a message to a **Fanout exchange** named `order.placed`
- [ ] Return `201 Created` with the order details

> 💡 A Fanout exchange broadcasts the same message to every queue bound to it — that's how both consumers will receive it.

---

### Task 3 — InventoryService Listens and Updates Stock

- [ ] On startup, bind a durable queue named `inventory.order.placed` to the `order.placed` exchange
- [ ] Consume messages from that queue and log:
  ```
  [InventoryService] Updating stock for ProductId: SHOE-42, Qty: 2
  ```
- [ ] Acknowledge each message after processing

---

### Task 4 — NotificationService Listens and Sends a Confirmation

- [ ] On startup, bind a separate durable queue named `notification.order.placed` to the **same** `order.placed` exchange
- [ ] Consume messages from that queue and log:
  ```
  [NotificationService] Sending confirmation email for order: SHOE-42
  ```
- [ ] Acknowledge each message after processing

> 💡 Both services bind to the same exchange but each has its own queue — so each gets its own independent copy of every message.

---

### Task 5 — Wire It All Together

- [ ] Add all three services and RabbitMQ to `docker-compose.yml`
- [ ] Make the services depend on the `rabbitmq` container
- [ ] Add `restart: on-failure` to each service so they reconnect if RabbitMQ isn't ready yet
- [ ] Run `docker-compose up --build` — all four containers should start cleanly

---

### Task 6 — Watch the Message Flow

- [ ] POST an order using Postman or curl
- [ ] Check the logs — both `InventoryService` and `NotificationService` should log a message for the same order
- [ ] Open the RabbitMQ UI → **Exchanges** → `order.placed` — confirm it shows 2 bindings
- [ ] Open **Queues** — confirm both queues exist and show activity

---

### Task 7 — Test Resilience

- [ ] Stop `NotificationService` with `docker-compose stop notificationservice`
- [ ] Send 3 more orders
- [ ] Check the RabbitMQ UI — how many messages are waiting in `notification.order.placed`?
- [ ] Restart `NotificationService` — watch it drain the backlog automatically

---

### Task 8 — Reflect in README.md

- [ ] Why does each service need its own queue even though they bind to the same exchange?
- [ ] What would happen if you used a direct queue instead of a Fanout exchange — would both services still get the message?
- [ ] What is the advantage of async messaging over `OrderService` calling both services directly via HTTP?

---

## ✅ Submission Checklist

- [ ] `docker-compose up --build` starts all four containers with no errors
- [ ] A single POST to `/orders` produces a log in **both** `InventoryService` and `NotificationService`
- [ ] Stopping `NotificationService` causes messages to queue up, then drain on restart
- [ ] `README.md` answers the 3 reflection questions

---

## 💡 Tips

- Use `rabbitmq:3-management` — the plain image has no UI
- RabbitMQ takes ~5–10 seconds to boot — `restart: on-failure` handles this automatically
- `docker-compose logs -f` streams logs from all containers at once
- The RabbitMQ UI **Exchanges** tab shows how many queues are bound to your exchange
- Declare the exchange and queue in the **consumer**, not just the producer — whichever service starts first will create it

---

> **One order in. Two services notified. Zero direct dependencies. That's the power of a message broker. 🐰📦🚀**
