using Contracts;
using EventSourcing.Core.Interfaces;
using EventSourcing.Core.Providers;
using EventSourcing.Models;

namespace EventSourcing.Core
{
    public class StateMachineHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IReducerProvider _reducerProvider;
        private readonly IStateDataProvider _stateDataProvider;
        private readonly OrderNumberHelper _orderNumberHelper;
        private readonly IHookExecutor _hookExecutor;

        public StateMachineHandler(
            IEventStore eventStore,
            IReducerProvider reducerProvider,
            IStateDataProvider stateDataProvider,
            OrderNumberHelper orderNumberHelper,
            IHookExecutor hookExecutor
        )
        {
            _eventStore = eventStore;
            _reducerProvider = reducerProvider;
            _stateDataProvider = stateDataProvider;
            _orderNumberHelper = orderNumberHelper;
            _hookExecutor = hookExecutor;
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

            var emptyStateData = await _stateDataProvider.GetStateDataByStateMachine(
                stateMachineId
            );

            _orderNumberHelper.AssignOrderNumbers(existingEvents, aggregateEventsToExecute);

            var initialStateInfo = StateInfo.Create(emptyStateData, stateMachineId, aggregateId);

            var existingStateInfo = await GetStateInfo(initialStateInfo, existingEvents);
            var newStateInfo = await GetStateInfo(existingStateInfo, aggregateEventsToExecute);

            await _eventStore.WriteEvents(aggregateEventsToExecute.ToArray());

            await _hookExecutor.RegisterHooksForExecution(aggregateEventsToExecute.ToArray());

            return newStateInfo;
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
    }
}
