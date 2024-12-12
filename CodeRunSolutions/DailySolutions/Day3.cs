using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

public class Day3 : ChallengeDay
{
    public TransportationFleet? ChallengeFleet { get; set; }

    public Day3()
        : base(
            3,
            @"
Transportation Fleet System
----------------------------
- Transportation Fleet
- Transportation Vehicle
- Driver
            ",
            @"
Key Control:
1. Recreate Fleet
2. Create Vehicle
3. Display Fleet Info
4. Display Vehicle List
5. Search Vehicle
    1. Update Status
    2. Display Driver Info
    3. Remove Vehicle
6. Display Fleet Cost
            ",
            60
        ) { }

    public override void InputInterface()
    {
        if (ChallengeFleet == null)
        {
            CreateFleetUI();
        }

        Console.WriteLine("\nEntry:");
        string? entry = Console.ReadLine();
        Console.WriteLine();

        switch (entry)
        {
            case "1":
                CreateFleetUI();
                break;
            case "2":
                CreateVehicleUI();
                break;
            case "3":
                ChallengeFleet?.DisplayFleetInfo();
                break;
            case "4":
                ChallengeFleet?.DisplayVehicleList();
                break;
            case "5":
                SearchVehicleUI();
                break;
            case "6":
                ChallengeFleet?.DisplayFleetCost();
                break;
            case "q":
                return;
            default:
                Console.WriteLine("Invalid Key");
                break;
        }

        InputInterface();
    }

    public void CreateFleetUI()
    {
        Console.WriteLine("-> Create Fleet");
        Console.WriteLine("Name:");
        string? fleetName = Console.ReadLine();
        Console.WriteLine("Company:");
        string? fleetCompany = Console.ReadLine();

        ChallengeFleet = new TransportationFleet(fleetName ?? "Unknown", fleetCompany ?? "-");

        Console.WriteLine("\n! Transportation Fleet Created");
    }

    public void CreateVehicleUI()
    {
        Console.WriteLine("-> Create Vehicle");
        Console.WriteLine("Vehicle Type:");
        string? vehicleType = Console.ReadLine();
        Console.WriteLine("Cost:");
        decimal? vehicleCost = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("-> Assign Driver");
        Console.WriteLine("Name:");
        string? driverName = Console.ReadLine();
        Console.WriteLine("Salary:");
        decimal? driverSalary = Convert.ToInt32(Console.ReadLine());

        Driver newDriver = new Driver(driverName ?? "Unknown", driverSalary ?? 0);
        Vehicle newVehicle = new Vehicle(vehicleType ?? "Unknown", vehicleCost ?? 0, newDriver);
        ChallengeFleet?.AddVehicle(newVehicle);
    }

    public void SearchVehicleUI()
    {
        Console.WriteLine("-> Searching Vehicle");
        string? searchId = Console.ReadLine();
        Vehicle? searchVehicle = ChallengeFleet?.SearchVehicle(searchId ?? "-");

        if (searchVehicle != null)
        {
            Console.WriteLine("\nEntry:");
            var entry = Console.ReadLine();

            switch (entry)
            {
                case "1":
                    UpdateVehicleStatusUI(searchVehicle);
                    break;
                case "2":
                    searchVehicle.VehicleDriver.DisplayDriverInfo();
                    break;
                case "3":
                    ChallengeFleet?.RemoveVehicle(searchVehicle);
                    break;
                case "b":
                    return;
                default:
                    Console.WriteLine("Invalid Key");
                    break;
            }
        }
    }

    public void UpdateVehicleStatusUI(Vehicle vehicle)
    {
        Console.WriteLine("-> Update Vehicle Status");
        Console.WriteLine("Entry:");
        int entry = Convert.ToInt32(Console.ReadLine());
        if (entry == 0 || entry == 1 || entry == 2 || entry == 3)
        {
            vehicle.VehicleStatus = (TransportationVehicleStatus)entry;
            Console.WriteLine("\n! Vehicle Status Updated");
        }
        else
        {
            Console.WriteLine("\n! Invalid Status");
        }
    }
}

public enum TransportationVehicleStatus
{
    Unavailable,
    Available,
    InTransit,
    UnderMaintenance,
}

public class TransportationFleet
{
    public string Name { get; set; }
    public string Company { get; set; }
    public Dictionary<string, Vehicle> VehicleList { get; set; } = [];

    public TransportationFleet(string name, string company)
    {
        this.Name = name;
        this.Company = company;
    }

    public void DisplayFleetInfo()
    {
        Console.WriteLine(
            @$"
{Name}
----------------------------------------
by {Company}
Transportation Cost: {CalculateTotalCost()}
"
        );
    }

    public void DisplayVehicleList()
    {
        Console.WriteLine(
            @"
Vehicle List
-----------------------------"
        );
        int count = 1;
        foreach (var vehicle in VehicleList)
        {
            Console.WriteLine($"{count++}.");
            vehicle.Value.DisplayVehicleInfo();
        }
    }

    public void AddVehicle(Vehicle vehicle)
    {
        VehicleList.Add(vehicle.Id, vehicle);
    }

    public void RemoveVehicle(Vehicle vehicle)
    {
        VehicleList.Remove(vehicle.Id);
        Console.WriteLine("\n! Vehicle Not Found");
    }

    public Vehicle? SearchVehicle(string searchId)
    {
        if (VehicleList.ContainsKey(searchId))
        {
            VehicleList[searchId].DisplayVehicleInfo();
            return VehicleList[searchId];
        }
        else
        {
            Console.WriteLine("\n! Vehicle Not Found");
            return null;
        }
    }

    public decimal CalculateTotalCost()
    {
        decimal total = 0;
        foreach (var item in VehicleList)
        {
            total += item.Value.TransportationCost;
            total += item.Value.VehicleDriver.Salary;
        }
        return total;
    }

    public void DisplayFleetCost()
    {
        Console.WriteLine(
            @$"
Transportation Fee Cost 
-------------------------------------------------------------------------------"
        );
        int count = 1;
        foreach (var item in VehicleList)
        {
            Console.WriteLine(
                @$"
{count++}.
{item.Value.VehicleType}({item.Value.Id})                   {item.Value.TransportationCost}$ 
{item.Value.VehicleDriver.Name}({item.Value.VehicleDriver.Id})                  {item.Value.VehicleDriver.Salary}$
"
            );
        }
        Console.WriteLine(
            @$"
-------------------------------------------------------------------------------
Total Cost:                                                             {CalculateTotalCost()}$
-------------------------------------------------------------------------------"
        );
    }
}

public class Vehicle
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string VehicleType { get; set; }
    public TransportationVehicleStatus VehicleStatus { get; set; } =
        TransportationVehicleStatus.Unavailable;
    public decimal TransportationCost { get; set; }
    public Driver VehicleDriver { get; set; }

    public Vehicle(string vehicleType, decimal transportationCost, Driver driver)
    {
        this.VehicleType = vehicleType;
        this.TransportationCost = transportationCost;
        this.VehicleDriver = driver;
    }

    public void DisplayVehicleInfo()
    {
        Console.WriteLine(
            @$"
ID: {Id}
Type: {VehicleType}
Status: {VehicleStatus}
Cost: {TransportationCost}$
Driver: {VehicleDriver.Name}({VehicleDriver.Id})
"
        );
    }
}

public class Driver
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public decimal Salary { get; set; }

    public Driver(string name, decimal salary)
    {
        this.Name = name;
        this.Salary = salary;
    }

    public void DisplayDriverInfo()
    {
        Console.WriteLine(
            @$"
ID: {Id}
Name: {Name}
Salary: {Salary}$
"
        );
    }
}
