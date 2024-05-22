using System.ComponentModel;
using Newtonsoft.Json;
using Spectre.Console.Cli;

partial class Program {
    class AssignCommand : AsyncCommand<AssignCommand.Settings> {
        public class Settings : CommandSettings {
            [CommandOption(template: "--seed|-s")]
            public string Seed { get; init; } = "";

            [CommandOption(template: "--no-receipt")]
            [DefaultValue(false)]
            public bool NoReceipt { get; init; } = false;

            [CommandArgument(position: 0, template: "[form-path]")]
            [DefaultValue("form.json")]
            public string Path { get; init; } = "";
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
            var path = Path.GetFullPath(settings.Path);

            if (string.IsNullOrWhiteSpace(settings.Seed)) {
                GenerateSeed();
            } else {
                SetSeed(settings.Seed);
            }

            using var stream = new FileInfo(path).OpenText();
            var formInput = await stream.ReadToEndAsync();
            var form = JsonConvert.DeserializeObject<Form>(formInput);

            var applicants = CollectApplicants(form!);
            SelectRoles(applicants);

            //todo handle receipt

            return 0;
        }
    }
}