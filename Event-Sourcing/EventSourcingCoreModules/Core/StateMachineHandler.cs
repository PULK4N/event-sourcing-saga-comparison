using EventSourcing.Core.Interfaces;
using EventSourcing.Core.Providers;
using EventSourcing.Shared.Models;

namespace EventSourcing.Core
{
    public class StateMachineHandler
    {
        private readonly IEventStoreWithOutbox _eventStore;
        private readonly IStateDataProvider _stateDataProvider;
        private readonly OrderNumberHelper _orderNumberHelper;

        public StateMachineHandler(
            IEventStoreWithOutbox eventStore,
            IStateDataProvider stateDataProvider,
            OrderNumberHelper orderNumberHelper
        )
        {
            _eventStore = eventStore;
            _stateDataProvider = stateDataProvider;
            _orderNumberHelper = orderNumberHelper;
        }

        public async Task<Dictionary<Guid, StateInfo>> ExecuteEvents(
            params EventPayload[] eventsToExecute
        )
        {
            var aggregateIds = eventsToExecute
                .Select(x => x.EventExecutionInfo.AggregateId)
                .Distinct()
                .ToArray();
            var existingEvents = await _eventStore.GetEventsByAggregate(aggregateIds);

            var stateInfoDictionary = new Dictionary<Guid, StateInfo>();

            foreach (var aggregateId in aggregateIds)
            {
                var aggregateEventsToExecute = eventsToExecute.Where(
                    x => x.EventExecutionInfo.AggregateId == aggregateId
                );
                var existingEventsByAggregate = existingEvents[aggregateId]
                    .ToList()
                    .OrderBy(x => x.EventExecutionInfo.OrderNumber);

                var stateInfo = await GenerateStateInfo(
                    aggregateId,
                    existingEventsByAggregate,
                    aggregateEventsToExecute
                );

                stateInfoDictionary.Add(aggregateId, stateInfo);
            }

            await _eventStore.WriteEventsWithOutbox(eventsToExecute.ToArray());

            return stateInfoDictionary;
        }

        // TODO: think how to implement impersonate
        // Idea: Store impersonate data in some cache in a separate module
        // Core modules -> ImpersonateModule
        // Executor Module -> ImpersonateModule
        // Executing action handles if Impersonating is On for a user
        // Stores it in ImpersonateModule
        // But since we have the same executor, how do we know that he executed it?
        // We can since it's from the SAME SCOPE, and impersonating instance is registered as SCOPED.
        private async Task<StateInfo> GenerateStateInfo(
            Guid aggregateId,
            IEnumerable<EventPayload> existingEvents,
            IEnumerable<EventPayload> aggregateEventsToExecute
        )
        {
            var firstEventData = aggregateEventsToExecute.First();
            var stateMachineId = firstEventData.EventExecutionInfo.StateMachineId;

            var emptyStateData = await _stateDataProvider.GetStateDataByStateMachine(
                stateMachineId
            );

            _orderNumberHelper.AssignOrderNumbers(existingEvents, aggregateEventsToExecute);

            var initialStateInfo = StateInfo.Create(emptyStateData, stateMachineId, aggregateId);

            var existingStateInfo = await GetStateInfo(initialStateInfo, existingEvents);
            var newStateInfo = await GetStateInfo(existingStateInfo, aggregateEventsToExecute);

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
                stateData = payload.EventData.Apply(stateData, payload.EventExecutionInfo);
                stateInfo.StateData = stateData;
                stateInfo.CurrentOrderNumber = payload.EventExecutionInfo.OrderNumber;
                stateInfo.LastUpdateTimestamp = payload.EventExecutionInfo.Timestamp;
            }

            return stateInfo;
        }
    }
}
