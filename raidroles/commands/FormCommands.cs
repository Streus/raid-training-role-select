using System.ComponentModel;
using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Json;

partial class Program {
    class FormCreateCommand : AsyncCommand<FormCreateCommand.Settings> {
        public class Settings : CommandSettings {
            [CommandArgument(position: 0, template: "[path]")]
            [DefaultValue("form.json")]
            public string Path { get; init; } = "";
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
            var path = Path.GetFullPath(settings.Path);

            var form = new Form { 
                Trainers = Query("[magenta]trainers:[/]"),
                Guaranteed = Query("[magenta]guaranteed:[/]"),
                PrimaryRoleApplicants = new() {
                    QHeal = Query("[green]quick heal:[/]"),
                    AHeal = Query("[green]alac heal:[/]"),
                    QDps = Query("[green]quick dps:[/]"),
                    ADps = Query("[green]alac dps:[/]"),
                    Dps = Query("[green]dps:[/]"),
                },
                MechanicsApplicants = new() {
                    Tank = Query("[cyan]tank:[/]"),
                },
                BackupRoleApplicants = new() {
                    BkHeal = Query("[yellow]backup heal:[/]"),
                    BkBoon = Query("[yellow]backup boon:[/]"),
                    BkDps = Query("[yellow]backup dps:[/]"),
                }
            };

            using StreamWriter writer = new(new FileInfo(path).Open(FileMode.Create));
            var formJson = JsonConvert.SerializeObject(form);
            await writer.WriteAsync(formJson);
            await writer.FlushAsync();

            return 0;
        }

        private static string[] Query(string message) => AnsiConsole.Prompt(
            new TextPrompt<string>(message)
                .AllowEmpty()
            ).Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    class FormShowCommand : AsyncCommand<FormShowCommand.Settings> {
        public class Settings : CommandSettings {
            [CommandArgument(position: 0, template: "[path]")]
            [DefaultValue("form.json")]
            public string Path { get; init; } = "";
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
            var path = Path.GetFullPath(settings.Path);

            using StreamReader reader = new FileInfo(path).OpenText();
            var formJson = await reader.ReadToEndAsync();

            AnsiConsole.Write(
                new Panel(new JsonText(formJson)) {
                    Header = new PanelHeader(settings.Path),
                    Width = 50
                }
            );

            return 0;
        }
    }
}