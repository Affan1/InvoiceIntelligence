// main.bicep — all Azure resources for InvoiceIntelligence, one command to deploy.
//
// Deploy (example):
//   az deployment group create -g <resource-group> -f infra/main.bicep -p environmentName=dev
//
// Build this out week by week (see README plan). Suggested resources:
//   Week 1: Container Registry, Container Apps environment, API container app, Log Analytics
//   Week 2: Worker container app
//   Week 3: Cosmos DB (or PostgreSQL + pgvector), Azure OpenAI
//   Week 4: Azure Cache for Redis
//   Week 5: Storage account + Blob, Event Grid, Service Bus, Function app, Document Intelligence
//   Week 6: Key Vault, managed identities, Application Insights

targetScope = 'resourceGroup'

@description('Short name used to derive resource names, e.g. "dev".')
param environmentName string

@description('Azure region for all resources.')
param location string = resourceGroup().location

// TODO: declare resources here.

// TODO: add outputs (e.g. API URL, registry login server).
