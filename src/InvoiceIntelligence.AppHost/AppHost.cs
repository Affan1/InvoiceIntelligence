using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Configures and runs the Azurite storage emulator
var azure = builder.AddAzureStorage("azurite")
                     .RunAsEmulator();

var blobs = azure.AddBlobs("Storage");
builder.AddProject<InvoiceIntelligence_Api>("api")
    .WithReference(blobs)
    .WaitFor(blobs);
builder.Build().Run();
