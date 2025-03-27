using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HabitTracker.Models;
using HabitTracker.Reference;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace HabitTracker.Database
{
    public class HabitRepository
    {

        private string connectionString = Constants.dbConnectionString;

        internal HabitRepository()
        {
            InitialiseDatabase();
        }

        internal List<Habit> GetHabbits()
        {
            var habits = new List<Habit>();
            using (var connection = new SqliteConnection(connectionString))

            {
                connection.Open();
                string query = "SELECT * FROM habit";
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habits.Add(new Habit(

                            Convert.ToInt32(reader["Id"]),
                            Convert.ToDateTime(reader["Date"]),
                            reader["Description"].ToString() ?? "",
                            Convert.ToInt32(reader["Count"])
                        ));
                    }

                }
            }
            return habits;
        }

        internal void AddHabit(Habit habit)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO habit (Date, Description, Count) VALUES (@Date, @Description, @Count)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", habit.Date);
                    command.Parameters.AddWithValue("@Description", habit.Description);
                    command.Parameters.AddWithValue("@Count", habit.Count);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void InitialiseDatabase()
        {
            if (!File.Exists("HabitTracker.db"))
            {
                Console.WriteLine("Database not found. Creating new database...");
                //Simply opening a connection to a non-existent SQLite file automatically creates it.

                try
                {
                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        string createTableQuery = @"CREATE TABLE IF NOT EXISTS habit (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Date DATETIME, 
                                            Description TEXT NOT NULL, 
                                            Count INTEGER NOT NULL)";
                        using (var command = new SqliteCommand(createTableQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Database created successfully.");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating database: {ex.Message}");
                    Console.ReadLine();

                }
            }
            else
            {
                Console.WriteLine("Database found. Proceeding...");
                Console.ReadLine();
            }
        }

        internal void EditHabitDescription(string desc, int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE habit SET Description = @description WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@description", desc);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void EditHabitCount(int count, int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE habit SET Count = @count WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@count", count);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void EditHabitDate(DateTime date, int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE habit SET Date = @date WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal void DeleteHabit(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM habit WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        ////////
        internal int ReadLineHelper(string type)
        {

            int result;

            while (true)
            {
                AnsiConsole.WriteLine($"Enter {type}");
                if (int.TryParse(Console.ReadLine(), out result) && Helper(result, type))
                {
                    return result;
                }
                else
                {
                    AnsiConsole.WriteLine("Please enter a valid number");
                }
            }
        }

        internal bool Helper(int n, string t)
        {
            switch (t)
            {
                case Constants.Day:
                    return n > 0 || n < 31;

                case Constants.Month:
                    return n > 0 || n < 12;

                case Constants.Year:
                    return n > 0 || n < 9999;

                default:
                    return false;

            }
        }
    }
}