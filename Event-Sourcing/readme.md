1. Create order
2. Reserve inventory
3. Charge payment
4. Create Shipment

- order-state-machine
- inventory-state-machine
- shipment-state-machine

# For sake and purposes
- Create configuration files that are going to create a base of something (like intial events)


# Modules
- [Last] means that it shouldn't contain a reference to any other modules, it can maybe contain a reference to something inside of the scope, like how EventSourcingCore contains a reference to Models

## Domain Modules [Last]
- Contain all domain data (Models, Reducers, Events, StateData)

## HookModules
- Reading from outbox and scheduling projections and hooks
---> IntegrationModules

## IntegrationModules
- Contain all hooks that are related to integrations

## EventSourcingModules [Last]
- Contains all event execution logic.
- Persistence is part of EventSourcingModules, but can be moved away

## CommunicationModule [Last]
- Contains Implementations for communication

## Utils [Last]
- Contains any kind of utils for every module to use.
- Is not integrated with anything like database or something like that.
