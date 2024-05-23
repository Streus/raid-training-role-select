using System.ComponentModel;
using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Cli;

partial class Program {
    class VerifyCommand : AsyncCommand<VerifyCommand.Settings> {
        public class Settings : CommandSettings {
            [CommandArgument(position: 0, template: "[receipt-path]")]
            [DefaultValue("form.receipt.json")]
            public string Path { get; init; } = "";
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
            var receiptPath = Path.GetFullPath(settings.Path);

            using var reader = new FileInfo(receiptPath).OpenText();
            var receiptContent = await reader.ReadToEndAsync();
            var receipt = JsonConvert.DeserializeObject<Receipt>(receiptContent)!;

            SetSeed(receipt.Seed);
            var replay = SelectRoles(receipt.Form, quiet: true);

            var passed = true;

            var table = new Table().AddColumns(settings.Path, "replay");
            for (int sub = 0; sub < 2; sub++) {
                for (int i = 0; i < 5; i++) {
                    var actual = receipt.Members[sub][i].ToString();
                    var expected = replay.Members[sub][i].ToString();
                    table.AddRow(actual, expected);
                }
            }
            AnsiConsole.Write(table);

            return passed ? 0 : 99;
        }
    }
}