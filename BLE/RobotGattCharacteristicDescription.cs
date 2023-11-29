using DotnetBleServer.Gatt.Description;
using System.Text;

namespace rpi.BLE
{
    /// <summary>
    /// Cette énumération permet de définir les types d'actions que le robot peut réaliser.
    /// </summary>
    public enum RobotActionType
    {
        Forward,
        Back,
        TurnLeft,
        TurnRight,
        Stop,
        MoveSequenceOne,
        MoveSequenceTwo,
        AvoidObstacleOne,
        AvoidObstacleTwo,
    }

    /// <summary>
    /// Cette classe permet de définir une action à éxécuter par le robot.
    /// </summary>
    public class RobotAction
    {
        /// <summary>
        /// Le type d'action à réaliser.
        /// </summary>
        public RobotActionType Command { get; set; }

        /// <summary>
        /// La valeur associée à l'action.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="command">Les données brutes représentant l'action</param>
        /// <example>
        /// <code>RobotAction action = new RobotAction(Encoding.ASCII.GetBytes("0:100"));</code>
        /// Où <code>0</code> est le type d'action et <code>100</code> est la valeur associée.
        /// </example>
        public RobotAction(byte[] command)
        {
            try
            {
                string message = new string(Encoding.ASCII.GetChars(command));
                var values = message.Split(':');
                Command = (RobotActionType)int.Parse(values[0]);
                Value = int.Parse(values[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du décodage d'une action reçue via Bluetooth: {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// Cette classe permet de définir le comportement d'une caractéristique GATT.
    /// </summary>
    public class RobotGattCharacteristicDescription : GattCharacteristicDescription
    {
        /// <summary>
        /// Evénement déclenché lorsqu'une commande est reçue.
        /// </summary>
        public event Action<RobotAction>? CommandReceived;


        /// <summary>
        /// Fonction appelée lorsqu'une commande est reçue via Bluetooth LE.
        /// </summary>
        /// <param name="value">Les données représentant la commande</param>
        /// <returns></returns>
        public override Task WriteValueAsync(byte[] value)
        {
            Console.WriteLine("Commande reçue:");

            var command = new RobotAction(value);

            // Si on a un abonné à l'événement, on le déclenche
            CommandReceived?.Invoke(command);
            Console.WriteLine($"  Commande: {command.Command}, Valeur: {command.Value}");

            return base.WriteValueAsync(value);
        }


        /// <summary>
        /// Fonction appelée lorsqu'une valeur est lue via Bluetooth LE.
        /// </summary>
        /// <remarks>
        /// Une requête de lecture signifie que le client souhaite recevoir les infos de télémétrie du robot.
        /// </remarks>
        /// <returns></returns>
        public override Task<byte[]> ReadValueAsync()
        {
            Console.WriteLine("Requête de lecture de télémétrie");
            return Task.FromResult(Encoding.ASCII.GetBytes("TODO: définir le format d'envoie des données. JSON?"));
        }
    }
}