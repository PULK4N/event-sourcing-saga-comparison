using Contracts;
using EventSourcing.Core.Interfaces;
using EventSourcing.Models;

namespace EventSourcing.Core
{
    public class StateMachineHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IReducerProvider _reducerProvider;

        public StateMachineHandler(IEventStore eventStore, IReducerProvider reducerProvider)
        {
            _eventStore = eventStore;
            _reducerProvider = reducerProvider;
        }

        public async Task<Dictionary<Guid, StateInfo>> ExecuteEvents(
            IEnumerable<EventPayload> eventsToExecute
        )
        {
            var aggregateIds = eventsToExecute.Select(x => x.AggregateId).Distinct().ToArray();
            var existingEvents = await _eventStore.GetEventsByAggregate(aggregateIds);

            var stateInfoDictionary = new Dictionary<Guid, StateInfo>();

            foreach (var aggregateId in aggregateIds)
            {
                var aggregateEventsToExecute = eventsToExecute.Where(
                    x => x.AggregateId == aggregateId
                );
                var existingEventsByAggregate = existingEvents[aggregateId].ToList();

                var stateInfo = await GenerateStateInfo(
                    aggregateId,
                    existingEventsByAggregate,
                    aggregateEventsToExecute
                );

                stateInfoDictionary.Add(aggregateId, stateInfo);
            }

            return stateInfoDictionary;
        }

        private async Task<StateInfo> GenerateStateInfo(
            Guid aggregateId,
            IEnumerable<EventPayload> existingEvents,
            IEnumerable<EventPayload> aggregateEventsToExecute
        )
        {
            var firstEventData = aggregateEventsToExecute.First();
            var stateMachineId = firstEventData.StateMachineId;
            var emptyStateData = GetStateDataByStateMachine(stateMachineId);
            AssignOrderNumbers(existingEvents, aggregateEventsToExecute);

            var initialStateInfo = StateInfo.Create(emptyStateData, stateMachineId, aggregateId);

            var existingStateInfo = await GetStateInfo(initialStateInfo, existingEvents);
            var newStateInfo = await GetStateInfo(existingStateInfo, aggregateEventsToExecute);

            return newStateInfo;
        }

        /*
         * Requires a test
         */

        private void AssignOrderNumbers(
            IEnumerable<EventPayload> existingEvents,
            IEnumerable<EventPayload> aggregateEventsToExecute
        )
        {
            uint currentLastOrderNumber = 0;
            if (existingEvents.Any())
                currentLastOrderNumber = existingEvents.Max(x => x.OrderNumber);

            foreach (var payload in aggregateEventsToExecute)
            {
                payload.OrderNumber = ++currentLastOrderNumber;
            }
        }

        private async Task<StateInfo> GetStateInfo(
            StateInfo stateInfo,
            IEnumerable<EventPayload> eventPayloads
        )
        {
            var stateData = stateInfo.StateData;
            foreach (var payload in eventPayloads)
            {
                var reducer = await _reducerProvider.GetReducer(payload);
                stateData = reducer.Reduce(stateData, payload);
                stateInfo.StateData = stateData;
                stateInfo.CurrentOrderNumber = payload.OrderNumber;
                stateInfo.LastUpdateTimestamp = payload.Timestamp;
            }

            return stateInfo;
        }

        /*
         * Supposed to read from the configuration file and create an object based on StateMachineProvider
         * Temporary set to the only state machine that we are using
         */
        private object GetStateDataByStateMachine(string stateMachineId)
        {
            var stateDataName = "AccountStateData";
            var type = Type.GetType(stateDataName);
            if (type is null)
                throw new StateDataNotFoundException(stateDataName);

            var stateData = Activator.CreateInstance(type);
            return stateDataName;
        }
    }
}
