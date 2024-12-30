public class Program
{
    public static void Main(string[] args) // entry point
    {
        int age = 19;
        string firstName = "Lin Thit";
        string lastName = "Htoo";

        string fullName = firstName + " " + lastName;

        // interpolation
        Console.WriteLine($"Your name is: {fullName}");
        //Console.WriteLine("Your full name: " + fullName);
    }
}
