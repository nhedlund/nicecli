using NiceCli;

// CLI app with overridden name and version, and description, examples and learn more info.

return await CliApp.WithArgs(args)
  .Named("custom-name")
  .Description("This app only serves as an example really")
  .Version("1.0-custom-version")
  .Example("custom-name run")
  .Example("custom-name migrate")
  .Example("custom-name export <start-date> <end-date>")
  .LearnMore("Read about defining the CLI at https://github.com/nhedlund/nicecli")
  .RunAsync();
