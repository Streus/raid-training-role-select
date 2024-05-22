using System.CommandLine;
using Newtonsoft.Json;

partial class Program {
    static async Task<int> Main(string[] args) {
        var root = new RootCommand();

        var formArg = new Argument<string>(
            name: "form",
            getDefaultValue: () => "",
            description: ""
        );
        root.Add(formArg);

        var seedOption = new Option<string>(
            name: "--seed",
            getDefaultValue: GenerateSeed,
            description: "Specify a 4-byte number in hexadecimal to seed the RNG"
        );
        seedOption.AddAlias("-s");
        root.AddOption(seedOption);

        var readFromFileOption = new Option<bool>(
            name: "--file",
            description: "Read form data from a path specified in <form>"
        ) {
            Arity = new ArgumentArity(minimumNumberOfValues: 0, maximumNumberOfValues: 0)
        };
        readFromFileOption.AddAlias("-f");
        root.AddOption(readFromFileOption);

        var interactiveOption = new Option<bool>(
            name: "--interactive",
            description: ""
        ) {
            Arity = new ArgumentArity(minimumNumberOfValues: 0, maximumNumberOfValues: 0)
        };
        interactiveOption.AddAlias("-i");
        root.AddOption(interactiveOption);

        root.SetHandler(HandleRoot, formArg, seedOption, readFromFileOption, interactiveOption);

        return await root.InvokeAsync(args);
    }

    static async Task HandleRoot(string formInput, string seed, bool fromFile, bool interactive) {
        SetSeed(seed);

        Form? form = null;

        if (interactive) {
            if (formInput != "" || fromFile) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Do not specify form options in interactive mode");
                Environment.Exit(1);
            }

            form = HandleInteractiveForm();
        } else {
            if (fromFile) {
                var file = new FileInfo(formInput);
                using var stream = new StreamReader(file.OpenRead());
                formInput = await stream.ReadToEndAsync();
            }
            
            form = JsonConvert.DeserializeObject<Form>(formInput);
        }

        if (form != null) {
            var applicants = CollectApplicants(form);
            SelectRoles(applicants);
        }
    }
}
