using EventSourcing.Core.Interfaces;

namespace EventSourcing.Core.Providers
{
    public class DemoStateDataProvider : IStateDataProvider
    {
        /*
         * Supposed to read from the configuration file and create an object based on StateMachineProvider
         * Temporary set to the only state machine that we are using
         */
        public Task<object> GetStateDataByStateMachine(string stateMachineId)
        {
            var stateDataName = "AccountStateData";
            var type = Type.GetType(stateDataName);
            if (type is null)
                throw new StateDataNotFoundException(stateDataName);

            var stateData = Activator.CreateInstance(type);
            if (stateData is null)
                throw new StateDataNotRegisteredException(stateDataName);

            return Task.FromResult(stateData);
        }
    }
}
