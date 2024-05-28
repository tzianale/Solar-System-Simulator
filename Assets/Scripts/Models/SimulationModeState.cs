namespace Models
{
    public static class SimulationModeState
    {
        public enum SimulationMode
        {
            Explorer,
            Sandbox
        }

        public static SimulationMode currentSimulationMode;
    }
}