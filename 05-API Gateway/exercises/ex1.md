# 🚪 Exercise — API Gateway with Ocelot or YARP

> **Lesson:** 05 · API Gateway  
> **Level:** Beginner  
> **Tech:** .NET 8, C#

---

## 🎯 Goal

Add an API Gateway to an existing microservices project using either **Ocelot** or **YARP**.  
By the end of this exercise you'll have a single entry point that routes requests to two services.

---

## 🗂️ Starting Point

You have two existing microservices already running:

| Service | Port | Endpoint |
|---|---|---|
| **UserService** | `5001` | `GET /users` → returns a list of users |
| **ProductService** | `5002` | `GET /products` → returns a list of products |

> If you don't have them yet — create two minimal ASP.NET Web API projects with one endpoint each. A hardcoded list response is fine.

---

## 📝 Tasks

---

### Task 1 — Create the Gateway Project

- [ ] Create a new empty ASP.NET Web API project named `ApiGateway`
- [ ] Make sure your solution structure looks like this:

```
solution/
├── UserService/
├── ProductService/
└── ApiGateway/          ← new project
```

- [ ] Confirm all three projects run without errors before continuing

---

### Task 2 — Choose Your Gateway: Ocelot or YARP

Pick **one** to implement. Both achieve the same goal — routing — but with different config styles.

---

#### 🅰️ Option A — Ocelot

**Install the package:**

```bash
dotnet add package Ocelot
```

**Create `ocelot.json` in the `ApiGateway` root:**

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/users",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "localhost", "Port": 5001 }
      ],
      "UpstreamPathTemplate": "/api/users",
      "UpstreamHttpMethod": [ "GET" ]
    },
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "localhost", "Port": 5002 }
      ],
      "UpstreamPathTemplate": "/api/products",
      "UpstreamHttpMethod": [ "GET" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

**Update `Program.cs`:**

```csharp
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();
app.Run();
```

- [ ] Gateway runs on port `5000`
- [ ] `GET http://localhost:5000/api/users` → returns users from UserService
- [ ] `GET http://localhost:5000/api/products` → returns products from ProductService

---

#### 🅱️ Option B — YARP

**Install the package:**

```bash
dotnet add package Yarp.ReverseProxy
```

**Update `appsettings.json`:**

```json
{
  "ReverseProxy": {
    "Routes": {
      "users-route": {
        "ClusterId": "users-cluster",
        "Match": { "Path": "/api/users/{**catch-all}" }
      },
      "products-route": {
        "ClusterId": "products-cluster",
        "Match": { "Path": "/api/products/{**catch-all}" }
      }
    },
    "Clusters": {
      "users-cluster": {
        "Destinations": {
          "destination1": { "Address": "http://localhost:5001/" }
        }
      },
      "products-cluster": {
        "Destinations": {
          "destination1": { "Address": "http://localhost:5002/" }
        }
      }
    }
  }
}
```

**Update `Program.cs`:**

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapReverseProxy();
app.Run();
```

- [ ] Gateway runs on port `5000`
- [ ] `GET http://localhost:5000/api/users` → returns users from UserService
- [ ] `GET http://localhost:5000/api/products` → returns products from ProductService

---

### Task 3 — Test with Postman or curl

Test all routes through the gateway only (not directly on 5001/5002):

```bash
# Users
curl http://localhost:5000/api/users

# Products
curl http://localhost:5000/api/products

# Should return 404 (no route defined)
curl http://localhost:5000/api/something-else
```

- [ ] Users route works ✅
- [ ] Products route works ✅
- [ ] Unknown route returns `404` ✅

---

### Task 4 — Reflect in README.md

Write a short `README.md` inside your `ApiGateway/` folder answering:

- [ ] Which option did you choose (Ocelot / YARP) and why?
- [ ] What does the gateway do that the services don't do themselves?
- [ ] What would happen if `UserService` was down — what does the gateway return?

---

### Bonus Task ⭐ (Optional)

Pick one to try:

- [ ] **Rate Limiting** — limit `/api/users` to 5 requests per minute (Ocelot has built-in support; YARP needs middleware)
- [ ] **Try Both** — implement Ocelot first, then switch to YARP and compare the config style
- [ ] **Docker Compose** — add all three services to a `docker-compose.yml` and run everything with one command

---

## ✅ Submission Checklist

- [ ] `ApiGateway` project created and runs on port `5000`
- [ ] Both routes (`/api/users`, `/api/products`) work through the gateway
- [ ] `README.md` answers the 3 reflection questions
- [ ] Code is pushed to your fork

---

## 💡 Tips

- Run all three services at the same time in separate terminals (or use `docker-compose`)
- If you get a `Connection refused` error — make sure the downstream service is actually running
- Ocelot config is in a separate file (`ocelot.json`); YARP config lives inside `appsettings.json`
- Both libraries are production-ready — companies use both in real systems

---

> **You just built an API Gateway. Every request now goes through one front door. 🚪🚀**
