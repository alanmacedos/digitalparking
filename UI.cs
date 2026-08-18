using System.Globalization;
using System.Text.RegularExpressions;

public class UI
{
    public void Run()
    {
        DisplayWelcomeMessage();

        bool applicationRunning = true;

        while (applicationRunning)
        {
            DisplayMenu();
            int option = GetOption();

            switch (option)
            {
                case 1:
                    EnterVehicle();
                    break;

                case 2:
                    ExitVehicle();
                    break;

                case 3:
                    DisplaySpaces(parkingLot);
                    break;

                case 4:
                    DisplayExpectedExit(parkingLot);
                    break;

                case 0:
                    applicationRunning = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Por favor tente novamente.");
                    break;
            }

            Console.WriteLine("\nPressione uma tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        DisplayGoodByeMessage();
    }

    ParkingLot parkingLot = new ParkingLot(5);

    private void DisplayWelcomeMessage()
    {
        Console.WriteLine(@"
        ██████╗ ██╗ ██████╗ ██╗████████╗ █████╗ ██╗
        ██╔══██╗██║██╔════╝ ██║╚══██╔══╝██╔══██╗██║
        ██║  ██║██║██║  ███╗██║   ██║   ███████║██║
        ██║  ██║██║██║   ██║██║   ██║   ██╔══██║██║
        ██████╔╝██║╚██████╔╝██║   ██║   ██║  ██║███████╗
        ╚═════╝ ╚═╝ ╚═════╝ ╚═╝   ╚═╝   ╚═╝  ╚═╝╚══════╝

        ██████╗  █████╗ ██████╗ ██╗  ██╗██╗███╗   ██╗ ██████╗
        ██╔══██╗██╔══██╗██╔══██╗██║ ██╔╝██║████╗  ██║██╔════╝
        ██████╔╝███████║██████╔╝█████╔╝ ██║██╔██╗ ██║██║  ███╗
        ██╔═══╝ ██╔══██║██╔══██╗██╔═██╗ ██║██║╚██╗██║██║   ██║
        ██║     ██║  ██║██║  ██║██║  ██╗██║██║ ╚████║╚██████╔╝
        ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝");

        Console.WriteLine("\nSeja bem vindo(a) ao Digital Parking!\nGerencie vagas, veículos e cobranças.");

        Thread.Sleep(2500);
    }

    private void DisplayMenu()
    {
        Console.WriteLine(@"
        Escolha uma das opções abaixo:

        1 - Estacionar veículo
        2 - Retirar veículo
        3 - Ver status das vagas
        4 - Previsão de liberação de vagas
        0 - Sair
        ");
    }

    private static int GetOption()
    {
        while (true)
        {
            Console.Write("Opção: ");
            string? option = Console.ReadLine()!;

            if (Regex.IsMatch(option, @"^-?[0-9]+$"))
            {
                return int.Parse(option!);
            }

            Console.WriteLine("A opção deve ser um número inteiro!");
        }
    }

    private void DisplayTittle(string tittle)
    {
        int count = tittle.Length;
        string equals = string.Empty.PadLeft(count + 4, '=');
        Console.WriteLine(equals);
        Console.WriteLine($"  {tittle}");
        Console.WriteLine(equals + "\n");
    }

    private void DisplayGoodByeMessage()
    {
        Console.WriteLine("\nObrigado por utilizar o Digital Parking. Até Logo!");
    }

    public void EnterVehicle()
    {
        Console.Clear();

        DisplayTittle("ENTRADA DE VEÍCULO");

        Console.WriteLine("Para registrar a entrada do veículo precisamos saber:");

        Console.Write("Seu nome: ");
        string owner = Console.ReadLine()!;

        Console.Write("\nPlaca(4 letras + 3 n°) do veículo: ");
        string plate = Console.ReadLine()!.ToUpper().Trim();

        while (parkingLot.VehicleIsAlreadyParked(plate))
        {
            Console.WriteLine("Veiculo com essa placa já estacionado");
            Console.WriteLine("Digite outra placa (4 letras + 3 n°): ");
            plate = Console.ReadLine()!;
        }

        while (!IsPlateValid(plate))
        {
            Console.WriteLine("Placa inválida. Digite novamente (4 letras + 3 n°): ");
            plate = Console.ReadLine()!;
        }

        Console.Write("\nModelo: ");
        string model = Console.ReadLine()!;

        Vehicle vehicle = new Vehicle(owner, plate, model);

        Console.Write(@"
        Tempo contratado:
        1 - 30 minutos
        2 - 1 hora
        3 - 2 horas
        4 - 3 horas
        ");

        int option = GetOption();

        while (option < 1 || option > 4)
        {
            Console.WriteLine("Opção inválida.");
            Console.Write("Insira novamente sua opção: ");
            option = GetOption();
        }

        ParkingSession parkingSession;

        switch (option)
        {
            case 1:
                parkingSession = new ParkingSession(vehicle, 30, DateTime.Now);
                break;

            case 2:
                parkingSession = new ParkingSession(vehicle, 60, DateTime.Now);
                break;

            case 3:
                parkingSession = new ParkingSession(vehicle, 120, DateTime.Now);
                break;

            case 4:
                parkingSession = new ParkingSession(vehicle, 180, DateTime.Now);
                break;

            default:
                throw new InvalidOperationException("Valor inválido.");
        }

        List<int> spacesAvailable = parkingLot.ParkingSpacesAvailable();

        int? selectedSpace = parkingLot.SelectParkingSpace(spacesAvailable);

        if (selectedSpace is int space)
        {
            parkingLot.ParkVehicle(space, parkingSession);
        }

        else
        {
            Console.WriteLine("Não há vagas disponíveis.");
        }
    }

    public void ExitVehicle()
    {
        Console.Clear();

        DisplayTittle("RETIRADA DE VEÍCULO");

        Console.Write("\nPlaca(4 letras + 3 n°) do veículo: ");
        string plate = Console.ReadLine()!.ToUpper().Trim();

        while (!IsPlateValid(plate))
        {
            Console.WriteLine("Placa inválida. Digite novamente (4 letras + 3 n°): ");
            plate = Console.ReadLine()!;
        }

        while (!parkingLot.VehicleIsAlreadyParked(plate))
        {
            Console.WriteLine("Não há veículo estacionado com essa placa.");
            Console.WriteLine("Digite novamente (4 letras + 3 n°): ");
            plate = Console.ReadLine()!;
        }

        Console.WriteLine("\nVeículo encontrado!");

        Console.WriteLine("\nData e horário de saída (HH:mm): ");
        DateTime exitTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out exitTime))
        {
            Console.WriteLine("\nData inválida. Digite novamente (HH:mm): ");
        }

        ParkingSession? session = parkingLot.ExitVehicle(plate, exitTime);

        if (session == null)
        {
            throw new InvalidCastException("Sessão nula");
        }

        Console.WriteLine("\nVeículo encontrado!");

        PaymentService paymentService = new PaymentService();
        decimal amount = paymentService.CalculateParkingFee(session!);

        string mehtod = PaymentMethod(amount);

        if (mehtod == "Pix" || mehtod == "Dinheiro")
        {
            amount = (amount / 100) * 95;
        }

        DateTime paidAt = exitTime.AddMinutes(2);

        Payment payment = new Payment(amount, mehtod, paidAt);

        PaymentSummary summary = paymentService.CreateSummary(session, payment);

        DisplayPaymentSummary(summary);
    }

    private bool IsPlateValid(string plate)
    {
        if (plate.Length != 7)
        {
            return false;
        }

        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(plate[i]))
            {
                return false;
            }
        }

        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(plate[i]))
            {
                return false;
            }
        }

        return true;
    }

    private string PaymentMethod(decimal amount)
    {
        Console.WriteLine();
        DisplayTittle("MÉTODO DE PAGAMENTO");

        Console.WriteLine($@"O valor da permanência foi de R${amount}.
        
        Métodos de pagamento:
        1 - Pix / 5% de desconto
        2 - Dinheiro / 5% de desconto
        3 - Débito
        4 - Crédito (à vista)
        ");

        int option = GetOption();

        while (option < 1 || option > 4)
        {
            Console.WriteLine("Opção inválida.");
            Console.Write("Insira novamente sua opção: ");
            option = GetOption();
        }

        switch (option)
        {
            case 1:
                return "Pix";

            case 2:
                return "Dinheiro";

            case 3:
                return "Débito";

            case 4:
                return "Crédito";

            default:
                throw new InvalidOperationException("Valor inválido.");
        }
    }

    // display payment summary before pay for real
    private void DisplayPaymentSummary(PaymentSummary summary)
    {
        DisplayTittle("DEMONSTRATIVO DE PAGAMENTO");

        Console.WriteLine($@"
        Placa do veículo:      {summary.Plate}
        Proprietário:          {summary.Owner}
        Modelo:                {summary.Model}              

        Horário de Entrada:    {summary.EntryTime:dd/MM/yyyy HH:mm}
        Horário de Saída:      {summary.ExitTime:dd/MM/yyyy HH:mm}

        Tempo Contratado:      {FormatDurarition(summary.ContractedTime)}
        Tempo Estacionado:     {FormatDurarition(summary.ParkedTime)}
        Tempo Adicional:       {FormatDurarition(summary.AdditionalTime)}

        --------------------------------------------------
        Valor total:           R$ {summary.Amount:N2}
        --------------------------------------------------

        Método de pagamento:   {summary.Method}
        Data/Hora:             {summary.PaidAt}

        Pressione ENTER para confirmar o pagamento...");

        PayConfirmation();
    }

    private string FormatDurarition(TimeSpan duration)
    {
        return $"{(int)duration.TotalHours}h {duration.Minutes:00}min";
    }

    private void PayConfirmation()
    {
        while (Console.ReadKey(true).Key != ConsoleKey.Enter)
        {

        }

        Console.WriteLine("Pagamento confirmado com sucesso.");
        Console.WriteLine("Volte sempre!");
    }

    private void DisplaySpaces(ParkingLot parkingLot)
    {
        Console.Clear();

        DisplayTittle("LISTAGEM DAS VAGAS");

        foreach (var space in parkingLot.GetParkingSpaces())
        {
            Console.WriteLine($@"
            Vaga:                 {space.Key}
            Status:               {(space.Value is null ? "Disponível" : "Ocupada")}
            ");
        }

        List<KeyValuePair<int, ParkingSession?>> parkingSpaces =
        parkingLot.GetParkingSpaces();

        Console.WriteLine($@"
        QTD total de vagas:   {parkingSpaces.Count}
        Vagas disponíveis:    {parkingLot.ParkingSpacesAvailable().Count}
        Vagas ocupadas:       {parkingSpaces.Count(space => space.Value != null)}");
    }

    private void DisplayExpectedExit(ParkingLot parkingLot)
    {
        Console.Clear();
        DisplayTittle("PROVÁVEL LIBERAÇÃO DE VAGAS");

        parkingLot.ExpectedExitTime();
    }
}