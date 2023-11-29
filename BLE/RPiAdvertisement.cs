using DotnetBleServer.Advertisements;
using DotnetBleServer.Core;
using DotnetBleServer.Gatt.BlueZModel;

namespace rpi.BLE
{
    /// <summary>
    /// Cette classe permet de définir l'annonce Bluetooth à utiliser par le robot.
    /// </summary>
    public class RPiAdvertisement
    {
        /// <summary>
        /// Méthode permettant d'enregistrer l'annonce Bluetooth dans BlueZ.
        /// </summary>
        /// <param name="context">Le contexte du service BlueZ</param>
        public static async Task RegisterRobotAdvertisement(ServerContext context)
        {
            var advertisementProperties = new LEAdvertisement1Properties
            {
                // Type représente le type de l'annonce. Ici, nous utilisons un périphérique BLE.
                Type = "peripheral",
                // ServiceUUIDs est une liste d'UUIDs qui seront inclus dans les données de l'annonce.
                // Utiliser les UUIDs de services GATT définis dans RPiGattApplication.cs
                ServiceUUIDs = new[] { "15ff0fcd-6481-4565-9fe0-388628769cce" },
                LocalName = "Marcel",
                // L'apparence est un entier de 16 bits qui représente l'apparence de l'appareil.
                // La liste des apparences est disponible ici: https://www.bluetooth.com/specifications/assigned-numbers/generic-access-profile/
                Appearance = (ushort)Convert.ToUInt32("0x01D", 16),
                // Discoverable indique si l'appareil doit être visible pour les autres appareils.
                Discoverable = true,
                // IncludeTxPower indique si la puissance de transmission doit être incluse dans les données de l'annonce.
                IncludeTxPower = true,
            };

            await new AdvertisingManager(context).CreateAdvertisement(advertisementProperties);
        }
    }
}