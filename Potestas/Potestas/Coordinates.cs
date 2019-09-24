namespace Potestas
{
    /* TASK. Implement this structure: 
     * 1. Implement custom constructor
     * 2. The valid range for X is [-90; 90], for Y [0; 180]
     * 3. Take into account boxing and unboxing issues
     * 4. Implement + and - operators for this structure.
     * 5. Implement a way to represent coordinates in string.
     * 6. Coordinates are equal each other when each dimension values are equal with thousand precision
     * 7. Implement == and != operators for this structure.
     * 8. 
     */
    public struct Coordinates
    {
        public double X { get; set; }

        public double Y { get; set; }
    }
}
