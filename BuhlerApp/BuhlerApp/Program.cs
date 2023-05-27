// Here an example of scheduler work is presented
using BuhlerApp.Model;

// 1. Create machines

IMachine flourMill = new GenericMachine("Large flour mill", MachineType.FlourMill);
Console.WriteLine($"The '{flourMill}' machine has been created.");
IMachine furnace = new GenericMachine("Old furnace", MachineType.Furnace);
Console.WriteLine($"The '{furnace}' machine has been created.");

// 2. Create schedulers
IRecipeSchedulerFactory schedulerFactory = new RecipeSchedulerFactory();
IRecipeScheduler flourMillScheduler = schedulerFactory.GetInstance(flourMill);
IRecipeScheduler furnaceScheduler = schedulerFactory.GetInstance(furnace);

// 3. Schedule recipes
DateTime startTime = DateTime.Now.AddHours(1);
flourMillScheduler.Schedule("Mill 2 tons of flour", startTime, 120);
flourMillScheduler.Schedule("Mill 3 tons of flour", startTime.AddHours(2), 180);
furnaceScheduler.Schedule("Bake 100 loafs of bread", startTime.AddHours(2), 240);
IRecipe theLastFurnaceRecipe = furnaceScheduler.Schedule("Bake 150 loafs of bread", startTime.AddHours(6), 360);

// 4. Get current schedule
Console.WriteLine($"The '{flourMill}' schedule:");

foreach (IRecipe recipe in flourMillScheduler.GetSchedule())
{
    Console.WriteLine(recipe);
}

Console.WriteLine($"The '{furnace}' schedule:");

foreach (IRecipe recipe in furnaceScheduler.GetSchedule())
{
    Console.WriteLine(recipe);
}

// 5. Cancel the last furnace recipe
furnaceScheduler.Cancel(theLastFurnaceRecipe);
Console.WriteLine($"The following recipe has been cancelled: {theLastFurnaceRecipe}");

//6. Get updated furnace schedule
Console.WriteLine($"The new '{furnace}' schedule:");

foreach (IRecipe recipe in furnaceScheduler.GetSchedule())
{
    Console.WriteLine(recipe);
}
