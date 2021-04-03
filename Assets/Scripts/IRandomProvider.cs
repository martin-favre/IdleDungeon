public interface IRandomProvider
{
    int RandomInt(int min, int max);

    // Get if the thing happens or not, given the chance
    // chance is a value 0-1
    bool ThingHappens(float chance); 

}