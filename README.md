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

### 🔄 08 · CI/CD
Continuous Integration & Continuous Deployment principles, building Pipelines, GitHub Actions, automated testing, Docker inside CI, and environment management (dev / staging / production).

### 🌐 09 · REST vs GraphQL vs gRPC
Understanding the three main API communication styles, when to use each, trade-offs in performance and complexity, and practical examples of each in a microservices context.

### 📋 10 · 12-Factor App
The industry-standard methodology for building scalable, maintainable apps — covering config, dependencies, statelessness, logs, and more. A mental model that ties together everything in the course.

### 🔐 11 · Authentication & Authorization
JWT, OAuth2, API Keys, and session-based auth. Difference between AuthN and AuthZ, role-based access control (RBAC), and how to implement auth correctly across microservices.

### 📊 12 · Logging & Monitoring
Centralized logging, structured logs, log levels, distributed tracing across services, alerting, and intro to observability tools such as the ELK Stack and Grafana.

### ☸️ 13 · Kubernetes Basics
Container orchestration concepts, Pods, Deployments, Services and Ingress, scaling, self-healing, and how Kubernetes extends what we built with Docker and Docker Compose.

---

## Repository Structure

```
architecture-course/
│
├── README.md
│
├── 02-docker/
│   ├── README.md                      ← Theory & explanation
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── Dockerfile.basic
│   │   ├── Dockerfile.multi-stage
│   │   └── docker-compose.yml
│   └── exercises/
│       ├── README.md                  ← Homework instructions
│       ├── exercise-01/               ← Build an Image for a Node.js app
│       ├── exercise-02/               ← Docker Compose with a DB
│       └── solution/                  ← Revealed after submission
│
├── 03-caching/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── redis-basic/
│   │   └── cache-aside-pattern/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Add cache to an existing service
│       ├── exercise-02/               ← TTL & invalidation strategy
│       └── solution/
│
├── 04-microservices/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── user-service/
│   │   ├── order-service/
│   │   └── docker-compose.yml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Split a monolith into two services
│       ├── exercise-02/               ← Inter-service communication
│       └── solution/
│
├── 05-api-gateway/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── nginx-gateway/
│   │   └── express-gateway/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Configure routing for 2 services
│       ├── exercise-02/               ← Add rate limiting & auth middleware
│       └── solution/
│
├── 06-load-balancer/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── nginx-lb/
│   │   └── docker-scale/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Round Robin across 3 instances
│       ├── exercise-02/               ← Health Checks & failover
│       └── solution/
│
├── 07-rabbitmq/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── basic-queue/
│   │   ├── exchange-types/
│   │   └── dead-letter-queue/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Async order queue implementation
│       ├── exercise-02/               ← Dead Letter Queue & retry logic
│       └── solution/
│
├── 08-ci-cd/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── .github/
│   │   │   └── workflows/
│   │   │       ├── ci.yml             ← Automated tests pipeline
│   │   │       └── cd.yml             ← Deployment pipeline
│   │   └── Dockerfile
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Build a CI Pipeline with tests
│       ├── exercise-02/               ← Add CD to staging environment
│       └── solution/
│
└── final-project/
    ├── README.md                      ← Final project description
    └── requirements.md                ← Requirements & submission structure
│
├── 09-rest-graphql-grpc/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── rest-api/
│   │   ├── graphql-api/
│   │   └── grpc-api/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Build the same endpoint in REST & GraphQL
│       ├── exercise-02/               ← gRPC service with .proto file
│       └── solution/
│
├── 10-12-factor/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   └── 12factor-app/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Refactor an app to follow 12-Factor
│       └── solution/
│
├── 11-auth/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── jwt-auth/
│   │   ├── oauth2-flow/
│   │   └── rbac/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← JWT login & protected routes
│       ├── exercise-02/               ← RBAC across two services
│       └── solution/
│
├── 12-logging-monitoring/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── structured-logging/
│   │   └── grafana-setup/
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Add structured logging to a service
│       ├── exercise-02/               ← Set up a basic Grafana dashboard
│       └── solution/
│
├── 13-kubernetes/
│   ├── README.md
│   ├── slides.pdf                     ← Lesson presentation
│   ├── examples/
│   │   ├── pod.yaml
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   └── exercises/
│       ├── README.md
│       ├── exercise-01/               ← Deploy a service to local K8s cluster
│       ├── exercise-02/               ← Scale a deployment & configure Ingress
│       └── solution/
│
└── final-project/
    ├── README.md                      ← Final project description
    └── requirements.md                ← Requirements & submission structure
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
- Email subject format: `[Architecture Course] Lesson 01 - Your Name`

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
