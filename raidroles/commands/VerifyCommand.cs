using Spectre.Console.Cli;

partial class Program {
    class VerifyCommand : AsyncCommand<VerifyCommand.Settings> {
        public class Settings : CommandSettings {

        }

        public override Task<int> ExecuteAsync(CommandContext context, Settings settings) {
            throw new NotImplementedException();
        }
    }
}