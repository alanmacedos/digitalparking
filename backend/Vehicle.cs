public class Vehicle
{
    public string Owner { get; set; }
    public string Plate { get; set; }
    public string Model { get; set; }


    public Vehicle(string owner, string plate, string model)
    {
        Owner = owner;
        Plate = plate;
        Model = model;

    }
}