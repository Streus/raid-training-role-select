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
            var formPath = Path.GetFullPath(settings.Path);

            if (string.IsNullOrWhiteSpace(settings.Seed)) {
                GenerateSeed();
            } else {
                SetSeed(settings.Seed);
            }

            using var reader = new FileInfo(formPath).OpenText();
            var formContent = await reader.ReadToEndAsync();
            var form = JsonConvert.DeserializeObject<Form>(formContent);

            var receipt = SelectRoles(form ?? new());

            if (!settings.NoReceipt) {
                var dir = Path.GetDirectoryName(formPath)!;
                var receiptPath = Path.Combine(dir, $"{Path.GetFileNameWithoutExtension(formPath)}.receipt.json");

                using StreamWriter writer = new(new FileInfo(receiptPath).Open(FileMode.Create));
                var receiptJson = JsonConvert.SerializeObject(receipt);
                await writer.WriteAsync(receiptJson);
                await writer.FlushAsync();
            }

            return 0;
        }
    }
}