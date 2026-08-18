public class ParkingLot
{
    private readonly Dictionary<int, ParkingSession?> parkingSpaces = new();

    public ParkingLot(int quantityParkingSpaces)
    {
        for (int space = 1; space <= quantityParkingSpaces; space++)
        {
            parkingSpaces.Add(space, null);
        }
    }

    public List<int> ParkingSpacesAvailable()
    {
        List<int> spacesAvailable = new List<int>();

        foreach (var space in parkingSpaces)
        {
            if (space.Value == null)
            {
                spacesAvailable.Add(space.Key);
            }
        }

        return spacesAvailable;
    }

    public int? SelectParkingSpace(List<int> spacesAvailable)
    {
        // Verify if there is a free space to park
        if (spacesAvailable.Count == 0)
        {
            return null;
        }

        // Randomly selects a place to park
        Random random = new Random();
        int randomIndex = random.Next(spacesAvailable.Count);
        int selectedSpace = spacesAvailable[randomIndex];
        return selectedSpace;
    }

    public void ParkVehicle(int selectedSpace, ParkingSession session)
    {
        if (VehicleIsAlreadyParked(session.Vehicle.Plate))
        {
            Console.WriteLine($"Este veículo ({session.Vehicle.Plate})já está estacionado.");
            return;
        }

        if (!ParkingSpaceExists(selectedSpace))
        {
            Console.WriteLine($"A vaga {selectedSpace} não existe");
            return;
        }

        if (!ParkingSpaceIsAvailable(selectedSpace))
        {
            Console.WriteLine($"A vaga {selectedSpace} está ocupada.");
            return;
        }

        parkingSpaces[selectedSpace] = session;
        Console.WriteLine($"\nVeículo estacionado na vaga {parkingSpaces[selectedSpace]} com sucesso!");
    }

    public bool VehicleIsAlreadyParked(string plate)
    {
        foreach (ParkingSession? session in parkingSpaces.Values)
        {
            if (session != null && session.Vehicle.Plate == plate)
            {
                return true;
            }
        }

        return false;
    }

    private bool ParkingSpaceExists(int selectedSpace)
    {
        return parkingSpaces.ContainsKey(selectedSpace);
    }

    private bool ParkingSpaceIsAvailable(int selectedSpace)
    {
        return parkingSpaces[selectedSpace] == null;
    }

    public ParkingSession? ExitVehicle(string plate, DateTime exitTime)
    {
        foreach (var item in parkingSpaces)
        {
            if (item.Value != null && item.Value.Vehicle.Plate == plate)
            {
                ParkingSession session = item.Value;
                session.RegisterExit(exitTime);
                parkingSpaces[item.Key] = null;

                return session;
            }
        }

        return null;
    }

    public void ExpectedExitTime()
    {
        var exits = new List<(int space, DateTime expectedExit)>();

        foreach (var space in parkingSpaces)
        {
            if (space.Value != null)
            {
                DateTime exit = space.Value.EntryTime.AddMinutes(space.Value.ContractedMinutes);

                exits.Add((space.Key, exit));
            }
        }

        var orderedExits = exits.OrderBy(exit => exit.expectedExit).ToList();

        foreach (var exit in orderedExits)
        {
            Console.WriteLine($"Vaga {exit.space} - Liberação prevista: {exit.expectedExit:HH:mm}");
        }
    }

    public List<KeyValuePair<int, ParkingSession?>> GetParkingSpaces()
    {
        return parkingSpaces.ToList();
    }
}
