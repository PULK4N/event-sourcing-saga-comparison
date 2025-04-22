using Contracts;
using EventSourcing.Models;

namespace EventSourcing.Core
{
    public class StateMachineHandler
    {
        private readonly IEventStore _eventStore;

        public StateMachineHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
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

                var stateInfo = GenerateStateInfo(
                    aggregateId,
                    existingEventsByAggregate,
                    aggregateEventsToExecute
                );

                stateInfoDictionary.Add(aggregateId, stateInfo);
            }

            return stateInfoDictionary;
        }

        private StateInfo GenerateStateInfo(
            Guid aggregateId,
            IEnumerable<EventPayload> existingEvents,
            IEnumerable<EventPayload> aggregateEventsToExecute
        )
        {
            var firstEventData = aggregateEventsToExecute.First();
            var stateMachineId = firstEventData.StateMachineId;
            var emptyStateData = GetStateDataByStateMachine(stateMachineId);

            var initialStateInfo = StateInfo.Create(emptyStateData, stateMachineId, aggregateId);

            var existingStateInfo = GetStateInfo(initialStateInfo, existingEvents);
            var newStateInfo = GetStateInfo(existingStateInfo, aggregateEventsToExecute);

            return newStateInfo;
        }

        private StateInfo GetStateInfo(StateInfo stateInfo, IEnumerable<EventPayload> eventPayloads)
        {
            throw new NotImplementedException();
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
                throw new NonExistingStateDataException(stateDataName);

            var stateData = Activator.CreateInstance(type);
            return stateDataName;
        }
    }
}
