using Core.Tests.TestModels;
using EventSourcing.Core;
using EventSourcing.Core.Providers;
using EventSourcing.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Tests;

public class AppSettingsConfigurationReducerProviderUnitTest
{
    public IConfiguration CreateConfiguration(Dictionary<string, string?> configurationDict)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationDict)
            .Build();

        return configuration;
    }

    [Fact()]
    public void ReducerNotSetInConfigurationTest()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", null }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var reducerProvider = new AppSettingsConfigurationReducerProvider(configuration);

        var payload = EventPayload.Create(
            new Dictionary<string, object>(),
            "RandomEvent1",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "test-state-machine"
        );

        try
        {
            reducerProvider.GetReducer(payload);
        }
        catch (EventReducerMapNotAddedException)
        {
            Assert.True(true);
        }
        catch (Exception)
        {
            Assert.False(true);
        }
    }

    [Fact()]
    public void ReducerObjectConfigurationTest()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", "TestReducer" }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var reducerProvider = new AppSettingsConfigurationReducerProvider(configuration);

        var payload = EventPayload.Create(
            new Dictionary<string, object>(),
            "RandomEvent1",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "test-state-machine"
        );

        try
        {
            reducerProvider.GetReducer(payload);
        }
        catch (ReducerNotFoundException)
        {
            Assert.True(true);
        }
        catch (Exception)
        {
            Assert.False(true);
        }
    }

    [Fact]
    public void MoneySubtracted()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            { "Key1", "Value1" },
            { "Nested:Key1", "NestedValue1" },
            { "Nested:Key2", "NestedValue2" },
            { "RandomEvent1", "TransferMoney" }
        };
        var configuration = CreateConfiguration(myConfiguration);

        var reducerProvider = new AppSettingsConfigurationReducerProvider(configuration);
        var eventData = new Dictionary<string, object> { { "moneySent", 550 } };

        var payload = EventPayload.Create(
            eventData,
            "RandomEvent1",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "test-state-machine"
        );

        var stateData = new AccountStateData() { Money = 1000 };

        try
        {
            var services = new ServiceCollection();
            services.RegisterReducers();
            var reducer = reducerProvider.GetReducer(payload).Result;
            stateData = (AccountStateData)reducer.Reduce(stateData, payload);

            Assert.Equal(450, stateData.Money);
        }
        catch (EventReducerMapNotAddedException)
        {
            Assert.False(true);
        }
        catch (ReducerNotFoundException)
        {
            Assert.False(true);
        }
        catch (ReducerNotRegisteredException)
        {
            Assert.False(true);
        }
        catch (Exception)
        {
            Assert.False(true);
        }
    }
}
