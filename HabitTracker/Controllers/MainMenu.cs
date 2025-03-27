using HabitTracker.Database;
using HabitTracker.Models;
using HabitTracker.Reference;
using Spectre.Console;

namespace HabitTracker.Controllers
{
    internal class MainMenu
    {

        HabitRepository habitRepository = new HabitRepository();

        internal void MainMenuPage()
        {

            while (true)
            {
                Console.Clear();

                var selection = AnsiConsole.Prompt(
                  new SelectionPrompt<Enums.MainMenuChoices>()
                  .Title("What action do you want to perform?")
                  .AddChoices(Enum.GetValues<Enums.MainMenuChoices>())
                  );


                switch (selection)
                {   //Create
                    case Enums.MainMenuChoices.Create:

                        Habit newHabit;
                        DateTime dateTime = DateTime.Now;
                        int day = 0;
                        int month = 0;
                        int year = 0;
                        int count = 0;


                        var dateSelection = AnsiConsole.Prompt(
                        new SelectionPrompt<Enums.DateChoice>()
                        .Title("Enter date habit completed on?")
                        .AddChoices(Enum.GetValues<Enums.DateChoice>())
                     );

                        switch (dateSelection)
                        {
                            case Enums.DateChoice.Today:

                                dateTime = DateTime.Now;

                                break;


                            case Enums.DateChoice.Previous:

                                day = habitRepository.ReadLineHelper(Constants.Day);

                                month = habitRepository.ReadLineHelper(Constants.Month);

                                year = habitRepository.ReadLineHelper(Constants.Year);

                                dateTime = new DateTime(year, month, day);

                                break;

                        }

                        AnsiConsole.WriteLine("Enter a description for your habit");
                        string description = Console.ReadLine();

                        while (true)
                        {
                            AnsiConsole.WriteLine($"Enter count");
                            if (int.TryParse(Console.ReadLine(), out count))
                            {
                                break;
                            }
                            else
                            {
                                AnsiConsole.WriteLine("Please enter a valid number");
                            }
                        }

                        newHabit = new Habit(dateTime, description, count);

                        habitRepository.AddHabit(newHabit);

                        break;

                    //Get
                    case Enums.MainMenuChoices.Read:
                        var listOfHabits = habitRepository.GetHabbits();
                        foreach (var habit in listOfHabits)
                        {
                            AnsiConsole.WriteLine($"{habit.Id} || {habit.Date} || {habit.Description} || {habit.Count}");
                        }
                        Console.ReadLine();
                        break;


                    //Edit
                    case Enums.MainMenuChoices.Update:
                        var habits = habitRepository.GetHabbits();

                        var habitToEdit = AnsiConsole.Prompt(
                        new SelectionPrompt<Habit>()
                        .Title("Who do you want to edit?")
                        .UseConverter(p => $"{p.Id} || {p.Date} || {p.Description}")
                        .AddChoices(habits)
                        );

                        AnsiConsole.WriteLine($"You are editing record with ID:{habitToEdit.Id} || {habitToEdit.Date} || {habitToEdit.Description}");

                        var elementToEdit = AnsiConsole.Prompt(
                        new SelectionPrompt<Enums.EditChoice>()
                        .Title("What do you want to edit?")
                        .AddChoices(Enum.GetValues<Enums.EditChoice>())
                        );

                        switch (elementToEdit)
                        {
                            case Enums.EditChoice.Description:
                                AnsiConsole.WriteLine("Insert updated description:");

                                var updatedDesc = Console.ReadLine();

                                habitRepository.EditHabitDescription(updatedDesc, habitToEdit.Id);
                                break;

                            case Enums.EditChoice.Count:
                                while (true)
                                {
                                    AnsiConsole.WriteLine($"Enter new count");
                                    if (int.TryParse(Console.ReadLine(), out count))
                                    {
                                        habitRepository.EditHabitCount(count, habitToEdit.Id);
                                        break;
                                    }
                                    else
                                    {
                                        AnsiConsole.WriteLine("Please enter a valid number");
                                    }
                                }
                                break;

                            case Enums.EditChoice.Date:

                                day = habitRepository.ReadLineHelper(Constants.Day);

                                month = habitRepository.ReadLineHelper(Constants.Month);

                                year = habitRepository.ReadLineHelper(Constants.Year);

                                dateTime = new DateTime(year, month, day);

                                habitRepository.EditHabitDate(dateTime, habitToEdit.Id);

                                break;
                        }

                        break;


                    //Delete
                    case Enums.MainMenuChoices.Delete:
                        var delHabits = habitRepository.GetHabbits();

                        var habitToDelete = AnsiConsole.Prompt(
                        new SelectionPrompt<Habit>()
                        .Title("What habit do you want to delete?")
                        .UseConverter(p => $"{p.Id} || {p.Date} || {p.Description}")
                        .AddChoices(delHabits)
                        );

                        if (AnsiConsole.Confirm($"Are you sure you want to delete this habit? {habitToDelete.Date} || {habitToDelete.Description}"))
                        {
                            habitRepository.DeleteHabit(habitToDelete.Id);
                        }
                        else
                        {
                            AnsiConsole.WriteLine("Deletion cancelled");
                            Console.ReadLine();
                        }

                        break;

                    case Enums.MainMenuChoices.Summary:
                        var summary = habitRepository.GetSummary();
                        
                        AnsiConsole.WriteLine($"{summary.MinDate} || {summary.MaxDate} || {summary.Sum}");
                        Console.ReadLine();
                        break;


                    case Enums.MainMenuChoices.Exit:
                        Environment.Exit(0);
                        break;
                }
            }

        }
    }
}