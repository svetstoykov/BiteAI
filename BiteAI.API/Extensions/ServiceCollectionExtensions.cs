using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace BiteAI.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFirebaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var credentialsPath = Path.Combine(Environment.CurrentDirectory, "firebase-credentials.json");

        if (!File.Exists(credentialsPath))
            throw new FileNotFoundException("Could not find firebase-credentials.json");
        
        var firebaseApp = FirebaseApp.Create(new AppOptions
        {
            ProjectId = configuration["Firebase:ProjectId"],
            Credential = GoogleCredential.FromFile(credentialsPath)
        });

        // Initialize Firestore
        var firestoreDb = new FirestoreDbBuilder
        {
            ProjectId = configuration["Firebase:ProjectId"],
            CredentialsPath = credentialsPath
        }.Build();

        // Register FirebaseApp and FirestoreDb as singletons
        services.AddSingleton(firebaseApp);
        services.AddSingleton(firestoreDb);

        return services;
    }
}