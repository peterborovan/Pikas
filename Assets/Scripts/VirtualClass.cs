public class VirtualClass
{
    public string classTag;

    public VirtualClass(string classTag)
    {
        this.classTag = classTag;
    }
    public string genClassTag()
    {
        return classTag;
    }
}