public interface IRandomProvider
{
    // Return int in range
    // min <= x < max
    int RandomInt(int min, int max);

    // Get if the thing happens or not, given the chance
    // chance is a value 0-1
    bool ThingHappens(float chance); 

}