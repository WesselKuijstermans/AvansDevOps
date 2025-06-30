using System.Diagnostics.CodeAnalysis;
using System.Text;
using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps {

    [ExcludeFromCodeCoverage]
    public class ConsoleInputHelper {

        public static string[] ParseCommandLine(string commandLine) {
            var args = new List<string>();
            var currentArg = new StringBuilder();
            bool inQuotes = false;

            foreach (var c in commandLine) {
                if (c == '"') {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (char.IsWhiteSpace(c) && !inQuotes) {
                    if (currentArg.Length > 0) {
                        args.Add(currentArg.ToString());
                        currentArg.Clear();
                    }
                } else {
                    currentArg.Append(c);
                }
            }
            if (currentArg.Length > 0)
                args.Add(currentArg.ToString());

            return [.. args];
        }

        public static string UserInput(string prompt) {
            var cancelTokens = new[] { "q", "quit", "cancel" };
            string input = AnsiConsole.Ask<string>($"[blue]{prompt}[/] (to cancel: {string.Join(", ", cancelTokens)}) >");
            if (string.IsNullOrWhiteSpace(input)) {
                AnsiConsole.MarkupLine("[red]Input cannot be empty. Please try again.[/]");
                return UserInput(prompt);
            } else if (cancelTokens.Contains(input.Trim().ToLower())) {
                return string.Empty; // Return empty string to indicate cancellation
            }
            return input.Trim();
        }

        public static TEnum? EnumSelection<TEnum>(string prompt) where TEnum : struct, Enum {
            var choices = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
            var cancelToken = "[red]Cancel[/]";

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[blue]{prompt}[/]")
                    .AddChoices(choices.Select(e => e.ToString()).Append(cancelToken))
            );

            if (selected == cancelToken) {
                AnsiConsole.MarkupLine("[yellow]Command Canceled[/]");
                return null;
            }

            return Enum.TryParse<TEnum>(selected, out var result) ? result : null;
        }

        public static T? ListSelection<T>(string prompt, IEnumerable<T> items) where T : class {
            var itemList = items.ToList();
            if (itemList.Count == 0) {
                AnsiConsole.MarkupLine("[red]No items available to select.[/]");
                return null;
            }

            var choices = itemList
                .Select((item, index) => new { Index = index, Display = item.ToString() ?? $"Item {index}" })
                .ToList();
            var cancelToken = "[red]Cancel[/]";

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[blue]{prompt}[/]")
                    .AddChoices(choices.Select(c => c.Display).Append(cancelToken))
            );

            if (selected == cancelToken) {
                AnsiConsole.MarkupLine("[yellow]Command Canceled[/]");
                return null;
            }

            var chosen = choices.First(c => c.Display == selected);
            return itemList[chosen.Index];
        }

        public static int IntUserInput(string prompt) {
            return AnsiConsole.Ask<int>($"[blue]{prompt}[/] (enter negative value to cancel) >");
        }

        public static bool BoolUserInput(string prompt, bool defaultValue) {
            string defaultValueString = defaultValue ? "Y/n" : "y/N";
            return AnsiConsole.Confirm($"[blue]{prompt}[/] ({defaultValueString}) >", defaultValue);
        }

        public static void HandleSCMCommand(string command, SprintItem sprintItem) {
            string commandBase = sprintItem.versionControlFacade.GetStrategy().GetType().Name.ToLowerInvariant().Replace("versioncontrolstrategy", string.Empty);
            string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) {
                AnsiConsole.MarkupLine("[red]No command provided.[/]");
                return;
            }
            if (parts[0] != commandBase) {
                AnsiConsole.MarkupLine($"[red]'{parts[0]}' not recognized. Expected '{commandBase}' command.[/]");
                return;
            }
            if (parts.Length == 1) {
                AnsiConsole.MarkupLine($"[red]No arguments provided for '{parts[0]}' command.[/]");
                return;
            }
            switch (parts[1].ToLowerInvariant()) {
                case "pull":
                    if (parts.Length < 3) {
                        AnsiConsole.MarkupLine("[red]No branch name provided for 'pull' command.[/]");
                    } else {
                        sprintItem.PullBranch(parts[2]);
                    }
                    break;
                case "checkout":
                    if (parts.Length < 3) {
                        AnsiConsole.MarkupLine("[red]No branch name provided for 'checkout' command.[/]");
                    } else {
                        sprintItem.CheckoutBranch(parts[2]);
                    }
                    break;
                case "commit":
                    if (parts.Length < 3) {
                        AnsiConsole.MarkupLine("[red]No commit message provided for 'commit' command.[/]");
                    } else {
                        sprintItem.Commit(string.Join(" ", parts.Skip(2)));
                    }
                    break;
                case "push":
                    sprintItem.Push();
                    break;
                default:
                    AnsiConsole.MarkupLine($"[red]Unknown SCM command '{parts[1]}'.[/]");
                    break;
            }

        }
    }
}
