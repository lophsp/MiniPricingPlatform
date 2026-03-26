# Mini Pricing Platform

A backend service for calculating service pricing based on configurable rules. Supports single quote calculation and bulk quote processing.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Setup Guide](#setup-guide)
3. [API Endpoints](#api-endpoints)

   * [Health Check](#get-health)
   * [Calculate Price](#post-quotesprice)
   * [Bulk Quotes](#post-quotesbulk)
   * [Job Status](#get-jobsjob_id)
   * [Rules Management](#rules-management)

---

## Architecture Overview

The project follows **Clean Architecture**:

```
MiniPricingPlatform
├─ API                 # Core API Layer
│  └─ Controllers
├─ Application         # Business logic & services
│  └─ Services
│  └─ Interfaces
├─ Domain              # Core entities & rules
│  └─ Entities
│  └─ Models
│  └─ Rules
├─ Infrastructure      # Data access / repository
│  └─ Repositories
├─ Tests               # Unit and integration tests
```

* **Domain**: Pricing rules and core models
* **Application**: Services that orchestrate rules, e.g., `PricingService`
* **API**: Exposes endpoints for price calculation, bulk jobs, and rule management
* **Infrastructure**: Repository implementations for rules storage

---

## Setup Guide

### Prerequisites

* .NET 8 SDK
* Docker (optional for containerized setup)

### Steps

1. Clone repository:

```bash
git clone https://github.com/your-username/mini-pricing-platform.git
cd mini-pricing-platform
```

2. Build project:

```bash
dotnet build
```

3. Run API:

```bash
dotnet run --project MiniPricingPlatform.API
```

4. (Optional) Run using Docker:

```bash
docker-compose up --build
```

5. Access Swagger UI at: `http://localhost:8080/swagger`

---

## API Endpoints

### GET `/health`

Check service status.

**Request**

```http
GET /health
```

**Response**

```json
{
  "status": "ok",
}
```

---

### POST `/quotes/price`

Calculate a single quote price.

**Request**

```json
{
  "weight": 3,
  "area": "RemoteArea",
  "requestTime": "2026-03-25T10:00:00Z"
}
```

**Response**

```json
{
  "price": 50
}
```

---

### POST `/quotes/bulk`

Submit multiple quotes for batch processing. Returns `job_id`.

**Request**

```json
{
  "type": "json",
  "items": [
    { "weight": 3, "area": "RemoteArea", "requestTime": "2026-03-25T10:00:00Z" },
    { "weight": 7, "area": "City", "requestTime": "2026-03-25T10:00:00Z" },
    { "weight": 12, "area": "RemoteArea", "requestTime": "2026-03-25T19:00:00Z" }
  ]
}
```

**Response**

```json
{
  "job_id": "b2a1e5c8-7f0a-4f1e-bd64-9c8dce2d5fcb"
}
```

#### csv type
**Request**
```
{
  "type": "csv",
  "items": "weight,area,requestTime\n3,RemoteArea,2026-03-25T10:00:00Z\n7,City,2026-03-25T10:00:00Z\n12,RemoteArea,2026-03-25T19:00:00Z"
}
```
**Response**

```json
{
  "jobId": "3721eca4-1378-4b44-939e-977d85bf0659"
}
```
---

### GET `/jobs/{job_id}`

Check status of a bulk job and retrieve results.

**Request**

```http
GET /jobs/b2a1e5c8-7f0a-4f1e-bd64-9c8dce2d5fcb
```

**Response (completed)**

```json
{
  "status": "completed",
  "results": [
    { "weight": 3,  "area": "RemoteArea", "price": 50 },
    { "weight": 7,  "area": "City",       "price": 0 },
    { "weight": 12, "area": "RemoteArea", "price": 45 }
  ]
}
```

---

### Rules Management

#### POST `/rules`

Create a new pricing rule.

**Request: WeightTierRule**

```json
{
  "type": "WeightTier",
  "priority": 1,
  "minWeight": 0,
  "maxWeight": 5,
  "price": 100,
  "effectiveFrom": "2026-01-01T00:00:00Z",
  "effectiveTo": "2026-12-31T23:59:59Z",
  "isActive": true
}
```

**Request: RemoteAreaSurchargeRule**

```json
{
  "type": "RemoteAreaSurcharge",
  "priority": 2,
  "remoteSurcharge": 50,
  "effectiveFrom": "2026-01-01T00:00:00Z",
  "effectiveTo": "2026-12-31T23:59:59Z",
  "isActive": true
}
```

**Request: TimeWindowPromotionRule**

```json
{
  "type": "TimeWindowPromotion",
  "priority": 3,
  "discountPercentage": 10,
  "startTime": "18:00:00",
  "endTime": "22:00:00",
  "effectiveFrom": "2026-03-01T00:00:00Z",
  "effectiveTo": "2026-03-31T23:59:59Z",
  "isActive": true
}
```



---

#### GET `/rules`

List all active rules.

**Request**

```http
GET /rules
```

**Response**

```json
[
  {
    "id": "a369685c-5607-4578-ac1b-03006123b37a",
    "type": "weighttier",
    "priority": 1,
    "effectiveFrom": "2026-01-01T00:00:00Z",
    "effectiveTo": "2026-12-31T23:59:59Z",
    "isActive": true
  },
  {
    "id": "4962b294-5f6c-416d-9e9b-eeebb95ec0fb",
    "type": "remotearea",
    "priority": 2,
    "effectiveFrom": "2026-01-01T00:00:00Z",
    "effectiveTo": "2026-12-31T23:59:59Z",
    "isActive": true
  },
  {
    "id": "475e6d77-feaa-4e35-a340-c640031c8b94",
    "type": "timewindow",
    "priority": 3,
    "effectiveFrom": "2026-03-01T00:00:00Z",
    "effectiveTo": "2026-03-31T23:59:59Z",
    "isActive": true
  }
]
```

---

#### PUT `/rules/{id}`
### WeightTier
```json
{
  "Type": "WeightTier",
  "Priority": 1,
  "EffectiveFrom": "2026-01-01T00:00:00Z",
  "EffectiveTo": "2026-12-31T23:59:59Z",
  "IsActive": true,
  "Price": 120.0,
  "Min": 1,
  "Max": 7
}
```
### RemoteAreaSurcharge
```json
{
  "Type": "RemoteAreaSurcharge",
  "Priority": 2,
  "EffectiveFrom": "2026-01-01T00:00:00Z",
  "EffectiveTo": "2026-12-31T23:59:59Z",
  "IsActive": true,
  "RemoteSurcharge": 60.0
}
```
### TimeWindowPromotion
```json
{
  "Type": "TimeWindowPromotion",
  "Priority": 3,
  "EffectiveFrom": "2026-03-01T00:00:00Z",
  "EffectiveTo": "2026-03-31T23:59:59Z",
  "IsActive": true,
  "DiscountPercentage": 15.0,
  "StartTime": "17:00:00",
  "EndTime": "21:00:00"
}
```

#### DELETE `/rules/{id}`

Delete a rule by ID.

---

### Notes

* **Rule Priority**: Lower `Priority` executes first.
* **Price Calculation**: `WeightTier` sets the base price, `RemoteAreaSurcharge` adds surcharge, `TimeWindowPromotion` applies discount.
* **Bulk Requests**: Use `/quotes/bulk` for multiple calculations, then check `/jobs/{job_id}` for results.
