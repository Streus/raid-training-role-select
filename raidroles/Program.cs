using System.CommandLine;
using Newtonsoft.Json;

partial class Program {
    static async Task<int> Main(string[] args) {
        var root = new RootCommand();

        var formArg = new Argument<string>(
            name: "form",
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
            name: "-f",
            description: "Read form data from a path specified in <form>"
        ) {
            Arity = new ArgumentArity(0, 0)
        };
        root.AddOption(readFromFileOption);

        root.SetHandler(async (formInput, seed, fromFile) => {
            SetSeed(seed);

            if (fromFile) {
                var file = new FileInfo(formInput);
                using var stream = new StreamReader(file.OpenRead());
                formInput = await stream.ReadToEndAsync();
            }
            
            var form = JsonConvert.DeserializeObject<Form>(formInput);
            if (form != null) {
                var applicants = CollectApplicants(form);
                SelectRoles(applicants);
            }
        }, formArg, seedOption, readFromFileOption);

        return await root.InvokeAsync(args);
    }
}
