# 🏗️ Modern System Architecture Course

> A comprehensive course on designing and building scalable software systems using modern architecture principles.

---

## 📋 Table of Contents

- [About](#about)
- [Syllabus](#syllabus)
- [Repository Structure](#repository-structure)
- [Prerequisites](#prerequisites)
- [Required Tools](#required-tools)
- [Homework & Submissions](#homework--submissions)
- [Contact](#contact)

---

## About

This course is designed for developers who want to deepen their knowledge in modern system architecture.  
We cover key technologies and concepts widely used in the industry, combining theory with hands-on practice.

---

## Syllabus

### 🐳 02 · Docker
Containerization fundamentals, containers vs virtual machines, building Images, writing `Dockerfile`s, managing networks and volumes, and orchestrating multi-service environments with Docker Compose.

### ⚡ 03 · Caching
Caching principles, cache types (in-memory, distributed), working with Redis, strategies such as Cache-Aside, Write-Through and TTL, and how caching improves performance and reduces database load.

### 🧩 04 · Microservices
Moving from monolith to Microservices, SOLID principles at the service level, sync vs async communication, distributed data management, Bounded Context, and Domain Driven Design basics.

### 🚪 05 · API Gateway
The Gateway as a single entry point, routing management, authentication, rate limiting, centralized logging, and how a Gateway simplifies communication with Microservices.

### ⚖️ 06 · Load Balancer
Load balancing principles, strategies (Round Robin, Least Connections, IP Hash), L4 vs L7 differences, High Availability, Health Checks, and Horizontal Scaling.

### 🐰 07 · RabbitMQ · Message Queues
Queue architecture, Producer/Consumer Pattern, Exchanges, Queues & Bindings, Acknowledgments, Dead Letter Queues, and async messaging for service decoupling.

### 📊 08 · Logging & Monitoring
Centralized logging, structured logs, log levels, distributed tracing across services, alerting, and intro to observability tools such as the ELK Stack and Grafana.

### 🗄️ 09 · Databases — SQL vs NoSQL + Indexes
Relational model and ACID transactions, SQL index types (B-Tree, composite, covering), query planning with `EXPLAIN`, NoSQL categories (Document, Key-Value, Column, Graph), CAP theorem, eventual consistency, and a decision framework for choosing the right database per microservice.

### 🔄 10 · CI/CD
Continuous Integration & Continuous Deployment principles, building Pipelines, GitHub Actions, automated testing, Docker inside CI, and environment management (dev / staging / production).

### 🌐 11 · REST vs GraphQL vs gRPC
Understanding the three main API communication styles, when to use each, trade-offs in performance and complexity, and practical examples of each in a microservices context.

### 📋 12 · 12-Factor App
The industry-standard methodology for building scalable, maintainable apps — covering config, dependencies, statelessness, logs, and more. A mental model that ties together everything in the course.

### 🔐 13 · Authentication & Authorization
JWT, OAuth2, API Keys, and session-based auth. Difference between AuthN and AuthZ, role-based access control (RBAC), and how to implement auth correctly across microservices.

### ☸️ 14 · Kubernetes Basics
Container orchestration concepts, Pods, Deployments, Services and Ingress, scaling, self-healing, and how Kubernetes extends what we built with Docker and Docker Compose.

---

## Repository Structure

```
architecture-course/
│
├── README.md
│
├── 02-docker/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── Dockerfile.basic
│   │   ├── Dockerfile.multi-stage
│   │   └── docker-compose.yml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 03-caching/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── redis-basic/
│   │   └── cache-aside-pattern/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 04-microservices/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── user-service/
│   │   ├── order-service/
│   │   └── docker-compose.yml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 05-api-gateway/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── nginx-gateway/
│   │   └── express-gateway/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 06-load-balancer/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── nginx-lb/
│   │   └── docker-scale/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 07-rabbitmq/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── basic-queue/
│   │   ├── exchange-types/
│   │   └── dead-letter-queue/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 08-logging-monitoring/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── structured-logging/
│   │   └── grafana-setup/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 09-databases/                          ← NEW
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── postgres-indexes/
│   │   ├── mongodb-documents/
│   │   └── docker-compose.yml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/                   ← Index impact measurement
│       ├── exercise-02/                   ← SQL vs NoSQL schema comparison
│       └── solution/
│
├── 10-ci-cd/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── .github/workflows/
│   │   │   ├── ci.yml
│   │   │   └── cd.yml
│   │   └── Dockerfile
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 11-rest-graphql-grpc/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── rest-api/
│   │   ├── graphql-api/
│   │   └── grpc-api/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 12-12-factor/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   └── 12factor-app/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       └── solution/
│
├── 13-auth/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── jwt-auth/
│   │   ├── oauth2-flow/
│   │   └── rbac/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
├── 14-kubernetes/
│   ├── README.md
│   ├── slides.pdf
│   ├── examples/
│   │   ├── pod.yaml
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/
│       ├── exercise-02/
│       └── solution/
│
└── final-project/
    ├── README.md
    └── requirements.md
```

---

## Prerequisites

- Basic knowledge of at least one of: **C# / .NET**, **Python**, or **JavaScript / Node.js**
- Familiarity with Git & GitHub
- Basic understanding of HTTP & REST APIs
- Basic experience with Terminal / Command Line

---

## Required Tools

| Tool | Min Version | Purpose |
|------|:-----------:|---------|
| Docker Desktop | 24+ | Containerization |
| .NET SDK | 8+ | Running C# services |
| Python | 3.11+ | Running Python services |
| Node.js | 18 LTS | Running JS services |
| Git | 2.40+ | Version control |
| VS Code | Latest | Code editing |
| Postman / Insomnia | Latest | API testing |

---

## Homework & Submissions

### How to Submit

1. **Fork** this repository to your personal GitHub account
2. Complete the exercises inside `exercises/exercise-XX/` on your fork
3. Make sure your fork is **public** so it can be reviewed
4. Send the link to your fork via **email** to the instructor before the deadline

### Submission Rules

- Submit at least **48 hours** before the next lesson
- Each exercise must include a short `README.md` explaining your solution
- Code must run without errors after `docker-compose up` (where applicable)
- Email subject format: `[Architecture Course] Lesson 0X - Your Name`

### Grading

| Component | Weight |
|-----------|:------:|
| Weekly homework | 50% |
| Final project | 40% |
| Class participation | 10% |

---

## Contact

For questions and help — open an **Issue** in this repository with the appropriate label:

- `question` — general question
- `bug` — problem with example code
- `feedback` — suggestions for improvement

---

> **Happy Coding! 🚀**
