using System.Reflection;
using SharedKernel;

namespace ArchitectureTests;

#pragma warning disable CA1515

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
}

#pragma warning restore CA1515
