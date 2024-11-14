namespace ActiveDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var skanskaDirectory = new SkanskaDirectoryServices();
            //skanskaDirectory.PrintGroupMembers("SE-SR-Oneskanska");
            skanskaDirectory.PrintGroupMembers("SE-Skanska Sverige AB");
        }
    }
}
