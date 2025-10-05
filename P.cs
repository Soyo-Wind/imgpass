class P
{
    public static void Main(string[] args)
    {
        if (args[0] == "e")
        {
            string inputPath = args[1];
            string message = Console.ReadLine()?? "";

            Steganography.Encode(inputPath, "stgn.png", message);
            Console.WriteLine("AC");
        }
        else if (args[0] == "d")
        {
            string message = Steganography.Decode(args[1]);
            Console.WriteLine(message);
        }
        else
        {
            Console.Error.WriteLine("WS");
        }
    }
}
