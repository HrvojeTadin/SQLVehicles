using NLog;
using SQLVehicles.Models;
using System;
using System.Collections.Generic;

namespace SQLVehicles.Helpers
{
    public static class Menu
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Repository repos = new Repository();

        private static void ShowMenu()
        {
            Console.WriteLine("\n---------- Vehicle maintenance application ----------");
            Console.WriteLine("\nChoose an option:\n");
            Console.WriteLine("1 - Add a driver");
            Console.WriteLine("2 - List all drivers");
            Console.WriteLine("3 - Delete the driver");
            Console.WriteLine("4 - Add a vehicle");
            Console.WriteLine("5 - List all vehicles");
            Console.WriteLine("6 - Delete the vehicle");
            Console.WriteLine("7 - Exit the application");
            Console.Write("\nSelection: ");
        }

        public static void StartTheMenu()
        {
            logger.Info("Starting the menu...");
            int answer = 1;
            do
            {
                ShowMenu();

                try
                {
                    answer = int.Parse(Console.ReadLine());
                    switch (answer)
                    {
                        case (int)MenuEnum.Izlaz:
                            Console.WriteLine("\nThanks for using this app!");
                            logger.Info("End of application execution.");
                            break;
                        case (int)MenuEnum.AddDriver:
                            AddDriverMenu();
                            break;
                        case (int)MenuEnum.ShowAllDrivers:
                            ShowAllDriversMenu();
                            break;
                        case (int)MenuEnum.DeleteDriver:
                            DeleteDriverMenu();
                            break;
                        case (int)MenuEnum.AddVehicle:
                            AddVehicleMenu();
                            break;
                        case (int)MenuEnum.ListAllVehicles:
                            ListAllVehiclesMenu();
                            break;
                        case (int)MenuEnum.DeleteVehicle:
                            DeleteVehicleMenu();
                            break;
                        default:
                            Console.WriteLine("\nYou have entered an unsupported entry. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nYou have entered an unsupported entry. Please try again.");
                    logger.Error("When selecting the menu, the user did not enter int.");
                    logger.Error("Exception message: " + ex.Message);
                    logger.Error("Exception stacktrace: " + ex.StackTrace);
                }
            } while (answer != 7);
        }

        private static void AddDriverMenu()
        {
            logger.Debug("Data entry for the new driver has started.");

            Console.Write("Enter the driver's Name: ");
            string Name = Console.ReadLine();
            logger.Debug("Driver's Name: " + Name);

            Console.Write("Enter the driver's last Name: ");
            string LastName = Console.ReadLine();
            logger.Debug("Driver's last Name: " + LastName);

            Console.Write("Enter the driver's PIN: ");
            string PIN = Console.ReadLine();
            logger.Debug("Driver's PIN: " + PIN);

            string BirthDate;
            bool repeatInput = true;

            do
            {
                Console.Write("Enter the date of birth (format: DD/MM/YYYY ili DD.MM.YYYY): ");
                BirthDate = Console.ReadLine();
                logger.Debug("Driver's date of birth: " + BirthDate);

                try
                {
                    DateTime dateOfBirth = Convert.ToDateTime(BirthDate);

                    try
                    {
                        repos.AddDriverDB(PIN, Name, LastName, dateOfBirth);
                        Console.WriteLine("PIN driver {0} successfully added to database!!", PIN);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("The entry of the driver into the base failed!");
                        logger.Error(ex.Message);
                    }
                    repeatInput = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("You have entered the date in the wrong format! Please try again..\n");
                    logger.Error("The birth date was entered in an invalid format: " + BirthDate);
                }
            }
            while (repeatInput);
            
        }

        private static void ShowAllDriversMenu()
        {
            logger.Debug("The menu for listing all drivers has been started.");
            List<Driver> Driveri = repos.GetAllDrivers();

            if (Driveri.Count > 0)
            {
                Console.WriteLine("\nThe list of all drivers...");
                foreach (var item in Driveri)
                {
                    Console.WriteLine("PIN: {0}, Name: {1}, Surname: {2}, Birth date: {3}", item.PIN, item.Name, item.LastName, item.BirthDate.ToShortDateString());
                }
            }
            else
            {
                Console.WriteLine("There are no existing drivers listed in the database. Please enter drivers first.");
            }
        }

        private static void DeleteDriverMenu()
        {
            logger.Debug("The menu for deleting one driver has been started.");
            List<Driver> Driveri = repos.GetAllDrivers();

            if (Driveri.Count > 0)
            {
                Console.WriteLine("\nSelect the driver you want to delete by PIN: ");
                foreach (var item in Driveri)
                {
                    Console.WriteLine("PIN: {0}, Name: {1}, SurName: {2}, Birth date: {3}", item.PIN, item.Name, item.LastName, item.BirthDate.ToShortDateString());
                }

                Console.Write("Enter the PIN: ");
                string PIN = Console.ReadLine();

                try
                {
                    repos.DeleteDriver(PIN);
                    Console.WriteLine("\nVDriver of PIN: {0} is successfuly delete from database!", PIN);
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                    Console.WriteLine("\nCan not delete driver from database!");
                }
            }
            else
            {
                Console.WriteLine("\nThere is no drivers in database. Please enter the drivers first.");
            }
        }

        private static void AddVehicleMenu()
        {
            logger.Debug("AddVehicleMenu() started.");

            Console.Write("Enter the registration: ");
            string reg = Console.ReadLine();

            Console.Write("Enter the name: ");
            string name = Console.ReadLine();

            Console.Write("Enter the color: ");
            string color = Console.ReadLine();

            Console.WriteLine("\nEnter the brand of vehicle by Code: ");
            foreach (var item in repos.VehicleBrands)
            {
                Console.WriteLine("Code: {0} - Name: {1}", item.Code, item.Name);
            }
            Console.Write("\nEnter the Code of brand of vehicle: ");
            string codeOfBrand = Console.ReadLine();

            Console.Write("\nEnter the type of vehicle by Code: ");
            foreach (var item in repos.VehicleTypes)
            {
                Console.WriteLine("Code: {0} - Name: {1}", item.Code, item.Name);
            }
            Console.Write("\nEnter the code of type of vehicle: ");
            string codeOfType = Console.ReadLine();

            // Offer existing list of drivers
            List<Driver> drivers = repos.GetAllDrivers();
            if (drivers.Count > 0)
            {
                Console.Write("\nSelect the driver by PIN: ");
                foreach (var item in drivers)
                {
                    Console.WriteLine("PIN: {0}, Name: {1}, Surname: {2}, Birth date: {3}", item.PIN, item.Name, item.LastName, item.BirthDate.ToShortDateString());
                }
                Console.WriteLine("\nEnter driver's PIN: ");
                string driverPIN = Console.ReadLine();
                Guid driverID = drivers.Find(x => x.PIN == driverPIN).ID;

                try
                {
                    repos.AddVehicleDB(reg, name, color, codeOfType, codeOfBrand, driverID);
                    Console.WriteLine("\nThe vehicle is successfuly inserted in database!");
                }
                catch(Exception ex)
                {
                    logger.Error(ex.Message);
                    Console.WriteLine("\nInsert of vehicle did not succeed!");
                }
            }
            else
            {
                try
                {
                    repos.AddVehicleDB(reg, name, color, codeOfType, codeOfBrand, null); // enter the vehicle withoud driverID
                    Console.WriteLine("\nThe vehicle is successfuly inserted in database!");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    Console.WriteLine("\nInsert of vehicle did not succeed!");
                }
            }
        }

        private static void ListAllVehiclesMenu()
        {
            logger.Debug("ListAllVehiclesMenu() started.");

            List<Vehicle> vehicles = repos.GetAllVehicles();

            if (vehicles.Count > 0)
            {
                Console.WriteLine("List of all vehicles...");
                foreach (var item in vehicles)
                {
                    Console.WriteLine("Registration: {0}, Name: {1}, Color: {2}, Type of vehicle: {3}, Brand of vehicle: {4}",
                        item.Registration, item.Name, item.Color, repos.VehicleTypes.Find(x => x.ID == item.TypeOfVehicleID).Name, repos.VehicleBrands.Find(x => x.ID == item.BrandOfVehicleID).Name);
                }
            }
            else
            {
                Console.WriteLine("There is no vehicle data in database. Please, insert some data first.");
            }
        }

        private static void DeleteVehicleMenu()
        {
            logger.Debug("DeleteVehicleMenu() started.");

            List<Vehicle> vehicles = repos.GetAllVehicles();

            if (vehicles.Count > 0)
            {
                Console.WriteLine("Enter the vehicle to delete by Registration: ");
                foreach (var item in vehicles)
                {
                    Console.WriteLine("Registration: {0}, Name: {1}, Color: {2}", item.Registration, item.Name, item.Color);
                }

                Console.Write("Enter the Registration: ");
                string registration = Console.ReadLine();

                try
                {
                    repos.DeleteVehicle(registration);
                    Console.WriteLine("\nVehicle of Registration: {0} is successfuly delete from database!", registration);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    Console.WriteLine("\nDelete of vehicle did not succeed!");
                }
            }
            else
            {
                Console.WriteLine("There is no vehicle data in databe. Please, enter some data first.");
            }
        }
    }

    public enum MenuEnum
    {
        AddDriver = 1,
        ShowAllDrivers = 2,
        DeleteDriver = 3,
        AddVehicle = 4,
        ListAllVehicles = 5,
        DeleteVehicle = 6,
        Izlaz = 7,
    }
}
