using DotnetBleServer.Core;
using DotnetBleServer.Gatt.Description;
using DotnetBleServer.Gatt;
using System;
using static System.Collections.Specialized.BitVector32;

namespace rpi.BLE
{
    /// <summary>
    /// Cette classe permet de définir le service GATT à utiliser par le robot.
    /// </summary>
    /// <remarks>
    /// Vous pourrez intégrer ce code à une autre classe si vous le souhaitez.
    /// L'important est qu'il soit exécuté au démarrage de l'application et qu'il 
    /// vous permette de capter les actions envoyées par le robot.
    /// </remarks>
    public partial class RPiGattApplication
    {
        /// <summary>
        /// Méthode permettant d'enregistrer le service GATT dans BlueZ.
        /// </summary>
        /// <param name="context">Le contexte du service BlueZ</param>
        public static async Task RegisterGattApplication(ServerContext context)
        {
            // Définition du service GATT
            var gattServiceDescription = new GattServiceDescription
            {
                // Générer votre UUID ici: https://www.uuidgenerator.net/
                UUID = "15ff0fcd-6481-4565-9fe0-388628769cce",
                Primary = true
            };

            // Définition de la caractéristique GATT
            var gattCharacteristicDescription = new RobotGattCharacteristicDescription
            {
                // Générer votre UUID ici: https://www.uuidgenerator.net/
                UUID = "34a28b10-1486-4c61-9fa1-878296fd0262",
                Flags = CharacteristicFlags.Read | CharacteristicFlags.Write | CharacteristicFlags.Notify,
            };

            // On attache une méthode à exécuter lorsqu'une action est reçue.
            gattCharacteristicDescription.CommandReceived += OnCommandReceived;

            // On ajoute la caractéristique GATT au service GATT
            gattServiceDescription.AddCharacteristic(gattCharacteristicDescription);

            // On ajoute le service GATT à l'application GATT
            var gab = new GattApplicationBuilder();
            gab.AddService(gattServiceDescription);
            
            // On enregistre l'application GATT dans BlueZ
            await new GattApplicationManager(context).RegisterGattApplication(gab.BuildServiceDescriptions());
        }

        private static void OnCommandReceived(RobotAction action)
        {
            // Notre événement qui sera déclenché lorsqu'une action sera reçue via Bluetooth.
            Console.WriteLine($"Action reçue: {action.Command} - {action.Value}");
        }
    }
}