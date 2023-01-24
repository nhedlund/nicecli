using Microsoft.Extensions.Hosting;

namespace NiceCli.Dotnet;

public static class HostBuilderContextExtensions
{
  public static TGlobalOptions GetCliGlobalOptions<TGlobalOptions>(this HostBuilderContext context)
  {
    if (context.Properties.TryGetValue(HostBuilderExtensions.GlobalOptionsKey, out var globalOptions))
      return (TGlobalOptions) globalOptions;

    throw new KeyNotFoundException(
      $"CLI global options not found in host builder context. Make sure that {nameof(HostBuilderExtensions.ConfigureCli)} " +
      $"has been added early in the host builder code and run before invoking {nameof(GetCliGlobalOptions)}.");
  }
}
