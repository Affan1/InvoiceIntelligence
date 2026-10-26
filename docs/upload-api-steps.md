# Week 1 — Upload API: step-by-step

> Your first build slice: a PDF uploaded through the API lands in Blob Storage.
> Build it **locally against Azurite** (no Azure, no cost). You write all the code — each step
> says *what to do* and *done when*; bring the code back for review when a step says so.

**Scope:** upload only. No OCR, no database, no queue, no auth yet.

**Where things live:**
- `InvoiceIntelligence.Core` — the storage interface (abstraction).
- `InvoiceIntelligence.Infrastructure` — the Blob storage implementation.
- `InvoiceIntelligence.Api` — the upload endpoint + DI registration.

---

## Step 0 — Before you start (prerequisites)

- [ ] Docker Desktop installed and running.
- [ ] Start **Azurite** (the local Blob emulator), e.g. `docker run -p 10000:10000 mcr.microsoft.com/azure-storage/azurite`
      — or the Azurite VS/VS Code tool.
- [ ] Install **Azure Storage Explorer** (to see your blobs) and connect it to Azurite (Local & Attached → Emulator).
- [ ] Confirm the solution still builds: `dotnet build`.

**Done when:** Azurite is running and Storage Explorer shows the local emulator.

---

## Step 1 — Add the Blob packages (Infrastructure)

- [ ] Add to `InvoiceIntelligence.Infrastructure`:
      `Azure.Storage.Blobs` and `Microsoft.Extensions.Azure`.

**Do:** `dotnet add src/InvoiceIntelligence.Infrastructure package Azure.Storage.Blobs` (and the same for `Microsoft.Extensions.Azure`).

**Done when:** `dotnet build` still succeeds with the packages referenced.

---

## Step 2 — Define the storage abstraction (Core)

- [ ] In `Core`, create an interface — e.g. `IInvoiceStorage` — with one method that takes the
      file stream, content type, tenant id, and original file name, and returns a blob reference
      (the blob name or URI).

**Why:** the API depends on this interface, not on the Azure SDK. Keeps Core clean and lets you
swap Azurite ↔ real Azure later.

**Done when:** the interface compiles in Core. (No implementation yet.)

> Bring to Claude for review: the interface shape (what it takes/returns).

---

## Step 3 — Implement the storage service (Infrastructure)

- [ ] Create a class implementing `IInvoiceStorage` that:
  - holds a `BlobServiceClient` (injected),
  - gets (or creates) the container,
  - builds the blob name as **`{tenantId}/{guid}.pdf`** (tenant-scoped path — do this now so
    isolation isn't a retrofit),
  - uploads the incoming **stream** to the blob,
  - stores the original file name as blob **metadata**,
  - returns the blob reference.

**Watch out:**
- Stream straight through — don't read the whole PDF into memory.
- Don't trust the client's filename; generate the blob name yourself (GUID).

**Done when:** the class compiles and implements the interface.

> Bring to Claude for review: streaming, container handling, blob naming.

---

## Step 4 — Register the clients in DI (Api)

- [ ] In the API's startup, register the `BlobServiceClient` (use `AddAzureClients` from
      `Microsoft.Extensions.Azure`) pointing at **Azurite's dev connection string** for now.
- [ ] Register your `IInvoiceStorage` → implementation.

**Note:** later, for real Azure, you swap the connection string for the account URI +
`DefaultAzureCredential` — that's the only line that changes.

**Done when:** the app starts without DI errors.

---

## Step 5 — Create the upload endpoint (Api)

- [ ] Add a `POST` endpoint (controller or minimal API) that accepts **`multipart/form-data`**
      with an `IFormFile`.
- [ ] For this slice, take the `tenantId` from a header or route param (auth comes later).
- [ ] Validate: content type is `application/pdf`, and enforce a **max file size**.
- [ ] Call `IInvoiceStorage.UploadAsync(...)` with `file.OpenReadStream()`.
- [ ] Return **`201 Created`** with the blob reference.

**Done when:** the endpoint builds and shows up in Swagger / the `.http` file.

> Bring to Claude for review: the endpoint — validation, model binding, status codes.

---

## Step 6 — Configure the connection (Api)

- [ ] Put Azurite's dev connection string in **user secrets** (not appsettings), e.g.
      `dotnet user-secrets set "Storage:ConnectionString" "UseDevelopmentStorage=true"`.
- [ ] Read it in the DI registration.

**Done when:** no connection string is hard-coded in source.

---

## Step 7 — Test it end to end

- [ ] Run the API (`dotnet run --project src/InvoiceIntelligence.Api`, or via the Aspire AppHost).
- [ ] Upload a real PDF — use the API project's `.http` file, or:
      `curl -F "file=@invoice.pdf" -H "X-Tenant-Id: tenant-a" http://localhost:<port>/invoices`
- [ ] Confirm in **Azure Storage Explorer** that the blob appears at `tenant-a/<guid>.pdf`.
- [ ] Try a non-PDF and an oversized file — confirm they're rejected.

**Done when:** a PDF lands in the right tenant path and bad uploads are rejected.

---

## Step 8 — Commit the slice

- [ ] Commit with a clear message (e.g. `Add PDF upload-to-Blob endpoint`).
- [ ] Push.

**Done when:** the slice is on GitHub and the build is green.

---

## Definition of done for Week 1 (this slice)

A PDF uploaded through the API is stored in Blob at a **tenant-scoped path**, with validation,
no secrets in source, running locally on Azurite — and the same code is ready to point at real
Azure Blob by swapping the connection to managed identity later.

## What's next (not now)

Week 1 continues with the **Dockerfile → ACR → Container Apps → CI/CD** deploy (see
[ai-200-roadmap.md](ai-200-roadmap.md), Phase 1). The upload endpoint becomes the first node of the
ingestion pipeline (Blob → Event Grid → Function) in Phase 3.

## AI-200 tie-in

This slice touches **Blob Storage** (Section 3 prep) and sets up the container/deploy work in
**Section 1**. See the roadmap for the full mapping.
