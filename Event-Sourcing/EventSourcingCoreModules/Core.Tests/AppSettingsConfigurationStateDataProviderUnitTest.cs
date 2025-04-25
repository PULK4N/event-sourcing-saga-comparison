using EventSourcing.Core;
using EventSourcing.Core.Providers;
using EventSourcing.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Tests;

public class AppSettingsConfigurationStateDataProviderUnitTest
{
    public IConfiguration CreateConfiguration(Dictionary<string, string?> configurationDict)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationDict)
            .Build();

        return configuration;
    }

    [Fact]
    public void StateMachineNotSetInConfigurationTest()
    {
        var services = new ServiceCollection();
        try
        {
            services.RegisterStateDataTypes();
        }
        catch (Exception) { }

        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", null }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var stateDataProvider = new AppSettingsConfigurationStateDataProvider(configuration);

        var payload = EventPayload.Create(
            new Dictionary<string, object>(),
            "RandomEvent1",
            Guid.NewGuid(),
            "test-state-machine"
        );

        try
        {
            stateDataProvider.GetStateDataByStateMachine(payload.StateMachineId);
        }
        catch (StateMachineNotRegisteredException)
        {
            Assert.True(true);
        }
        catch (Exception)
        {
            Assert.True(false);
        }
    }

    [Fact]
    public void StateDataTypeNotFoundTest()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", null },
            { "test-state-machine", "AccountTestStateData" }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var stateDataProvider = new AppSettingsConfigurationStateDataProvider(configuration);

        var services = new ServiceCollection();
        try
        {
            services.RegisterStateDataTypes();
        }
        catch (Exception) { }

        var payload = EventPayload.Create(
            new Dictionary<string, object>(),
            "RandomEvent1",
            Guid.NewGuid(),
            "test-state-machine"
        );

        try
        {
            stateDataProvider.GetStateDataByStateMachine(payload.StateMachineId);
        }
        catch (StateDataTypeNotFoundException)
        {
            Assert.True(true);
        }
        catch (StateMachineNotRegisteredException)
        {
            Assert.True(false);
        }
        catch (Exception)
        {
            Assert.True(false);
        }
    }

    [Fact]
    public void StateDataExists()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", null },
            { "test-state-machine", "AccountStateData" }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var stateDataProvider = new AppSettingsConfigurationStateDataProvider(configuration);

        var services = new ServiceCollection();
        try
        {
            services.RegisterStateDataTypes();
        }
        catch (Exception) { }

        var payload = EventPayload.Create(
            new Dictionary<string, object>(),
            "RandomEvent1",
            Guid.NewGuid(),
            "test-state-machine"
        );

        try
        {
            stateDataProvider.GetStateDataByStateMachine(payload.StateMachineId);
            Assert.True(true);
        }
        catch (StateDataNotRegisteredException)
        {
            Assert.True(false);
        }
        catch (StateMachineNotRegisteredException)
        {
            Assert.True(false);
        }
        catch (Exception)
        {
            Assert.True(false);
        }
    }
}
