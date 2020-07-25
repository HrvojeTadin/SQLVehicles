using NLog;
using System;
using System.Data.SqlClient;
using System.Data;
using SQLVehicles.Models;
using System.Collections.Generic;

namespace SQLVehicles
{
    public class Repository
    {
        readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string connectionString = "Data Source=DESKTOP-RGHHS6S;Integrated Security=True;Database=VehicleServisDB";
        public List<TypeOfVehicle> VehicleTypes { get; set; }
        public List<BrandOfVehicle> VehicleBrands { get; set; }

        public Repository()
        {
            logger.Info("Repository() constructor started.");
            VehicleTypes = new List<TypeOfVehicle>();
            VehicleBrands = new List<BrandOfVehicle>();
            GetTypesOfVehcile();
            GetBrandsOfVehicle();
        }

        public void AddDriverDB(string pin, string name, string lastName, DateTime birthDate)
        {
            logger.Debug("Insert new driver into database...");
            using SqlConnection openConnection = new SqlConnection(connectionString);

            string insertQuery = "INSERT INTO Drivers (PIN, Name, LastName, BirthDate) VALUES (@PIN, @Name, @LastName, @BirthDate)";

            using SqlCommand insertCommand = new SqlCommand(insertQuery);

            insertCommand.Connection = openConnection;
            insertCommand.Parameters.Add("@PIN", SqlDbType.NVarChar, 10).Value = pin;
            insertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 30).Value = name;
            insertCommand.Parameters.Add("@LastName", SqlDbType.NVarChar, 30).Value = lastName;
            insertCommand.Parameters.Add("@BirthDate", SqlDbType.Date).Value = birthDate.Date;

            openConnection.Open();

            int rowsAffected = insertCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                logger.Debug("Driver is inserted successfuly.");
            }
            else
            {
                logger.Debug("Driver insert was not succeed.");
                throw new Exception();
            }
        }

        public List<Driver> GetAllDrivers()
        {
            logger.Info("Get all drivers from database started.");
            List<Driver> drivers = new List<Driver>();
            using SqlConnection openConnection = new SqlConnection(connectionString);

            string selectQuery = "SELECT ID, PIN, Name, LastName, BirthDate FROM Drivers";

            using SqlCommand selectCommand = new SqlCommand(selectQuery);
            selectCommand.Connection = openConnection;

            try
            {
                openConnection.Open();
                using SqlDataReader reader = selectCommand.ExecuteReader();
                logger.Info("Getting drivers data from database...");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        drivers.Add(new Driver((Guid)reader["ID"], (string)reader["PIN"], (string)reader["Name"], (string)reader["LastName"], (DateTime)reader["BirthDate"]));
                    }
                    logger.Info("Drivers data retrieved successfuly.");
                }
                else
                {
                    logger.Info("There is not drivers data in database.");
                }

                reader.Close();
                openConnection.Close();

                return drivers;
            }
            catch (Exception ex)
            {
                logger.Error("Drivers data retrieval didn't succeed.");
                logger.Error(ex.Message);
                return drivers;
            }
        }

        public void DeleteDriver(string pin)
        {
            logger.Debug("Calling delete driver (PIN: {0}) from database.", pin);
            using SqlConnection openConnection = new SqlConnection(connectionString);

            string deleteQuery = "DELETE FROM Drivers WHERE PIN = @PIN";

            using SqlCommand deleteCommand = new SqlCommand(deleteQuery);
            deleteCommand.Connection = openConnection;

            deleteCommand.Connection = openConnection;
            deleteCommand.Parameters.Add("@PIN", SqlDbType.NVarChar, 10).Value = pin;

            openConnection.Open();

            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                logger.Debug("Driver delete is succeed.");
            }
            else
            {
                logger.Error("Delete of driver is not succeed.");
                throw new Exception();
            }
        }

        public void AddVehicleDB(string registration, string name, string color, string typeOfVehicle, string brandOfVehcile, Guid? driverID)
        {
            using SqlConnection openConnection = new SqlConnection(connectionString);

            using SqlCommand command = new SqlCommand("AddVehicle", openConnection);
            command.CommandType = CommandType.StoredProcedure;

            Guid typeOfVehicleID = VehicleTypes.Find(x => x.Code == typeOfVehicle).ID;
            Guid brandOfVehicleID = VehicleBrands.Find(x => x.Code == brandOfVehcile).ID;

            command.Parameters.Add("@Registration", SqlDbType.NVarChar, 10).Value = registration;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 30).Value = name;
            command.Parameters.Add("@Color", SqlDbType.NVarChar, 30).Value = color;
            command.Parameters.Add("@TypeOfVehicleID", SqlDbType.UniqueIdentifier, 30).Value = typeOfVehicleID;
            command.Parameters.Add("@BrandOfVehicleID", SqlDbType.UniqueIdentifier, 30).Value = brandOfVehicleID;
            command.Parameters.Add("@DriverID", SqlDbType.UniqueIdentifier, 30).Value = driverID;

            openConnection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                logger.Debug("The vehicle is inserted in database.");
            }
            else
            {
                logger.Debug("Insert of vehicle did not succeed.");
                throw new Exception();
            } 
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            using SqlConnection openConnection = new SqlConnection(connectionString);
            using SqlCommand selectCommand = new SqlCommand("GetAllVehicles", openConnection);
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.Connection = openConnection;

            try
            {
                openConnection.Open();
                using SqlDataReader reader = selectCommand.ExecuteReader();
                logger.Info("Getting all vehicled data from database...");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        vehicles.Add(new Vehicle((Guid)reader["ID"], (string)reader["Registration"], (string)reader["Name"], (string)reader["Color"], (Guid)reader["TypeOfVehicleID"], (Guid)reader["BrandOfVehicleID"], (Guid)reader["DriverID"]));
                    }
                    logger.Info("The vehicles data are retrieved.");
                }
                else
                {
                    logger.Info("There is no data of vehicles in database.");
                }

                reader.Close();
                openConnection.Close();

                return vehicles;
            }
            catch (Exception ex)
            {
                logger.Error("Retrieval of vehicles data did not succeed.");
                logger.Error(ex.Message);
                return vehicles;
            }
        }

        public void DeleteVehicle(string registration)
        {
            logger.Debug("Starting delete of vehicle, Registration: {0}", registration);

            using SqlConnection openConnection = new SqlConnection(connectionString);
            using SqlCommand deleteCommand = new SqlCommand("DeleteVehicle", openConnection);
            deleteCommand.CommandType = CommandType.StoredProcedure;
            deleteCommand.Connection = openConnection;

            deleteCommand.Parameters.Add("@Registration", SqlDbType.NVarChar, 10).Value = registration;

            openConnection.Open();
            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                logger.Debug("The vehicle is deleted from database.");
            }
            else
            {
                logger.Error("Delete of vehicle did not succeed.");
                throw new Exception();
            }
        }

        private void GetTypesOfVehcile()
        {
            using SqlConnection openConnection = new SqlConnection(connectionString);

            string selectQuery = "SELECT ID, Code, Name FROM TypeOfVehicle";

            using SqlCommand selectCommand = new SqlCommand(selectQuery);
            selectCommand.Connection = openConnection;

            try
            {
                openConnection.Open();

                using SqlDataReader reader = selectCommand.ExecuteReader();
                logger.Info("Getting type of vehicles data from database..");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VehicleTypes.Add(new TypeOfVehicle((Guid)reader["ID"], (string)reader["Code"], (string)reader["Name"]));
                    }
                    logger.Info("Type of vehicle data is successfuly retieved.");
                }
                else
                {
                    logger.Info("There is no types of vehicle data in database.");
                }

                reader.Close();
                openConnection.Close();
            }
            catch (Exception ex)
            {
                logger.Error("The type of vehicles data retrieval did not succeed.");
                logger.Error(ex.Message);
            }
        }

        private void GetBrandsOfVehicle()
        {
            using SqlConnection openConnection = new SqlConnection(connectionString);

            string selectQuery = "SELECT ID, Code, Name FROM BrandOfVehicle";

            using SqlCommand selectCommand = new SqlCommand(selectQuery);
            selectCommand.Connection = openConnection;

            try
            {
                openConnection.Open();

                using SqlDataReader reader = selectCommand.ExecuteReader();
                logger.Info("Getting brand of vehicle data from database...");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VehicleBrands.Add(new BrandOfVehicle((Guid)reader["ID"], (string)reader["Code"], (string)reader["Name"]));
                    }
                    logger.Info("Brand of vehicle data is successfuly retieved.");
                }
                else
                {
                    logger.Info("There is no brand of vehicle data in database.");
                }

                reader.Close();
                openConnection.Close();
            }
            catch (Exception ex)
            {
                logger.Error("The brand of vehicles data retrieval did not succeed.");
                logger.Error(ex.Message);
            }
        }
    }
}