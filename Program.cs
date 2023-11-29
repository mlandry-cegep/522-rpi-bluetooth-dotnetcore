using DotnetBleServer.Core;
using rpi.BLE;

Console.WriteLine("Démarrage du serveur Bluetooth...");
// Démarrage du serveur Bluetooth en tâche de fond
Task.Run(async () =>
{
    using (var context = new ServerContext())
    {
        Console.WriteLine($"Connexion à BlueZ...");
        await context.ConnectAndSetDefaultAdapter();

        Console.WriteLine($"Utilisation de l'adapteur Bluetooth : {context.Adapter.ObjectPath}");

        // Démarrage de l'adaptateur
        await context.Adapter.SetPoweredAsync(true);

        if (!await context.Adapter.GetPoweredAsync())
            throw new Exception($"Problème lors du démarrage de l'adapteur Bluetooth {context.Adapter.ObjectPath}");

        // Enregistrement de l'application GATT et publication de l'annonce
        await RPiAdvertisement.RegisterRobotAdvertisement(context);
        await RPiGattApplication.RegisterGattApplication(context);

        Console.WriteLine("Appuyer sur Ctrl+C pour quitter");
        await Task.Delay(-1);
    }

}).Wait();