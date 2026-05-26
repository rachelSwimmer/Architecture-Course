# ⚖️ Exercise — Load Balancer with Nginx & Docker

> **Lesson:** 05 · Load Balancer  
> **Level:** Beginner  
> **Estimated Time:** 45–60 minutes  
> **Tech:** Docker, Docker Compose, Nginx, .NET 8

---

## 🎯 Goal

Spin up **3 instances** of the same .NET 8 service and put Nginx in front of them.  
Watch requests spread across all instances — that's load balancing in action.

---

## 🗂️ Folder Structure

```
exercise-load-balancer/
├── service/
│   ├── Program.cs
│   ├── service.csproj
│   └── Dockerfile
├── nginx/
│   └── nginx.conf
├── docker-compose.yml
└── README.md
```

---

## 📝 Tasks

---

### Task 1 — Create the Service

- [ ] Create a new .NET 8 minimal Web API project
- [ ] Add a single `GET /` endpoint that returns the container's hostname  
  > 💡 Hint: `System.Net.Dns.GetHostName()`
- [ ] Run it locally and confirm you see the hostname in the response

---

### Task 2 — Dockerize the Service

- [ ] Write a `Dockerfile` for the service using a multi-stage build
- [ ] Build the image and run it on port `8080`
- [ ] Confirm the endpoint still works inside the container

---

### Task 3 — Configure Nginx

- [ ] Create `nginx/nginx.conf` with an `upstream` block pointing to the service
- [ ] Add a `server` block that listens on port `80` and proxies to the upstream

> 💡 Nginx Round Robin is the default — you don't need to configure it explicitly.

---

### Task 4 — Wire It All Together

- [ ] Write a `docker-compose.yml` that runs **3 replicas** of the service and the Nginx container
- [ ] Make sure Nginx depends on the service and mounts your `nginx.conf`
- [ ] Run `docker-compose up --build` — all containers should start cleanly

---

### Task 5 — Watch the Load Balancing

- [ ] Hit `http://localhost:8080` at least 6 times (use `curl` or Postman)
- [ ] Confirm you see **different hostnames** rotating across responses

---

### Task 6 — Reflect in README.md

- [ ] What is Round Robin and what did you observe?
- [ ] What would happen if you scaled to 5 instances instead of 3?
- [ ] What problem does a load balancer solve that a single server can't?

---

### Bonus ⭐ (Optional)

- [ ] Scale to 5 replicas — does the rotation pattern change?
- [ ] Stop one container mid-run — what does Nginx return?
- [ ] Add a `GET /health` endpoint and configure a health check in `docker-compose.yml`

---

## ✅ Submission Checklist

- [ ] `docker-compose up --build` runs with no errors
- [ ] Different hostnames appear across responses
- [ ] `README.md` answers the 3 reflection questions

---

## 💡 Tips

- .NET 8 containers default to port `8080` — set `ASPNETCORE_URLS=http://+:8080` in Compose
- Docker assigns each container a random hostname — that's what makes the rotation visible
- `docker-compose ps` shows all running containers
- `docker-compose down` cleans everything up

---

> **You just built a load balancer. Three servers, one front door, zero downtime. ⚖️🚀**
