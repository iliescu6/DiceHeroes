public interface IPlatformInput 
{
    //Tap and hold
    bool IsInputPressed();
    //Frame is pressed down only
    bool IsInputDown();
    //Frame is release
    bool IsInputUp();
}
