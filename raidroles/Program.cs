using System.CommandLine;
using Newtonsoft.Json;

partial class Program {
    static async Task<int> Main(string[] args) {
        var root = new RootCommand();

        var seedOption = new Option<string>(
            name: "--seed",
            getDefaultValue: GenerateSeed,
            description: ""
        );
        seedOption.AddAlias("-s");
        root.AddOption(seedOption);

        var formArg = new Argument<string>(
            name: "form",
            description: ""
        );
        root.Add(formArg);

        root.SetHandler((seed, rawForm) => {
            SetSeed(seed);
            var form = JsonConvert.DeserializeObject<Form>(rawForm);
            if (form != null) {
                var applicants = CollectApplicants(form);
                SelectRoles(applicants);
            }
        }, seedOption, formArg);

        return await root.InvokeAsync(args);
    }
}
