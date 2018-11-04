using System.Linq;

public class Student
{
    private string nickName;
    private VirtualClass vcl;
    private string passwd;
    private string email;  // nechcel som tam mat email, ale bez neho to zatial neviem

    public Student(string nickName, string email, VirtualClass vcl, string passwd)
    {
        this.nickName = nickName.First().ToString().ToUpper() + nickName.Substring(1);
        this.vcl = vcl;
        this.passwd = passwd;
        this.email = email;
    }
    public string getName()
    {
        return nickName;
    }
    public string getPasswd()
    {
        return passwd;
    }
    public VirtualClass getClass()
    {
        return vcl;
    }
    public string getUnique()
    {
        //string hopefullyUnique = nickName + "@" + passwd + "." + vcl.genClassTag();
        //return hopefullyUnique;
        return email;
    }
}