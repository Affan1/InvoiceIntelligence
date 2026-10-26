# Architecture

> TODO: add the architecture diagram (ingestion and query paths) and the design decisions below.

## Overview

Uploads never block the API: Event Grid and Service Bus decouple OCR and indexing, while queries
hit Redis first and fall back to the tenant-filtered vector index before Azure OpenAI writes the
answer.

## Ingestion path

```
Tenant upload → Blob Storage → Event Grid → Azure Function (ingest_trigger)
  → Document Intelligence (OCR) → Service Bus → Worker (chunk, embed, index) → Vector store
```

## Query path

```
API query → Redis cache ─(miss)→ tenant-filtered vector search → Azure OpenAI → grounded answer + citations
```

## Design decisions (interview talking points)

- **Why event-driven ingestion?** TODO
- **How tenant isolation works** (storage, cache, search, logs). TODO
- **Why Cosmos DB vs PostgreSQL + pgvector.** TODO
- **Secrets: Key Vault + managed identity, zero connection strings in config.** TODO
- **Observability: OpenTelemetry → Application Insights, end-to-end trace.** TODO
