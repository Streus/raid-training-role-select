using Spectre.Console.Cli;

partial class Program {
    static async Task<int> Main(string[] args) {
        var app = new CommandApp();

        app.Configure(config => {
            config.AddBranch("form", form => {
                form.AddCommand<FormCreateCommand>("create");
                form.AddCommand<FormShowCommand>("show");
            });
            config.AddCommand<AssignCommand>("assign");
            config.AddCommand<VerifyCommand>("verify");
        });

        return await app.RunAsync(args);
    }
}
